using System;
using SQLite4Unity3d;
using UnityEngine;

namespace MuseumApp
{
    [Table("User")]
    public class User
    {
        public static string loggedUserSaveKey = "loggedUserSaveKey";
        public static string LoggedInUsername => PlayerPrefs.GetString(loggedUserSaveKey, String.Empty);
        public static bool IsLoggedIn => !LoggedInUsername.Equals(String.Empty);

        public static void Login(string username) => PlayerPrefs.SetString(loggedUserSaveKey, username);
        public static void LogOff() => PlayerPrefs.DeleteKey(loggedUserSaveKey);

        [PrimaryKey] public string Username { get; set; }

        // Don't ever store passwords like this! Just an example!!
        public string Password { get; set; }
    }
}