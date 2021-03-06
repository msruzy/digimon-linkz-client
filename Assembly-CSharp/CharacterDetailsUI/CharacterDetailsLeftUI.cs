﻿using EvolutionRouteMap;
using Master;
using System;
using UnityEngine;

namespace CharacterDetailsUI
{
	public class CharacterDetailsLeftUI : MonoBehaviour
	{
		[SerializeField]
		private CMD_CharacterDetailed dialogRoot;

		[SerializeField]
		private GameObject buttonProtection;

		[SerializeField]
		private UILabel labelModelView;

		[SerializeField]
		private GameObject buttonGarden;

		[SerializeField]
		private GameObject buttonEvolution;

		private CharacterDetailsProtection protection;

		private bool isLocked;

		private void OpenDialogFailedProtection(CMD_CharacterDetailed.LockMode lockMode)
		{
			global::Debug.Assert(!this.isLocked, "ロック中に更にロックすることはできない");
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("CharaDetailsNotLock");
			if (this.protection == null)
			{
				this.protection = new CharacterDetailsProtection();
			}
			this.protection.SetErrorText(lockMode);
			cmd_ModalMessage.Info = this.protection.GetErrorText();
		}

		private APIRequestTask RequestUserProtectMonster(string userMonsterId, bool changeLockState)
		{
			GameWebAPI.UserProtectMonsterLogic request = new GameWebAPI.UserProtectMonsterLogic
			{
				SetSendData = delegate(GameWebAPI.ReqDataUS_UserProtectMonsterLogic param)
				{
					param.userMonsterId = userMonsterId;
					param.setFlg = ((!changeLockState) ? "0" : "1");
				},
				OnReceived = delegate(GameWebAPI.RespDataMS_UserProtectMonsterLogic response)
				{
					CMD_CharacterDetailed.DataChg.userMonster.SetLock(changeLockState);
				}
			};
			return new APIRequestTask(request, false);
		}

		private void SetProtectionImage(bool isLock)
		{
			UISprite component = this.buttonProtection.GetComponent<UISprite>();
			if (null != component)
			{
				if (isLock)
				{
					component.spriteName = "Common02_Icon_KeyL";
				}
				else
				{
					component.spriteName = "Common02_Icon_KeyO";
				}
			}
		}

		private void OnSuccessMonsterLock()
		{
			RestrictionInput.EndLoad();
			this.isLocked = !this.isLocked;
			this.SetProtectionImage(this.isLocked);
		}

		private void OnPushedProtectionButton()
		{
			bool flag = false;
			if (this.dialogRoot.Mode != CMD_CharacterDetailed.LockMode.None)
			{
				if (this.dialogRoot.Mode != CMD_CharacterDetailed.LockMode.Evolution)
				{
					flag = true;
				}
				else if (!this.isLocked && CMD_BaseSelect.DataChg == CMD_CharacterDetailed.DataChg)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.OpenDialogFailedProtection(this.dialogRoot.Mode);
			}
			else
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
				string userMonsterId = CMD_CharacterDetailed.DataChg.userMonster.userMonsterId;
				bool changeLockState = !this.isLocked;
				APIRequestTask task = this.RequestUserProtectMonster(userMonsterId, changeLockState);
				base.StartCoroutine(task.Run(new Action(this.OnSuccessMonsterLock), delegate(Exception noop)
				{
					RestrictionInput.EndLoad();
				}, null));
			}
		}

		private void OnPushedModelViewButton()
		{
			this.dialogRoot.OnClickedScreen();
		}

		private void OnPushedGardenButton()
		{
			FarmCameraControlForCMD.ClearRefCT();
			FarmCameraControlForCMD.On();
			GUIMain.DestroyAllDialog(null);
			GUIMain.ShowCommonDialog(null, "CMD_DigiGarden", null);
		}

		private void OnPushedEvolutionRouteMapButton()
		{
			MonsterData showCharacterMonsterData = this.dialogRoot.GetShowCharacterMonsterData();
			CMD_EvolutionRouteMap.CreateDialog(null, showCharacterMonsterData.GetMonsterMaster());
		}

		public void Initialize(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonsterData)
		{
			this.labelModelView.text = StringMaster.GetString("CharaDetailsFullScreen");
			if (userMonsterData.IsEgg())
			{
				this.buttonProtection.SetActive(false);
			}
			else
			{
				this.isLocked = userMonsterData.IsLocked;
				this.SetProtectionImage(userMonsterData.IsLocked);
				this.buttonEvolution.SetActive(true);
			}
		}

		public void SetModelViewState(CharacterDetailsLeftUI.ModelViewState state)
		{
			if (state != CharacterDetailsLeftUI.ModelViewState.SIMPLE)
			{
				if (state == CharacterDetailsLeftUI.ModelViewState.FULL_SCREEN)
				{
					this.labelModelView.text = StringMaster.GetString("SystemButtonReturn");
				}
			}
			else
			{
				this.labelModelView.text = StringMaster.GetString("CharaDetailsFullScreen");
			}
		}

		public GameObject GetProtectionButton()
		{
			return this.buttonProtection;
		}

		public void ShowGardenButton()
		{
			this.buttonGarden.SetActive(true);
		}

		public void DeleteEvolutionButton()
		{
			this.buttonEvolution.SetActive(false);
		}

		public enum ModelViewState
		{
			SIMPLE,
			FULL_SCREEN
		}
	}
}
