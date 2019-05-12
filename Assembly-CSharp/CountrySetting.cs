﻿using Master;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public static class CountrySetting
{
	public static readonly Dictionary<int, string> CountryPrefix = new Dictionary<int, string>
	{
		{
			1,
			"jp"
		},
		{
			2,
			"en"
		},
		{
			3,
			"cn"
		},
		{
			4,
			"kr"
		}
	};

	public static readonly Dictionary<SystemLanguage, CountrySetting.CountryCode> SystemLangCountryCode = new Dictionary<SystemLanguage, CountrySetting.CountryCode>
	{
		{
			SystemLanguage.Chinese,
			CountrySetting.CountryCode.CN
		},
		{
			SystemLanguage.ChineseSimplified,
			CountrySetting.CountryCode.CN
		},
		{
			SystemLanguage.ChineseTraditional,
			CountrySetting.CountryCode.CN
		},
		{
			SystemLanguage.Korean,
			CountrySetting.CountryCode.KR
		}
	};

	public static void SetCountryCode(string countryCode, CountrySetting.CountryCode defaultCountryCode = CountrySetting.CountryCode.EN)
	{
		if (string.IsNullOrEmpty(countryCode) || !CountrySetting.CountryPrefix.ContainsKey(int.Parse(countryCode)))
		{
			int num = (int)defaultCountryCode;
			countryCode = num.ToString();
		}
		PlayerPrefs.SetString("PlayerCountryCode", countryCode);
		PlayerPrefs.Save();
		if (DataMng.Instance().RespDataUS_PlayerInfo != null)
		{
			DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.countryCode = countryCode;
		}
	}

	public static string GetCountryCode(CountrySetting.CountryCode defaultCountryCode = CountrySetting.CountryCode.EN)
	{
		string text = PlayerPrefs.GetString("PlayerCountryCode");
		if (string.IsNullOrEmpty(text) && DataMng.Instance().RespDataUS_PlayerInfo != null)
		{
			string countryCode = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.countryCode;
			if (!string.IsNullOrEmpty(countryCode))
			{
				text = countryCode;
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			text = CountrySetting.GetSystemCountryCode(CountrySetting.CountryCode.EN);
		}
		else if (!CountrySetting.CountryPrefix.ContainsKey(int.Parse(text)))
		{
			int num = (int)defaultCountryCode;
			text = num.ToString();
		}
		return text;
	}

	public static string GetCountryPrefix(CountrySetting.CountryCode defaultCountryCode = CountrySetting.CountryCode.EN)
	{
		string countryCode = CountrySetting.GetCountryCode(defaultCountryCode);
		return CountrySetting.CountryPrefix[int.Parse(countryCode)];
	}

	public static bool IsReloadRequired(string countryCode)
	{
		return string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerCountryCode")) || !countryCode.Equals(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
	}

	public static string GetSystemCountryCode(CountrySetting.CountryCode defaultCountryCode = CountrySetting.CountryCode.EN)
	{
		SystemLanguage systemLanguage = Application.systemLanguage;
		if (CountrySetting.SystemLangCountryCode.ContainsKey(systemLanguage))
		{
			return ((int)CountrySetting.SystemLangCountryCode[systemLanguage]).ToString();
		}
		int num = (int)defaultCountryCode;
		return num.ToString();
	}

	public static void ReloadMaster()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);
		foreach (FileInfo fileInfo in directoryInfo.GetFiles())
		{
			string name = fileInfo.Name;
			if (name.StartsWith("MA_"))
			{
				File.Delete(Application.persistentDataPath + "/" + name);
			}
		}
		MasterDataMng.Instance().ClearCache();
		MissionBannerCacheBuffer.ClearCacheBuffer();
		MonsterIconCacheBuffer.ClearCacheBuffer();
		PresentBoxItemIconCacheBuffer.ClearCacheBuffer();
		TitleIconCacheBuffer.ClearCacheBuffer();
		StringMaster.Reload();
		AlertMaster.Reload();
	}

	public static void ConvertTMProText(ref TextMeshPro textMeshPro)
	{
		string text = string.Format("<font=\"US/{0}\" material=\"US/mat/{1}\">{2}</font>", textMeshPro.font.name, textMeshPro.fontSharedMaterial.name, textMeshPro.text);
		textMeshPro.text = text;
	}

	public enum CountryCode
	{
		JP = 1,
		EN,
		CN,
		KR
	}
}
