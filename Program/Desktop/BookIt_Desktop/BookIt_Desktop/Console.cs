using System;
using BookItDependancies;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookIt_Desktop
{
    public partial class Console : Form
    {
        CommandHandler handler;
        public Console()
        {
            InitializeComponent();

            txtConsoleDisplay.BackColor = Color.Black;
            txtConsoleDisplay.ForeColor = Color.White;
            txtConsole.BackColor = Color.Black;
            txtConsole.ForeColor = Color.White;

            handler = new CommandHandler();
            txtConsole.Focus();
        }

        private void BtnEnter_Click(object sender, EventArgs e)
        {
            string cmd = txtConsole.Text;
            AddConsoleText(">" + cmd);

            List<string> commandString = cmd.Split(null).ToList();
            AddConsoleText(handler.Run(commandString));
            txtConsole.Clear();
        }

        private void AddConsoleText(List<string> text)
        {
            foreach (string s in text) AddConsoleText(s);
        }

        private void AddConsoleText(string text)
        {
            txtConsoleDisplay.Text += text + Environment.NewLine;
        }

        private void Console_ResizeEnd(object sender, EventArgs e)
        {
            //12, 12 : 500,340 (540, 430)
            txtConsoleDisplay.Width = Width - 40;
            txtConsoleDisplay.Height = Height - 90;
            txtConsole.Width = txtConsoleDisplay.Width - 50;
            btnEnter.Location = new Point(btnEnter.Location.X, Width = 60);
            txtConsole.Height = 20;
        }
    }

    class Command
    {
        private string commandName;
        private List<string> commandAlias;
        private int commandsRequired;
        private Action<List<string>> commandAction;
        private string commandDescription;

        public Command(string name, Action<List<string>> method, string description, int requiredFields, List<string> alias = null)
        {
            commandName = name;
            commandAction = method;
            commandDescription = description;
            commandAlias = alias;
            commandsRequired = requiredFields;
        }
        public Command(string name, Action<List<string>> method, string description, int requiredFields, string alias)
        {
            commandName = name;
            commandAction = method;
            commandDescription = description;
            commandAlias = new List<string>() { alias };
            commandsRequired = requiredFields;
        }

        /// <summary>
        /// Runs the command
        /// </summary>
        /// <param name="parse"></param>
        public void Run(List<string> parse)
        {
            if (CanVerifyParamaters(parse))
                commandAction(parse);
        }

        private bool CanVerifyParamaters(List<string> parse)
        {
            if (parse.Count() >= commandsRequired)
                return true;
            else return false;
        }
        /// <summary>Command name</summary>
        public string Name { get => commandName; set => commandName = value; }
        ///<summary>Command alias</summary>
        public List<string> Alias { get => commandAlias; set => commandAlias = value; }
        /// <summary>Command action method called</summary>
        public Action<List<string>> Action { get => commandAction; }
        /// <summary>Command description</summary>
        public string Description { get => commandDescription; set => commandDescription = value; }
        /// <summary>Checks if the string is an alias of the command</summary>
        public bool IsAlias(string check)
        {
            if (commandAlias == null) return false;
            foreach (string s in commandAlias)
            {
                if (check == s) return true;
            }
            return false;
        }
    }

    class CommandHandler
    {
        List<Command> commandList;
        List<string> returnString;

        public CommandHandler()
        {
            Intialise();
        }

        /// <summary>
        /// Check for command, run command if found
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Command response</returns>
        public List<string> Run(List<string> input)
        {
            bool commandFound = false;
            string cmd = input[0].ToLower();    //Not case sensitive

            returnString.Clear();
            input.RemoveAt(0);
            if (cmd == "help") { commandFound = true; Help(); }

            foreach (Command command in commandList)
            {
                if (commandFound) break;
                if (cmd == command.Name.ToLower() || command.IsAlias(cmd))
                {
                    commandFound = true;
                    command.Run(input);
                }
            }
            if (!commandFound) return new List<string>() { "Unknown command, type 'help' to view all commands" };
            else if (returnString.Count() == 0) return new List<string>() { "Command failed" };
            return returnString;
        }

        private void Intialise()
        {
            returnString = new List<string>();
            commandList = new List<Command>() {
                new Command("Encrypt", EncryptString,"Encrypt a string, [column] <data>",1,"Enc"),
                new Command("Decrypt", DecryptString,"Decrypt a string, [column] <data>",1,"Decry"),
                new Command("Login", LogUserIn, "Attempt to log in, <username> <password>",2,"Log"),
                //new Command("New", NewEntry, "Creates a new entry to the specified table. Usage: 'New <table> <new data>'", 2, "Create"),
                //new Command("Fetch", FetchData, "Returns data from the database by primary key. Usage : Fetch <table> <key>", 2, "Get")
            };

        }

        private void EncryptString(List<string> data)
        {
            string d = data[0];
            string n = "";
            if (data.Count() > 1)
            {
                d = data[1]; n = data[0];
            }
            string enc1 = "";
            string salt = "";
            if (n == "password")
            {
                if (data.Count > 2) salt = data[2];
                else
                    salt = SecurityManager.GenerateNewSALT();
                enc1 = SecurityManager.OneWayEncryptor(data[1], salt);
                returnString.Add("E:"+enc1);
            }
            if (enc1 != "") d = enc1;
            string enc = SecurityManager.EncryptDatabaseData(n, d);
            returnString.Add("F:"+enc);
            if (salt != "")
                returnString.Add("S:"+salt);
        }

        private void DecryptString(List<string> data)
        {
            string d = data[0];
            string n = "";
            if (data.Count() > 1)
            {
                d = data[1]; n = data[0];
            }

            string enc = SecurityManager.DecryptDatabaseData(n, d);
            returnString.Add(enc);

        }

        private void LogUserIn(List<string> data)
        {
            string s;
            if (Server.Login(data[0], data[1]))
                s = "Successfully logged in, " + data[0];
            else s = "Failed to log in user";
            returnString.Add(s);
        }

        private void Help()
        {
            returnString.Add("Available commands:  (Note [] denote optional parameters, <> are required.");
            foreach (Command c in commandList)
            {
                returnString.Add(c.Name + " : " + c.Description);
            }
        }

        /// <summary>
        /// Create a new entry in the database
        /// </summary>
        /// <param name="newData"></param>
        private void NewEntry(List<string> newData)
        {
            string s = "";
            switch (newData[0].ToLower())
            {
                case "users":
                case "user":
                    try
                    {
                        //s = Server.AddUser(newData[1], newData[2], newData[3], newData[4], newData[5], newData[6], newData[7], Convert.ToInt32(newData[8]));
                        returnString.Add(s);
                    }
                    catch
                    {
                        returnString.Add("Invalid arguments, Users requires : Name, Address, Postcode, Email, Phone, Username, Password, PermissionLevel");
                    }
                    break;
                case "employees":
                case "employee":
                    try
                    {
                        List<string> availability = newData[4].Split('.').ToList();
                        List<string> ammendments = newData[5].Split('.').ToList();
                        //s = Server.AddEmployee(Convert.ToInt32(newData[1]), Convert.ToInt32(newData[2]), newData[3], availability, ammendments);
                        returnString.Add(s);
                    }
                    catch
                    {
                        returnString.Add("Invalid arguments, Users requires : BusinessID, UserID, PermissionLevel, Availability(seperate with .), Ammendments(seperate with .)");
                    }
                    break;
                case "businesses":
                case "business":
                    try
                    {
                        //s = Server.AddBusiness(newData[1], newData[2], newData[3], newData[4], newData[5], newData[6], Convert.ToBoolean(newData[7]), Convert.ToBoolean(newData[8]));
                        returnString.Add(s);
                    }
                    catch
                    {
                        returnString.Add("Invalid arguments, Users requires : Name, Address, Postcode, Email, Phone, Description, Active(true/false), SharedBookings(true/false)");
                    }
                    break;
                case "bookings":
                case "booking":
                    try
                    {
                        //s = Server.AddBooking(Convert.ToInt32(newData[1]), Convert.ToInt32(newData[2]), DateTime.Now, Convert.ToInt32(newData[3]), newData[4]);
                        returnString.Add(s);
                    }
                    catch
                    {
                        returnString.Add("Invalid arguments, Users requires : EmployeeID, UserID, Duration, Description");
                    }
                    break;
                case "invites":
                case "invite":
                    try
                    {
                        //s = Server.AddInvite(Convert.ToInt32(newData[1]), Convert.ToInt32(newData[2]), Convert.ToDateTime(newData[3]), Convert.ToInt32(newData[4]));
                        returnString.Add(s);
                    }
                    catch
                    {
                        returnString.Add("Invalid arguments, Users requires : BusinessID, UserID, Expirary, Uses");
                    }
                    break;
                case "mailbox":
                case "mail":
                    try
                    {
                        //s = Server.AddMail(Convert.ToInt32(newData[1]), Convert.ToInt32(newData[2]), newData[3]);
                        returnString.Add(s);
                    }
                    catch
                    {
                        returnString.Add("Invalid arguments, Users requires : UserID, EmployeeID, Message");
                    }
                    break;
            }
        }


    }
}
