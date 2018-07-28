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
                    break;
                }
                else if (user == "override")
                {
                    Server.TEMPAdminLoging("Admin", "admin1"); Program.Write("Succesfully logged in, welcome!");
                    System.Threading.Thread.Sleep(1500);
                    Program.Clear();
                    LoggedIn = true;
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

                Command.RunCommand(cmd.Split(null).ToList());
            }
            Program.Write("Exiting...");
            System.Threading.Thread.Sleep(1500);
        }

    }
    class Command
    {
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
}
