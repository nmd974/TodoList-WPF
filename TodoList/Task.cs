using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows.Controls;

namespace TodoList
{
    class Task
    {
        public string label { get; set; }
        public string ended { get; set; }
        public string archived { get; set; }
        public Int32 id { get; set; }

        public static void CreateTableTask(SQLiteConnection db)
        {
            SQLiteCommand sqliteCommand;
            string query = "CREATE TABLE Task(id INTEGER PRIMARY KEY AUTOINCREMENT, label VARCHAR(255) NOT NULL, ended INT, archived INT)";
            sqliteCommand = db.CreateCommand();
            sqliteCommand.CommandText = query;
            sqliteCommand.ExecuteNonQuery();
        }

        public SQLiteDataReader GetTasks(SQLiteConnection db)
        {
            SQLiteCommand sqliteCommand;
            SQLiteDataReader sqliteReader;
            sqliteCommand = db.CreateCommand();
            sqliteCommand.CommandText = "SELECT * FROM Task WHERE archived <> 1";
            try
            {
                sqliteReader = sqliteCommand.ExecuteReader();
                return sqliteReader;

            }
            catch (SQLiteException e)
            {
                Debug.WriteLine(e.Message);
            }
            sqliteReader = null;
            return sqliteReader;
        }

        public static void AddTask(SQLiteConnection db, Task task)
        {
            const string query = "INSERT INTO Task(label, ended, archived) VALUES(@label, @ended, @archived)";
            var args = new Dictionary<string, object>
            {
                {"@label", task.label},
                {"@ended", task.ended},
                {"@archived", task.archived}
            };

            Task.ExecuteWrite(db, query, args);
        }

        //public int ArchiveTask(SQLiteConnection db, Task task)
        //{
        //    const string query = "UPDATE Task SET archived = @archvied WHERE Id = @id";
        //    var args = new Dictionary<string, object>
        //    {
        //        {"@archived", task.archived}
        //    };

        //    return ExecuteWrite(db, query, args);
        //}

        //public int EndTask(SQLiteConnection db, Task task)
        //{
        //    const string query = "UPDATE Task SET ended = @ended WHERE Id = @id";
        //    var args = new Dictionary<string, object>
        //    {
        //        {"@ended", task.ended}
        //    };

        //    return ExecuteWrite(db, query, args);
        //}

        public static void ExecuteWrite(SQLiteConnection db, string query, Dictionary<string, object> args)
        {
            int numberOfRowsAffected;
            SQLiteCommand sqliteCommand = new SQLiteCommand(query, db);
            foreach (var pair in args)
            {
                sqliteCommand.Parameters.AddWithValue(pair.Key, pair.Value);
            }
            numberOfRowsAffected = sqliteCommand.ExecuteNonQuery();

            //return numberOfRowsAffected;
        }
    }
}
