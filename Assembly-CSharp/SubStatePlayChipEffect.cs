﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubStatePlayChipEffect : BattleStateController
{
	private Func<bool> IsSkillEnd;

	public SubStatePlayChipEffect(Action OnExit, Action<EventState> OnExitGotEvent, Func<bool> isSkillEnd = null) : base(null, OnExit, OnExitGotEvent)
	{
		this.IsSkillEnd = isSkillEnd;
	}

	protected override void AwakeThisState()
	{
		base.AddState(new SubStatePlayInvocationEffectAction(null, new Action<EventState>(base.SendEventState)));
		if (base.stateManager.battleMode == BattleMode.PvP)
		{
			base.AddState(new SubStateWaitRandomSeedSync(null, new Action<EventState>(base.SendEventState)));
			base.AddState(new SubStatePvPSkillDetailsFunction(null, new Action<EventState>(base.SendEventState)));
		}
		else if (base.stateManager.battleMode == BattleMode.Multi)
		{
			base.AddState(new SubStateWaitRandomSeedSync(null, new Action<EventState>(base.SendEventState)));
			base.AddState(new SubStateMultiSkillDetailsFunction(null, new Action<EventState>(base.SendEventState)));
		}
		else
		{
			base.AddState(new SubStateSkillDetailsFunction(null, new Action<EventState>(base.SendEventState)));
		}
	}

	protected override void EnabledThisState()
	{
		base.battleStateData.SetChipSkillFlag(true);
	}

	protected override IEnumerator MainRoutine()
	{
		if (base.stateManager.battleMode == BattleMode.PvP)
		{
			if (base.stateManager.isLastBattle && (base.battleStateData.GetCharactersDeath(true) || base.battleStateData.GetCharactersDeath(false)))
			{
				yield break;
			}
		}
		else if (base.stateManager.isLastBattle && base.battleStateData.GetCharactersDeath(true))
		{
			yield break;
		}
		bool flag = !BattleStateManager.current.battleStateData.enemies.Where((CharacterStateControl item) => !item.isDied).Any<CharacterStateControl>();
		bool flag2 = !BattleStateManager.current.battleStateData.playerCharacters.Where((CharacterStateControl item) => !item.isDied).Any<CharacterStateControl>();
		foreach (CharacterStateControl characterStateControl in this.GetTotalCharacters())
		{
			if ((characterStateControl.isEnemy && flag) || (!characterStateControl.isEnemy && flag2))
			{
				characterStateControl.RemoveDeadStagingChips();
			}
		}
		CharacterStateControl[] chipActors = this.GetChipActors();
		while (chipActors.Length > 0)
		{
			List<SubStatePlayChipEffect.ReturnData> resultList = this.CheckPlayStagingChipEffect(chipActors);
			SubStatePlayChipEffect.ChipPlayType chipPlayType = SubStatePlayChipEffect.ChipPlayType.None;
			foreach (SubStatePlayChipEffect.ReturnData returnData in resultList)
			{
				if (returnData.chipPlayType > chipPlayType)
				{
					chipPlayType = returnData.chipPlayType;
				}
			}
			string cameraKey = string.Empty;
			if ((chipPlayType == SubStatePlayChipEffect.ChipPlayType.ChipAndSKillPlay || chipPlayType == SubStatePlayChipEffect.ChipPlayType.SKillPlay) && this.IsSkillEnd != null && this.IsSkillEnd())
			{
				List<CharacterStateControl> list = new List<CharacterStateControl>();
				foreach (SubStatePlayChipEffect.ReturnData returnData2 in resultList)
				{
					if (returnData2.chipPlayType == SubStatePlayChipEffect.ChipPlayType.ChipAndSKillPlay)
					{
						returnData2.characterStateControl.CharacterParams.gameObject.SetActive(true);
						list.Add(returnData2.characterStateControl);
					}
				}
				if (list.Count > 0)
				{
					if (list.Count == 1)
					{
						cameraKey = "skillF";
					}
					else
					{
						cameraKey = "skillA";
					}
					list[0].CharacterParams.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
					base.stateManager.cameraControl.PlayCameraMotionActionCharacter(cameraKey, list[0]);
					base.stateManager.cameraControl.SetTime(cameraKey, 1f);
				}
			}
			if (chipPlayType != SubStatePlayChipEffect.ChipPlayType.None)
			{
				foreach (SubStatePlayChipEffect.ReturnData returnData3 in resultList)
				{
					if (returnData3.chipPlayType != SubStatePlayChipEffect.ChipPlayType.None)
					{
						returnData3.characterStateControl.CharacterParams.gameObject.SetActive(true);
						returnData3.characterStateControl.CharacterParams.StopAnimation();
						returnData3.characterStateControl.CharacterParams.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
					}
				}
				IEnumerator chipWait = this.PlayChipEffectPerformance(resultList);
				while (chipWait.MoveNext())
				{
					object obj = chipWait.Current;
					yield return obj;
				}
				if (!string.IsNullOrEmpty(cameraKey))
				{
					base.stateManager.cameraControl.StopCameraMotionAction(cameraKey);
				}
				IEnumerator chipEffectWait = this.PlayStagingChipEffect(resultList);
				while (chipEffectWait.MoveNext())
				{
					object obj2 = chipEffectWait.Current;
					yield return obj2;
				}
			}
			chipActors = this.GetChipActors();
			if (chipActors.Length == 0)
			{
				foreach (CharacterStateControl characterStateControl2 in base.battleStateData.GetTotalCharacters())
				{
					if (!characterStateControl2.isDied)
					{
						characterStateControl2.CharacterParams.gameObject.SetActive(true);
						base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(new CharacterStateControl[]
						{
							characterStateControl2
						});
					}
					else
					{
						characterStateControl2.CharacterParams.gameObject.SetActive(false);
					}
				}
			}
		}
		yield break;
	}

	protected override void DisabledThisState()
	{
		base.battleStateData.SetChipSkillFlag(false);
	}

	private CharacterStateControl[] GetTotalCharacters()
	{
		CharacterStateControl[] result;
		if (base.stateManager.battleMode == BattleMode.PvP)
		{
			if (base.stateManager.pvpFunction.IsOwner)
			{
				result = base.battleStateData.GetTotalCharacters();
			}
			else
			{
				result = base.battleStateData.GetTotalCharactersEnemyFirst();
			}
		}
		else
		{
			result = base.battleStateData.GetTotalCharacters();
		}
		return result;
	}

	private CharacterStateControl[] GetChipActors()
	{
		CharacterStateControl[] totalCharacters = this.GetTotalCharacters();
		return totalCharacters.Where((CharacterStateControl item) => item.stagingChipIdList.Count > 0).ToArray<CharacterStateControl>();
	}

	private List<SubStatePlayChipEffect.ReturnData> CheckPlayStagingChipEffect(CharacterStateControl[] chipActor)
	{
		List<SubStatePlayChipEffect.ReturnData> list = new List<SubStatePlayChipEffect.ReturnData>();
		foreach (CharacterStateControl characterStateControl in chipActor)
		{
			SubStatePlayChipEffect.ReturnData returnData = new SubStatePlayChipEffect.ReturnData();
			returnData.characterStateControl = characterStateControl;
			SubStatePlayChipEffect.ChipPlayType chipPlayType = SubStatePlayChipEffect.ChipPlayType.None;
			foreach (KeyValuePair<int, int> keyValuePair in characterStateControl.stagingChipIdList)
			{
				GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffectDataToId = ChipDataMng.GetChipEffectDataToId(keyValuePair.Key.ToString());
				if (!characterStateControl.isDied || chipEffectDataToId.effectTrigger.ToInt32() == 6)
				{
					if (!returnData.dictionary.ContainsKey(keyValuePair.Value))
					{
						returnData.dictionary[keyValuePair.Value] = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
					}
					if (chipEffectDataToId.effectType.ToInt32() == 60)
					{
						SkillStatus skillStatus = base.hierarchyData.GetSkillStatus(chipEffectDataToId.effectValue);
						CharacterStateControl target = this.GetTarget(characterStateControl, chipEffectDataToId);
						if (skillStatus != null && target != null)
						{
							returnData.chipPlayType = SubStatePlayChipEffect.ChipPlayType.ChipAndSKillPlay;
							returnData.dictionary[keyValuePair.Value].Add(chipEffectDataToId);
						}
						else
						{
							returnData.characterStateControl.AddChipEffectCount(chipEffectDataToId.chipEffectId.ToInt32(), 1);
						}
					}
					else if (chipEffectDataToId.effectType.ToInt32() == 56)
					{
						SkillStatus skillStatus2 = base.hierarchyData.GetSkillStatus(chipEffectDataToId.effectValue);
						CharacterStateControl target2 = this.GetTarget(characterStateControl, chipEffectDataToId);
						if (skillStatus2 != null && target2 != null)
						{
							chipPlayType = SubStatePlayChipEffect.ChipPlayType.SKillPlay;
							returnData.dictionary[keyValuePair.Value].Add(chipEffectDataToId);
						}
						else
						{
							returnData.characterStateControl.AddChipEffectCount(chipEffectDataToId.chipEffectId.ToInt32(), 1);
						}
					}
					else
					{
						if (returnData.chipPlayType == SubStatePlayChipEffect.ChipPlayType.None)
						{
							returnData.chipPlayType = SubStatePlayChipEffect.ChipPlayType.ChipPlay;
						}
						returnData.dictionary[keyValuePair.Value].Add(chipEffectDataToId);
					}
				}
			}
			if (returnData.chipPlayType == SubStatePlayChipEffect.ChipPlayType.None && chipPlayType != SubStatePlayChipEffect.ChipPlayType.None)
			{
				returnData.chipPlayType = chipPlayType;
			}
			list.Add(returnData);
			characterStateControl.stagingChipIdList.Clear();
		}
		return list;
	}

	private IEnumerator PlayStagingChipEffect(List<SubStatePlayChipEffect.ReturnData> returnDataList)
	{
		foreach (SubStatePlayChipEffect.ReturnData returnData in returnDataList)
		{
			foreach (KeyValuePair<int, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>> data in returnData.dictionary)
			{
				foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in data.Value)
				{
					if (!returnData.characterStateControl.isDied || chipEffect.effectTrigger.ToInt32() == 6)
					{
						SufferStateProperty death = returnData.characterStateControl.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.InstantDeath);
						if (death == null || !death.isActive || death.isMiss || chipEffect.effectTrigger.ToInt32() == 6)
						{
							if (chipEffect.effectType.ToInt32() == 60 || chipEffect.effectType.ToInt32() == 56)
							{
								SkillStatus status = base.hierarchyData.GetSkillStatus(chipEffect.effectValue);
								CharacterStateControl target = this.GetTarget(returnData.characterStateControl, chipEffect);
								if (status == null || target == null)
								{
									returnData.characterStateControl.AddChipEffectCount(chipEffect.chipEffectId.ToInt32(), 1);
								}
								else
								{
									IEnumerator function = this.ChipSkillDetailsFunction(returnData.characterStateControl, data.Key, chipEffect, status, target);
									while (function.MoveNext())
									{
										object obj = function.Current;
										yield return obj;
									}
								}
							}
						}
					}
				}
			}
		}
		yield break;
	}

	private IEnumerator PlayChipEffectPerformance(List<SubStatePlayChipEffect.ReturnData> returnDataList)
	{
		float waitTime = 1f;
		int count = 0;
		int maxChipCount = 0;
		foreach (SubStatePlayChipEffect.ReturnData returnData in returnDataList)
		{
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>> keyValuePair in returnData.dictionary)
			{
				if (keyValuePair.Value.Count > 0)
				{
					if (returnData.chipPlayType != SubStatePlayChipEffect.ChipPlayType.SKillPlay)
					{
						int key = keyValuePair.Key;
						if (list.IndexOf(key) < 0)
						{
							list.Add(key);
						}
					}
				}
			}
			int num = list.Count<int>();
			if (num > maxChipCount)
			{
				maxChipCount = num;
			}
			if (num > 0)
			{
				base.battleStateData.stageGimmickUpEffect[count].SetPosition(returnData.characterStateControl.CharacterParams.transform, null);
				base.stateManager.threeDAction.PlayAlwaysEffectAction(base.battleStateData.stageGimmickUpEffect[count], AlwaysEffectState.In);
				base.stateManager.soundPlayer.TryPlaySE(base.battleStateData.stageGimmickUpEffect[count], AlwaysEffectState.In);
				base.StartCoroutine(base.stateManager.uiControl.ShowChipIcon(waitTime, base.battleStateData.stageGimmickUpEffect[count].targetPosition.position, list.ToArray()));
			}
			count++;
		}
		if (maxChipCount > 0)
		{
			bool isEnd = false;
			base.stateManager.uiControl.ShowBattleChipEffect((float)maxChipCount * waitTime + 1.9f, delegate
			{
				isEnd = true;
			});
			while (!isEnd)
			{
				yield return new WaitForEndOfFrame();
			}
		}
		for (int i = 0; i < base.battleStateData.stageGimmickUpEffect.Length; i++)
		{
			base.stateManager.threeDAction.PlayAlwaysEffectAction(base.battleStateData.stageGimmickUpEffect[i], AlwaysEffectState.Out);
		}
		base.stateManager.threeDAction.StopAlwaysEffectAction(base.battleStateData.stageGimmickUpEffect);
		yield break;
	}

	private IEnumerator ChipSkillDetailsFunction(CharacterStateControl chipCharacter, int chipId, GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect, SkillStatus status, CharacterStateControl target)
	{
		chipCharacter.chipSkillId = chipEffect.effectValue;
		chipCharacter.currentChipId = chipId.ToString();
		base.battleStateData.SetAutoCounterCharacter(chipCharacter);
		chipCharacter.targetCharacter = target;
		base.stateManager.cameraControl.PlayTweenCameraMotion(base.battleStateData.commandSelectTweenTargetCamera, chipCharacter.targetCharacter);
		base.battleStateData.isInvocationEffectPlaying = false;
		base.stateManager.time.SetPlaySpeed(base.hierarchyData.on2xSpeedPlay, base.battleStateData.isShowMenuWindow);
		base.stateManager.uiControl.ApplyTurnActionBarSwipeout(true);
		base.SetState(typeof(SubStatePlayInvocationEffectAction));
		while (base.isWaitState)
		{
			yield return null;
		}
		base.stateManager.time.SetPlaySpeed(base.hierarchyData.on2xSpeedPlay, base.battleStateData.isShowMenuWindow);
		base.stateManager.uiControl.ApplyTurnActionBarSwipeout(false);
		if (base.stateManager.battleMode == BattleMode.PvP)
		{
			base.SetState(typeof(SubStateWaitRandomSeedSync));
			while (base.isWaitState)
			{
				yield return null;
			}
			base.SetState(typeof(SubStatePvPSkillDetailsFunction));
		}
		else if (base.stateManager.battleMode == BattleMode.Multi)
		{
			base.SetState(typeof(SubStateWaitRandomSeedSync));
			while (base.isWaitState)
			{
				yield return null;
			}
			base.SetState(typeof(SubStateMultiSkillDetailsFunction));
		}
		else
		{
			base.SetState(typeof(SubStateSkillDetailsFunction));
		}
		while (base.isWaitState)
		{
			yield return null;
		}
		yield break;
	}

	private CharacterStateControl GetTarget(CharacterStateControl chipCharacter, GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect)
	{
		SkillStatus skillStatus = base.hierarchyData.GetSkillStatus(chipEffect.effectValue);
		if (chipEffect.effectType.ToInt32() != 56)
		{
			CharacterStateControl[] skillTargetList = base.stateManager.targetSelect.GetSkillTargetList(chipCharacter, skillStatus.target);
			if (chipCharacter.targetCharacter != null && !chipCharacter.targetCharacter.isDied)
			{
				bool flag = false;
				foreach (CharacterStateControl b in skillTargetList)
				{
					if (chipCharacter.targetCharacter == b)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					List<CharacterStateControl> list = new List<CharacterStateControl>();
					list.Add(chipCharacter.targetCharacter);
					foreach (CharacterStateControl characterStateControl in skillTargetList)
					{
						if (chipCharacter.targetCharacter != characterStateControl && !chipCharacter.targetCharacter.isDied)
						{
							list.Add(characterStateControl);
						}
					}
				}
			}
			foreach (AffectEffectProperty affectEffectProperty in skillStatus.affectEffect)
			{
				if (affectEffectProperty.type == AffectEffect.SufferStatusClear)
				{
					bool flag2 = affectEffectProperty.clearPoisonIncidenceRate > 0f;
					bool flag3 = affectEffectProperty.clearConfusionIncidenceRate > 0f;
					bool flag4 = affectEffectProperty.clearParalysisIncidenceRate > 0f;
					bool flag5 = affectEffectProperty.clearSleepIncidenceRate > 0f;
					bool flag6 = affectEffectProperty.clearStunIncidenceRate > 0f;
					bool flag7 = affectEffectProperty.clearSkillLockIncidenceRate > 0f;
					int num = 0;
					CharacterStateControl result = null;
					for (int k = 0; k < skillTargetList.Length; k++)
					{
						if (chipEffect.targetValue2.ToInt32() <= 0 || !(skillTargetList[k] == chipCharacter))
						{
							HaveSufferState currentSufferState = skillTargetList[k].currentSufferState;
							int num2 = 0;
							num2 += ((!flag2 || !currentSufferState.FindSufferState(SufferStateProperty.SufferType.Poison)) ? 0 : 1);
							num2 += ((!flag3 || !currentSufferState.FindSufferState(SufferStateProperty.SufferType.Confusion)) ? 0 : 1);
							num2 += ((!flag4 || !currentSufferState.FindSufferState(SufferStateProperty.SufferType.Paralysis)) ? 0 : 1);
							num2 += ((!flag5 || !currentSufferState.FindSufferState(SufferStateProperty.SufferType.Sleep)) ? 0 : 1);
							num2 += ((!flag6 || !currentSufferState.FindSufferState(SufferStateProperty.SufferType.Stun)) ? 0 : 1);
							num2 += ((!flag7 || !currentSufferState.FindSufferState(SufferStateProperty.SufferType.SkillLock)) ? 0 : 1);
							if (num2 > num)
							{
								num = num2;
								result = skillTargetList[k];
							}
						}
					}
					return result;
				}
			}
			return (skillTargetList.Length <= 0) ? null : skillTargetList[0];
		}
		CharacterStateControl[] skillTargetList2;
		if (chipCharacter.isEnemy)
		{
			skillTargetList2 = base.stateManager.targetSelect.GetSkillTargetList(base.battleStateData.playerCharacters[0], skillStatus.target);
		}
		else
		{
			skillTargetList2 = base.stateManager.targetSelect.GetSkillTargetList(base.battleStateData.enemies[0], skillStatus.target);
		}
		if (skillTargetList2 == null || skillTargetList2.Length == 0)
		{
			return null;
		}
		if (skillStatus.numbers != EffectNumbers.Simple)
		{
			return skillTargetList2[0];
		}
		if (skillStatus.target == EffectTarget.Attacker)
		{
			return chipCharacter;
		}
		if (skillStatus.target == EffectTarget.Enemy)
		{
			if (base.battleStateData.currentSelectCharacterState == null || base.battleStateData.currentSelectCharacterState.isDied)
			{
				return null;
			}
			return base.battleStateData.currentSelectCharacterState;
		}
		else
		{
			if (skillStatus.target == EffectTarget.Ally)
			{
				return chipCharacter;
			}
			if (base.battleStateData.currentSelectCharacterState == null || base.battleStateData.currentSelectCharacterState.isDied)
			{
				return null;
			}
			return base.battleStateData.currentSelectCharacterState;
		}
	}

	private enum ChipPlayType
	{
		None,
		ChipPlay,
		ChipAndSKillPlay,
		SKillPlay
	}

	private class ReturnData
	{
		public SubStatePlayChipEffect.ChipPlayType chipPlayType;

		public CharacterStateControl characterStateControl;

		public Dictionary<int, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>> dictionary = new Dictionary<int, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>>();
	}
}
