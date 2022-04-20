using System;
using System.Linq;
using SQLite4Unity3d;
using UnityEngine;

namespace MuseumApp
{
    public static class Database
    {
        private static string databasePath = "GameDatabase.db";
        private static readonly SQLiteConnection connection;

        static Database()
        {
            connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

            connection.CreateTable<User>();
        }

        public static void RegisterPlayer(string username, string password)
        {
            connection.Insert(new User
            {
                Username = username,
                Password = password,
            });
        }

        public static User GetUser(string username)
        {
            try
            {
                return connection.Get<User>(username);
            }
            catch
            {
                return null;
            }
        }
    }
}