using System;
using System.Collections.Generic;
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

        public Dictionary<string, string> titleData;
        public Action titleDataAcquired;

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

        public void RegisterPlayFabUser(
            string email, string password, Action callback = null)
        {
            var request = new RegisterPlayFabUserRequest
            {
                RequireBothUsernameAndEmail = false,
                Email = email,
                Password = password
            };

            PlayFabClientAPI.RegisterPlayFabUser(
                request,
                result => OnRegister(result, callback),
                error => OnPlayFabFailure(error, "LoginWithEmailAddress"));
        }

        public void FetchTitleData()
        {
            PlayFabClientAPI.GetTitleData(
                new GetTitleDataRequest(),
                OnTitleDataAcquired,
                error => OnPlayFabFailure(error, "GetTitleData"));
        }

        private void OnTitleDataAcquired(GetTitleDataResult result)
        {
            foreach (var entry in result.Data)
            {
                Debug.Log($"Data: {entry.Key} - {entry.Value}");
            }

            titleData = result.Data;
            titleDataAcquired?.Invoke();
        }

        private void OnRegister(RegisterPlayFabUserResult result, Action callback)
        {
            Debug.Log($"Registered new user: {result.PlayFabId}");
            callback?.Invoke();
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