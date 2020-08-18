using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Data.Sqlite;
using STR001.Core.Resources;

namespace STR001.Core.Utilities
{
    /// <summary>
    /// Contain methods that are responsible for ensuring a correct DB file exists for the application
    /// to use.
    /// </summary>
    public static class DBUtils
    {

        #region DB Properties

        internal static Dictionary<string, string> ColumnNames = new Dictionary<string, string>();

        #endregion

        /// <summary>
        /// Invoking other DB helper methods to ensure all tables are properly setup,
        /// and that the DB file exists.
        /// </summary>
        public static void SetupDB()
        {

            // *Should* resolve to something like this >  C:/ProgramData/stream/Maintenance.db
            string dbPath = Path.Combine(
                path1: Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                path2: "stream",
                path3: "Maintenance.db"
            );

            // TODO: Take password / path as input from the user.
            SqliteConnection connection = CreateDBConnection(dbPath, "STRPassword1!");

            CreateMaintenanceTable(connection, CommonStrings.DB_TableName_Maintenance);

        }

        #region Table Creation Methods

        /// <summary>
        /// Ensures that the "Maintenance" DB Table exists, and that the associated columns also exist
        /// with the correct constraints.
        /// </summary>
        /// <param name="connection">Connection to the DB.</param>
        /// <param name="tableName">Name of the DB Table.</param>
        private static void CreateMaintenanceTable(SqliteConnection connection, string tableName)
        {

            bool created = CreateTable(connection, tableName);
            bool colAdded = false;

            List<string> colNames = GetColumnNames(connection, tableName);

            string colName = "TaskName";
            if (!colNames.Contains(colName))
            {
                colAdded |= AddColumn(connection, tableName, colName, CommonStrings.DB_ColType_Varchar);
            }

            colName = "TaskDescription";
            if (!colNames.Contains(colName))
            {
                colAdded |= AddColumn(connection, tableName, colName, CommonStrings.DB_ColType_Varchar);
            }

            colName = "PeriodString";
            if (!colNames.Contains(colName))
            {
                colAdded |= AddColumn(connection, tableName, colName, CommonStrings.DB_ColType_Varchar);
            }

            colName = "StartDate";
            if (!colNames.Contains(colName))
            {
                colAdded |= AddColumn(connection, tableName, colName, CommonStrings.DB_ColType_DateTime);
            }

            colName = "EndDate";
            if (!colNames.Contains(colName))
            {
                colAdded |= AddColumn(connection, tableName, colName, CommonStrings.DB_ColType_DateTime);
            }

            colName = "LastCompletedDate";
            if (!colNames.Contains(colName))
            {
                colAdded |= AddColumn(connection, tableName, colName, CommonStrings.DB_ColType_DateTime);
            }

            CreateDateModifiedTrigger(connection, tableName);

        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Helper function to drop table(s)
        /// </summary>
        /// <param name="connection">SqliteConnection</param>
        /// <param name="tableNames">Name(s) of table(s) in DB</param>
        /// <returns></returns>
        private static bool DropTables(SqliteConnection connection, params string[] tableNames)
        {
            foreach (string tableName in tableNames)
            {
                try
                {
                    using (SqliteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "DROP TABLE " + tableName + CommonStrings.DB_Semicolon;

                        int affectedRows = command.ExecuteNonQuery();

                        if (affectedRows < 0)
                        {
                            return false;
                        }
                    }
                }
                catch (Exception tableException)
                {
                    // Logger.Error(tableException, "DBI104");
                    throw;
                }
            }
            return true;
        }

        /// <summary>
        /// Helper function to create DB table
        /// </summary>
        /// <param name="connection">SqliteConnection</param>
        /// <param name="tableName">Name of table in DB</param>
        /// <param name="columns">Any additional columns with column definitions to initialize the table with</param>
        private static bool CreateBareTable(SqliteConnection connection, string tableName, params string[] columns)
        {
            try
            {
                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = CommonStrings.DB_CreateTable + CommonStrings.DB_IfNotExists +
                        tableName + CommonStrings.DB_OpenParenthesis + "Id " + CommonStrings.DB_Constraints_Guid;

                    if (columns.Length > 0 && columns.ToList() is List<string> cols)
                    {
                        cols.ForEach(s => command.CommandText += CommonStrings.DB_Comma + s);
                    }

                    command.CommandText += CommonStrings.DB_CloseParenthesis +
                        CommonStrings.DB_Semicolon;

                    int affectedRows = command.ExecuteNonQuery();

                    if (affectedRows >= 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (System.Exception tableException)
            {
                // Logger.Error(tableException, "DBI009");
                throw;
            }

        }

        /// <summary>
        /// Helper function to create DB table
        /// </summary>
        /// <param name="connection">SqliteConnection</param>
        /// <param name="tableName">Name of table in DB</param>
        /// <param name="columns">Any additional columns with column definitions to initialize the table with</param>
        private static bool CreateTable(SqliteConnection connection, string tableName, params string[] columns)
        {
            string dateCreatedColumn = "DateCreated " + CommonStrings.DB_Constraints_DateTime;
            string dateModifiedColumn = "DateModified " + CommonStrings.DB_Constraints_DateTime;
            columns = new string[] { dateCreatedColumn, dateModifiedColumn }.ToList().Concat(columns.ToList()).ToArray();
            return CreateBareTable(connection, tableName, columns);
        }

        private static bool CreateTableWithDateOfRequest(SqliteConnection connection, string tableName, params string[] columns)
        {
            string dateCreatedColumn = "DateCreated " + CommonStrings.DB_Constraints_DateTime;
            string dateModifiedColumn = "DateModified " + CommonStrings.DB_Constraints_DateTime;
            string dateOfRequestColumn = "DateOfRequest " + CommonStrings.DB_Constraints_DateTime;
            columns = new string[] { dateCreatedColumn, dateModifiedColumn, dateOfRequestColumn }.ToList().Concat(columns.ToList()).ToArray();
            return CreateBareTable(connection, tableName, columns);
        }

        /// <summary>
        /// Helper function to get type of Id column in table
        /// </summary>
        /// <param name="connection">SqliteConnection</param>
        /// <param name="tableName">Name of table in DB</param>
        /// <returns></returns>
        private static bool HasGuidIdType(SqliteConnection connection, string tableName)
        {
            try
            {
                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = CommonStrings.DB_Pragma_TableInfo + CommonStrings.DB_OpenParenthesis +
                        tableName + CommonStrings.DB_CloseParenthesis + CommonStrings.DB_Semicolon;
                    SqliteDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (reader.GetString(1) == "Id" && reader.GetString(2) == "BLOB")
                        {
                            return true;
                        }
                    }
                }
            }
            catch (System.Exception guidIdTypeException)
            {
                // Logger.Error(guidIdTypeException, "DBI110");
            }

            return false;
        }

        /// <summary>
        /// Helper function to get names of all columns in table
        /// </summary>
        /// <param name="connection">SqliteConnection</param>
        /// <param name="tableName">Name of table in DB</param>
        /// <returns></returns>
        private static List<string> GetColumnNames(SqliteConnection connection, string tableName)
        {
            List<string> columnNames = new List<string>();

            try
            {
                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = CommonStrings.DB_Pragma_TableInfo + CommonStrings.DB_OpenParenthesis +
                        tableName + CommonStrings.DB_CloseParenthesis + CommonStrings.DB_Semicolon;
                    SqliteDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        columnNames.Add(reader.GetString(1));
                    }
                }
            }
            catch (System.Exception columnNameException)
            {
                // Logger.Error(columnNameException, "DBI010");
            }

            return columnNames;
        }

        /// <summary>
        /// Helper function to get names of all columns in table as a comma-separated string
        /// </summary>
        /// <param name="connection">SqliteConnection</param>
        /// <param name="tableName">Name of table in DB</param>
        /// <returns></returns>
        private static string GetColumnNamesList(SqliteConnection connection, string tableName)
        {
            if (!ColumnNames.ContainsKey(tableName))
            {
                ColumnNames.Add(tableName, string.Join(",", GetColumnNames(connection, tableName).ToArray()));
            }
            return ColumnNames[tableName];
        }

        /// <summary>
        /// Helper function to create a SQLite Database Connection
        /// </summary>
        /// <param name="dbPath">Path to the SQLite Database file</param>
        /// <param name="password">Password to Database</param>
        /// <returns></returns>
        internal static SqliteConnection CreateDBConnection(string dbPath, string password)
        {
            // Create path if it doesn't exist
            string path = Path.GetDirectoryName(dbPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Check if db file exists
            if (!File.Exists(dbPath))
            {
                FileStream stream = File.Create(dbPath);
                stream.Close();
            }

            SqliteConnection connection = new SqliteConnection("DataSource=" + dbPath + CommonStrings.DB_Semicolon);
            connection.Open();
            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT quote($password);";
                command.Parameters.AddWithValue("$password", password);
                string escapedPassword = (string)command.ExecuteScalar(); // Protects against SQL injection

                command.Parameters.Clear();
                command.CommandText = "PRAGMA key = " + escapedPassword + CommonStrings.DB_Semicolon;
                command.ExecuteNonQuery();
            }

            return connection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sourceTableName"></param>
        /// <param name="sourceColumnName"></param>
        /// <param name="sourceReferenceColumnName"></param>
        /// <param name="destinationTableName"></param>
        /// <param name="destinationColumnName"></param>
        /// <param name="destinationReferenceColumnName"></param>
        private static void MoveColumnInfo(SqliteConnection connection, string sourceTableName, string sourceColumnName, string sourceReferenceColumnName, string destinationTableName, string destinationColumnName, string destinationReferenceColumnName)
        {
            try
            {
                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO " + destinationTableName + CommonStrings.DB_OpenParenthesis +
                        destinationColumnName + CommonStrings.DB_CloseParenthesis + " SELECT " + sourceColumnName + " FROM " +
                        sourceTableName + CommonStrings.DB_Semicolon;

                    command.ExecuteNonQuery();
                }
            }
            catch (System.Exception renameException)
            {
                // Logger.Error(renameException, "DBI213");
                throw;
            }
        }

        /// <summary>
        /// Helper function to delete column from existing table
        /// </summary>
        /// <param name="connection">SqliteConnection</param>
        /// <param name="tableName">Name of table in DB</param>
        /// <param name="columnName">Name of column to delete</param>
        /// <returns></returns>
        private static bool DeleteColumn(SqliteConnection connection, string tableName, string columnName)
        {
            string colNames = string.Join(",", GetColumnNames(connection, tableName).ToArray());

            SqliteTransaction transaction = null;

            try
            {
                #region 1. Disable Foreign Keys 

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = CommonStrings.DB_Pragma_ForeignKeysOff;
                    command.ExecuteNonQuery();
                }

                #endregion

                transaction = connection.BeginTransaction();

                #region 2. Get the original table's DDL/SQL creation statement.

                string createTableCmd = "";
                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = string.Format(CommonStrings.DB_SqliteMaster_Table, tableName);
                    SqliteDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        createTableCmd = reader.GetString(0);
                    }
                }

                #endregion

                if (createTableCmd.Contains(columnName))
                {
                    #region 3. Rename the original table to tableName_old for temp storage.

                    using (SqliteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = CommonStrings.DB_AlterTable + tableName + " RENAME TO " +
                            tableName + "_old" + CommonStrings.DB_Semicolon;
                        command.ExecuteNonQuery();
                    }

                    #endregion

                    #region 4. Removing the columnName from DDL/SQL command 'section'

                    /// 2 Possible Cases ?
                    /// 1. 'tableName' is in the middle of the DDL, and as such is followed by a `, `
                    /// 2. 'tableName' is at the very end of the DDL, and as such is followed by a `);`

                    int startIndex;
                    int endIndex;
                    string substring = string.Empty;

                    try
                    {
                        /* ---- CASE # 1 ---- */

                        // Getting the index of the columnName since it's in the middle somewhere.
                        startIndex = createTableCmd.IndexOf(columnName);

                        // Getting the index of the next comma, since we're assuming it's in the middle.
                        endIndex = createTableCmd.IndexOf(CommonStrings.DB_Comma, startIndex) + 1;

                        // Storing the whole string from columnName and comma for removal.
                        substring = createTableCmd.Substring(startIndex, endIndex - startIndex);

                        // Used for complex DATETIME defaults that have multiple commas and parentheses.
                        if (substring.Contains("%"))
                        {
                            substring = createTableCmd.Substring(
                                startIndex: startIndex,
                                length: createTableCmd.IndexOf(CommonStrings.DB_Comma, endIndex) - startIndex + 1
                            );
                        }
                    }
                    catch (Exception)
                    {
                        /* ---- CASE # 2 ---- */

                        // Getting the index of the last comma since we're now assuming it's at the end.
                        startIndex = createTableCmd.LastIndexOf(CommonStrings.DB_Comma);

                        // Getting the index of the end of the DDL, since we're still assuming colName is at the end.
                        endIndex = createTableCmd.IndexOf(CommonStrings.DB_CloseParenthesis, startIndex);

                        // Storing the whole string from comma before colName and final char for removal.
                        substring = createTableCmd.Substring(startIndex, endIndex - startIndex);
                    }

                    // Removing the tableName
                    using (SqliteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = createTableCmd.Replace(substring, "") + CommonStrings.DB_Semicolon;
                        command.ExecuteNonQuery();
                    }

                    #endregion

                    #region 5. Re-Creating the old table w/o deleted columnName

                    using (SqliteCommand command = connection.CreateCommand())
                    {
                        string oldValue = columnName + ",";

                        if (!colNames.Contains(oldValue))
                        {
                            oldValue = colNames.Contains("," + columnName) ? "," + columnName : columnName;
                        }

                        colNames = colNames.Replace(oldValue, "");

                        command.CommandText = "INSERT INTO " + tableName + CommonStrings.DB_OpenParenthesis +
                            colNames + CommonStrings.DB_CloseParenthesis + " SELECT " + colNames + " FROM " +
                            tableName + "_old" + CommonStrings.DB_Semicolon;

                        command.ExecuteNonQuery();
                    }

                    #endregion

                    #region 6. Deleting temporary _old table.

                    using (SqliteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "DROP TABLE " + tableName + "_old" + CommonStrings.DB_Semicolon;
                        command.ExecuteNonQuery();
                    }

                    #endregion 
                }

                transaction.Commit();
                transaction.Dispose();

                #region 7. Re-enable Foreign Keys

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = CommonStrings.DB_Pragma_ForeignKeysOn;
                    command.ExecuteNonQuery();
                }

                #endregion

                return true;
            }
            catch (System.Exception renameException)
            {
                transaction?.Rollback();
                // Logger.Error(renameException, "DBI212");
                throw;
            }
        }

        /// <summary>
        /// Helper function to add column to existing table
        /// </summary>
        /// <param name="connection">SqliteConnection</param>
        /// <param name="tableName">Name of table in DB</param>
        /// <param name="columnName">Name of column to add</param>
        /// <param name="columnDefinitions">Any column definitions, like type, constraints, etc.</param>
        /// <returns></returns>
        private static bool AddColumn(SqliteConnection connection, string tableName, string columnName, params string[] columnDefinitions)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                columnDefinitions.ToList().ForEach(s => sb.Append(" ").Append(s));

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = CommonStrings.DB_AlterTable + tableName +
                        CommonStrings.DB_AddColumn + columnName + sb.ToString() +
                        CommonStrings.DB_Semicolon;

                    int affectedRows = command.ExecuteNonQuery();

                    if (affectedRows >= 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (System.Exception columnAddException)
            {
                // Logger.Error(columnAddException, "DBI011");
                return false;
            }
        }

        /// <summary>
        /// Helper function to rename an existing column in a table
        /// </summary>
        /// <param name="connection">SqliteConnection</param>
        /// <param name="tableName">Name of table in DB</param>
        /// <param name="columnName">Current name of column to rename</param>
        /// <param name="newColumnName">New name for column</param>
        /// <returns></returns>
        private static bool RenameColumn(SqliteConnection connection, string tableName, string columnName, string newColumnName)
        {
            string colNames = string.Join(",", GetColumnNames(connection, tableName).ToArray());

            SqliteTransaction transaction = null;

            try
            {
                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = CommonStrings.DB_Pragma_ForeignKeysOff;
                    command.ExecuteNonQuery();
                }

                transaction = connection.BeginTransaction();

                string createTableCmd = "";
                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = string.Format(CommonStrings.DB_SqliteMaster_Table, tableName);
                    SqliteDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        createTableCmd = reader.GetString(0);
                    }
                }

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = CommonStrings.DB_AlterTable + tableName + " RENAME TO " +
                        tableName + "_old" + CommonStrings.DB_Semicolon;
                    command.ExecuteNonQuery();
                }

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = createTableCmd.Replace(columnName + " ", newColumnName + " ") +
                        CommonStrings.DB_Semicolon;
                    command.ExecuteNonQuery();
                }

                using (SqliteCommand command = connection.CreateCommand())
                {
                    string oldValue = columnName + ",", newValue = newColumnName + ",";

                    if (!colNames.Contains(oldValue))
                    {
                        if (colNames.Contains("," + columnName))
                        {
                            oldValue = "," + columnName;
                            newValue = "," + newColumnName;
                        }
                        else
                        {
                            oldValue = columnName;
                            newValue = newColumnName;
                        }
                    }

                    command.CommandText = "INSERT INTO " + tableName + CommonStrings.DB_OpenParenthesis +
                        colNames.Replace(oldValue, newValue) + CommonStrings.DB_CloseParenthesis +
                        " SELECT " + colNames + " FROM " + tableName + "_old" + CommonStrings.DB_Semicolon;

                    command.ExecuteNonQuery();
                }

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DROP TABLE " + tableName + "_old" + CommonStrings.DB_Semicolon;
                    command.ExecuteNonQuery();
                }

                transaction.Commit();
                transaction.Dispose();

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = CommonStrings.DB_Pragma_ForeignKeysOn;
                    command.ExecuteNonQuery();
                }

                return true;
            }
            catch (System.Exception renameException)
            {
                transaction?.Rollback();
                // Logger.Error(renameException, "DBI012");
                throw;
            }
        }

        /// <summary>
        /// Helper function to create foreign key statement
        /// </summary>
        /// <param name="tableName">Name of table in DB</param>
        /// <returns></returns>
        private static string References(string tableName)
        {
            return "REFERENCES " + tableName + "(Id)";
        }

        /// <summary>
        /// Helper function to create trigger on update for DateModified field
        /// </summary>
        /// <param name="connection">SqliteConnection</param>
        /// <param name="tableName">Name of table in DB</param>
        private static void CreateDateModifiedTrigger(SqliteConnection connection, string tableName)
        {
            try
            {
                // Drop Trigger, if exists
                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = CommonStrings.DB_DropTrigger + CommonStrings.DB_IfExists +
                        tableName + CommonStrings.DB_Trigger_DateModified + CommonStrings.DB_Semicolon;
                    command.ExecuteNonQuery();
                }

                // Get current columns in table
                List<string> cols = GetColumnNames(connection, tableName);
                cols.Remove("Id");
                cols.Remove("DateCreated");
                cols.Remove("DateModified");

                if (cols.Count > 0)
                {
                    // Create Trigger
                    StringBuilder sb = new StringBuilder();
                    cols.ForEach(s => sb.Append(s).Append(CommonStrings.DB_Comma));
                    sb.Remove(sb.Length - 1, 1); // Remove last comma

                    using (SqliteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = CommonStrings.DB_CreateTrigger + tableName +
                            CommonStrings.DB_Trigger_DateModified + CommonStrings.DB_Trigger_AfterUpdate +
                            sb.ToString() + string.Format(CommonStrings.DB_Trigger_UpdateStatement, tableName);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception dateModifiedException)
            {
                // Logger.Error(dateModifiedException, "DBI013");
            }
        }

        /// <summary>
        /// Helper function to ensure unique values in a table
        /// </summary>
        /// <param name="connection">SqliteConnection</param>
        /// <param name="tableName">Name of table in DB</param>
        /// <param name="uniqueColumns">Name(s) of column(s) (comma-separated if needed) required to be unique</param>
        private static void CreateUniqueIndex(SqliteConnection connection, string tableName, string uniqueColumns)
        {
            try
            {
                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = CommonStrings.DB_CreateUniqueIndex + CommonStrings.DB_IfNotExists +
                        "Unique_" + tableName + " ON " + tableName + CommonStrings.DB_OpenParenthesis +
                        uniqueColumns +
                        CommonStrings.DB_CloseParenthesis + CommonStrings.DB_Semicolon;
                    command.ExecuteNonQuery();
                }
            }
            catch (System.Exception uniqueIndexException)
            {
                // Logger.Error(uniqueIndexException, "DBI014");
            }
        }

        /// <summary>
        /// Helper function to insert values into tables
        /// </summary>
        /// <param name="connection">SqliteConnection</param>
        /// <param name="ignoreOnConflict">If true, use "INSERT OR IGNORE" (used to insert into column with UNIQUE constraint). If false, use "INSERT"</param>
        /// <param name="tableName">Name of table in DB</param>
        /// <param name="columnNames">Name(s) of column(s) (comma-separated if needed) being inserted into</param>
        /// <param name="values">Value(s) being inserted (comma-separated and 'single-quoted text' if needed)</param>
        private static List<string> Insert(SqliteConnection connection, bool ignoreOnConflict, string tableName, string columnNames, params string[] values)
        {
            List<string> ids = new List<string>();
            try
            {
                if (values.Length > 0 && values.ToList() is List<string> vals)
                {
                    //StringBuilder sb = new StringBuilder();
                    string guid = DataFunctions.GuidToSQLiteHexString(DataFunctions.NewGuidComb());
                    ids.Add(guid);

                    //vals.ForEach(s => sb.Append(CommonStrings.DB_OpenParenthesis + guid + CommonStrings.DB_Comma + s + CommonStrings.DB_CloseParenthesis + CommonStrings.DB_Comma));
                    //sb.Remove(sb.Length - 1, 1); // Remove last comma

                    using (SqliteCommand command = connection.CreateCommand())
                    {
                        foreach (string item in vals)
                        {
                            // Creates a string that resembles => "(X'F67B93EE2412BF40B148AB67000000FC','1 - Critical')"
                            string valueToInsert = CommonStrings.DB_OpenParenthesis + guid + CommonStrings.DB_Comma + item + CommonStrings.DB_CloseParenthesis;

                            command.CommandText = $"INSERT {(ignoreOnConflict ? "OR IGNORE " : "")}INTO {tableName}{CommonStrings.DB_OpenParenthesis}Id,{columnNames}{CommonStrings.DB_CloseParenthesis} VALUES{valueToInsert}";
                            command.ExecuteNonQuery();

                            // Generating a new guid for this item.
                            guid = DataFunctions.GuidToSQLiteHexString(DataFunctions.NewGuidComb());
                        }

                        /// Former developer's method! Didn't work :(
                        /// This required n number of application startups to insert all content into the DB successfully, 
                        /// where n was the number of items that needed to be inserted into the database.
                        //command.CommandText = "INSERT " + (ignoreOnConflict ? "OR IGNORE " : "") + "INTO " + tableName +
                        //CommonStrings.DB_OpenParenthesis + "Id," + columnNames + CommonStrings.DB_CloseParenthesis +
                        //" VALUES" + sb.ToString();
                        //command.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception insertException)
            {
                // Logger.Error(insertException, "DBI024");
            }
            return ids;
        }

        #endregion

    }
}
