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
        public Int32 ended { get; set; }
        public Int32 archived { get; set; }
        public Int32 id { get; set; }

        public static void CreateTableTask(SQLiteConnection db)
        {
            SQLiteCommand sqliteCommand;
            string query = "CREATE TABLE Task(id INTEGER PRIMARY KEY AUTOINCREMENT, label VARCHAR(255) NOT NULL, ended INT, archived INT)";
            sqliteCommand = db.CreateCommand();
            sqliteCommand.CommandText = query;
            try
            {
                sqliteCommand.ExecuteNonQuery();
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine(e.Message);
            }

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

        public int AddTask(SQLiteConnection db, Task task)
        {
            const string query = "INSERT INTO Task(label, ended, archived) VALUES(@label, @ended, @archived)";
            var args = new Dictionary<string, object>
            {
                {"@label", task.label},
                {"@ended", task.ended},
                {"@archived", task.archived}
            };

            return ExecuteWrite(db, query, args);
        }

        public int ArchiveTask(SQLiteConnection db, Int32 id)
        {
            const string query = "UPDATE Task SET archived = @archived WHERE id = @id";
            var args = new Dictionary<string, object>
            {
                {"@id", id},
                {"@archived", 1},
            };

            return ExecuteWrite(db, query, args);
        }

        public int EndTask(SQLiteConnection db, Int32 id)
        {
            const string query = "UPDATE Task SET ended = @ended WHERE id = @id";
            var args = new Dictionary<string, object>
            {
                {"@id", id},
                {"@ended", 1},
            };

            return ExecuteWrite(db, query, args);
        }

        public int ReactivateTask(SQLiteConnection db, Int32 id)
        {
            const string query = "UPDATE Task SET ended = @ended WHERE id = @id";
            var args = new Dictionary<string, object>
            {
                {"@id", id},
                {"@ended", 0},
            };

            return ExecuteWrite(db, query, args);
        }

        public int UpdateLabel(SQLiteConnection db, Int32 id, String label)
        {
            const string query = "UPDATE Task SET label = @label WHERE id = @id";
            var args = new Dictionary<string, object>
            {
                {"@id", id},
                {"@label", label},
            };

            return ExecuteWrite(db, query, args);
        }

        public int ExecuteWrite(SQLiteConnection db, string query, Dictionary<string, object> args)
        {
            int numberOfRowsAffected = 0;
            SQLiteCommand sqliteCommand = new SQLiteCommand(query, db);
            foreach (var pair in args)
            {
                sqliteCommand.Parameters.AddWithValue(pair.Key, pair.Value);
            }
            
            try
            {
                numberOfRowsAffected = sqliteCommand.ExecuteNonQuery();
                return numberOfRowsAffected;
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine(e.Message);
            }

            return numberOfRowsAffected;
        }
    }
}
