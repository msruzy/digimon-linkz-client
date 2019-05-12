﻿using Facebook.Unity.Mobile.IOS;
using System;
using System.Runtime.InteropServices;

namespace Facebook.Unity
{
	internal class IOSWrapper : IIOSWrapper
	{
		public void Init(string appId, bool frictionlessRequests, string urlSuffix, string unityUserAgentSuffix)
		{
			IOSWrapper.IOSInit(appId, frictionlessRequests, urlSuffix, unityUserAgentSuffix);
		}

		public void LogInWithReadPermissions(int requestId, string scope)
		{
			IOSWrapper.IOSLogInWithReadPermissions(requestId, scope);
		}

		public void LogInWithPublishPermissions(int requestId, string scope)
		{
			IOSWrapper.IOSLogInWithPublishPermissions(requestId, scope);
		}

		public void LogOut()
		{
			IOSWrapper.IOSLogOut();
		}

		public void SetShareDialogMode(int mode)
		{
			IOSWrapper.IOSSetShareDialogMode(mode);
		}

		public void ShareLink(int requestId, string contentURL, string contentTitle, string contentDescription, string photoURL)
		{
			IOSWrapper.IOSShareLink(requestId, contentURL, contentTitle, contentDescription, photoURL);
		}

		public void FeedShare(int requestId, string toId, string link, string linkName, string linkCaption, string linkDescription, string picture, string mediaSource)
		{
			IOSWrapper.IOSFeedShare(requestId, toId, link, linkName, linkCaption, linkDescription, picture, mediaSource);
		}

		public void AppRequest(int requestId, string message, string actionType, string objectId, string[] to = null, int toLength = 0, string filters = "", string[] excludeIds = null, int excludeIdsLength = 0, bool hasMaxRecipients = false, int maxRecipients = 0, string data = "", string title = "")
		{
			IOSWrapper.IOSAppRequest(requestId, message, actionType, objectId, to, toLength, filters, excludeIds, excludeIdsLength, hasMaxRecipients, maxRecipients, data, title);
		}

		public void AppInvite(int requestId, string appLinkUrl, string previewImageUrl)
		{
			IOSWrapper.IOSAppInvite(requestId, appLinkUrl, previewImageUrl);
		}

		public void CreateGameGroup(int requestId, string name, string description, string privacy)
		{
			IOSWrapper.IOSCreateGameGroup(requestId, name, description, privacy);
		}

		public void JoinGameGroup(int requestId, string groupId)
		{
			IOSWrapper.IOSJoinGameGroup(requestId, groupId);
		}

		public void FBSettingsActivateApp(string appId)
		{
			IOSWrapper.IOSFBSettingsActivateApp(appId);
		}

		public void LogAppEvent(string logEvent, double valueToSum, int numParams, string[] paramKeys, string[] paramVals)
		{
			IOSWrapper.IOSFBAppEventsLogEvent(logEvent, valueToSum, numParams, paramKeys, paramVals);
		}

		public void LogPurchaseAppEvent(double logPurchase, string currency, int numParams, string[] paramKeys, string[] paramVals)
		{
			IOSWrapper.IOSFBAppEventsLogPurchase(logPurchase, currency, numParams, paramKeys, paramVals);
		}

		public void FBAppEventsSetLimitEventUsage(bool limitEventUsage)
		{
			IOSWrapper.IOSFBAppEventsSetLimitEventUsage(limitEventUsage);
		}

		public void GetAppLink(int requestId)
		{
			IOSWrapper.IOSGetAppLink(requestId);
		}

		public string FBSdkVersion()
		{
			return IOSWrapper.IOSFBSdkVersion();
		}

		public void FetchDeferredAppLink(int requestId)
		{
			IOSWrapper.IOSFetchDeferredAppLink(requestId);
		}

		public void RefreshCurrentAccessToken(int requestId)
		{
			IOSWrapper.IOSRefreshCurrentAccessToken(requestId);
		}

		[DllImport("__Internal")]
		private static extern void IOSInit(string appId, bool frictionlessRequests, string urlSuffix, string unityUserAgentSuffix);

		[DllImport("__Internal")]
		private static extern void IOSLogInWithReadPermissions(int requestId, string scope);

		[DllImport("__Internal")]
		private static extern void IOSLogInWithPublishPermissions(int requestId, string scope);

		[DllImport("__Internal")]
		private static extern void IOSLogOut();

		[DllImport("__Internal")]
		private static extern void IOSSetShareDialogMode(int mode);

		[DllImport("__Internal")]
		private static extern void IOSShareLink(int requestId, string contentURL, string contentTitle, string contentDescription, string photoURL);

		[DllImport("__Internal")]
		private static extern void IOSFeedShare(int requestId, string toId, string link, string linkName, string linkCaption, string linkDescription, string picture, string mediaSource);

		[DllImport("__Internal")]
		private static extern void IOSAppRequest(int requestId, string message, string actionType, string objectId, string[] to = null, int toLength = 0, string filters = "", string[] excludeIds = null, int excludeIdsLength = 0, bool hasMaxRecipients = false, int maxRecipients = 0, string data = "", string title = "");

		[DllImport("__Internal")]
		private static extern void IOSAppInvite(int requestId, string appLinkUrl, string previewImageUrl);

		[DllImport("__Internal")]
		private static extern void IOSCreateGameGroup(int requestId, string name, string description, string privacy);

		[DllImport("__Internal")]
		private static extern void IOSJoinGameGroup(int requestId, string groupId);

		[DllImport("__Internal")]
		private static extern void IOSFBSettingsActivateApp(string appId);

		[DllImport("__Internal")]
		private static extern void IOSFBAppEventsLogEvent(string logEvent, double valueToSum, int numParams, string[] paramKeys, string[] paramVals);

		[DllImport("__Internal")]
		private static extern void IOSFBAppEventsLogPurchase(double logPurchase, string currency, int numParams, string[] paramKeys, string[] paramVals);

		[DllImport("__Internal")]
		private static extern void IOSFBAppEventsSetLimitEventUsage(bool limitEventUsage);

		[DllImport("__Internal")]
		private static extern void IOSGetAppLink(int requestID);

		[DllImport("__Internal")]
		private static extern string IOSFBSdkVersion();

		[DllImport("__Internal")]
		private static extern void IOSFetchDeferredAppLink(int requestID);

		[DllImport("__Internal")]
		private static extern void IOSRefreshCurrentAccessToken(int requestID);
	}
}
