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

        public void LoginWithPlayFab(Action callback = null)
        {
            var request = new LoginWithCustomIDRequest
                { CustomId = "GettingStartedGuide", CreateAccount = true};

            PlayFabClientAPI.LoginWithCustomID(
                request,
                result => OnLoginSuccess(result, callback),
                error => OnPlayFabFailure(error, "LoginWithCustomID"));
        }

        public void LoginWithPlayFab(string email, string password, Action callback = null)
        {
            var request = new LoginWithEmailAddressRequest
            {
                Email = email,
                Password = password
            };

            PlayFabClientAPI.LoginWithEmailAddress(
                request,
                result => OnLoginSuccess(result, callback),
                error => OnPlayFabFailure(error, "LoginWithEmailAddress"));
        }

        private void OnLoginSuccess(LoginResult result, Action callback)
        {
            Debug.Log($"Successfully logged in with: {result.PlayFabId}");
            callback?.Invoke();
        }

        private void OnPlayFabFailure(PlayFabError error, string requestName)
        {
            Debug.LogWarning($"Something went wrong with the request {requestName}");
            Debug.LogError(error.GenerateErrorReport());
        }
    }
}