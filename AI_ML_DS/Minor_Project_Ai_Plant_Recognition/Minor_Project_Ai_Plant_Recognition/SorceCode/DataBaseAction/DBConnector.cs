using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Minor_Project_Ai_Plant_Recognition.SorceCode.DataBaseAction
{
    internal class DBConnector
    {
        public string Host { get; private set; }
        public string User { get; private set; }
        public string DBname { get; private set; }
        public string Password { get; private set; }
        public string Port { get; private set; }

        private NpgsqlConnection? connection;

        public DBConnector()
        {
            Host = Environment.GetEnvironmentVariable("DB_HOST")!;
            User = Environment.GetEnvironmentVariable("DB_USER")!;
            DBname = Environment.GetEnvironmentVariable("DB_NAME")!;
            Port = Environment.GetEnvironmentVariable("DB_PORT")!;
            Password = Environment.GetEnvironmentVariable("DB_PASSWORD")!;

            this.connection = GetConnection();
        }

        private NpgsqlConnection GetConnection()
        {
            string connString = $"Host={Host};Username={User};Password={Password};Database={DBname};Port={Port}";
            var connection = new NpgsqlConnection(connString);

            try
            {
                connection.Open();
                WriteLine("Successfully connected to the database.");
            }
            catch (Exception ex)
            {
                WriteLine("Failed to connect to the database.");
                WriteLine("Error: " + ex.Message);
            }

            return connection;
        }
    }
}
}