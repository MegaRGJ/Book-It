using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookItDependancies
{
    public class ServerCommunication
    {
        private static SqlConnection connection;
        private static bool connectionSet = false;

        //string defaultPass = "B%LVZA4£nMl#43No";
        #region General Database commands
        /// <summary>
        /// Set's the connection string for the database connection
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="server">Server name</param>
        /// <param name="database">Connecting database</param>
        /// <param name="trusted">Trusted connection</param>
        /// <param name="timeout">Timeout (s)</param>
        public static void SetConnection(string username, string password, string server, string database, bool trusted = false, int timeout = 15)
        {
            string connectionString = "";
            connectionString += "user id=" + username + ";";
            connectionString += "pwd=" + password + ";";
            connectionString += "server=" + server + ";";
            if (trusted)
                connectionString += "Trusted_Connection=true;";
            connectionString += "Initial Catalog=" + database + ";";
            connectionString += "connection timeout=" + timeout;

            connection = new SqlConnection(connectionString);
            connectionSet = true;
        }

        /// <summary>
        /// Generic fetch query for data from given information
        /// </summary>
        /// <param name="tableName">Name of table in database</param>
        /// <param name="columnsGrab">Names of columns to fetch data from</param>
        /// <param name="columnCheck">Name of column to check</param>
        /// <param name="dataQuery">Data query to check for</param>
        /// <returns></returns>
        private static List<string> GeneralFetchQuery(string tableName, List<string> columnsGrab, string columnCheck, string dataQuery)
        {
            List<string> cmdResponse = new List<string>();
            string commandString = "SELECT " + ListToColumnString(columnsGrab) + " FROM " + tableName + " WHERE " + columnCheck + " = " + dataQuery;

            SqlDataReader reader = null;
            SqlCommand cmd = new SqlCommand(commandString, connection);

            reader = cmd.ExecuteReader();
            while (reader.Read())
                foreach (string s in columnsGrab)
                    cmdResponse.Add(reader[s].ToString());

            return cmdResponse;
        }

        /// <summary>
        /// Sends an update query to overwrite data as specified
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="rowID"></param>
        /// <param name="columnsChange"></param>
        /// <param name="newData"></param>
        /// <returns></returns>
        private static string GeneralUpdateNonQuery(string tableName, string rowID, List<string> columnsChange, List<string> newData)
        {
            //First get SET String (Tells the database which columns to update, and with what data)
            string setString = "";
            if (columnsChange.Count() != newData.Count()) return "Failed to remove data, update data mismatch";
            for (int n = 0; n < columnsChange.Count(); n++)
            {
                setString += columnsChange[n] + " = '" + newData[n] + "'";
                if (n != columnsChange.Count() - 1)
                    setString += ",";
            }

            string strCommand = "UPDATE " + tableName + "SET " + setString + " WHERE " + GetPrimaryKey(tableName) + " = " + rowID;
            SqlCommand cmd = new SqlCommand(strCommand, connection);
            try
            {
                cmd.ExecuteNonQuery();
                return "Row " + rowID + " in " + tableName + " has been deleted successfully.";
            }
            catch { return "Failed to remove data"; }
        }
               

        /// <summary>
        /// Creates a new data entry in the provided table
        /// </summary>
        /// <param name="tableName">Name of table in database</param>
        /// <param name="data">Data to be added</param>
        /// <returns></returns>
        public static string NewTableEntry(string tableName, List<String> data)
        {
            List<string> columns = GetColumns(tableName);
            string columnString = ListToColumnBracket(columns);
            string strCommand = "INSERT INTO " + tableName + "(" + columnString;
            SqlCommand cmd = new SqlCommand(strCommand, connection);
            try
            {
                cmd.ExecuteNonQuery();
                return "New entry added in " + tableName;
            }
            catch { return "Failed to add data"; }
        }

        /// <summary>
        /// Remove a specific row in the provided table
        /// </summary>
        /// <param name="tableName">Name of table in database</param>
        /// <param name="rowID">Row id value, primary key in table</param>
        /// <returns></returns>
        public static string DeleteRow(string tableName, string rowID)
        {
            string strCommand = "DELETE FROM " + tableName + " WHERE " + GetPrimaryKey(tableName) + " = " + rowID;
            SqlCommand cmd = new SqlCommand(strCommand, connection);
            try
            {
                cmd.ExecuteNonQuery();
                return "Row " + rowID + " in " + tableName + " has been deleted successfully." ;
            }
            catch { return "Failed to remove data"; }
        }

        

        /// <summary>
        /// Opens the database connection
        /// </summary>
        public static void Open()
        {
            if (connectionSet)
                connection.Open();
        }

        /// <summary>
        /// Closes the database connection
        /// </summary>
        public static void Close()
        {
            connection.Close();
        }

        #endregion
        #region SpecialisedCommands
        /// <summary>
        /// Fetches the data ID from the table given a value found in a given column
        /// </summary>
        /// <param name="tableName">Name of table in database</param>
        /// <param name="columnName">Name of column to check</param>
        /// <param name="dataQuery">Search data in column</param>
        /// <returns></returns>
        public static int GetIDFromData(string tableName, string columnName, string dataQuery)
        {
            List<string> queryResponse = GeneralFetchQuery(tableName, new List<string>() { GetPrimaryKey(tableName) }, columnName, dataQuery);
            return Convert.ToInt32(queryResponse[0]);
        }
        #endregion
        /// <summary>
        /// Gets data from the database for a given table and given columns
        /// </summary>
        /// <param name="ID">The primary identifier</param>
        /// <param name="tableName">Name of table in database</param>
        /// <param name="columnsRequired">List of columns required</param>
        /// <returns></returns>
        public static List<String> GetRowFromID(int ID, string tableName, List<String> columnsRequired)
        {
            return GeneralFetchQuery(tableName, columnsRequired, GetPrimaryKey(tableName), ID.ToString());
        }

        /// <summary>
        /// Returns all table names in the database
        /// </summary>
        /// <returns></returns>
        public static List<string> GetTables()
        {
            List<string> tables = new List<string>();
            DataTable dt = connection.GetSchema("Tables");
            foreach (DataRow r in dt.Rows)
                tables.Add(r[2].ToString());
            return tables;
        }

        /// <summary>
        /// Returns the titles of all column headers in the table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static List<string> GetColumns(string tableName)
        {
            string strCommand = "SELECT * FROM " + tableName;
            SqlDataReader reader = null;
            SqlCommand cmd = new SqlCommand(strCommand, connection);
            List<string> columns = new List<string>();
            //string[] columns = new string[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++)
                columns.Add(reader.GetName(i));
            return columns;
        }

        /// <summary>
        /// Checks whether connection has been established
        /// </summary>
        public static bool IsActive { get { return connectionSet; } }

        #region Private routines       

        /// <summary>
        /// Returns the name of the primary key column for the given table
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static string GetPrimaryKey(string table)
        {
            switch (table.ToLower())
            {
                case "users":
                    return "UserID";
                case "businesses":
                    return "BusinessID";
                case "employees":
                    return "EmployeeID";
                case "bookings":
                    return "BookingID";
                case "invites":
                    return "InviteID";
                case "mailbox":
                    return "MailID";
            }
            return null;
        }

        /// <summary>
        /// Converts column list to a single string for SQL query
        /// </summary>
        /// <param name="columns">Column names</param>
        /// <returns></returns>
        private static string ListToColumnString(List<string> columns)
        {
            string columnString = "";
            foreach (string s in columns)
                columnString = "[" + s + "]";
            return columnString.Remove(columnString.Length - 1);
        }

        /// <summary>
        /// Returns a string of columns formatted within brackets
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="ignorePrimary"></param>
        /// <returns></returns>
        private static string ListToColumnBracket(List<string> columns, bool ignorePrimary = true)
        {
            string str = "(";
            for (int n = 0; n < columns.Count(); n++)
            {
                if (n == 0 && !ignorePrimary) str += columns[0] + ", ";
                else str += columns[n] + ", ";

            }
            return str.Remove(str.Length - 1);
        }
        #endregion

    }

    /// <summary>
    /// Handles program to server communications
    /// </summary>
    public class Server
    {
        //Requried routines
        //User Login
        //GET User information -> input userID and which columns are required, get back required information

        #region Main Commands
        /// <summary>
        /// Log in routine, used to verify identity of user
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">User's password</param>
        /// <returns></returns>
        public bool Login(string username, string password)
        {
            if (!ServerCommunication.IsActive) return false; //Ensure there is an active connection to the database
            //Firstly find username in database
            int userID = ServerCommunication.GetIDFromData("Users", "Username", username);
            //Fetch encrypted password from database
            List<string> fetchedData = ServerCommunication.GetRowFromID(userID, "Users", new List<string>() { "Password", "Salt" });
            string pass = fetchedData[0];
            string salt = fetchedData[1];
            //Check with user provided password
            string encPass = SecurityManager.OneWayEncryptor(password, salt);
            if (encPass == pass)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Get a users details from their user ID
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="details">Which details are required</param>
        /// <returns></returns>
        public List<String> GetUser(int userID, List<String> details)
        {
            //Ensure that server communication is active (i.e. user is logged in), Also ensure columns are valid names
            if (!ServerCommunication.IsActive || !ParamaterCheck("Users", details))
                return null;

            List<String> returnString = ServerCommunication.GetRowFromID(userID, "Users", details); //Gets required information from the database
            return DecryptList(returnString);
        }

        /// <summary>
        /// Add a new user to the Users table
        /// </summary>
        /// <param name="Name">Full name of user</param>
        /// <param name="Address">Number and Street name</param>
        /// <param name="Postcode">User's postcode</param>
        /// <param name="Email">Email address</param>
        /// <param name="Phone">Contact phone number</param>
        /// <param name="Username">Username</param>
        /// <param name="Password">Password</param>
        /// <returns></returns>
        public string AddUser(string Name, string Address, string Postcode, string Email, string Phone, string Username, string Password)
        {
            //First encrypt user's new password
            string salt = SecurityManager.GenerateNewSALT();
            string encPass = SecurityManager.OneWayEncryptor(Password, salt);
            //Then compile into list string
            List<string> newData = new List<string>() { Name, Address, Postcode, Email, Phone, Username, DateTime.Today.Date.ToString(), salt };
            //Now Encrypt data in list
            List<string> encryptedData = EncryptList(newData);
            return ServerCommunication.NewTableEntry("Users", encryptedData);
        }
        #endregion        
        #region Internal routines
        /// <summary>
        /// Converts bool array to int array (Used for column numbering)
        /// </summary>
        /// <param name="boolArray"></param>
        /// <returns></returns>
        private int[] BoolToIntArray(bool[] boolArray)
        {
            int[] intArray = new int[boolArray.Length];
            int counter = 0;
            for (int n = 0; n < boolArray.Length; n++)
            {
                if (boolArray[n] == true) { intArray[counter] = n; counter++; }
            }
            Array.Resize(ref intArray, counter + 1);
            return intArray;
        }

        /// <summary>
        /// Checks that all given columns exist in table
        /// </summary>
        /// <param name="tableName">Name of table in database</param>
        /// <param name="columns">List of column names searched</param>
        /// <returns></returns>
        private bool ParamaterCheck(string tableName, List<string> columns)
        {
            //First check if table exists
            List<string> tables = ServerCommunication.GetTables();
            if (tables.Contains(tableName))
            {
                //Now check if all columns are valid
                List<string> columnNames = ServerCommunication.GetColumns(tableName);
                foreach (string col in columns)
                    if (!columnNames.Contains(col)) //If column is not found then routine fails (returns false)
                        return false;
            }
            return true;
        }

        private List<String> DecryptList(List<String> stringList)
        {
            List<String> Decrypted = new List<String>();
            foreach (string s in stringList)
            {
                Decrypted.Add(SecurityManager.DecryptSK(s));
            }
            return Decrypted;
        }

        private List<String> EncryptList(List<String> stringList)
        {
            List<String> Encrypted = new List<String>();
            foreach (string s in stringList)
            {
                Encrypted.Add(SecurityManager.EncryptSK(s));
            }
            return Encrypted;
        }
        #endregion
    }
}
