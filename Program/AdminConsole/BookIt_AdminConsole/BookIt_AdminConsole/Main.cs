using BookItDependancies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookIt_AdminConsole
{
    class Main
    {
        private List<string> appInfo = new List<string>() { "Book It Application Admin Console.", "Version " + "0.0.1", "Type command below, 'help' to see available commands" };
        CommandHandler handler = new CommandHandler();
        /// <summary>
        /// Initalises the application, attempts to connect to the database
        /// </summary>
        public void InitiateAdminConsole()
        {
            int connectionAttempts = 1;

            while (connectionAttempts < 4)
            {
                if (connectionAttempts > 1)
                    Program.Write("Establishing server connection..." + " Attempt : " + connectionAttempts);
                else Program.Write("Establishing server connection...");

                bool connect = Server.ConnectToDatabase("SA", "TSTboKK4A", "127.0.0.1,14333", "BOOKIT");
                if (connect)
                {
                    Program.Write("Connection successful");
                    System.Threading.Thread.Sleep(800);
                    Program.Clear();
                    LogInUser();            //Log user into the console program
                    connectionAttempts = 5; //Ensures program exits when user closes program
                }
                else
                {
                    connectionAttempts += 1;
                    Program.Write("Failed to connect to server...");
                    Program.Write("Retrying in...");
                    int waitTime = 3 * (connectionAttempts - 1) + 1;
                    for (int n = waitTime; n > 0; n--)
                    {
                        Program.Write(n.ToString());
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
            if (connectionAttempts == 4)
            {
                Program.Write("Unable to establish server connection, please try again later");
                System.Threading.Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// Log user into the program (checks against database information)
        /// </summary>
        public void LogInUser()
        {
            bool LoggedIn = false;
            int attempts = 1;
            Program.Write("Please log in...");
            System.Threading.Thread.Sleep(250);
            while (attempts < 4)
            {
                attempts += 1;
                Program.Write("Username:");
                string user = Program.Read();
                Program.Write("Password:");
                string pass = Program.ReadPassword();

                if (Server.Login(user, pass))
                {
                    if (Server.GetUserPermissionLevel() == 3)
                    {
                        Program.Write("Succesfully logged in, welcome!");
                        System.Threading.Thread.Sleep(1500);
                        Program.Clear();
                        LoggedIn = true;
                        break;
                    }
                    Program.Write("Insufficient permission for user");
                    System.Threading.Thread.Sleep(1000);
                    break;
                }
                else
                {
                    if (attempts != 4)
                        Program.Write("Invalid log in information, try again");
                    else
                        Program.Write("Invalid log in information...");
                }
            }
            if (LoggedIn)
                Console();
            else
            {
                Program.Write("Exiting...");
                System.Threading.Thread.Sleep(2000);
            }

        }

        /// <summary>
        /// Main console code
        /// </summary>
        public void Console()
        {
            //DateTime lastCommand = DateTime.Now;   //TODO prevent fast requests
            Program.WriteLines(appInfo);
            while (true)
            {
                string cmd = Program.Read();
                if (cmd == "exit" || cmd == "quit") break;

                List<string> commandString = cmd.Split(null).ToList();
                Program.WriteLines(handler.Run(commandString));
            }
            Program.Write("Exiting...");
            System.Threading.Thread.Sleep(1500);
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
        /// <summary>Run the command</summary>        
        public static void RunCommand(List<string> command)
        {

            switch (command[0].ToLower())
            {
                case "fetch":
                    int amount = command.Count();
                    string table = command[1];
                    List<string> columns = new List<string>();
                    for (int n = 2; n < amount - 1; n++)
                        columns.Add(command[n]);
                    string queryID = command[command.Count - 1];

                    Server.FetchData(table, columns, Convert.ToInt32(queryID));
                    break;
                case "new":
                    Program.Write(Server.AddUser("Thorn", "ADMIN", "AD158IN", "admin@admin.com", "00000000000", "Thorn", "GrainFuseQuark4", 3)); break;
                case "delete":
                    string id = command[1];
                    Program.Write(Server.DeleteTableRow("Users", Convert.ToInt32(id)));
                    break;
                case "test":
                    //Program.Write(Server.FetchData("Users", new List<string>() { "UserID", "Name", "Address" }, 1)[0]);
                    Program.Write(Server.FetchData("Users", null, 1)[0]);
                    break;
                case "encrypt":
                    string enc = command[1];
                    Program.Write(SecurityManager.EncryptDatabaseData("a", enc));
                    break;
                case "decrypt":
                    string val = command[1];
                    Program.Write(SecurityManager.DecryptDatabaseData("a", val));
                    break;
            }
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
                new Command("New", NewEntry, "Creates a new entry to the specified table. Usage: 'New <table> <new data>'", 2, "Create"),
                new Command("Fetch", FetchData, "Returns data from the database by primary key. Usage : Fetch <table> <key>", 2, "Create")
            };

        }

        private void Help()
        {
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
                        s = Server.AddUser(newData[1], newData[2], newData[3], newData[4], newData[5], newData[6], newData[7], Convert.ToInt32(newData[8]));
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
                        s = Server.AddEmployee(Convert.ToInt32(newData[1]), Convert.ToInt32(newData[2]), newData[3], availability, ammendments);
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
                        s = Server.AddBusiness(newData[1], newData[2], newData[3], newData[4], newData[5], newData[6], Convert.ToBoolean(newData[7]), Convert.ToBoolean(newData[8]));
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
                        s = Server.AddBooking(Convert.ToInt32(newData[1]), Convert.ToInt32(newData[2]), DateTime.Now, Convert.ToInt32(newData[3]), newData[4]);
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
                        s = Server.AddInvite(Convert.ToInt32(newData[1]), Convert.ToInt32(newData[2]), Convert.ToDateTime(newData[3]), Convert.ToInt32(newData[4]));
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
                        s = Server.AddMail(Convert.ToInt32(newData[1]), Convert.ToInt32(newData[2]), newData[3]);
                        returnString.Add(s);
                    }
                    catch
                    {
                        returnString.Add("Invalid arguments, Users requires : UserID, EmployeeID, Message");
                    }
                    break;
            }
        }

        private void FetchData(List<string> request)
        {
            List<string> fetchData = Server.FetchData(request[0], null, Convert.ToInt32(request[1]));
            foreach (string s in fetchData)
                returnString.Add(s);
        }


    }
}
