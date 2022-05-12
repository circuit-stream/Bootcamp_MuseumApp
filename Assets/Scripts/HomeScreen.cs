using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MuseumApp
{
    public class HomeScreen : MonoBehaviour
    {
        public GameObject loginButton;
        public TMP_Text username;

        public RectTransform attractionEntriesParent;

        public AttractionEntryGraphics attractionPrefab;
        public List<AttractionConfig> attractions;
        public List<AttractionEntryGraphics> attractionEntries;

        private List<string> enabledAttractions;

        public void Signup()
        {
            SceneManager.LoadScene("SignupPopup", LoadSceneMode.Additive);
        }

        public void LogOff()
        {
            User.LogOff();

            Refresh();
        }

        public void Refresh()
        {
            SetupUsername();

            foreach (var attraction in attractionEntries)
                attraction.Refresh(IsAttractionEnabled(attraction));
        }

        private void Awake()
        {
            SetEnabledAttractions();
            SetupAttractions();
            SetupUsername();

            PlayFabController.Instance.titleDataAcquired += OnTitleDataFetched;
            PlayFabController.Instance.LoginWithPlayFab(PlayFabController.Instance.FetchTitleData);
        }

        private void OnTitleDataFetched()
        {
            SetEnabledAttractions();
            Refresh();
        }

        private void SetupAttractions()
        {
            attractionEntries = new List<AttractionEntryGraphics>(attractions.Count);
            foreach (var attraction in attractions)
            {
                var newAttraction = Instantiate(attractionPrefab, attractionEntriesParent);
                newAttraction.Setup(attraction);
                newAttraction.Refresh(IsAttractionEnabled(newAttraction));

                attractionEntries.Add(newAttraction);
            }
        }

        private bool IsAttractionEnabled(AttractionEntryGraphics attraction)
        {
            return enabledAttractions != null && enabledAttractions.Contains(attraction.Id);
        }

        private void SetEnabledAttractions()
        {
            if (PlayFabController.Instance.titleData == null)
                return;

            enabledAttractions = PlayFabController
                .Instance
                .titleData["EnabledAttractions"]
                .Split(',')
                .ToList();
        }

        private void SetupUsername()
        {
            if (!User.IsLoggedIn)
            {
                loginButton.SetActive(true);
                username.gameObject.SetActive(false);
                return;
            }

            loginButton.SetActive(false);
            username.gameObject.SetActive(true);

            username.text = User.LoggedInUsername;
        }
    }
}