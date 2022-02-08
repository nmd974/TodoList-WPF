using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Diagnostics;

namespace TodoList
{
    class DBConnection
    {
        public static SQLiteConnection DBInit()
        {
            SQLiteConnection db = ConnectDb();
            if (!File.Exists("database.db"))
            {
                Task.CreateTableTask(db);
            }
            return db;
        }
        public static SQLiteConnection ConnectDb()
        {
            SQLiteConnection sqliteConnect;
            sqliteConnect = new SQLiteConnection("Data Source=database.db");
            try
            {
                sqliteConnect.Open();
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine(e.Message);
            }

            return sqliteConnect;
        }

        
    }
}
