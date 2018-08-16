using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookItDependancies
{
    class ServerCommunication
    {
        private static SqlConnection connection;
        private static bool connectionSet = false;
        private static Client client;

        //string defaultPass = "B%LVZA4£nMl#43No";
        #region Database primary commands
        /// <summary>
        /// Set's the connection string for the database connection
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="server">Server name</param>
        /// <param name="database">Connecting database</param>
        /// <param name="trusted">Trusted connection</param>
        /// <param name="timeout">Timeout (s)</param>
        public static void SetConnection(Client userClient, string username, string password, string server, string database, bool trusted = false, int timeout = 15)
        {
            string connectionString = "";
            connectionString += "user id=" + username + ";";
            connectionString += "pwd=" + password + ";";
            connectionString += "server=" + server + ";";
            if (trusted)
                connectionString += "Trusted_Connection=true;";
            connectionString += "Initial Catalog=" + database + ";";
            connectionString += "MultipleActiveResultSets=true;";
            connectionString += "connection timeout=" + timeout;

            connection = new SqlConnection(connectionString);
            connectionSet = true;
            client = userClient;
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

        #region Generic command routines
        private static List<List<string>> GeneralFetchQuery(string tableName, string columnRequested, string queryColumn, string dataQuery)
        {
            List<string> columnsReq = null;
            if (columnRequested == null)
                return GeneralFetchQuery(tableName, columnsReq, queryColumn, dataQuery);
            else
                return GeneralFetchQuery(tableName, new List<string>() { columnRequested }, queryColumn, dataQuery);
        }

        private static List<List<string>> GeneralFetchQuery(string tableName, List<string> columnsRequested, string queryColumn, string dataQuery)
        {
            string columns;
            if (columnsRequested == null)
                columns = "*";
            else
                columns = ColumnString(columnsRequested);

            //Set command string
            string commandString = "SELECT " + columns + " FROM " + tableName;
            if (queryColumn != null && dataQuery != null)
                commandString += " WHERE " + queryColumn + " = '" + dataQuery + "';";

            SqlDataReader reader = null;
            SqlCommand cmd = new SqlCommand(commandString, connection);
            List<List<string>> responses = new List<List<string>>();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                List<string> response = new List<string>();
                int n = 0;
                while (n < reader.FieldCount)
                {
                    response.Add(reader[n].ToString());
                    n++;
                }
                responses.Add(response);
            }
            return responses;
        }

        private static string GeneralUpdateNonQuery(string tableName, string identifyingID, List<string> columnsChange, List<string> newData)
        {
            string setString = "";
            if (columnsChange.Count() != newData.Count()) return "Failed to remove data, update data mismatch";
            for (int n = 0; n < columnsChange.Count(); n++)
            {
                setString += columnsChange[n] + " = '" + newData[n] + "'";
                if (n != columnsChange.Count() - 1)
                    setString += ",";
            }

            string commandString = "UPDATE " + tableName + "SET " + setString + " WHERE " + GetPrimaryKey(tableName) + " = " + identifyingID;
            SqlCommand cmd = new SqlCommand(commandString, connection);
            try
            {
                cmd.ExecuteNonQuery();
                return "Row " + identifyingID + " in " + tableName + " has been deleted successfully.";
            }
            catch { return "Failed to remove data"; }
        }

        private static string GeneralInsertNonQuery(string tableName, List<string> data)
        {
            List<string> columns = Validation.GetColumns(tableName);
            string columnString = ListToColumnBracket(columns);
            string dataString = ListToColumnBracket(data, false, true);
            string strCommand = "INSERT INTO " + tableName + columnString + " VALUES " + dataString + ";";
            SqlCommand cmd = new SqlCommand(strCommand, connection);
            try
            {
                cmd.ExecuteNonQuery();
                return "New entry added in " + tableName;
            }
            catch (Exception e) { return "Failed to add data, ERROR: " + e; }
        }
        #endregion

        #region Data fetch commands

        /// <summary>
        /// Returns all data found in a table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static List<List<string>> GetAllData(string tableName)
        {
            string nullable = null; //Used to remove routine ambiguity
            return GeneralFetchQuery(tableName, nullable, null, null);
        }

        /// <summary>
        /// Returns row data from the sepcificed primary identifier
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="queryID"></param>
        /// <param name="columnsRequested"></param>
        /// <returns></returns>
        public static List<string> GetRowFromID(string tableName, int queryID, List<string> columnsRequested)
        {
            return GeneralFetchQuery(tableName, columnsRequested, GetPrimaryKey(tableName), queryID.ToString())[0];
        }

        /// <summary>
        /// Return row information found with given query data
        /// </summary>
        /// <param name="tableName">Name of table</param>
        /// <param name="queryColumn">Column to query</param>
        /// <param name="queryString">Query to search</param>
        /// <param name="columnsRequested">Columns requested</param>
        /// <returns></returns>
        public static List<string> GetRowFromQuery(string tableName, string queryColumn, string queryString, List<string> columnsRequested)
        {
            return GeneralFetchQuery(tableName, columnsRequested, queryColumn, queryString)[0];
        }

        /// <summary>
        /// Return row information found with given query data for multiple rows
        /// </summary>
        /// <param name="tableName">Name of table</param>
        /// <param name="queryColumn">Column to query</param>
        /// <param name="queryString">Query to search</param>
        /// <param name="columnsRequested">Columns requested</param>
        /// <returns></returns>
        public static List<List<string>> GetRowsFromQuery(string tableName, string queryColumn, string queryString, List<string> columnsRequested)
        {
            return GeneralFetchQuery(tableName, columnsRequested, queryColumn, queryString);
        }

        /// <summary>
        /// Return the ID from given query data
        /// </summary>
        /// <param name="tableName">Name of table</param>
        /// <param name="queryColumn">Column to query</param>
        /// <param name="queryString">Query to search</param>
        /// <returns></returns>
        public static int GetIDFromQuery(string tableName, string queryColumn, string queryString)
        {
            return Convert.ToInt32(GeneralFetchQuery(tableName, GetPrimaryKey(tableName), queryColumn, queryString)[0]);
        }
        #endregion

        #region Data update commands
        /// <summary>
        /// Edit data for the requested column for a given row
        /// </summary>
        /// <param name="tableName">Name of table</param>
        /// <param name="queryID">ID of row to edit</param>
        /// <param name="columnChange">Column to be changed</param>
        /// <param name="newData">New data to place in requested column</param>
        /// <returns></returns>
        public static string EditRowFromID(string tableName, int queryID, string columnChange, string newData)
        {
            return EditRowFromID(tableName, queryID, new List<string>() { columnChange }, new List<string>() { newData });
        }

        /// <summary>
        /// Edit data for the requested columns for a given row
        /// </summary>
        /// <param name="tableName">Name of table</param>
        /// <param name="queryID">ID of row to edit</param>
        /// <param name="columnChange">Columns to be changed</param>
        /// <param name="newData">New data to place in requested columns</param>
        public static string EditRowFromID(string tableName, int queryID, List<string> columnsChange, List<string> newData)
        {
            return GeneralUpdateNonQuery(tableName, queryID.ToString(), columnsChange, newData);
        }
        #endregion

        #region Data Sorting Commands
        private static string ColumnString(List<string> columnList)
        {
            string columnString = "";
            foreach (string s in columnList)
                columnString += "[" + s + "],";
            return columnString.Remove(columnString.Length - 1);
        }

        /// <summary>
        /// Returns a string of columns formatted within brackets
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="ignorePrimary"></param>
        /// <returns></returns>
        private static string ListToColumnBracket(List<string> columns, bool ignorePrimary = true, bool includeApos = false)
        {
            string str = "(";
            int p = 0;
            if (ignorePrimary) p++;
            for (int n = p; n < columns.Count(); n++)
            {
                if (includeApos) str += "'";
                str += columns[n];
                if (includeApos) str += "'";
                if (n != columns.Count() - 1)
                    str += ", ";
            }
            return str + ")";
        }

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
        #endregion

        /// <summary>
        /// Checks whether connection has been established
        /// </summary>
        public static bool IsActive { get { return connectionSet; } }
    }

    public class Server
    {
        private static bool LoggedIn = false;
        private static Client client;
        private static string cryptoToken;

        #region Connection Setup
        public static bool ConnectToDatabase(string username, string password, string server, string database)
        {
            //First set connection to database
            try
            {
                ServerCommunication.SetConnection(client, username, password, server, database);
                //Now check if connection is valid
                ServerCommunication.Open();
                Validation.Intialise();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Log in to the application, verifies users credentials
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="stayLoggedIn"></param>
        /// <returns></returns>
        public static bool Login(string username, string password, bool stayLoggedIn = false)
        {
            int userID = GetUserID(username);
            if (userID == -1) return false;
            //ID has been found, now check if password matches
            List<string> userData = ServerCommunication.GetRowFromID("Users", userID, new List<string>() { "Password", "Salt", "Permission" });
            string pass = userData[0];
            string salt = SecurityManager.DecryptDatabaseData("Salt", userData[1]);
            if (SecurityManager.ValidatePassword(password, pass, salt))
            {
                LoggedIn = true;
                //Set client information
                SetClientData(userID, Convert.ToInt32(userData[2]));
            }
            else return false;

            if (stayLoggedIn)
            {
                SetNewCryptographicToken(salt);
            }
            return true;
        }

        /// <summary>
        /// Log user in with a provided cryptographic token, used for "keep me logged in" purposes
        /// </summary>
        /// <param name="CryptographicToken"></param>
        /// <returns></returns>
        public static bool CrypticTokenLogin(string username, string cryptographicToken)
        {
            int userID = GetUserID(username);
            if (userID == -1) return false;
            List<string> userData = ServerCommunication.GetRowFromID("Users", userID, new List<string>() { "Salt", "Permission", "CryptoToken" });
            string providedToken = SecurityManager.DecryptDatabaseData("CryptoToken", cryptographicToken);
            string encProvidedToken = SecurityManager.OneWayEncryptor(providedToken, userData[0]);
            if (encProvidedToken == userData[2])
            {
                LoggedIn = true;
                //Set client information
                SetClientData(userID, Convert.ToInt32(userData[1]));
                return true;
            }
            else return false;
        }

        private static void SetClientData(int userID, int permissionLevel)
        {
            List<string> requestedInformation = new List<string>() { "EmpoyeeID", "BusinessID", "PermissionLevel" };
            List<string> employeeInformation = ServerCommunication.GetRowFromQuery("Employees", "UserID", userID.ToString(), requestedInformation);
            if (employeeInformation != null)
            {
                int emplID = Convert.ToInt32(employeeInformation[0]);
                int busID = Convert.ToInt32(employeeInformation[1]);
                int busPerm = Convert.ToInt32(employeeInformation[2]);
                client = new Client(permissionLevel, userID, busID, emplID, busPerm);
            }
            else { client = new Client(permissionLevel, userID); }
        }

        private static int GetUserID(string username)
        {
            if (!ServerCommunication.IsActive || LoggedIn) return -1;
            try
            {
                return ServerCommunication.GetIDFromQuery("Users", "Username", SecurityManager.EncryptDatabaseData("Username", username));
            }
            catch { return -1; }
        }

        private static void SetNewCryptographicToken(string userSalt)
        {
            //Save token to clientside device
            string cryptographicToken = SecurityManager.GenerateCryptographicToken();
            cryptoToken = SecurityManager.EncryptDatabaseData("CryptoToken", cryptographicToken);
            client.CryptographicToken = cryptoToken;

            //Upload new cryptographic token to database
            string encryptedToken = SecurityManager.OneWayEncryptor(cryptographicToken, userSalt);
            ServerCommunication.EditRowFromID("Users", client.UserID, "CryptoToken", encryptedToken);
        }
        #endregion

        #region Fetch Commands

        #endregion
        #region Edit Commands

        #endregion
        #region Delete Commands

        #endregion

        #region Other Commands
        /// <summary>
        /// Returns whether command request is allowed or not
        /// </summary>
        /// <param name="tableName">Name of table</param>
        /// <param name="columnsToRequest">Columns that have been requested for view/edit</param>
        /// <param name="queryingID">Row identifier</param>
        /// <param name="view">Is request for view (false for edit)</param>
        /// <returns></returns>
        public bool IsRequestAllowed(string tableName, List<string> columnsToRequest, int queryingID, bool view = true)
        {
            bool isValidated;
            if (view)
                isValidated = SecurityManager.ValidateFetchRequest(tableName, columnsToRequest, queryingID, client.UserID, client.PermissionLevel, client.BusinessID, client.EmployeeID, client.BusinessPermission);
            else
                isValidated = SecurityManager.ValidateEditRequest(tableName, columnsToRequest, queryingID, client.UserID, client.PermissionLevel, client.BusinessID, client.EmployeeID, client.BusinessPermission);
            return isValidated;
        }

        /// <summary>
        /// Returns whether user has required global permission for request
        /// </summary>
        /// <param name="tableName">Name of table</param>
        /// <param name="columnsToRequest">Columns that have been requested for view/edit</param>
        /// <param name="view">Is request for view (false for edit)</param>
        /// <returns></returns>
        public bool HasGlobalPermission(string tableName, List<string> columnsToRequest, bool view = true)
        {
            bool isValidated;
            if (view)
                isValidated = Validation.HasViewingPermission(tableName, columnsToRequest, client.PermissionLevel);
            else
                isValidated = Validation.HasEditingPermission(tableName, columnsToRequest, client.PermissionLevel);
            return isValidated;
        }
        #endregion
        #region Useful functions

        #endregion
    }

    public class Client
    {
        protected int clientPermissionLevel;
        protected int clientUserID;

        protected int clientBusinessID = 0;
        protected int clientEmployeeID = 0;
        protected int clientEmployeePermissionLvl = 0;

        protected string cryptoToken = "";

        /// <summary>
        /// Define a new client, holds basic permissions information about the client
        /// </summary>
        /// <param name="permissionLevel"></param>
        /// <param name="id"></param>
        /// <param name="businessID"></param>
        /// <param name="employeeID"></param>
        /// <param name="businessPermissionLevel"></param>
        public Client(int permissionLevel, int id, int businessID = 0, int employeeID = 0, int businessPermissionLevel = 0)
        {
            clientPermissionLevel = permissionLevel;
            clientUserID = id;
            clientBusinessID = businessID;
            clientEmployeeID = employeeID;
            clientEmployeePermissionLvl = businessPermissionLevel;
        }

        /// <summary>Returns the permission level of the client</summary>
        public int PermissionLevel { get => clientPermissionLevel; }
        /// <summary>Returns the user ID of the client</summary>
        public int UserID { get => clientUserID; }
        /// <summary>Returns the business ID of the client</summary>
        public int BusinessID { get => clientBusinessID; }
        /// <summary>Returns the Employee ID of the client</summary>
        public int EmployeeID { get => clientEmployeeID; }
        /// <summary>Returns the business permission level of the client</summary>
        public int BusinessPermission { get => clientEmployeePermissionLvl; }
        /// <summary> Get/Set client cryptographic login token (should be encrypted)</summary>
        public string CryptographicToken { get => cryptoToken; set => cryptoToken = value; }
        /// <summary>Returns whether the user is connected to a business account</summary>
        public bool IsBusinessUser()
        {
            if (clientBusinessID == 0) return false; else return true;
        }
    }

}

