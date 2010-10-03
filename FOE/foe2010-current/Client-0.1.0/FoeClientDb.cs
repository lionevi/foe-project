using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace Foe.Client
{
    public static class FoeClientDb
    {
        static string _dbPath = "./foe.db";

        public static SQLiteConnection Open()
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + _dbPath);
            conn.Open();
            return conn;
        }
    }
}
