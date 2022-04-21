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
            connection.CreateTable<UserRating>();
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

        public static void Rate(string attractionId, int rating)
        {
            var userRating = GetUserAttractionRating(attractionId);
            if (userRating != null)
            {
                userRating.Rating = rating;
                connection.Update(userRating);
                return;
            }

            connection.Insert(new UserRating
            {
                AttractionId = attractionId,
                Username = User.LoggedInUsername,
                Rating = rating,
            });
        }

        public static UserRating GetUserAttractionRating(string attractionId)
        {
            var username = User.LoggedInUsername;
            if (username.Equals(String.Empty)) return null;

            var results = connection.Query<UserRating>(
                @$"SELECT * FROM {nameof(UserRating)} WHERE
                {nameof(UserRating.AttractionId)} = '{attractionId}' AND 
                {nameof(UserRating.Username)} = '{username}'"
            );

            Debug.Assert(results.Count <= 1, $"{username} has multiple ratings for the same attraction");

            return results.Count == 1 ? results[0] : null;
        }

        public static int GetAttractionTotalRating(string attractionId)
        {
            var ratings =
                (from userRating in connection.Table<UserRating>()
                where userRating.AttractionId == attractionId
                select userRating);

            // var ratings = connection.Table<UserRating>()
            //     .Where(userRating => userRating.AttractionId == attractionId);

            return ratings.Any() ? ratings.Sum(userRating => userRating.Rating) / ratings.Count() : 0;
        }
    }
}