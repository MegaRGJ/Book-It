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
        public string AddUser(string Name, string Address, string Postcode, string Email, string Phone, string Username, string Password, int PermissionLevel)
        {
            if (clientPermissionLevel < 2)
                return null;
            //First encrypt user's new password
            string salt = SecurityManager.GenerateNewSALT();
            string encPass = SecurityManager.OneWayEncryptor(Password, salt);

            string permission = SecurityManager.GetPermissionString(PermissionLevel); //Get permission string

            //Then compile into list string
            List<string> newData = new List<string>() { Name, Address, Postcode, Email, Phone, Username, DateTime.Today.Date.ToString(), salt, permission }; //Place all data ready for encryption
            //Now Encrypt data in list
            List<string> encryptedData = EncryptList(newData);                      //Encrypt all data
            return ServerCommunication.NewTableEntry("Users", encryptedData);       //Add data to table
        }
        /// <summary>
        /// Create a new booking
        /// </summary>
        /// <param name="employeeID">Booking EmployeeID (Owner employeeID if sharedBookings is selected)</param>
        /// <param name="time">DateTime for when booking is set</param>
        /// <param name="duration">Duration of booking (in minutes)</param>
        /// <param name="desc">Description of booking being made</param>
        /// <param name="userID">For users with higher permission, userID can be set</param>
        /// <returns></returns>
        public string CreateBooking(int employeeID, DateTime time, int duration, string desc, int userID = -1)
        {
            if (userID != -1 && clientPermissionLevel < 2)      //Prohibts a user from creating a booking for another user, unless they have the requried permission
                if (userID == -1) userID = clientUserID;        //Sets the userID to the client's ID 
            string timeString = time.ToString("yyyMMddHHmm");   //Convert date to YYYY/MM/DD HH:MM format

            List<string> newData = new List<string>() { employeeID.ToString(), userID.ToString(), timeString, duration.ToString(), desc, "false" };
            List<string> encryptedData = EncryptList(newData);  //Encrypts data before adding to database

            return ServerCommunication.NewTableEntry("Bookings", encryptedData);
        }
        #endregion
        #region Get || Edit || Delete - Row commands
        /// <summary>
        /// Get a users details from their user ID
        /// </summary>
        /// <param name="queryID">User ID</param>
        /// <param name="details">Which details are required</param>
        /// <returns></returns>
        public List<String> GetTableRow(string tableName, int queryID, List<String> details)
        {
            //Ensure that server communication is active (i.e. user is logged in), Also ensure columns are valid names
            if (!ServerCommunication.IsActive || !ParamaterCheck(tableName, details))
                return null;
            //Check user has permission to view data
            if (!ValidateGrabRequest(tableName, queryID))
                return null;

            List<String> returnString = ServerCommunication.GetRowFromID(queryID, tableName, details); //Gets required information from the database
            return DecryptList(returnString);
        }

        /// <summary>
        /// Edits user data in the table
        /// </summary> 
        /// <param name="columnsChanged">Names of columns to update</param>
        /// <param name="newDetails">New details, must follow order of columnsChanged</param>
        /// <param name="userID">User ID of row to change (Leave unchanged for client ID)</param>
        /// <returns></returns>
        public string EditTableRow(string tableName, List<string> columnsChanged, List<string> newDetails, int queryID)
        {
            //Ensure that server communication is active (i.e. user is logged in) AND validate column names to change
            if (!ServerCommunication.IsActive || ParamaterCheck(tableName, columnsChanged))
                return null;
            //Check user has permission to edit data


            return ServerCommunication.GeneralUpdateNonQuery(tableName, queryID.ToString(), columnsChanged, newDetails);
        }

        /// <summary>
        /// Removes user from the database, must have admin privallages
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string DeleteTableRow(string tableName, int queryID)
        {
            //Ensure that server communication is active (i.e. user is logged in) AND User has required permissions to delete a row
            if (!ServerCommunication.IsActive || clientPermissionLevel < 3 || !ValidateTableName(tableName))
                return null;
            return ServerCommunication.DeleteRow(tableName, queryID.ToString());
        }
		
		 /// <summary>
        /// Returns if the user has permission to view the requested data
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="queryID"></param>
        /// <returns></returns>
        private bool ValidateGrabRequest(string tableName, int queryID)
        {
            switch (tableName)
            {
                case "Users":
                    if (queryID == clientUserID || clientPermissionLevel >= 2) return true; //Users can only grab their own user data (Unless have moderator pemission[2])
                    break;
                case "Businesses":
                    return true; //Should be able to view all business data irrelevant of permission level                     
                case "Employeees":
                    if (clientPermissionLevel >= 2) return true;   // Viewable by moderators
                    if (queryID == clientEmployeeID) return true;  // Employees can view there own data
                    //Now check if business ID is the same, if user has business permission 2+ they can view all employee data for their business
                    string businessID = ServerCommunication.GetDataFromData("Employees", "BusinessID", "EmployeeID", SecurityManager.EncryptSK(queryID.ToString()));
                    if (SecurityManager.DecryptSK(businessID) == clientBusinessID.ToString() && clientEmployeePermissionLvl >= 2)
                        return true;
                    break;
                case "Bookings":
                    if (clientPermissionLevel >= 2) return true;  // Viewable by moderators
                    List<string> encDataIDs = ServerCommunication.GetDataFromData("Bookings", new List<string>() { "EmployeeID", "UserID" }, "BookingID", SecurityManager.EncryptSK(queryID.ToString()));
                    List<string> dataIDs = DecryptList(encDataIDs);
                    //string employeeID = dataIDs[0];                    
                    if (dataIDs[1] == clientUserID.ToString())       //Client can see their own bookings
                        return true;
                    //TODO - Fix for shared bookings and check for other employees of same businesss                
                    break;
                case "MailBox":
                    if (clientPermissionLevel >= 3) return true;    //Viewable by admins (moderators can not view all mail for privacy reasons)
                    List<string> encData = ServerCommunication.GetDataFromData("MailBox", new List<string>() { "UserID", "EmployeeID" }, "MailID", SecurityManager.EncryptSK(queryID.ToString()));
                    List<string> data = DecryptList(encData);

                    if (data[0] == clientUserID.ToString() || data[1] == clientEmployeeID.ToString()) return true; //If user has clientID OR employeeID matching, then they are sender or recipient. And can see mail.

                    //Now check if user has high permission level in business of employee
                    if (clientEmployeePermissionLvl < 3) return false;
                    string foundBusinessID = ServerCommunication.GetDataFromData("Employees", "BusinessID", "EmployeeID", encData[1]); //Get business ID of employee recipient
                    if (clientBusinessID.ToString() == SecurityManager.DecryptSK(foundBusinessID)) return true;                        //If user is employeed at business then permit access

                    break;
                case "Invites":
                    if (clientPermissionLevel >= 2) return true;    //Moderators can view all invites

                    //Get Business ID and UserID from table
                    List<string> encFoundData = ServerCommunication.GetDataFromData("Invites", new List<string>() { "BusinessID", "UserID" }, "InviteID", SecurityManager.EncryptSK(queryID.ToString()));
                    List<string> foundData = DecryptList(encFoundData);

                    if (foundData[1] == clientUserID.ToString()) return true; //Users can view their own invites
                    if (foundData[0] == clientBusinessID.ToString()) return true;//Business users can view their sent invites
                    break;
            }
            return false;
        }

        /// <summary>
        /// Returns if the user has permission to edit the requested data
        /// </summary>
        /// <param name="tableName">Name of table in database</param>
        /// <param name="queryID"></param>
        /// <returns></returns>
        private bool ValidateEditRequest(string tableName, int queryID)
        {
            switch (tableName)
            {
                case "Users":
                    if (queryID == clientUserID || clientPermissionLevel >= 2) return true; //Users can only grab their own user data (Unless have moderator pemission[2])
                    break;
                case "Businesses":
                    return true; //Should be able to view all business data irrelevant of permission level                     
                case "Employeees":
                    if (clientPermissionLevel >= 2) return true;   // Viewable by moderators
                    if (queryID == clientEmployeeID) return true;  // Employees can view there own data
                    //Now check if business ID is the same, if user has business permission 2+ they can view all employee data for their business
                    string businessID = ServerCommunication.GetDataFromData("Employees", "BusinessID", "EmployeeID", SecurityManager.EncryptSK(queryID.ToString()));
                    if (SecurityManager.DecryptSK(businessID) == clientBusinessID.ToString() && clientEmployeePermissionLvl >= 2)
                        return true;
                    break;
                case "Bookings":
                    if (clientPermissionLevel >= 2) return true;  // Viewable by moderators
                    List<string> encDataIDs = ServerCommunication.GetDataFromData("Bookings", new List<string>() { "EmployeeID", "UserID" }, "BookingID", SecurityManager.EncryptSK(queryID.ToString()));
                    List<string> dataIDs = DecryptList(encDataIDs);
                    //string employeeID = dataIDs[0];                    
                    if (dataIDs[1] == clientUserID.ToString())       //Client can see their own bookings
                        return true;
                    //TODO - Fix for shared bookings and check for other employees of same businesss                
                    break;
                case "MailBox":
                    if (clientPermissionLevel >= 3) return true;    //Viewable by admins (moderators can not view all mail for privacy reasons)
                    List<string> encData = ServerCommunication.GetDataFromData("MailBox", new List<string>() { "UserID", "EmployeeID" }, "MailID", SecurityManager.EncryptSK(queryID.ToString()));
                    List<string> data = DecryptList(encData);

                    if (data[0] == clientUserID.ToString() || data[1] == clientEmployeeID.ToString()) return true; //If user has clientID OR employeeID matching, then they are sender or recipient. And can see mail.

                    //Now check if user has high permission level in business of employee
                    if (clientEmployeePermissionLvl < 3) return false;
                    string foundBusinessID = ServerCommunication.GetDataFromData("Employees", "BusinessID", "EmployeeID", encData[1]); //Get business ID of employee recipient
                    if (clientBusinessID.ToString() == SecurityManager.DecryptSK(foundBusinessID)) return true;                        //If user is employeed at business then permit access

                    break;
                case "Invites":
                    if (clientPermissionLevel >= 2) return true;    //Moderators can view all invites

                    //Get Business ID and UserID from table
                    List<string> encFoundData = ServerCommunication.GetDataFromData("Invites", new List<string>() { "BusinessID", "UserID" }, "InviteID", SecurityManager.EncryptSK(queryID.ToString()));
                    List<string> foundData = DecryptList(encFoundData);

                    if (foundData[1] == clientUserID.ToString()) return true; //Users can view their own invites
                    if (foundData[0] == clientBusinessID.ToString()) return true;//Business users can view their sent invites
                    break;
            }
            return false;
        }