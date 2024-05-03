using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Crop_Recommendation_Project.src.DBManupulator
{
    internal class DatabaseConnector
    {
        public string Host { get; private set; }
        public string User { get; private set; }
        public string DBname { get; private set; }
        public string Port { get; private set; }
        public string Password { get; private set; }

        private NpgsqlConnection? connection;

        public DatabaseConnector()
        {
            // Retrieve these values from a secure source
            Host = Environment.GetEnvironmentVariable("DB_HOST")!;
            User = Environment.GetEnvironmentVariable("DB_USER")!;
            DBname = Environment.GetEnvironmentVariable("DB_NAME")!;
            Port = Environment.GetEnvironmentVariable("DB_PORT")!;
            Password = Environment.GetEnvironmentVariable("DB_PASSWORD")!;

            this.connection = GetConnection();
        }

        private NpgsqlConnection GetConnection()
        {
            string connectionString = String.Format("Host={0};Username={1};Database={2};Port={3};Password={4}", Host, User, DBname, Port, Password);
            var connection = new NpgsqlConnection(connectionString);

            try
            {
                connection.Open();
                Console.WriteLine("Successfully connected to the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect to the database.");
                Console.WriteLine("Error: " + ex.Message);
                // Consider re-throwing the exception or handling it appropriately
            }

            return connection;
        }

        public void CloseConnection()
        {
            if (this.connection != null)
            {
                this.connection.Close();
                Console.WriteLine("Connection closed.");
            }
        }

        public void Create(string query)
        {
            using (var cmd = new NpgsqlCommand(query, this.connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public NpgsqlDataReader Read(string query)
        {
            using (var cmd = new NpgsqlCommand(query, this.connection))
            {
                return cmd.ExecuteReader();
            }
        }

        public void Update(string query)
        {
            using (var cmd = new NpgsqlCommand(query, this.connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(string query)
        {
            using (var cmd = new NpgsqlCommand(query, this.connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void ExecuteQuery(string query)
        {
            using (var cmd = new NpgsqlCommand(query, this.connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}