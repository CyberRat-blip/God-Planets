﻿using ConsentManager.Common;
//using ConsentManager.Platforms.iOS;
using ConsentManager.Platforms.Android;

namespace ConsentManager.Platforms
{
    internal static class ConsentManagerClientFactory
    {
        internal static IConsentManager GetConsentManager()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return new AndroidConsentManager();
#elif UNITY_IPHONE && !UNITY_EDITOR
            return new iOSConsentManager();
#else
            return new Dummy.Dummy();
#endif
        }

        internal static IVendorBuilder GetVendorBuilder(string name, string bundle, string policyUrl)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return new AndroidVendorBuilder(name, bundle, policyUrl);
#elif UNITY_IPHONE && !UNITY_EDITOR
            return new iOSVendorBuilder(name, bundle, policyUrl);
#else
            return new Dummy.Dummy();
#endif
        }

        internal static IConsentForm GetConsentForm(IConsentFormListener listener)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return new AndroidConsentForm(listener);
#elif UNITY_IPHONE && !UNITY_EDITOR
            var builder = new iOSConsentFormBuilder();
            builder.withListener(listener);
            return builder.build();
#else
            return new Dummy.Dummy();
#endif
        }

        internal static IConsentManagerException GetConsentManagerException()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return new AndroidConsentManagerException();
#elif UNITY_IPHONE && !UNITY_EDITOR
            return new iOSConsentManagerException();
#else
            return new Dummy.Dummy();
#endif
        }
    }
}
