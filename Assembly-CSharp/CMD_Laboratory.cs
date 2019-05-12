﻿using Cutscene;
using Evolution;
using Master;
using Monster;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_Laboratory : CMD_PairSelectBase
{
	[SerializeField]
	private LaboratoryPartsStatusDetail digitamaDetail;

	protected override void ShowSecondTutorial()
	{
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (tutorialObserver != null)
		{
			GUIMain.BarrierON(null);
			tutorialObserver.StartSecondTutorial("second_tutorial_laboratory", new Action(GUIMain.BarrierOFF), delegate
			{
				GUICollider.EnableAllCollider("CMD_Laboratory");
			});
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_Laboratory");
		}
		base.SetTutorialAnyTime("anytime_second_tutorial_laboratory");
	}

	protected override void SetTextConfirmPartnerArousal(CMD_ResearchModalAlert cd)
	{
		cd.SetTitle(StringMaster.GetString("LaboratoryResearchAlertTitle"));
		cd.SetExp(StringMaster.GetString("LaboratoryResearchAlertInfo"));
		cd.SetBtnText_YES(StringMaster.GetString("SystemButtonYes"));
		cd.SetBtnText_NO(StringMaster.GetString("SystemButtonNo"));
	}

	protected override void SetTextConfirmBaseVersionUp(CMD_ResearchModalAlert cd)
	{
		cd.SetTitle(StringMaster.GetString("LaboratoryResearchAlertTitle"));
		cd.SetExp(StringMaster.GetString("LaboratoryResearchAlertInfo3"));
		cd.SetBtnText_YES(StringMaster.GetString("SystemButtonYes"));
		cd.SetBtnText_NO(StringMaster.GetString("SystemButtonNo"));
	}

	protected override void SetTextConfirmPartnerVersionUp(CMD_ResearchModalAlert cd)
	{
		cd.SetTitle(StringMaster.GetString("LaboratoryResearchAlertTitle"));
		cd.SetExp(StringMaster.GetString("LaboratoryResearchAlertInfo2"));
		cd.SetBtnText_YES(StringMaster.GetString("SystemButtonYes"));
		cd.SetBtnText_NO(StringMaster.GetString("SystemButtonNo"));
	}

	protected override void OpenConfirmTargetParameter(int selectButtonIndex)
	{
		if (selectButtonIndex == 1)
		{
			MonsterEggStatusInfo digitamaStatus = this.CreateDigitamaStatus(this.baseDigimon, this.partnerDigimon);
			CMD_ResearchModal cmd_ResearchModal = GUIMain.ShowCommonDialog(new Action<int>(base.OnCloseConfirm), "CMD_ResearchModal", null) as CMD_ResearchModal;
			cmd_ResearchModal.SetChipParams(this.baseDigimon, this.partnerDigimon);
			cmd_ResearchModal.SetDigitamaStatus(digitamaStatus);
		}
	}

	protected override void DoExec(int result)
	{
		if (result == -1)
		{
			return;
		}
		if (result > 0)
		{
			RestrictionInput.EndLoad();
			DataMng.Instance().CampaignErrorCloseAllCommonDialog(result == 1, delegate
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
				DataMng.Instance().ReloadCampaign(delegate
				{
					RestrictionInput.EndLoad();
				});
			});
			RestrictionInput.EndLoad();
			return;
		}
		this.useClusterBK = this.CalcCluster();
		GameWebAPI.RequestMN_MonsterCombination requestMN_MonsterCombination = new GameWebAPI.RequestMN_MonsterCombination();
		requestMN_MonsterCombination.SetSendData = delegate(GameWebAPI.MN_Req_Labo param)
		{
			param.baseUserMonsterId = this.baseDigimon.userMonster.userMonsterId;
			param.materialUserMonsterId = this.partnerDigimon.userMonster.userMonsterId;
		};
		requestMN_MonsterCombination.OnReceived = delegate(GameWebAPI.RespDataMN_LaboExec response)
		{
			DataMng.Instance().RespDataMN_LaboExec = response;
			if (response.userMonster != null)
			{
				ClassSingleton<MonsterUserDataMng>.Instance.AddUserMonsterData(response.userMonster);
			}
		};
		GameWebAPI.RequestMN_MonsterCombination request = requestMN_MonsterCombination;
		AppCoroutine.Start(request.Run(delegate()
		{
			AppCoroutine.Start(base.GetChipSlotInfo(), false);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
			this.ClosePanel(true);
		}, null), false);
	}

	protected override void EndSuccess()
	{
		int num = int.Parse(this.baseDigimon.userMonster.friendship);
		int friendshipMaxValue = MonsterFriendshipData.GetFriendshipMaxValue(this.baseDigimon.monsterMG.growStep);
		bool upArousal = num >= friendshipMaxValue;
		bool isAwakening = num == friendshipMaxValue;
		bool hasChip = this.ResetChipAfterExec();
		string[] userMonsterIdList = new string[]
		{
			this.baseDigimon.userMonster.userMonsterId,
			this.partnerDigimon.userMonster.userMonsterId
		};
		ClassSingleton<MonsterUserDataMng>.Instance.DeleteUserMonsterData(userMonsterIdList);
		GooglePlayGamesTool.Instance.Laboratory();
		ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonsterData = this.GetUserMonsterData();
		MonsterData userMonster = ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(userMonsterData.userMonsterId);
		CutsceneDataFusion cutsceneData = new CutsceneDataFusion
		{
			path = "Cutscenes/Fusion",
			baseModelId = this.baseDigimon.GetMonsterMaster().Group.modelId,
			materialModelId = this.partnerDigimon.GetMonsterMaster().Group.modelId,
			eggModelId = ClassSingleton<EvolutionData>.Instance.GetEggType(userMonster.userMonster.monsterEvolutionRouteId),
			upArousal = upArousal,
			endCallback = delegate()
			{
				CutSceneMain.FadeReqCutSceneEnd();
				if (null != this.characterDetailed)
				{
					this.DisableCutinButton(this.characterDetailed.transform);
				}
				PartsMenu partsMenu = UnityEngine.Object.FindObjectOfType<PartsMenu>();
				if (null != partsMenu)
				{
					partsMenu.SetEnableMenuButton(false);
				}
			}
		};
		Loading.Invisible();
		CutSceneMain.FadeReqCutScene(cutsceneData, new Action(base.StartCutSceneCallBack), null, delegate(int index)
		{
			if (PartsUpperCutinController.Instance != null)
			{
				if (isAwakening)
				{
					PartsUpperCutinController.Instance.PlayAnimator(PartsUpperCutinController.AnimeType.ResearchComplete, delegate
					{
						PartsUpperCutinController.Instance.PlayAnimator(PartsUpperCutinController.AnimeType.AwakeningComplete, delegate
						{
							this.ShowStoreChipDialog(hasChip);
						});
					});
				}
				else
				{
					PartsUpperCutinController.Instance.PlayAnimator(PartsUpperCutinController.AnimeType.ResearchComplete, delegate
					{
						this.ShowStoreChipDialog(hasChip);
					});
				}
			}
			if (!hasChip)
			{
				RestrictionInput.EndLoad();
				this.EnableCutinButton();
				PartsMenu partsMenu = UnityEngine.Object.FindObjectOfType<PartsMenu>();
				if (null != partsMenu)
				{
					partsMenu.SetEnableMenuButton(true);
				}
			}
		}, 0.5f, 0.5f);
	}

	private bool ResetChipAfterExec()
	{
		bool result = this.baseDigimon.GetChipEquip().IsAttachedChip() || this.partnerDigimon.GetChipEquip().IsAttachedChip();
		GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] monsterChipList = ChipDataMng.GetMonsterChipList(this.baseDigimon.userMonster.userMonsterId);
		if (monsterChipList != null)
		{
			foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipList in monsterChipList)
			{
				if (userChipList.userMonsterId == int.Parse(this.baseDigimon.userMonster.userMonsterId))
				{
					userChipList.userMonsterId = 0;
				}
			}
		}
		GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] monsterChipList2 = ChipDataMng.GetMonsterChipList(this.partnerDigimon.userMonster.userMonsterId);
		if (monsterChipList2 != null)
		{
			foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipList2 in monsterChipList2)
			{
				if (userChipList2.userMonsterId == int.Parse(this.partnerDigimon.userMonster.userMonsterId))
				{
					userChipList2.userMonsterId = 0;
				}
			}
		}
		return result;
	}

	protected override string GetTitle()
	{
		return StringMaster.GetString("LaboratoryTitle");
	}

	protected override string GetStoreChipInfo()
	{
		return StringMaster.GetString("LaboratoryCautionChip");
	}

	protected override void ClearTargetStatus()
	{
		this.digitamaDetail.ClearDigitamaStatus();
	}

	protected override GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList GetUserMonsterData()
	{
		return DataMng.Instance().RespDataMN_LaboExec.userMonster;
	}

	protected override void AddButton()
	{
		CMD_CharacterDetailed.AddButton = CMD_CharacterDetailed.ButtonType.Garden;
	}

	protected override int CalcCluster()
	{
		int num = 0;
		if (this.baseDigimon != null)
		{
			int num2 = this.baseDigimon.monsterM.GetArousal() + ConstValue.LABORATORY_BASE_PLUS_COEFFICIENT;
			num += num2 * ConstValue.LABORATORY_BASE_COEFFICIENT;
		}
		if (this.partnerDigimon != null)
		{
			int num3 = this.partnerDigimon.monsterM.GetArousal() + ConstValue.LABORATORY_PARTNER_PLUS_COEFFICIENT;
			num += num3 * ConstValue.LABORATORY_PARTNER_COEFFICIENT;
		}
		return num;
	}

	protected override void SetTargetStatus()
	{
		MonsterEggStatusInfo digitamaStatus = this.CreateDigitamaStatus(this.baseDigimon, this.partnerDigimon);
		this.digitamaDetail.SetDigitamaStatus(digitamaStatus);
	}

	protected override bool CanEnter()
	{
		List<MonsterData> list = MonsterDataMng.Instance().GetMonsterDataList();
		list = MonsterFilter.Filter(list, MonsterFilterType.GROWING_IN_GARDEN);
		return list.Count < ConstValue.MAX_CHILD_MONSTER;
	}

	protected override string GetInfoCannotEnter()
	{
		return StringMaster.GetString("LaboratoryMaxGarden");
	}

	protected override bool CanSelectMonster(int idx)
	{
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		List<MonsterData> list = monsterDataMng.GetMonsterDataList();
		list = MonsterFilter.Filter(list, MonsterFilterType.RESEARCH_TARGET);
		monsterDataMng.SortMDList(list);
		return list.Count > 1;
	}

	protected override void OpenCanNotSelectMonsterPop()
	{
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("LaboratoryNoUltimateTitle");
		cmd_ModalMessage.Info = StringMaster.GetString("LaboratoryNoUltimateInfo");
	}

	protected override void SetBaseTouchAct_L(GUIMonsterIcon cs)
	{
		cs.SetTouchAct_L(new Action<MonsterData>(base.ActMIconLong));
	}

	protected override void SetPartnerTouchAct_L(GUIMonsterIcon cs)
	{
		cs.SetTouchAct_L(new Action<MonsterData>(base.ActMIconLong));
	}

	protected override void SetBaseSelectType()
	{
		CMD_BaseSelect.BaseType = CMD_BaseSelect.BASE_TYPE.LABO;
	}

	protected override void OnBaseSelected()
	{
	}

	protected override void OpenBaseDigimonNonePop()
	{
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("LaboratoryNotSelectedTitle");
		cmd_ModalMessage.Info = StringMaster.GetString("LaboratoryNotSelectedInfo");
	}

	private MonsterEggStatusInfo CreateDigitamaStatus(MonsterData baseData, MonsterData partnerData)
	{
		MonsterEggStatusInfo monsterEggStatusInfo = new MonsterEggStatusInfo();
		monsterEggStatusInfo.rare = baseData.monsterM.rare;
		int num = int.Parse(baseData.userMonster.friendship);
		int friendshipMaxValue = MonsterFriendshipData.GetFriendshipMaxValue(baseData.monsterMG.growStep);
		int num2 = monsterEggStatusInfo.rare.ToInt32();
		monsterEggStatusInfo.isArousal = false;
		monsterEggStatusInfo.isReturn = false;
		if (num2 >= 6)
		{
			monsterEggStatusInfo.isReturn = true;
		}
		else if (num == friendshipMaxValue && num2 < 5)
		{
			monsterEggStatusInfo.isArousal = true;
		}
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster = baseData.userMonster;
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster2 = partnerData.userMonster;
		int goldMedalCount = this.GetGoldMedalCount(userMonster, userMonster2);
		monsterEggStatusInfo.hpAbilityFlg = this.GetCandidateMedal(userMonster.hpAbilityFlg, userMonster2.hpAbilityFlg, goldMedalCount);
		monsterEggStatusInfo.attackAbilityFlg = this.GetCandidateMedal(userMonster.attackAbilityFlg, userMonster2.attackAbilityFlg, goldMedalCount);
		monsterEggStatusInfo.defenseAbilityFlg = this.GetCandidateMedal(userMonster.defenseAbilityFlg, userMonster2.defenseAbilityFlg, goldMedalCount);
		monsterEggStatusInfo.spAttackAbilityFlg = this.GetCandidateMedal(userMonster.spAttackAbilityFlg, userMonster2.spAttackAbilityFlg, goldMedalCount);
		monsterEggStatusInfo.spDefenseAbilityFlg = this.GetCandidateMedal(userMonster.spDefenseAbilityFlg, userMonster2.spDefenseAbilityFlg, goldMedalCount);
		monsterEggStatusInfo.speedAbilityFlg = this.GetCandidateMedal(userMonster.speedAbilityFlg, userMonster2.speedAbilityFlg, goldMedalCount);
		monsterEggStatusInfo.luck = baseData.userMonster.luck;
		return monsterEggStatusInfo;
	}

	private ConstValue.CandidateMedal GetCandidateMedal(string baseMedalType, string partnerMedalType, int goldMedalCount)
	{
		int num = baseMedalType.ToInt32();
		int num2 = partnerMedalType.ToInt32();
		ConstValue.CandidateMedal result = ConstValue.CandidateMedal.NONE;
		if (num == 1)
		{
			if (ConstValue.MAX_GOLD_MEDAL_COUNT < goldMedalCount)
			{
				result = ConstValue.CandidateMedal.GOLD_OR_SILVER;
			}
			else
			{
				result = ConstValue.CandidateMedal.GOLD;
			}
		}
		else if (num2 == 1)
		{
			result = ConstValue.CandidateMedal.GOLD_OR_SILVER;
		}
		else if (num == 2)
		{
			result = ConstValue.CandidateMedal.SILVER;
		}
		else if (num2 == 2)
		{
			result = ConstValue.CandidateMedal.SILVER_OR_NONE;
		}
		return result;
	}

	private int GetGoldMedalCount(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList baseUserMonsterData, GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList partnerUserMonsterData)
	{
		int num = 0;
		if (this.IsGoldMedal(baseUserMonsterData.hpAbilityFlg, partnerUserMonsterData.hpAbilityFlg))
		{
			num++;
		}
		if (this.IsGoldMedal(baseUserMonsterData.attackAbilityFlg, partnerUserMonsterData.attackAbilityFlg))
		{
			num++;
		}
		if (this.IsGoldMedal(baseUserMonsterData.defenseAbilityFlg, partnerUserMonsterData.defenseAbilityFlg))
		{
			num++;
		}
		if (this.IsGoldMedal(baseUserMonsterData.spAttackAbilityFlg, partnerUserMonsterData.spAttackAbilityFlg))
		{
			num++;
		}
		if (this.IsGoldMedal(baseUserMonsterData.spDefenseAbilityFlg, partnerUserMonsterData.spDefenseAbilityFlg))
		{
			num++;
		}
		if (this.IsGoldMedal(baseUserMonsterData.speedAbilityFlg, partnerUserMonsterData.speedAbilityFlg))
		{
			num++;
		}
		return num;
	}

	private bool IsGoldMedal(string baseMedalType, string partnerMedalType)
	{
		int num = baseMedalType.ToInt32();
		int num2 = partnerMedalType.ToInt32();
		return num == 1 || num2 == 1;
	}
}
