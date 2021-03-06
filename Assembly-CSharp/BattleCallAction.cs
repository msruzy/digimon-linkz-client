﻿using Master;
using System;
using System.Collections;
using UnityEngine;

public class BattleCallAction : BattleFunctionBase
{
	private int setOnAutoPlay;

	private bool tapAutoPlayNonFlag;

	private bool autoCoroutinePlay;

	private bool initFlag;

	public void SetAttack()
	{
		this.SetSkillInternal(0, true);
	}

	public void SetSkill(int index)
	{
		this.SetSkillInternal(index + 1, base.battleStateData.playerCharacters[base.battleStateData.currentSelectCharacterIndex].isUseSkill(index + 1) && !base.battleStateData.playerCharacters[base.battleStateData.currentSelectCharacterIndex].currentSufferState.FindSufferState(SufferStateProperty.SufferType.SkillLock));
	}

	private void SetSkillInternal(int index, bool isPossibleSelectSkill = true)
	{
		if (isPossibleSelectSkill)
		{
			CharacterStateControl characterStateControl = base.battleStateData.playerCharacters[base.battleStateData.currentSelectCharacterIndex];
			base.stateManager.uiControl.ApplySkillButtonRotation(characterStateControl.isSelectSkill, index);
			if (characterStateControl.isSelectSkill == index)
			{
				SoundPlayer.PlayButtonEnter();
				this.OnSkillTrigger();
			}
			else
			{
				characterStateControl.isSelectSkill = index;
				SoundPlayer.PlayButtonSelect();
			}
		}
	}

	public void ShowHideSkillDescription(int index = -1)
	{
		if (index != -1 && !base.battleStateData.isPossibleShowDescription)
		{
			return;
		}
		this.ShowHideSkillDescriptionInternal(index);
	}

	public void ForceHideSkillDescription()
	{
		this.ShowHideSkillDescriptionInternal(-1);
	}

	private void ShowHideSkillDescriptionInternal(int index = -1)
	{
		for (int i = 0; i < base.stateManager.uiControl.GetSkillButtonLength(); i++)
		{
			base.stateManager.uiControl.ApplySkillDescriptionEnable(i, index == i);
		}
	}

	private void OnSkillTrigger()
	{
		base.battleStateData.onSkillTrigger = true;
	}

	public void ShowMonsterDescription(int index)
	{
		base.battleStateData.isShowCharacterDescription = true;
		base.stateManager.uiControl.ApplyMonsterDescription(true, base.battleStateData.playerCharacters[index], base.battleStateData.currentSelectCharacterIndex);
	}

	public void HideMonsterDescription()
	{
		base.battleStateData.isShowCharacterDescription = false;
		base.stateManager.uiControl.ApplyMonsterDescription(false, null, 0);
	}

	public void OnCharacterRevival(int index)
	{
		if (!base.battleStateData.playerCharacters[index].isDied)
		{
			return;
		}
		if (base.battleStateData.isRevivalReservedCharacter[index])
		{
			return;
		}
		if (!base.hierarchyData.isPossibleContinue)
		{
			return;
		}
		SoundPlayer.PlayButtonEnter();
		base.battleStateData.isShowRevivalWindow = true;
		base.battleStateData.currentSelectRevivalCharacter = index;
		base.stateManager.uiControl.ApplyDigiStoneNumber(base.battleStateData.currentDigiStoneNumber);
		base.stateManager.uiControl.ApplyEnableCharacterRevivalWindow(true, base.battleStateData.currentDigiStoneNumber >= 1, null);
	}

	public void OnDecisionCharacterRevival()
	{
		base.stateManager.uiControl.SetHudCollider(false);
		int currentSelectRevivalCharacter = base.battleStateData.currentSelectRevivalCharacter;
		base.StartCoroutine(base.stateManager.deadOrAlive.OnDecisionCharacterRevivalFunction(currentSelectRevivalCharacter));
	}

	public void OnCancelCharacterRevival()
	{
		Action onFinishedAction = delegate()
		{
			base.battleStateData.isShowRevivalWindow = false;
		};
		base.stateManager.uiControl.ApplyEnableCharacterRevivalWindow(false, false, onFinishedAction);
		SoundPlayer.PlayButtonCancel();
		base.stateManager.uiControl.SetHudCollider(true);
	}

	public void OnAutoPlay()
	{
		if (!this.initFlag)
		{
			this.setOnAutoPlay = base.hierarchyData.onAutoPlay;
			this.initFlag = true;
		}
		if (this.setOnAutoPlay == 0)
		{
			this.tapAutoPlayNonFlag = true;
		}
		else
		{
			this.tapAutoPlayNonFlag = false;
		}
		this.setOnAutoPlay++;
		if (this.setOnAutoPlay > 2)
		{
			this.setOnAutoPlay = 0;
		}
		base.stateManager.uiControl.ApplyAutoPlay(this.setOnAutoPlay);
		SoundPlayer.PlayButtonSelect();
		if (this.tapAutoPlayNonFlag)
		{
			base.StartCoroutine(this.OnAutoPlayWait());
			if (this.autoCoroutinePlay)
			{
				base.StopCoroutine(this.OnAutoPlayWait());
				this.autoCoroutinePlay = false;
				base.hierarchyData.onAutoPlay = this.setOnAutoPlay;
				bool sleepOff = base.hierarchyData.onAutoPlay > 0;
				base.stateManager.sleep.SetSleepOff(sleepOff);
			}
		}
		else if (!this.autoCoroutinePlay)
		{
			base.hierarchyData.onAutoPlay = this.setOnAutoPlay;
			bool sleepOff2 = base.hierarchyData.onAutoPlay > 0;
			base.stateManager.sleep.SetSleepOff(sleepOff2);
		}
	}

	private IEnumerator OnAutoPlayWait()
	{
		yield return null;
		this.autoCoroutinePlay = true;
		yield return new WaitForSeconds(0.5f);
		base.hierarchyData.onAutoPlay = this.setOnAutoPlay;
		bool sleep = base.hierarchyData.onAutoPlay > 0;
		base.stateManager.sleep.SetSleepOff(sleep);
		this.autoCoroutinePlay = false;
		yield break;
	}

	public void OnShowMenu()
	{
		bool onPose = base.battleMode != BattleMode.Multi;
		base.stateManager.time.SetPlaySpeed(base.hierarchyData.on2xSpeedPlay, onPose);
		base.battleStateData.isShowMenuWindow = true;
		base.stateManager.uiControl.ApplyShowMenuWindow(true, base.hierarchyData.isPossibleRetire, null);
		base.stateManager.soundPlayer.SetPauseVolume(true);
		SoundPlayer.PlayMenuOpen();
		base.stateManager.uiControl.SetHudCollider(false);
	}

	public void OnHideMenu()
	{
		Action onFinishedAction = delegate()
		{
			base.battleStateData.isShowMenuWindow = false;
			base.stateManager.time.SetPlaySpeed(base.hierarchyData.on2xSpeedPlay, false);
			base.stateManager.soundPlayer.SetPauseVolume(false);
			base.stateManager.uiControl.SetHudCollider(true);
		};
		base.stateManager.uiControl.ApplyShowMenuWindow(false, true, onFinishedAction);
		SoundPlayer.PlayMenuClose();
	}

	public void On2xSpeedPlay()
	{
		base.hierarchyData.on2xSpeedPlay = !base.hierarchyData.on2xSpeedPlay;
		base.stateManager.uiControl.Apply2xPlay(base.hierarchyData.on2xSpeedPlay);
		base.stateManager.time.SetPlaySpeed(base.hierarchyData.on2xSpeedPlay, base.battleStateData.isShowMenuWindow);
		SoundPlayer.PlayButtonSelect();
	}

	public void OnRetireCheck()
	{
		base.stateManager.uiControl.ApplySetContinueUIColliders(false);
		base.battleStateData.isShowRetireWindow = true;
		base.stateManager.uiControl.ApplyShowRetireWindow(true, null);
		SoundPlayer.PlayButtonEnter();
	}

	public void OnOkRetire()
	{
		if (!base.battleStateData.isShowContinueWindow)
		{
			base.battleStateData.isBattleRetired = true;
		}
		base.stateManager.uiControl.ApplySetContinueUIColliders(true);
		base.battleStateData.isShowRetireWindow = false;
		base.stateManager.time.SetPlaySpeed(false, false);
		base.stateManager.uiControl.ApplyCurrentSelectArrow(false, default(Vector3), 0);
		base.stateManager.soundPlayer.SetPauseVolume(false);
		SoundPlayer.PlayButtonEnter();
		base.stateManager.log.GetBattleFinishedLogData(DataMng.ClearFlag.Defeat, true, base.battleStateData.isBattleRetired);
		base.stateManager.events.CallRetireEvent();
		base.stateManager.battleUiComponents.dialogRetire.gameObject.SetActive(false);
		base.stateManager.battleUiComponents.menuDialog.gameObject.SetActive(false);
	}

	public void OnCancelRetire()
	{
		base.stateManager.uiControl.ApplySetContinueUIColliders(true);
		Action onFinishedAction = delegate()
		{
			base.battleStateData.isShowRetireWindow = false;
		};
		base.stateManager.uiControl.ApplyShowRetireWindow(false, onFinishedAction);
		SoundPlayer.PlayButtonCancel();
	}

	public void OnDeathEndRevival()
	{
		if (base.battleStateData.isShowRetireWindow)
		{
			return;
		}
		if (base.battleStateData.isContinueFlag)
		{
			return;
		}
		if (base.battleStateData.isShowShop)
		{
			return;
		}
		SoundPlayer.PlayButtonEnter();
		base.battleStateData.isContinueFlag = true;
	}

	public void OnShowEnemyDescription(int index)
	{
		if (index < 0 || base.battleStateData.enemies.Length <= index)
		{
			return;
		}
		base.stateManager.uiControl.ApplyEnemyDescription(true, base.battleStateData.enemies[index]);
	}

	public void OnHideEnemyDescriotion()
	{
		base.stateManager.uiControl.ApplyEnemyDescription(false, null);
	}

	public void OnShowHelp()
	{
		if (base.battleStateData.isShowHelp)
		{
			return;
		}
		base.stateManager.help.ApplyShowHideHelpWindow(true);
	}

	public void OnHideHelp()
	{
		base.stateManager.help.ApplyShowHideHelpWindow(false);
	}

	public void OnSkipWinnerAction()
	{
		base.battleStateData.isSkipWinnerAction = true;
	}

	public void OnShowSpecificTrade()
	{
		if (!base.onServerConnect)
		{
			return;
		}
		if (base.battleStateData.isShowSpecificTrade)
		{
			return;
		}
		if (base.battleStateData.isShowShop)
		{
			return;
		}
		SoundPlayer.PlayButtonEnter();
		base.stateManager.uiControl.ApplySpecificTrade(true);
		Action<int> action = delegate(int x)
		{
			base.stateManager.callAction.OnHideSpecificTrade();
		};
		CommonDialog commonDialog = GUIMain.ShowCommonDialog(action, "CMDWebWindow", null);
		((CMDWebWindow)commonDialog).TitleText = StringMaster.GetString("ShopRule-02");
		((CMDWebWindow)commonDialog).Url = WebAddress.EXT_ADR_TRADE;
		base.battleStateData.isShowSpecificTrade = true;
	}

	public void OnHideSpecificTrade()
	{
		if (!base.battleStateData.isShowSpecificTrade)
		{
			return;
		}
		base.battleStateData.isShowSpecificTrade = false;
		base.stateManager.uiControl.ApplySpecificTrade(false);
	}

	public void OnCloseInitialInduction()
	{
		if (base.battleScreen != BattleScreen.PlayerFailed)
		{
			return;
		}
		base.battleStateData.isShowInitialIntroduction = false;
	}
}
