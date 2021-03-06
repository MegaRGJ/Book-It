/// <summary>
    /// Handles program to server communications
    /// </summary>
    public class Server
    {
        //Client user information
        protected static int clientPermissionLevel;
        protected static int clientUserID;

        //Client employee information (0 if user is not an employee)
        protected static int clientBusinessID = 0;
        protected static int clientEmployeeID = 0;
        protected static int clientEmployeePermissionLvl = 0;
        //Business permission levels
        //1 - General  - View own employee data
        //2 - Elevated - View all employee data
        //3 - Business Admin - View and edit all employee data
        //4 - Owner    - View and edit all employee and business data

        /// <summary>
        /// Set the database connection, returns false if connection fails
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="server"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public static bool ConnectToDatabase(string username, string password, string server, string database)
        {
            //First set connection to database
            try
            {
                ServerCommunication.SetConnection(username, password, server, database);
                //Now check if connection is valid
                ServerCommunication.Open();
                Validation.Intialise();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Log in routine, used to verify identity of user
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">User's password</param>
        /// <returns></returns>
        public static bool Login(string username, string password)
        {
            if (!ServerCommunication.IsActive) return false; //Ensure there is an active connection to the database
            //Firstly find username in database
            int userID = ServerCommunication.GetIDFromData("Users", "Username", SecurityManager.EncryptDatabaseData("Username", username));
            if (userID == -1) return false;
            //Fetch encrypted password from database
            List<string> fetchedData = ServerCommunication.GetRowFromID(userID, "Users", new List<string>() { "Password", "Salt" });
            string pass = fetchedData[0];
            string salt = SecurityManager.DecryptDatabaseData("Salt", fetchedData[1]);
            //Check with user provided password
            string encPass1 = SecurityManager.OneWayEncryptor(password, salt);
            string encPass2 = SecurityManager.EncryptDatabaseData("Password", encPass1);
            if (encPass2 == pass)
            {
                string encPermissionString = ServerCommunication.GetRowFromID(userID, "Users", new List<string>() { "Permission" })[0]; //Get encrypted permission string
                string permissionString = SecurityManager.DecryptDatabaseData("PermissionLevel", encPermissionString);       //Decrypt to get permission string
                clientPermissionLevel = SecurityManager.GetPermissionLevel(permissionString);   //Get permission level

                clientUserID = userID;  //Save user ID to protected class int

                //Now need to get business information
                List<string> requestingColumns = new List<string>() { "EmployeeID", "BusinessID", "PermissionLevel" };
                List<string> encEmployeeData = ServerCommunication.GetDataFromData("Employees", requestingColumns, "UserID", clientUserID.ToString());
                if (encEmployeeData.Count() > 0) //If user is an employee
                {
                    List<string> employeeData = SecurityManager.DecryptDatabaseData(requestingColumns, encEmployeeData);
                    clientEmployeeID = Convert.ToInt32(employeeData[0]);
                    clientBusinessID = Convert.ToInt32(employeeData[1]);
                    clientEmployeePermissionLvl = Convert.ToInt32(employeeData[3]);
                }
                return true;
            }
            else
                return false;
        }
        #region Create new table entry commands
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
        public static string AddUser(string Name, string Address, string Postcode, string Email, string Phone, string Username, string Password, int PermissionLevel)
        {
            if (!ServerCommunication.IsActive) return "Connection not set"; //Ensure server communication is active
            if (clientPermissionLevel < 2)
                return null;
            //First encrypt user's new password
            string salt = SecurityManager.GenerateNewSALT();
            string encPass = SecurityManager.OneWayEncryptor(Password, salt);

            string permission = SecurityManager.GetPermissionString(PermissionLevel); //Get permission string

            //Then compile into list string
            List<string> columns = new List<string>() { "Name", "Address", "Postcode", "Email", "Phone", "Username", "Password", "LastLogin", "Salt", "Permisison" };
            List<string> newData = new List<string>() { Name, Address, Postcode, Email, Phone, Username, encPass, DateTime.Today.Date.ToString(), salt, permission }; //Place all data ready for encryption
            //Now Encrypt data in list
            List<string> encryptedData = SecurityManager.EncryptDatabaseData(columns, newData);                      //Encrypt all data
            return ServerCommunication.NewTableEntry("Users", encryptedData);       //Add data to table
        }
        /// <summary>
        /// Create new Business to table
        /// </summary>
        /// <param name="Name">Full name of user</param>
        /// <param name="Address">Number and Street name</param>
        /// <param name="Postcode">User's postcode</param>
        /// <param name="Phone">Contact phone number</param>
        /// <param name="Email">Email address</param>
        /// <param name="Description">Description of the business (Should include what the company offers, can include http links</param>
        /// <param name="active">Is the business active?</param>
        /// <param name="sharedBookings">Does the business have bookings split between all employees?</param>
        /// <returns></returns>
        public static string AddBusiness(string Name, string Address, string Postcode, string email, string phone, string Description, bool active, bool sharedBookings)
        {
            if (!ServerCommunication.IsActive) return "Connection not set"; //Ensure server communication is active
            if (clientPermissionLevel != 3) return null; //Only admins can add new business accounts
            List<string> columns = new List<string>() { "Name", "Address", "Postcode", "Email", "Phone", "Description ", "Active", "SharedBookings" };

            List<string> newData = new List<string>() { Name, Address, Postcode, email, phone, Description, active.ToString(), sharedBookings.ToString() };
            List<string> encryptedData = SecurityManager.EncryptDatabaseData(columns, newData);                      //Encrypt all data
            return ServerCommunication.NewTableEntry("Businesses", encryptedData);  //Add data to table
        }
        /// <summary>
        /// Create a new Employee entry to database
        /// </summary>
        /// <param name="businessID">Business ID of employee</param>
        /// <param name="userID">User ID of employee</param>
        /// <param name="permissionLevel">Permission level of new employee</param>
        /// <param name="availability">Availability of employee (see documentation for details)</param>
        /// <param name="ammendments">Ammendments to availability (see documentation for details) </param>
        /// <returns></returns>
        public static string AddEmployee(int businessID, int userID, string permissionLevel, List<string> availability, List<string> ammendments)
        {
            if (!ServerCommunication.IsActive) return "Connection not set"; //Ensure server communication is active
            if (clientPermissionLevel != 3 || (businessID == clientBusinessID && clientEmployeePermissionLvl == 4)) return null; //Only admins and business owners can create employee accounts
            List<string> newData = new List<string>() { businessID.ToString(), userID.ToString(), permissionLevel, FormatAvailability(availability), FormatAmmendments(ammendments) };
            List<string> columns = new List<string>() { "BusinessIDUserID", "PermissionLevel", "Availability", "Ammendments", };
            List<string> encryptedData = SecurityManager.EncryptDatabaseData(columns, newData); //Encrypt all data
            return ServerCommunication.NewTableEntry("Employees", encryptedData);   //Add data to table
        }
        /// <summary>
        /// Create a new booking in the database
        /// </summary>
        /// <param name="employeeID">ID of employee booked with</param>
        /// <param name="userID">User ID being booked in</param>
        /// <param name="bookingDateTime">Date Time of the booking</param>
        /// <param name="duration">Duration (in minutes) of the booking</param>
        /// <param name="description">Description (if required)</param>
        /// <param name="cancellation">Cancellation reason (booking is conisdered cancelled if this is NOT blank)</param>
        /// <returns></returns>
        public static string AddBooking(int employeeID, int userID, DateTime bookingDateTime, int duration, string description, string cancellation = "")
        {
            if (!ServerCommunication.IsActive) return "Connection not set"; //Ensure server communication is active
            if (userID != clientUserID && clientPermissionLevel < 2) return "Insufficient Permissions";
            string bookingTime = bookingDateTime.ToString("yyyMMddHHmm");

            List<string> columns = new List<string>() { "EmployeeID  ", "UserID", "DateTime", "Duration", "Description", "Cancellation", };
            List<string> newData = new List<string>() { employeeID.ToString(), userID.ToString(), bookingTime, duration.ToString(), description, cancellation };

            List<string> encryptedData = SecurityManager.EncryptDatabaseData(columns, newData); //Encrypt all data                
            return ServerCommunication.NewTableEntry("Employees", encryptedData);   //Add data to table
        }
        /// <summary>
        /// Creates a new mail entry in the database
        /// </summary>
        /// <param name="userID">ID of user sending mail</param>
        /// <param name="employeeID">ID of employee recipient</param>
        /// <param name="message">Message string</param>
        /// <returns></returns>
        public static string AddMail(int userID, int employeeID, string message)
        {
            if (!ServerCommunication.IsActive) return "Connection not set"; //Ensure server communication is active
            if (userID != clientUserID) return "Invalid request (you cannot send mail on behalf of another user)";
            string sentTime = DateTime.Now.ToString("yyyMMddHHmm");
            List<string> columns = new List<string>() { "UserID", "EmployeeID", "Message", "DateTime", };
            List<string> newData = new List<string>() { userID.ToString(), employeeID.ToString(), message, sentTime };

            List<string> encryptedData = SecurityManager.EncryptDatabaseData(columns, newData);  //Encrypt all data           
            return ServerCommunication.NewTableEntry("Employees", encryptedData);   //Add data to table
        }
        /// <summary>
        /// Create a new invite and add to database
        /// </summary>
        /// <param name="businessID">Business ID for sending invite</param>
        /// <param name="userID">Target user ('0' for any user)</param>
        /// <param name="Expirary">DateTime for expirary(Irrelivant if noExpirary == true)</param>
        /// <param name="Uses">Number of uses allowed ('-1' for unlimited)</param>
        /// <param name="noExpirary">True if invite does not expire</param>
        /// <returns></returns>
        public static string AddInvite(int businessID, int userID, DateTime Expirary, int Uses, bool noExpirary = false)
        {
            if (!ServerCommunication.IsActive) return "Connection not set"; //Ensure server communication is active
            if (clientBusinessID != businessID || clientPermissionLevel < 2) return "Insufficient Permissions";

            string expiraryTime = Expirary.ToString("yyyMMddHHmm");
            if (noExpirary) expiraryTime = "NONE";

            string inviteTime = DateTime.Now.ToString("yyyMMddHHmm"); //Current time of sending
            string inviteCode = GenerateInviteCode();

            List<string> columns = new List<string>() { "BusinessID", "UserID", "InviteCode", "DateTime", "Expires", "Uses", };
            List<string> newData = new List<string>() { businessID.ToString(), userID.ToString(), inviteCode, inviteTime, expiraryTime, Uses.ToString() };

            List<string> encryptedData = SecurityManager.EncryptDatabaseData(columns, newData);   //Encrypt all data
            return ServerCommunication.NewTableEntry("Employees", encryptedData);   //Add data to table
        }
        #endregion
        #region General Fetch||Edit||Delete Commands
        /// <summary>
        /// General fetch routine, grabs row data for required columns for given primary key ID
        /// </summary>
        /// <param name="tableName">Name of table in database</param>
        /// <param name="columnsRequested">Columns requested</param>
        /// <param name="queryID">Query ID (Primary key of tableName) to search for</param>
        /// <returns></returns>
        public static List<string> FetchData(string tableName, List<string> columnsRequested, int queryID)
        {
            if (!ServerCommunication.IsActive) return new List<string>() { "Connection not set" }; //Ensure server communication is active
            if (columnsRequested == null) columnsRequested = Validation.GetColumns(tableName);
            List<String> returnString = ServerCommunication.GetRowFromID(queryID, tableName, columnsRequested); //Gets required information from the database
            return SecurityManager.DecryptDatabaseData(columnsRequested, returnString);
        }

        /// <summary>
        /// General edit routine, edit row data for required columns for given primary key ID
        /// </summary>
        /// <param name="tableName">Name of table in database</param>
        /// <param name="columnsRequested">Columns requested to be changed</param>
        /// <param name="queryID">Query ID (Primary key of tableName) to search for</param>
        /// <param name="updatedData">Data to replace</param>
        /// <returns></returns>
        public static string EditData(string tableName, List<string> columnsRequested, int queryID, List<string> updatedData)
        {
            if (!ServerCommunication.IsActive) return "Connection not set"; //Ensure server communication is active
            if (!SecurityManager.ValidateEditRequest(tableName, columnsRequested, queryID, clientUserID, clientPermissionLevel, clientBusinessID, clientEmployeeID, clientEmployeePermissionLvl))
                return "Insufficient Permissions";
            //If user has requried permissions, then edit data
            List<string> encryptData = SecurityManager.EncryptDatabaseData(columnsRequested, updatedData);
            return ServerCommunication.GeneralUpdateNonQuery(tableName, queryID.ToString(), columnsRequested, encryptData);
        }

        /// <summary>
        /// Removes user from the database, must have admin privallages
        /// </summary>
        /// <param name="queryID">ID of row to be deleted</param>
        /// <returns></returns>
        public static string DeleteTableRow(string tableName, int queryID)
        {
            //Ensure that server communication is active (i.e. user is logged in) AND User has required permissions to delete a row
            if (!ServerCommunication.IsActive || clientPermissionLevel < 3 || !ValidateTableName(tableName))
                return null;
            return ServerCommunication.DeleteRow(tableName, queryID.ToString());
        }
        #endregion

        #region Internal routines
        /// <summary>
        /// Returns the logged in user's permission level
        /// </summary>
        /// <returns></returns>
        public static int GetUserPermissionLevel() { return clientPermissionLevel; }
        /// <summary>
        /// Converts bool array to int array (Used for column numbering)
        /// </summary>
        /// <param name="boolArray"></param>
        /// <returns></returns>
        private static int[] BoolToIntArray(bool[] boolArray)
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
        private static bool ParamaterCheck(string tableName, List<string> columns)
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
            else return false;
            return true;
        }

        /// <summary>
        /// Returns whether the table exists or not
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private static bool ValidateTableName(string tableName)
        {
            List<string> tables = ServerCommunivecation.GetTables();
            if (tables.Contains(tableName))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region DataHandling routines
        //TODO
        private static string FormatAvailability(List<string> availability) { return null; }
        private static string FormatAmmendments(List<string> ammendments) { return null; }

        private static List<string> GetAvailability(string availability) { return null; }
        private static List<string> GetAmmendments(string ammendments) { return null; }

        private static string GenerateInviteCode()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHJKLMNOPQRSTUVWXYabcdefghijkmnopqrstuvwxy0123456789"; //Note the following characters have been omitted : 'I' 'Z' 'l' 'z' (for convienience)
            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #endregion
    }