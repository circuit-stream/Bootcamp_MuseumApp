using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MuseumApp
{
    public class HomeScreen : MonoBehaviour
    {
        public GameObject loginButton;
        public GameObject deleteUserButton;
        public TMP_Text username;

        public RectTransform attractionEntriesParent;

        public AttractionEntryGraphics attractionPrefab;
        public List<AttractionConfig> attractions;
        public List<AttractionEntryGraphics> attractionEntries;

        public void Signup()
        {
            SceneManager.LoadScene("SignupPopup", LoadSceneMode.Additive);
        }

        public void LogOff()
        {
            User.LogOff();

            Refresh();
        }

        public void DeleteUser()
        {
            Database.RemoveAllUserRatings();
            Database.DeleteUser();
            User.LogOff();

            Refresh();
        }

        public void Refresh()
        {
            SetupUsername();

            foreach (var attraction in attractionEntries)
                attraction.Refresh();
        }

        private void Awake()
        {
            attractionEntries = new List<AttractionEntryGraphics>(attractions.Count);
            foreach (var attraction in attractions)
            {
                var newAttraction = Instantiate(attractionPrefab, attractionEntriesParent);
                newAttraction.Setup(attraction);
                attractionEntries.Add(newAttraction);
            }

            SetupUsername();
        }

        private void SetupUsername()
        {
            loginButton.SetActive(!User.IsLoggedIn);
            username.gameObject.SetActive(User.IsLoggedIn);
            deleteUserButton.SetActive(User.IsLoggedIn);

            if (User.IsLoggedIn)
                username.text = User.LoggedInUsername;
        }
    }
}