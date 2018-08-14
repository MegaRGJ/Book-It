class ServerCommunication
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
            connectionString += "MultipleActiveResultSets=true;";
            connectionString += "connection timeout=" + timeout;

            connection = new SqlConnection(connectionString);
            connectionSet = true;
        }

        /// <summary>
        /// Generic fetch query for data from given information
        /// </summary>
        /// <param name="tableName">Name of table in database</param>
        /// <param name="columnsRequested">Names of columns to fetch data from (null for all columns)</param>
        /// <param name="columnCheck">Name of column to check</param>
        /// <param name="dataQuery">Data query to check for</param>
        /// <returns></returns>
        private static List<string> GeneralFetchQuery(string tableName, List<string> columnsRequested, string columnCheck, string dataQuery)
        {
            List<string> cmdResponse = new List<string>();
            string columns;
            if (columnsRequested == null) //Will select all columns if non are specified
                columns = "*";
            else
                columns = ListToColumnString(columnsRequested);

            string commandString = "SELECT " + columns + " FROM " + tableName + " WHERE " + columnCheck + " = '" + dataQuery + "';";

            SqlDataReader reader = null;
            SqlCommand cmd = new SqlCommand(commandString, connection);

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int n = 0;
                while (n < reader.FieldCount)
                {
                    cmdResponse.Add(reader[n].ToString());
                    n++;
                }
            }
            return cmdResponse;
        }
        /// <summary>
        /// Generic fetch query for data from given information for multiple row returns
        /// </summary>
        /// <param name="tableName">Name of table in database</param>
        /// <param name="columnsRequested">Names of columns to fetch data from (null for all columns)</param>
        /// <param name="columnCheck">Name of column to check</param>
        /// <param name="dataQuery">Data query to check for</param>
        /// <returns></returns>
        private static List<List<string>> GeneralMultiFetchQuery(string tableName, List<string> columnsRequested, string columnCheck, string dataQuery)
        {
            List<List<string>> cmdResponse = new List<List<string>>();
            string columns;
            if (columnsRequested == null) //Will select all columns if non are specified
                columns = "*";
            else
                columns = ListToColumnString(columnsRequested);
            string commandString = "SELECT " + columns + " FROM " + tableName;
            if (columnCheck != null && dataQuery != null)
                commandString += " WHERE " + columnCheck + " = '" + dataQuery + "';";

            SqlDataReader reader = null;
            SqlCommand cmd = new SqlCommand(commandString, connection);

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                List<string> row = new List<string>();
                int n = 0;
                while (n < reader.FieldCount)
                {
                    row.Add(reader[n].ToString());
                    n++;
                }
                cmdResponse.Add(row);
            }
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
        public static string GeneralUpdateNonQuery(string tableName, string rowID, List<string> columnsChange, List<string> newData)
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

            string commandString = "UPDATE " + tableName + "SET " + setString + " WHERE " + GetPrimaryKey(tableName) + " = " + rowID;
            SqlCommand cmd = new SqlCommand(commandString, connection);
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
                return "Row " + rowID + " in " + tableName + " has been deleted successfully.";
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
        public static List<List<string>> GetAllData(string tableName)
        {
            return GeneralMultiFetchQuery(tableName, null, "", "");
        }

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
            if (queryResponse != null && queryResponse.Count() != 0)
                return Convert.ToInt32(queryResponse[0]);
            else return -1;
        }

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
        /// Gets data from the database for a given table and given column
        /// </summary>
        /// <param name="ID">The primary identifier</param>
        /// <param name="tableName">Name of table in database</param>
        /// <param name="columnsRequired">List of columns required</param>
        /// <returns></returns>
        public static string GetRowFromID(int ID, string tableName, string columnsRequired)
        {
            return GeneralFetchQuery(tableName, new List<string>() { columnsRequired }, GetPrimaryKey(tableName), ID.ToString())[0];
        }

        /// <summary>
        /// Get Row data where column data is found to match
        /// </summary>
        /// <param name="tableName">Name of table in database</param>
        /// <param name="columnsRequested">Names of columns that have been requested</param>
        /// <param name="columnName">Column name to check</param>
        /// <param name="dataQuery">Data to check for</param>
        /// <returns></returns>
        public static List<String> GetDataFromData(string tableName, List<string> columnsRequested, string columnName, string dataQuery)
        {
            return GeneralFetchQuery(tableName, columnsRequested, columnName, dataQuery);
        }

        /// <summary>
        /// Get Row data where column data is found to match
        /// </summary>
        /// <param name="tableName">Name of table in database</param>
        /// <param name="columnsRequested">Names of column that is requested</param>
        /// <param name="columnName">Column name to check</param>
        /// <param name="dataQuery">Data to check for</param>
        /// <returns></returns>
        public static string GetDataFromData(string tableName, string columnRequested, string columnName, string dataQuery)
        {
            return GeneralFetchQuery(tableName, new List<string>() { columnRequested }, columnName, dataQuery)[0];
        }
        #endregion
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
            string strCommand = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tableName + "'";
            SqlDataReader reader = null;
            SqlCommand cmd = new SqlCommand(strCommand, connection);
            List<string> columns = new List<string>();
            //string[] columns = new string[reader.FieldCount];
            reader = cmd.ExecuteReader();
            while (reader.Read()) columns.Add(reader.GetString(0));
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
        #endregion
    }