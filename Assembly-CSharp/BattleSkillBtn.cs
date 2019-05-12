﻿using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleSkillBtn : MonoBehaviour
{
	[SerializeField]
	[Header("スキルボタンが2つのときの位置")]
	private Vector3 twoButtonPosition = Vector3.zero;

	[SerializeField]
	[Header("スキルボタンが3つのときの位置")]
	private Vector3 threeButtonPosition = Vector3.zero;

	[SerializeField]
	[Header("ボタン")]
	private UIButton button;

	[SerializeField]
	[Header("スキル名")]
	private UILabel skillName;

	[Header("スキル説明")]
	[SerializeField]
	private UILabel skillDescription;

	[SerializeField]
	[Header("必要AP数")]
	private UILabel ap;

	[SerializeField]
	[Header("命中率")]
	private UILabel hitRate;

	[SerializeField]
	[Header("威力")]
	private UILabel power;

	[SerializeField]
	[Header("Tweener1（開く）")]
	private UITweener rotationEffect1;

	[SerializeField]
	[Header("Tweener2（閉じる）")]
	private UITweener rotationEffect2;

	[Header("Collider")]
	[SerializeField]
	private Collider colliderValue;

	[Header("HoldPress")]
	[SerializeField]
	private HoldPressButton skillDescriptionSwitch;

	[SerializeField]
	[Header("属性アイコン")]
	private UISprite attributeSprite;

	[SerializeField]
	[Header("スキル説明UIのルート")]
	private GameObject skillDescriptionRoot;

	[Header("スキルロックアイコン")]
	[SerializeField]
	private UISprite skillLockSprite;

	[Header("スキルONボタン")]
	[SerializeField]
	private GameObject onSkillButton;

	[Header("実行ボタンスプライト")]
	[SerializeField]
	private UISprite execButtonSprite;

	[SerializeField]
	[Header("スキルOFfボタン")]
	private GameObject offSkillButton;

	[Header("スキルボタンスプライト")]
	[SerializeField]
	private UISprite skillButtonSprite;

	[Header("スキル使用回数の掛け算記号")]
	[SerializeField]
	private UILabel skillCountMulti;

	[SerializeField]
	[Header("スキル使用回数の数値")]
	private UILabel skillCountNum;

	private SkillType skillType;

	private void SetSkillUseCount(int value)
	{
		if (this.skillCountMulti == null || this.skillCountNum == null)
		{
			return;
		}
		if (value >= 0)
		{
			this.skillCountMulti.gameObject.SetActive(true);
			this.skillCountNum.gameObject.SetActive(true);
			this.skillCountNum.text = value.ToString("D2");
		}
		else
		{
			this.skillCountMulti.gameObject.SetActive(false);
			this.skillCountNum.gameObject.SetActive(false);
		}
	}

	public void Awake()
	{
	}

	public void ApplySkillButtonData(SkillStatus skills, bool onEnable, bool onSkillLock, CharacterStateControl selectCharacter, int useCount)
	{
		List<ExtraEffectStatus> extraEffectStatus = BattleStateManager.current.battleStateData.extraEffectStatus;
		List<ExtraEffectStatus> invocationList = ExtraEffectStatus.GetInvocationList(extraEffectStatus, EffectStatusBase.EffectTriggerType.Usually);
		AffectEffectProperty affectEffectProperty = skills.GetAffectEffectFirst();
		foreach (AffectEffectProperty affectEffectProperty2 in skills.affectEffect)
		{
			if (affectEffectProperty2.ThisSkillIsAttack)
			{
				affectEffectProperty = affectEffectProperty2;
				break;
			}
		}
		int num = 0;
		if (AffectEffectProperty.IsDamage(affectEffectProperty.type))
		{
			num = ExtraEffectStatus.GetSkillPowerCorrectionValue(invocationList, affectEffectProperty, selectCharacter);
		}
		float num2 = ExtraEffectStatus.GetSkillHitRateCorrectionValue(invocationList, affectEffectProperty, selectCharacter);
		num2 = Mathf.Clamp01(num2);
		this.skillType = skills.skillType;
		this.skillName.text = skills.name;
		this.skillDescription.text = skills.description;
		int num3 = (int)Math.Round((double)(num2 * 100f), MidpointRounding.AwayFromZero);
		string @string = StringMaster.GetString("SystemPercent");
		this.hitRate.text = string.Format(@string, num3);
		this.power.text = num.ToString();
		int correctedAp = skills.GetCorrectedAp(selectCharacter);
		if (correctedAp <= selectCharacter.ap)
		{
			this.ap.text = string.Format(StringMaster.GetString("BattleSkillUI-01"), correctedAp);
		}
		else
		{
			this.ap.text = StringMaster.GetString("BattleSkillUI-02");
		}
		if (skills.useCountValue > 0)
		{
			this.SetSkillUseCount(useCount);
		}
		this.SetAttributeIcon(skills.attribute);
		if (onSkillLock)
		{
			this.SetButtonType(BattleSkillBtn.Type.Invalid);
			this.SetSkillLock(onSkillLock);
		}
		else if (onEnable)
		{
			this.SetButtonType(BattleSkillBtn.Type.Off);
			this.SetSkillLock(false);
		}
		else
		{
			this.SetButtonType(BattleSkillBtn.Type.Invalid);
			this.SetSkillLock(false);
		}
	}

	public void SetColliderActive(bool active)
	{
		if (this.colliderValue != null)
		{
			this.colliderValue.enabled = active;
		}
	}

	public void Reflesh()
	{
		this.rotationEffect2.enabled = false;
		this.rotationEffect2.ResetToBeginning();
		this.rotationEffect2.tweenFactor = 0f;
		this.rotationEffect1.enabled = false;
		this.rotationEffect1.ResetToBeginning();
		this.rotationEffect1.tweenFactor = 0f;
		this.rotationEffect1.transform.localScale = Vector3.one;
		this.rotationEffect2.transform.localScale = Vector3.one;
		this.SetColliderActive(true);
		this.SetButtonType(BattleSkillBtn.Type.Off);
		this.SetSkillUseCount(-1);
	}

	public void ApplySkillDescriptionEnable(bool onEnable)
	{
		if (this.skillDescriptionRoot != null)
		{
			this.skillDescriptionRoot.SetActive(onEnable);
		}
	}

	public void SetButtonCallback(Action<int> callback, int value)
	{
		BattleInputUtility.AddEvent(this.button.onClick, callback, value);
	}

	public void SetClickCallback(Action<int> callback, int value)
	{
		BattleInputUtility.AddEvent(this.skillDescriptionSwitch.onClick, callback, value);
	}

	public void SetHoldWaitPressCallback(Action<int> callback, int value)
	{
		BattleInputUtility.AddEvent(this.skillDescriptionSwitch.onHoldWaitPress, callback, value);
	}

	public void SetDisengagePressCallback(Action<int> callback, int value)
	{
		BattleInputUtility.AddEvent(this.skillDescriptionSwitch.onDisengagePress, callback, value);
	}

	public void SetRotationEffectCallback(Action<int> callback, int value)
	{
		UITweenEventSystem component = this.rotationEffect1.gameObject.GetComponent<UITweenEventSystem>();
		BattleInputUtility.AddEvent(component.onFinished, callback, value);
		UITweenEventSystem component2 = this.rotationEffect2.gameObject.GetComponent<UITweenEventSystem>();
		BattleInputUtility.AddEvent(component2.onFinished, callback, value);
	}

	public void PlayOpenRotationEffect()
	{
		this.rotationEffect1.enabled = true;
		this.rotationEffect1.PlayForward();
	}

	public void PlayCloseRotationEffect()
	{
		this.rotationEffect2.enabled = true;
		this.rotationEffect2.PlayForward();
	}

	public void SetButtonType(BattleSkillBtn.Type type)
	{
		switch (type)
		{
		case BattleSkillBtn.Type.On:
			this.onSkillButton.SetActive(true);
			this.offSkillButton.SetActive(false);
			if (this.skillType == SkillType.Attack)
			{
				this.skillButtonSprite.spriteName = "Battle_Attackbtn";
			}
			else
			{
				this.skillButtonSprite.spriteName = "Battle_Skillbtn";
			}
			break;
		case BattleSkillBtn.Type.Off:
			this.onSkillButton.SetActive(false);
			this.offSkillButton.SetActive(true);
			if (this.skillType == SkillType.Attack)
			{
				this.skillButtonSprite.spriteName = "Battle_Attackbtn";
			}
			else
			{
				this.skillButtonSprite.spriteName = "Battle_Skillbtn";
			}
			break;
		case BattleSkillBtn.Type.Invalid:
			this.onSkillButton.SetActive(false);
			this.offSkillButton.SetActive(true);
			this.skillButtonSprite.spriteName = "Battle_Skillbtn_g";
			break;
		}
	}

	private void SetAttributeIcon(global::Attribute attribute)
	{
		if (this.attributeSprite == null)
		{
			return;
		}
		switch (attribute)
		{
		case global::Attribute.None:
			this.attributeSprite.spriteName = "Battle_Attribute_1";
			break;
		case global::Attribute.Red:
			this.attributeSprite.spriteName = "Battle_Attribute_2";
			break;
		case global::Attribute.Blue:
			this.attributeSprite.spriteName = "Battle_Attribute_3";
			break;
		case global::Attribute.Yellow:
			this.attributeSprite.spriteName = "Battle_Attribute_4";
			break;
		case global::Attribute.Green:
			this.attributeSprite.spriteName = "Battle_Attribute_5";
			break;
		case global::Attribute.White:
			this.attributeSprite.spriteName = "Battle_Attribute_6";
			break;
		case global::Attribute.Black:
			this.attributeSprite.spriteName = "Battle_Attribute_7";
			break;
		}
	}

	private void SetSkillLock(bool islock)
	{
		if (this.skillLockSprite != null)
		{
			this.skillLockSprite.gameObject.SetActive(islock);
		}
	}

	public void ApplyTwoButtonPosition()
	{
		base.transform.localPosition = this.twoButtonPosition;
	}

	public void ApplyThreeButtonPosition()
	{
		base.transform.localPosition = this.threeButtonPosition;
	}

	public enum Type
	{
		On,
		Off,
		Invalid
	}
}
