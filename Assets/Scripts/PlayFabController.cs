using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace MuseumApp
{
    public class PlayFabController
    {
        private static PlayFabController instance;
        public static PlayFabController Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlayFabController();

                return instance;
            }
            private set => instance = value;
        }

        private PlayFabController() {}

        public void LoginWithPlayFab()
        {
            var request = new LoginWithCustomIDRequest
                { CustomId = "GettingStartedGuide", CreateAccount = true};

            PlayFabClientAPI.LoginWithCustomID(
                request,
                OnLoginSuccess,
                error => OnPlayFabFailure(error, "LoginWithCustomID"));
        }

        private void OnLoginSuccess(LoginResult result)
        {
            Debug.Log($"Successfully logged in with: {result.PlayFabId}");
        }

        private void OnPlayFabFailure(PlayFabError error, string requestName)
        {
            Debug.LogWarning($"Something went wrong with the request {requestName}");
            Debug.LogError(error.GenerateErrorReport());
        }
    }
}


