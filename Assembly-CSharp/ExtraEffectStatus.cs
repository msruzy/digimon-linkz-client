﻿using BattleStateMachineInternal;
using Monster;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ExtraEffectStatus : EffectStatusBase
{
	[SerializeField]
	private string _worldDungeonExtraEffectId;

	[SerializeField]
	private EffectStatusBase.ExtraEffectType _extraEffectType;

	[SerializeField]
	private int _targetType;

	[SerializeField]
	private int _targetSubType;

	[SerializeField]
	private int _targetValue;

	[SerializeField]
	private string _targetValue2;

	[SerializeField]
	private int _effectType;

	[SerializeField]
	private int _effectSubType;

	[SerializeField]
	private float _effectValue;

	[SerializeField]
	private int _effectTrigger;

	[SerializeField]
	private string _effectTriggerValue;

	public ExtraEffectStatus()
	{
	}

	public ExtraEffectStatus(GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM worldDungeonExtraEffectM)
	{
		this._worldDungeonExtraEffectId = worldDungeonExtraEffectM.worldDungeonExtraEffectId;
		this._targetType = worldDungeonExtraEffectM.targetType.ToInt32();
		this._targetSubType = worldDungeonExtraEffectM.targetSubType.ToInt32();
		this._targetValue = worldDungeonExtraEffectM.targetValue.ToInt32();
		this._targetValue2 = worldDungeonExtraEffectM.targetValue2;
		this._effectType = worldDungeonExtraEffectM.effectType.ToInt32();
		this._effectSubType = worldDungeonExtraEffectM.effectSubType.ToInt32();
		this._effectValue = worldDungeonExtraEffectM.effectValue.ToFloat();
		this._effectTrigger = worldDungeonExtraEffectM.effectTrigger.ToInt32();
		this._effectTriggerValue = worldDungeonExtraEffectM.effectTriggerValue;
	}

	public string name
	{
		get
		{
			return this._extraEffectType.ToString();
		}
	}

	public string WorldDungeonExtraEffectId
	{
		get
		{
			return this._worldDungeonExtraEffectId;
		}
	}

	public int TargetType
	{
		get
		{
			return this._targetType;
		}
	}

	public int TargetSubType
	{
		get
		{
			return this._targetSubType;
		}
	}

	public int TargetValue
	{
		get
		{
			return this._targetValue;
		}
	}

	public string TargetValue2
	{
		get
		{
			return this._targetValue2;
		}
	}

	public int EffectType
	{
		get
		{
			return this._effectType;
		}
	}

	public int EffectSubType
	{
		get
		{
			return this._effectSubType;
		}
	}

	public float EffectValue
	{
		get
		{
			return this._effectValue;
		}
	}

	public int EffectTrigger
	{
		get
		{
			return this._effectTrigger;
		}
	}

	public string EffectTriggerValue
	{
		get
		{
			return this._effectTriggerValue;
		}
	}

	public static List<ExtraEffectStatus> GetInvocationList(List<ExtraEffectStatus> extraEffectStatuses, EffectStatusBase.EffectTriggerType effectTriggerType)
	{
		List<ExtraEffectStatus> list = new List<ExtraEffectStatus>();
		foreach (ExtraEffectStatus extraEffectStatus in extraEffectStatuses)
		{
			EffectStatusBase.EffectTriggerType effectTrigger = (EffectStatusBase.EffectTriggerType)extraEffectStatus.EffectTrigger;
			if (effectTrigger == effectTriggerType)
			{
				list.Add(extraEffectStatus);
			}
		}
		List<ExtraEffectStatus> list2 = new List<ExtraEffectStatus>();
		if (effectTriggerType != EffectStatusBase.EffectTriggerType.WaveStarted)
		{
			if (effectTriggerType != EffectStatusBase.EffectTriggerType.RoundEnd)
			{
				list2.AddRange(list);
			}
			else
			{
				List<ExtraEffectStatus> loopPlayExtraEffectStatusList = ExtraEffectStatus.GetLoopPlayExtraEffectStatusList(list, BattleStateManager.current.battleStateData.currentRoundNumber);
				list2.AddRange(loopPlayExtraEffectStatusList);
			}
		}
		else
		{
			int targetNumber = BattleStateManager.current.battleStateData.currentWaveNumber + 1;
			List<ExtraEffectStatus> loopPlayExtraEffectStatusList2 = ExtraEffectStatus.GetLoopPlayExtraEffectStatusList(list, targetNumber);
			list2.AddRange(loopPlayExtraEffectStatusList2);
		}
		return list2;
	}

	private static List<ExtraEffectStatus> GetLoopPlayExtraEffectStatusList(List<ExtraEffectStatus> list, int targetNumber)
	{
		List<ExtraEffectStatus> list2 = new List<ExtraEffectStatus>();
		foreach (ExtraEffectStatus extraEffectStatus in list)
		{
			if (string.IsNullOrEmpty(extraEffectStatus.EffectTriggerValue) || extraEffectStatus.EffectTriggerValue == "0")
			{
				list2.Add(extraEffectStatus);
			}
			else
			{
				string[] array = extraEffectStatus.EffectTriggerValue.Split(new char[]
				{
					','
				});
				if (array.Length == 1)
				{
					if (array[0].ToInt32() == targetNumber)
					{
						list2.Add(extraEffectStatus);
					}
				}
				else
				{
					int num = array[1].ToInt32();
					int num2 = targetNumber % num;
					if (num2 == 0)
					{
						num2 = num;
					}
					if (array[0].ToInt32() == num2)
					{
						list2.Add(extraEffectStatus);
					}
				}
			}
		}
		return list2;
	}

	public static int GetExtraEffectValue(List<ExtraEffectStatus> extraEffectStatusList, int baseValue, CharacterStateControl character, EffectStatusBase.ExtraEffectType effectType)
	{
		List<ExtraEffectStatus> list = ChipEffectStatus.CheckStageEffectInvalid(extraEffectStatusList, character);
		if (list.Count == 0)
		{
			return baseValue;
		}
		return (int)ExtraEffectStatus.GetExtraEffectCorrectionValue(list, (float)baseValue, character.characterStatus.monsterIntegrationIds, character.groupId, character.tolerance, character.characterDatas.tribe, character.characterDatas.growStep, null, character.currentSufferState, ExtraEffectStatus.GetExtraTargetType(character), effectType);
	}

	private static float GetExtraEffectCorrectionValue(List<ExtraEffectStatus> extraEffectStatusList, float baseValue, string[] monsterIntegrationIds, string groupId, Tolerance tolerance, string tribe, GrowStep growStep, AffectEffectProperty skillPropety, HaveSufferState currentSufferState, ExtraEffectStatus.ExtraTargetType targetType, EffectStatusBase.ExtraEffectType effectType)
	{
		List<ExtraEffectStatus> totalExtraEffectStatusList = ExtraEffectStatus.GetTotalExtraEffectStatusList(extraEffectStatusList, monsterIntegrationIds, groupId, tolerance, tribe, growStep, skillPropety, currentSufferState, targetType, effectType);
		if (totalExtraEffectStatusList.Count > 0)
		{
			return ExtraEffectStatus.GetCorrectionValue(baseValue, totalExtraEffectStatusList);
		}
		return baseValue;
	}

	public static int GetExtraEffectCorrectionValue(int areaId, List<ExtraEffectStatus> extraEffectStatusList, int baseValue, MonsterData[] chipPlayers, MonsterData[] chipEnemys, MonsterData chipTarget, AffectEffectProperty affectEffectProperty, EffectStatusBase.ExtraEffectType effectType)
	{
		List<ExtraEffectStatus> list = ChipEffectStatus.CheckStageEffectInvalid(areaId, extraEffectStatusList, chipPlayers, chipEnemys, chipTarget);
		if (list.Count == 0)
		{
			return baseValue;
		}
		bool flag = chipEnemys.Where((MonsterData item) => item.userMonster.userMonsterId == chipTarget.userMonster.userMonsterId).Any<MonsterData>();
		GameWebAPI.RespDataMA_GetMonsterMG.MonsterM group = MonsterMaster.GetMonsterMasterByMonsterGroupId(chipTarget.monsterM.monsterGroupId).Group;
		GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster responseMonsterIntegrationGroupMaster = MasterDataMng.Instance().ResponseMonsterIntegrationGroupMaster;
		GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup[] source = responseMonsterIntegrationGroupMaster.monsterIntegrationGroupM.Where((GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup item) => item.monsterId == chipTarget.monsterM.monsterId).ToArray<GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup>();
		string[] monsterIntegrationIds = source.Select((GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup item) => item.monsterIntegrationId).ToArray<string>();
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceMaster = MonsterResistanceData.GetResistanceMaster(chipTarget.monsterM.resistanceId);
		List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> uniqueResistanceList = MonsterResistanceData.GetUniqueResistanceList(chipTarget.GetResistanceIdList());
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM data = MonsterResistanceData.AddResistanceFromMultipleTranceData(resistanceMaster, uniqueResistanceList);
		Tolerance tolerance = ServerToBattleUtility.ResistanceToTolerance(data);
		GrowStep growStep = MonsterGrowStepData.ToGrowStep(group.growStep);
		List<ExtraEffectStatus> totalExtraEffectStatusList = ExtraEffectStatus.GetTotalExtraEffectStatusList(list, monsterIntegrationIds, chipTarget.monsterM.monsterGroupId, tolerance, group.tribe, growStep, affectEffectProperty, null, (!flag) ? ExtraEffectStatus.ExtraTargetType.Player : ExtraEffectStatus.ExtraTargetType.Enemy, effectType);
		if (totalExtraEffectStatusList.Count > 0)
		{
			return (int)ExtraEffectStatus.GetCorrectionValue((float)baseValue, totalExtraEffectStatusList);
		}
		return baseValue;
	}

	public static int GetSkillPowerCorrectionValue(List<ExtraEffectStatus> extraEffectStatusList, AffectEffectProperty skillPropety, CharacterStateControl character)
	{
		List<ExtraEffectStatus> list = ChipEffectStatus.CheckStageEffectInvalid(extraEffectStatusList, character);
		if (list.Count == 0)
		{
			return skillPropety.GetPower(character);
		}
		EffectStatusBase.ExtraEffectType effectType;
		if (skillPropety.skillId.ToString() == BattleStateManager.current.publicAttackSkillId)
		{
			effectType = EffectStatusBase.ExtraEffectType.DefaultAttackDamage;
		}
		else
		{
			effectType = EffectStatusBase.ExtraEffectType.SkillDamage;
		}
		return (int)ExtraEffectStatus.GetExtraEffectCorrectionValue(list, (float)skillPropety.GetPower(character), character.characterStatus.monsterIntegrationIds, character.groupId, character.tolerance, character.characterDatas.tribe, character.characterDatas.growStep, skillPropety, character.currentSufferState, ExtraEffectStatus.GetExtraTargetType(character), effectType);
	}

	public static float GetSkillHitRateCorrectionValue(List<ExtraEffectStatus> extraEffectStatusList, AffectEffectProperty skillPropety, CharacterStateControl character)
	{
		List<ExtraEffectStatus> list = ChipEffectStatus.CheckStageEffectInvalid(extraEffectStatusList, character);
		if (list.Count == 0)
		{
			return skillPropety.hitRate;
		}
		EffectStatusBase.ExtraEffectType effectType;
		if (skillPropety.skillId.ToString() == BattleStateManager.current.publicAttackSkillId)
		{
			effectType = EffectStatusBase.ExtraEffectType.DefaultAttackHit;
		}
		else
		{
			effectType = EffectStatusBase.ExtraEffectType.SkillHit;
		}
		int num = ServerToBattleUtility.PercentageToPermillion(skillPropety.hitRate);
		float extraEffectCorrectionValue = ExtraEffectStatus.GetExtraEffectCorrectionValue(list, (float)num, character.characterStatus.monsterIntegrationIds, character.groupId, character.tolerance, character.characterDatas.tribe, character.characterDatas.growStep, skillPropety, character.currentSufferState, ExtraEffectStatus.GetExtraTargetType(character), effectType);
		return ServerToBattleUtility.PermillionToPercentage((int)extraEffectCorrectionValue);
	}

	private static float GetCorrectionValue(float baseValue, List<ExtraEffectStatus> extraEffectStatusList)
	{
		float num = baseValue;
		float num2 = 0f;
		bool flag = false;
		float num3 = 0f;
		float num4 = 0f;
		foreach (ExtraEffectStatus extraEffectStatus in extraEffectStatusList)
		{
			if (extraEffectStatus.EffectSubType == 1)
			{
				num2 = ((num2 <= extraEffectStatus.EffectValue) ? extraEffectStatus.EffectValue : num2);
				flag = true;
			}
			else if (extraEffectStatus.EffectSubType == 2)
			{
				num3 += extraEffectStatus.EffectValue;
			}
			else if (extraEffectStatus.EffectSubType == 3)
			{
				num4 += extraEffectStatus.EffectValue;
			}
		}
		num += num4;
		num += num * (num3 / 100f);
		if (flag)
		{
			num = num2;
		}
		if (baseValue > 1f)
		{
			return Math.Max(num, 1f);
		}
		return num;
	}

	public bool IsHitExtraEffect(CharacterStateControl character, EffectStatusBase.ExtraEffectType extraEffectType)
	{
		List<ExtraEffectStatus> list = ChipEffectStatus.CheckStageEffectInvalid(new List<ExtraEffectStatus>
		{
			this
		}, character);
		if (list.Count == 0)
		{
			return false;
		}
		List<ExtraEffectStatus> totalExtraEffectStatusList = ExtraEffectStatus.GetTotalExtraEffectStatusList(list, character.characterStatus.monsterIntegrationIds, character.groupId, character.tolerance, character.characterDatas.tribe, character.characterDatas.growStep, null, null, ExtraEffectStatus.GetExtraTargetType(character), extraEffectType);
		return totalExtraEffectStatusList.Count > 0;
	}

	private static List<ExtraEffectStatus> GetTotalExtraEffectStatusList(List<ExtraEffectStatus> extraEffectStatusList, string[] monsterIntegrationIds, string groupId, Tolerance tolerance, string tribe, GrowStep growStep, AffectEffectProperty skillPropety, HaveSufferState currentSufferState, ExtraEffectStatus.ExtraTargetType targetType, EffectStatusBase.ExtraEffectType effectType)
	{
		List<ExtraEffectStatus> list = new List<ExtraEffectStatus>();
		if (skillPropety != null)
		{
			ConstValue.ResistanceType skillResistanceType = EffectStatusBase.GetSkillResistanceType(skillPropety);
			if (skillResistanceType != ConstValue.ResistanceType.NONE)
			{
				list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, EffectStatusBase.ExtraTargetSubType.Skill, 0, skillResistanceType, effectType));
			}
			list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, EffectStatusBase.ExtraTargetSubType.Skill, skillPropety.skillId, ConstValue.ResistanceType.NONE, effectType));
		}
		List<ConstValue.ResistanceType> attributeStrengthList = tolerance.GetAttributeStrengthList();
		foreach (ConstValue.ResistanceType resistanceType in attributeStrengthList)
		{
			list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, EffectStatusBase.ExtraTargetSubType.MonsterResistance, 0, resistanceType, effectType));
			list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, EffectStatusBase.ExtraTargetSubType.MonsterResistance, (int)resistanceType, ConstValue.ResistanceType.NONE, effectType));
			list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, EffectStatusBase.ExtraTargetSubType.MonsterResistance, (int)resistanceType, resistanceType, effectType));
		}
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, EffectStatusBase.ExtraTargetSubType.MonsterTribe, 0, ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, EffectStatusBase.ExtraTargetSubType.MonsterTribe, tribe.ToInt32(), ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, EffectStatusBase.ExtraTargetSubType.MonsterGroup, 0, ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, EffectStatusBase.ExtraTargetSubType.MonsterGroup, groupId.ToInt32(), ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, EffectStatusBase.ExtraTargetSubType.GrowStep, 0, ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, EffectStatusBase.ExtraTargetSubType.GrowStep, (int)growStep, ConstValue.ResistanceType.NONE, effectType));
		if (currentSufferState != null)
		{
			list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, EffectStatusBase.ExtraTargetSubType.Suffer, 0, ConstValue.ResistanceType.NONE, effectType));
			foreach (object obj in Enum.GetValues(typeof(SufferStateProperty.SufferType)))
			{
				SufferStateProperty.SufferType sufferType = (SufferStateProperty.SufferType)((int)obj);
				if (sufferType != SufferStateProperty.SufferType.Null)
				{
					if (currentSufferState.FindSufferState(sufferType))
					{
						list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, EffectStatusBase.ExtraTargetSubType.Suffer, (int)sufferType, ConstValue.ResistanceType.NONE, effectType));
					}
				}
			}
		}
		list.AddRange(ExtraEffectStatus.GetMonsterIntegrationGroupList(extraEffectStatusList, monsterIntegrationIds, targetType, effectType));
		return list;
	}

	private static List<ExtraEffectStatus> GetExtraEffectStatusList(List<ExtraEffectStatus> extraEffectStatusList, ExtraEffectStatus.ExtraTargetType targetType, EffectStatusBase.ExtraTargetSubType targetSubType, int targetValue, ConstValue.ResistanceType resistanceType, EffectStatusBase.ExtraEffectType effectType)
	{
		ExtraEffectStatus.<GetExtraEffectStatusList>c__AnonStorey2E8 <GetExtraEffectStatusList>c__AnonStorey2E = new ExtraEffectStatus.<GetExtraEffectStatusList>c__AnonStorey2E8();
		<GetExtraEffectStatusList>c__AnonStorey2E.effectType = effectType;
		<GetExtraEffectStatusList>c__AnonStorey2E.targetType = targetType;
		<GetExtraEffectStatusList>c__AnonStorey2E.targetSubType = targetSubType;
		<GetExtraEffectStatusList>c__AnonStorey2E.targetValue = targetValue;
		List<ExtraEffectStatus> list = new List<ExtraEffectStatus>();
		if (extraEffectStatusList.Count == 0)
		{
			return list;
		}
		<GetExtraEffectStatusList>c__AnonStorey2E.searchEffectType = ((int x) => x == (int)<GetExtraEffectStatusList>c__AnonStorey2E.effectType);
		if (EffectStatusBase.ExtraEffectType.Atk <= <GetExtraEffectStatusList>c__AnonStorey2E.effectType && <GetExtraEffectStatusList>c__AnonStorey2E.effectType < EffectStatusBase.ExtraEffectType.AllStatus)
		{
			<GetExtraEffectStatusList>c__AnonStorey2E.searchEffectType = ((int x) => x == (int)<GetExtraEffectStatusList>c__AnonStorey2E.effectType || x == 27);
		}
		IEnumerable<ExtraEffectStatus> enumerable;
		if (resistanceType == ConstValue.ResistanceType.NONE)
		{
			enumerable = extraEffectStatusList.Where((ExtraEffectStatus x) => (x.TargetType == (int)<GetExtraEffectStatusList>c__AnonStorey2E.targetType || x.TargetType == 0) && x.TargetSubType == (int)<GetExtraEffectStatusList>c__AnonStorey2E.targetSubType && x.TargetValue == <GetExtraEffectStatusList>c__AnonStorey2E.targetValue && <GetExtraEffectStatusList>c__AnonStorey2E.searchEffectType(x.EffectType));
		}
		else
		{
			ExtraEffectStatus.<GetExtraEffectStatusList>c__AnonStorey2E9 <GetExtraEffectStatusList>c__AnonStorey2E2 = new ExtraEffectStatus.<GetExtraEffectStatusList>c__AnonStorey2E9();
			<GetExtraEffectStatusList>c__AnonStorey2E2.<>f__ref$744 = <GetExtraEffectStatusList>c__AnonStorey2E;
			ExtraEffectStatus.<GetExtraEffectStatusList>c__AnonStorey2E9 <GetExtraEffectStatusList>c__AnonStorey2E3 = <GetExtraEffectStatusList>c__AnonStorey2E2;
			int num = (int)resistanceType;
			<GetExtraEffectStatusList>c__AnonStorey2E3.resistance = num.ToString();
			enumerable = extraEffectStatusList.Where((ExtraEffectStatus x) => (x.TargetType == (int)<GetExtraEffectStatusList>c__AnonStorey2E2.<>f__ref$744.targetType || x.TargetType == 0) && x.TargetSubType == (int)<GetExtraEffectStatusList>c__AnonStorey2E2.<>f__ref$744.targetSubType && x.TargetValue == <GetExtraEffectStatusList>c__AnonStorey2E2.<>f__ref$744.targetValue && x.TargetValue2.Contains(<GetExtraEffectStatusList>c__AnonStorey2E2.resistance) && <GetExtraEffectStatusList>c__AnonStorey2E2.<>f__ref$744.searchEffectType(x.EffectType));
		}
		foreach (ExtraEffectStatus item in enumerable)
		{
			list.Add(item);
		}
		return list;
	}

	private static List<ExtraEffectStatus> GetMonsterIntegrationGroupList(List<ExtraEffectStatus> extraEffectStatusList, string[] monsterIntegrationIds, ExtraEffectStatus.ExtraTargetType targetType, EffectStatusBase.ExtraEffectType effectType)
	{
		List<ExtraEffectStatus> list = new List<ExtraEffectStatus>();
		List<ExtraEffectStatus> list2 = new List<ExtraEffectStatus>();
		foreach (ExtraEffectStatus extraEffectStatus in extraEffectStatusList)
		{
			EffectStatusBase.ExtraTargetSubType targetSubType = (EffectStatusBase.ExtraTargetSubType)extraEffectStatus.TargetSubType;
			if (targetSubType == EffectStatusBase.ExtraTargetSubType.MonsterIntegrationGroup)
			{
				list2.Add(extraEffectStatus);
			}
		}
		while (list2.Count > 0)
		{
			ExtraEffectStatus extraEffectStatus2 = list2[0];
			List<ExtraEffectStatus> list3 = new List<ExtraEffectStatus>();
			List<ExtraEffectStatus> list4 = new List<ExtraEffectStatus>();
			foreach (ExtraEffectStatus extraEffectStatus3 in list2)
			{
				if (extraEffectStatus3.TargetValue == extraEffectStatus2.TargetValue)
				{
					list3.Add(extraEffectStatus3);
				}
				else
				{
					list4.Add(extraEffectStatus3);
				}
			}
			string id = extraEffectStatus2.TargetValue.ToString();
			bool flag = monsterIntegrationIds.Where((string item) => item == id).Any<string>();
			if (flag)
			{
				List<ExtraEffectStatus> extraEffectStatusList2 = ExtraEffectStatus.GetExtraEffectStatusList(list3, targetType, EffectStatusBase.ExtraTargetSubType.MonsterIntegrationGroup, extraEffectStatus2.TargetValue, ConstValue.ResistanceType.NONE, effectType);
				if (extraEffectStatusList2.Count > 0)
				{
					list.AddRange(extraEffectStatusList2);
				}
			}
			list2 = list4;
		}
		return list;
	}

	private static ExtraEffectStatus.ExtraTargetType GetExtraTargetType(CharacterStateControl characterStateControl)
	{
		ExtraEffectStatus.ExtraTargetType result = ExtraEffectStatus.ExtraTargetType.Player;
		if (characterStateControl.isEnemy)
		{
			result = ExtraEffectStatus.ExtraTargetType.Enemy;
		}
		if (BattleStateManager.current != null && BattleStateManager.current.battleMode == BattleMode.PvP)
		{
			result = ExtraEffectStatus.ExtraTargetType.All;
		}
		return result;
	}

	public enum ExtraTargetType
	{
		All,
		Player,
		Enemy
	}
}
