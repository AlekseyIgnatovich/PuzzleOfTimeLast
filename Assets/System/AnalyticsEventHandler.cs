using System;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Services.Analytics
{
    public class AnalyticsEventHandler : MonoBehaviour {

        async void Start()
        {
            await UnityServices.InitializeAsync();

            Debug.Log($"Started UGS Analytics Sample with user ID: {AnalyticsService.Instance.GetAnalyticsUserID()}");

            GiveConsent();
        }

        public void GiveConsent()
        {
            AnalyticsService.Instance.StartDataCollection();

            Debug.Log($"Consent has been provided. The SDK is now collecting data!");
        }
    }
}
