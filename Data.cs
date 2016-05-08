//*****************************************************************************
// Data.cs - This is used to hold the data to be queryed.
//*****************************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace QEngine
{
    public class Data
    {
        List<object> mRecords = new List<object>();
        string mTableName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tableName">Name of the database.</param>
        public Data(string tableName)
        {
            mTableName = tableName;
        }

        /// <summary>
        /// Represents the first row (Property names) of database.
        /// </summary>
        public Row Headder { get; set; }

        /// <summary>
        /// Represents the body of database table.
        /// </summary>
        public IEnumerable<object> Records
        {
            get { return mRecords; }
        }

        /// <summary>
        /// Queries the databse based on the given command.
        /// </summary>
        /// <param name="cmd">Commad to be used for querying.</param>
        /// <returns>IQueryable of queried data</returns>
        public IQueryable<object> ProcessCommand(string cmd)
        {
            Regex regEx = new Regex("SELECT (.*) FROM .*");
            var match = regEx.Match(cmd);
            var selectCondition = match.Groups[1].Value;
            regEx = new Regex("WHERE (.*)");
            match = regEx.Match(cmd);
            var whereCondition = match.Groups[1].Value;

            var result = mRecords.AsQueryable<object>().Where(whereCondition, Headder.Values)
                .Select(selectCondition, Headder.Values);
            return result;
        }

        /// <summary>
        /// Creates and returns a database table which can be queried.
        /// </summary>
        /// <param name="lines">Rows to be added to database</param>
        /// <param name="tableName">name of the database table</param>
        /// <returns>Data representing database</returns>
        public static Data CreateDB(IEnumerable<string> lines, string tableName)
        {
            var rows = lines.ToList();
            var table = new Data(tableName);
            table.Headder = new Row(rows[0]);
            // Removing the headder to add the rest as body.
            rows.RemoveAt(0);
            foreach (var row in rows)
            {
                // Creating a dynamic class with headder values as properties
                // and row entities as values.
                var obj = DynTypeBuilder.CreateNewObject(table.Headder.Values);
                var type = obj.GetType();
                var values = row.Split(',');
                for (int i = 0; i < values.Count(); i++)
                {
                    var propertyInfo = type.GetProperty(table.Headder.Values.ElementAt(i));
                    propertyInfo.SetValue(obj, values.ElementAt(i));
                }
                table.mRecords.Add(obj);
            }
            return table;
        }
    }
}
