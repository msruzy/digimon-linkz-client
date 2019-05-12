﻿using Master;
using Monster;
using MonsterIcon;
using MonsterPicturebook;
using System;
using UnityEngine;

public class PicturebookMonsterIcon : GUIListPartBS
{
	[SerializeField]
	private UISprite spBASE;

	[SerializeField]
	private UITexture txCHAR;

	[SerializeField]
	private UISprite spFRAME;

	[SerializeField]
	private UILabel ngTX_IS_UNKOWN;

	private Action<PicturebookMonster> _actTouchShort;

	private PicturebookMonster _monsterData;

	private Vector2 _beganPosition;

	private void Start()
	{
		this.ngTX_IS_UNKOWN.text = StringMaster.GetString("EvolutionUnkown");
	}

	public void SetMonsterIcon(PicturebookItem.TextureData textureData, string growStep, bool isUnknown)
	{
		if (null == this.txCHAR.material)
		{
			Shader iconShader = GUIMonsterIcon.GetIconShader();
			this.txCHAR.material = new Material(iconShader);
		}
		if (!textureData._isMainTexture)
		{
			this.txCHAR.material.SetTexture("_MaskTex", textureData._monsterAlphaTexture);
			this.txCHAR.material.SetTexture("_MainTex", textureData._monsterTexture);
		}
		else
		{
			NGUIUtil.ChangeUITexture(this.txCHAR, textureData._monsterTexture, false);
			this.txCHAR.material.SetTexture("_MaskTex", Texture2D.whiteTexture);
		}
		int growStep2 = int.Parse(growStep);
		PicturebookMonsterIcon.SetThumbnailFrame(this.spBASE, this.spFRAME, growStep2);
		this.ngTX_IS_UNKOWN.gameObject.SetActive(isUnknown);
		MonsterIconGrayout.SetGrayout(base.gameObject, (!isUnknown) ? GUIMonsterIcon.DIMM_LEVEL.ACTIVE : GUIMonsterIcon.DIMM_LEVEL.DISABLE);
	}

	public void SetMonsterData(PicturebookMonster monsterData)
	{
		this._monsterData = monsterData;
	}

	public void SetTouchAct_S(Action<PicturebookMonster> act)
	{
		this._actTouchShort = act;
	}

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		this._beganPosition = pos;
		base.OnTouchBegan(touch, pos);
	}

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchEnded(touch, pos, flag);
		if (flag)
		{
			float magnitude = (this._beganPosition - pos).magnitude;
			if (magnitude < 40f && this._actTouchShort != null)
			{
				this._actTouchShort(this._monsterData);
			}
		}
	}

	public static void SetTextureMonsterParts(ref PicturebookItem.TextureData textureData, string resourcePath, string assetBundlePath)
	{
		string localizedPath = AssetDataMng.GetLocalizedPath(resourcePath);
		Texture2D texture2D = Resources.Load(localizedPath) as Texture2D;
		Texture2D texture2D2 = Resources.Load(localizedPath + "_alpha") as Texture2D;
		Texture2D texture2D3 = texture2D ?? (Resources.Load(resourcePath) as Texture2D);
		Texture2D texture2D4 = texture2D2 ?? (Resources.Load(resourcePath + "_alpha") as Texture2D);
		string localizedPath2 = AssetDataMng.GetLocalizedPath(assetBundlePath);
		string path = localizedPath2 + "_alpha";
		if (texture2D3 == null && texture2D4 == null)
		{
			Texture2D texture2D5 = MonsterIconCacheBuffer.Instance().LoadAndCacheObj(localizedPath2, null) as Texture2D;
			Texture2D monsterAlphaTexture = MonsterIconCacheBuffer.Instance().LoadAndCacheObj(path, null) as Texture2D;
			if (null != texture2D5)
			{
				textureData._monsterTexture = texture2D5;
				textureData._isMainTexture = true;
			}
			if (null != texture2D5)
			{
				textureData._monsterAlphaTexture = monsterAlphaTexture;
			}
		}
		else
		{
			textureData._monsterTexture = texture2D3;
			textureData._monsterAlphaTexture = texture2D4;
			textureData._isMainTexture = false;
		}
	}

	public static void SetThumbnailFrame(UISprite background, UISprite frame, int growStep)
	{
		if (MonsterGrowStepData.IsEggScope(growStep) || MonsterGrowStepData.IsChild1Scope(growStep) || MonsterGrowStepData.IsChild2Scope(growStep))
		{
			background.spriteName = "Common02_Thumbnail_bg1";
			frame.spriteName = "Common02_Thumbnail_waku1";
		}
		else if (MonsterGrowStepData.IsGrowingScope(growStep))
		{
			background.spriteName = "Common02_Thumbnail_bg2";
			frame.spriteName = "Common02_Thumbnail_waku2";
		}
		else if (MonsterGrowStepData.IsRipeScope(growStep))
		{
			background.spriteName = "Common02_Thumbnail_bg3";
			frame.spriteName = "Common02_Thumbnail_waku3";
		}
		else if (MonsterGrowStepData.IsPerfectScope(growStep))
		{
			background.spriteName = "Common02_Thumbnail_bg4";
			frame.spriteName = "Common02_Thumbnail_waku4";
		}
		else if (MonsterGrowStepData.IsUltimateScope(growStep))
		{
			background.spriteName = "Common02_Thumbnail_bg5";
			frame.spriteName = "Common02_Thumbnail_waku5";
		}
		else
		{
			background.spriteName = "Common02_Thumbnail_Question";
			frame.spriteName = "Common02_Thumbnail_wakuQ";
		}
	}
}
