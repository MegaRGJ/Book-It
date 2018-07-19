using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookItDependancies
{
    public class ServerCommunication {
        private static SqlConnection connection;
        private static bool connectionSet = false;

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

        public static List<String> GetRowFromID(int ID, List<String> columnsRequired) {
            List<String> cmdResponse = new List<string>();
            if (columnsRequired.Count() == 0) return null; //Ensure atleast one column is being 'asked for'

            string commandString = "SELECT " + ListToColumnString(columnsRequired) + " FROM Users Where UserID = "+ID;
            
            SqlDataReader reader = null;
            SqlCommand cmd = new SqlCommand(commandString, connection);

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                foreach (string s in columnsRequired)
                    cmdResponse.Add(reader[s].ToString());
            }

            return cmdResponse;
        }

        /// <summary>
        /// Checks whether connection has been established
        /// </summary>
        public static bool IsActive { get { return connectionSet; } }

        private static string ListToColumnString(List<String> columns)
        {
            string columnString = "";
            foreach (string s in columns)
                columnString = "[" + s + "]";
            return columnString.Remove(columnString.Length - 1);
        }
        
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
        /// Get a users details from their user ID
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="details">Which details are required</param>
        /// <returns></returns>
        public List<String> GetUser(int userID, List<String> details) {      
            //Ensure that server communication is active (i.e. user is logged in)
            if (!ServerCommunication.IsActive)
                return null;
            List<String> returnString = ServerCommunication.GetRowFromID(userID, details); //Gets required information from the database
            return returnString;
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
            for (int n = 0; n < boolArray.Length; n++) {
                if(boolArray[n] == true) { intArray[counter] = n; counter++; }
            }
            Array.Resize(ref intArray, counter + 1);
            return intArray;
        }        
        #endregion
    }
}
