﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityExtension;

[Serializable]
public class AffectEffectProperty
{
	private const int IntValueLength = 2;

	private const int FloatValueLength = 6;

	[FormerlySerializedAs("_numbers")]
	[SerializeField]
	private EffectNumbers _effectNumbers;

	[SerializeField]
	private global::Attribute _attribute;

	[SerializeField]
	private AffectEffect _type;

	[SerializeField]
	private PowerType _powerType;

	[SerializeField]
	private TechniqueType _techniqueType;

	[FormerlySerializedAs("intValue")]
	[SerializeField]
	private int[] _intValue = new int[2];

	[FormerlySerializedAs("floatValue")]
	[SerializeField]
	private float[] _floatValue = new float[6];

	public AffectEffectProperty()
	{
	}

	public AffectEffectProperty(AffectEffect type, int skillId, float hitRate, EffectTarget target, EffectNumbers effectNumbers, int[] intValue, float[] floatValue, bool useDrain, PowerType powerType, TechniqueType techniqueType, global::Attribute attribute, bool isMissThrough)
	{
		this._type = type;
		this.skillId = skillId;
		this.hitRate = hitRate;
		this._techniqueType = techniqueType;
		this._attribute = attribute;
		this.target = target;
		this._effectNumbers = effectNumbers;
		this._powerType = powerType;
		this._floatValue = floatValue;
		this._intValue = intValue;
		this.useDrain = useDrain;
		this.isMissThrough = isMissThrough;
	}

	public EffectTarget target { get; private set; }

	public AffectEffect type
	{
		get
		{
			return this._type;
		}
	}

	public TechniqueType techniqueType
	{
		get
		{
			return this._techniqueType;
		}
	}

	public int skillId { get; private set; }

	private int power
	{
		get
		{
			return this._intValue[1];
		}
	}

	public int GetPower(CharacterStateControl characterStateControl = null)
	{
		int result = this.power;
		if (characterStateControl == null)
		{
			return result;
		}
		if (this.type == AffectEffect.HpBorderlineDamage || this.type == AffectEffect.HpBorderlineSpDamage)
		{
			float num = (float)characterStateControl.hp / (float)characterStateControl.maxHp;
			if (num >= this.borderlineRange1)
			{
				result = (int)this.borderlineDamage1;
			}
			else if (num >= this.borderlineRange2)
			{
				result = (int)this.borderlineDamage2;
			}
			else
			{
				result = (int)this.defaultDamage;
			}
		}
		return result;
	}

	public int upPower
	{
		get
		{
			return this._intValue[1];
		}
	}

	public int downPower
	{
		get
		{
			return this._intValue[1];
		}
	}

	public int revivalPower
	{
		get
		{
			return this._intValue[1];
		}
	}

	public int damagePower
	{
		get
		{
			return this._intValue[1];
		}
	}

	public float damagePercent
	{
		get
		{
			return this._floatValue[0];
		}
	}

	public float hitRate { get; private set; }

	public float satisfactionRate
	{
		get
		{
			return this._floatValue[1];
		}
	}

	public float incidenceRate
	{
		get
		{
			return this._floatValue[0];
		}
	}

	public float upPercent
	{
		get
		{
			return this._floatValue[0];
		}
	}

	public float downPercent
	{
		get
		{
			return this._floatValue[0];
		}
	}

	public float revivalPercent
	{
		get
		{
			return this._floatValue[0];
		}
	}

	public float recieveDamageRate
	{
		get
		{
			return this._floatValue[0];
		}
	}

	public float damageGetupIncidenceRate
	{
		get
		{
			return this._floatValue[1];
		}
	}

	public float selfGetupIncidenceRate
	{
		get
		{
			return this._floatValue[0];
		}
	}

	public int keepRoundNumber
	{
		get
		{
			return this._intValue[0];
		}
	}

	public int hitNumber
	{
		get
		{
			return this._intValue[0];
		}
	}

	public PowerType powerType
	{
		get
		{
			return this._powerType;
		}
	}

	public EffectNumbers effectNumbers
	{
		get
		{
			return this._effectNumbers;
		}
	}

	public global::Attribute attribute
	{
		get
		{
			return this._attribute;
		}
	}

	public float clearPoisonIncidenceRate
	{
		get
		{
			return this._floatValue[0];
		}
	}

	public float clearConfusionIncidenceRate
	{
		get
		{
			return this._floatValue[1];
		}
	}

	public float clearParalysisIncidenceRate
	{
		get
		{
			return this._floatValue[2];
		}
	}

	public float clearSleepIncidenceRate
	{
		get
		{
			return this._floatValue[3];
		}
	}

	public float clearStunIncidenceRate
	{
		get
		{
			return this._floatValue[4];
		}
	}

	public float clearSkillLockIncidenceRate
	{
		get
		{
			return this._floatValue[5];
		}
	}

	public bool useDrain { get; private set; }

	public float turnRate
	{
		get
		{
			return this._floatValue[1];
		}
	}

	public float maxValue
	{
		get
		{
			if (this._floatValue[2] <= 0f)
			{
				return float.MaxValue;
			}
			return this._floatValue[2];
		}
	}

	public int chargeRoundNumber
	{
		get
		{
			return this._intValue[0];
		}
	}

	public float physicUpPercent
	{
		get
		{
			return this._floatValue[0];
		}
	}

	public float specialUpPercent
	{
		get
		{
			return this._floatValue[1];
		}
	}

	public bool isMultiHitThrough
	{
		get
		{
			return this._intValue[0] > 0;
		}
	}

	public int damageRateKeepRoundNumber
	{
		get
		{
			return this._intValue[0];
		}
	}

	public float damageRateForPhantomStudents
	{
		get
		{
			return this._floatValue[0];
		}
	}

	public float damageRateForHeatHaze
	{
		get
		{
			return this._floatValue[1];
		}
	}

	public float damageRateForGlacier
	{
		get
		{
			return this._floatValue[2];
		}
	}

	public float damageRateForElectromagnetic
	{
		get
		{
			return this._floatValue[3];
		}
	}

	public float damageRateForEarth
	{
		get
		{
			return this._floatValue[4];
		}
	}

	public float damageRateForShaftOfLight
	{
		get
		{
			return this._floatValue[5];
		}
	}

	public float damageRateForAbyss
	{
		get
		{
			return this._floatValue[6];
		}
	}

	public int apDrainPower
	{
		get
		{
			return this._intValue[1];
		}
	}

	public float defaultDamage
	{
		get
		{
			return this._floatValue[2];
		}
	}

	public float borderlineDamage1
	{
		get
		{
			return this._floatValue[3];
		}
	}

	public float borderlineRange1
	{
		get
		{
			return this._floatValue[4];
		}
	}

	public float borderlineDamage2
	{
		get
		{
			return this._floatValue[5];
		}
	}

	public float borderlineRange2
	{
		get
		{
			return this._floatValue[6];
		}
	}

	public int DefenseThrough
	{
		get
		{
			return (int)this._floatValue[2];
		}
	}

	public bool ThisSkillIsAttack
	{
		get
		{
			return AffectEffectProperty.IsDamage(this.type);
		}
	}

	public static bool IsDamage(AffectEffect affectEffect)
	{
		return affectEffect == AffectEffect.Damage || affectEffect == AffectEffect.ReferenceTargetHpRate || affectEffect == AffectEffect.HpBorderlineDamage || affectEffect == AffectEffect.HpBorderlineSpDamage || affectEffect == AffectEffect.DefenseThroughDamage || affectEffect == AffectEffect.SpDefenseThroughDamage;
	}

	public bool isMissThrough { get; private set; }

	public bool OnHit(CharacterStateControl target)
	{
		return this.OnHit(null, target);
	}

	public bool OnHit(CharacterStateControl attacker, CharacterStateControl target)
	{
		if (SkillStatus.onHitRate100Percent)
		{
			return true;
		}
		float num = this.hitRate;
		if (attacker != null)
		{
			num += this.GetSkillHitRateCorrectionValue(attacker);
		}
		if (this.powerType != PowerType.Fixable && Tolerance.OnInfluenceToleranceAffectEffect(this.type))
		{
			Tolerance tolerance = target.tolerance;
			Strength affectEffectStrength = tolerance.GetAffectEffectStrength(this.type);
			if (affectEffectStrength == Strength.Strong)
			{
				num *= 0.5f;
			}
			else if (affectEffectStrength == Strength.Weak)
			{
				num *= 1.5f;
			}
			else if (affectEffectStrength == Strength.Invalid)
			{
				num *= 0f;
			}
		}
		return RandomExtension.Switch(Mathf.Clamp01(num));
	}

	private float GetSkillHitRateCorrectionValue(CharacterStateControl attacker)
	{
		float num = 0f;
		List<ExtraEffectStatus> extraEffectStatus = BattleStateManager.current.battleStateData.extraEffectStatus;
		List<ExtraEffectStatus> invocationList = ExtraEffectStatus.GetInvocationList(extraEffectStatus, EffectStatusBase.EffectTriggerType.Usually);
		num += ExtraEffectStatus.GetSkillHitRateCorrectionValue(invocationList, this, attacker) - this.hitRate;
		num += attacker.chipAddHit;
		foreach (int num2 in attacker.potencyChipIdList.Keys)
		{
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffectDataToId = ChipDataMng.GetChipEffectDataToId(num2.ToString());
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects = new GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[]
			{
				chipEffectDataToId
			};
			num += ChipEffectStatus.GetSkillHitRateCorrectionValue(chipEffects, this, attacker);
		}
		if (this.powerType != PowerType.Fixable)
		{
			if (attacker.currentSufferState.FindSufferState(SufferStateProperty.SufferType.HitRateUp))
			{
				num += attacker.currentSufferState.onHitRateUp.upPercent;
			}
			if (attacker.currentSufferState.FindSufferState(SufferStateProperty.SufferType.HitRateDown))
			{
				num -= attacker.currentSufferState.onHitRateDown.downPercent;
			}
			num += attacker.leaderSkillResult.hitRateUpPercent;
		}
		return num;
	}

	public int GetHate()
	{
		if (AffectEffectProperty.IsDamage(this.type))
		{
			return 20;
		}
		if (this.type == AffectEffect.Poison || this.type == AffectEffect.Paralysis || this.type == AffectEffect.Sleep || this.type == AffectEffect.SkillLock || this.type == AffectEffect.InstantDeath || this.type == AffectEffect.Confusion || this.type == AffectEffect.Stun)
		{
			return 10;
		}
		if (this.type == AffectEffect.HpRevival)
		{
			return 15;
		}
		return 5;
	}
}
