﻿using Master;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;

public sealed class CMD_Profile : CMD_ProfileBase
{
	public static CMD_Profile instance;

	[SerializeField]
	private UILabel labelUserID;

	[SerializeField]
	private UILabel labelCollection;

	[SerializeField]
	private UIInput inputNickName;

	[SerializeField]
	private UIInput inputComment;

	[SerializeField]
	private GooglePlayGamesObjects googlePlay;

	private string inputTextBackup;

	private static readonly int NICK_NAME_LENGTH_MIN = 1;

	private static readonly int NICK_NAME_LENGTH_MAX = 10;

	private static readonly int COMMENT_LENGTH_MAX = 30;

	public GameObject OPEN_BLOCK_LIST;

	private Vector3 vOrgSCR_BLOCK = Vector3.zero;

	private Vector3 vPosSCR_BLOCK = Vector3.zero;

	[SerializeField]
	private GameObject playHistoryButton;

	private Vector3 vOrgSCR_HISTORY = Vector3.zero;

	private Vector3 vPosSCR_HISTORY = Vector3.zero;

	private Vector3 vOrgSCR_TITLE = Vector3.zero;

	private Vector3 vPosSCR_TITLE = Vector3.zero;

	[SerializeField]
	private UITexture titleIcon;

	[CompilerGenerated]
	private static Action <>f__mg$cache0;

	protected override void Awake()
	{
		base.Awake();
		this.googlePlay.Bootup();
		CMD_Profile.instance = this;
	}

	public override void Show(Action<int> closeEvent, float sizeX, float sizeY, float showAnimationTime)
	{
		GUICollider.DisableAllCollider("CMD_Profile");
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		GameWebAPI.RequestUS_UserProfile requestUS_UserProfile = new GameWebAPI.RequestUS_UserProfile();
		requestUS_UserProfile.SetSendData = delegate(GameWebAPI.PRF_Req_ProfileData param)
		{
			param.targetUserId = DataMng.Instance().RespDataCM_Login.playerInfo.UserId;
		};
		requestUS_UserProfile.OnReceived = new Action<GameWebAPI.RespDataPRF_Profile>(DataMng.Instance().SetUserProfile);
		GameWebAPI.RequestUS_UserProfile request = requestUS_UserProfile;
		base.StartCoroutine(request.RunOneTime(delegate()
		{
			RestrictionInput.EndLoad();
			this.ShowDLG();
			this.SetTutorialAnyTime("anytime_second_tutorial_profile");
			this.<Show>__BaseCallProxy0(closeEvent, sizeX, sizeY, showAnimationTime);
		}, delegate(Exception nop)
		{
			RestrictionInput.EndLoad();
			this.<ClosePanel>__BaseCallProxy1(false);
			GUICollider.EnableAllCollider("CMD_Profile");
		}, null));
	}

	protected override void RefreshComponents()
	{
		this.userProfile = DataMng.Instance().RespDataPRF_Profile;
		this.labelUserID.text = this.userProfile.userData.userCode;
		this.labelCollection.text = string.Format(StringMaster.GetString("SystemFraction"), this.userProfile.collection.possessionNum, this.userProfile.collection.totalNum);
		this.inputComment.value = this.userProfile.userData.description;
		this.inputNickName.value = this.userProfile.userData.nickname;
		TitleDataMng.SetTitleIcon(this.userProfile.userData.titleId, this.titleIcon);
	}

	public static void RefreshParams()
	{
		if (null != CMD_Profile.instance)
		{
			CMD_Profile.instance.RefreshComponents();
		}
	}

	public void OnSelectComment()
	{
		this.googlePlay.EnableMenu(false);
		this.inputTextBackup = this.inputComment.value;
	}

	public void OnDecisionComment()
	{
		this.inputComment.value = this.TrimmingSpaceChar(this.inputComment.value, "[\n]");
		CMD_Profile.CheckInputCharResult checkInputCharResult = this.CheckInputChar(this.inputComment.value, 0, CMD_Profile.COMMENT_LENGTH_MAX);
		if (checkInputCharResult != CMD_Profile.CheckInputCharResult.OK)
		{
			this.ShowErrorPopUp(checkInputCharResult);
			this.inputComment.value = this.inputTextBackup;
		}
	}

	public void OnSelectNickname()
	{
		this.googlePlay.EnableMenu(false);
		this.inputTextBackup = this.inputNickName.value;
	}

	public void OnDecisionNickname()
	{
		this.inputNickName.value = this.TrimmingSpaceChar(this.inputNickName.value, "[ \n]");
		CMD_Profile.CheckInputCharResult checkInputCharResult = this.CheckInputChar(this.inputNickName.value, CMD_Profile.NICK_NAME_LENGTH_MIN, CMD_Profile.NICK_NAME_LENGTH_MAX);
		if (checkInputCharResult != CMD_Profile.CheckInputCharResult.OK)
		{
			this.ShowErrorPopUp(checkInputCharResult);
			this.inputNickName.value = this.inputTextBackup;
		}
	}

	private bool IsUpdateNickName()
	{
		return !string.IsNullOrEmpty(this.inputNickName.value) && this.inputNickName.value != this.userProfile.userData.nickname;
	}

	private bool IsUpdateComment()
	{
		return this.inputComment.value != this.userProfile.userData.description;
	}

	public void OnUserCodeCopy()
	{
		this.googlePlay.EnableMenu(false);
		if (!string.IsNullOrEmpty(this.labelUserID.text))
		{
			Clipboard.Text = this.labelUserID.text.Replace(" ", string.Empty);
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
			if (null != cmd_ModalMessage)
			{
				cmd_ModalMessage.Title = StringMaster.GetString("SystemCopy");
				cmd_ModalMessage.Info = StringMaster.GetString("MyProfile-06");
			}
		}
	}

	public override void ClosePanel(bool animation = true)
	{
		this.inputNickName.value = this.TrimmingSpaceChar(this.inputNickName.value, "[ \n]");
		this.inputComment.value = this.TrimmingSpaceChar(this.inputComment.value, "[\n]");
		CMD_Profile.CheckInputCharResult checkInputCharResult = this.CheckInputChar(this.inputComment.value, 0, CMD_Profile.COMMENT_LENGTH_MAX);
		checkInputCharResult = ((checkInputCharResult != CMD_Profile.CheckInputCharResult.OK) ? checkInputCharResult : this.CheckInputChar(this.inputNickName.value, CMD_Profile.NICK_NAME_LENGTH_MIN, CMD_Profile.NICK_NAME_LENGTH_MAX));
		if (checkInputCharResult != CMD_Profile.CheckInputCharResult.OK)
		{
			this.ShowErrorPopUp(checkInputCharResult);
			this.RefreshComponents();
		}
		else
		{
			bool flag = this.IsUpdateNickName();
			bool flag2 = this.IsUpdateComment();
			if (flag || flag2)
			{
				this.OpenDialogSubmitConfirm(flag, flag2, new Action<int>(this.OnCloseNicknameUpdate));
			}
			else
			{
				CMD_Profile.instance = null;
				base.ClosePanel(animation);
			}
		}
	}

	private string TrimmingSpaceChar(string input, string pattern)
	{
		return Regex.Replace(input, pattern, string.Empty);
	}

	private CMD_Profile.CheckInputCharResult CheckInputChar(string text, int min, int limit)
	{
		CMD_Profile.CheckInputCharResult result = CMD_Profile.CheckInputCharResult.OK;
		if (TextUtil.SurrogateCheck(text))
		{
			result = CMD_Profile.CheckInputCharResult.FORBIDDEN_CHAR;
		}
		else if (limit < text.Length)
		{
			result = CMD_Profile.CheckInputCharResult.LIMIT_OVER;
		}
		else if (min > text.Length)
		{
			result = CMD_Profile.CheckInputCharResult.FEW_CHAR;
		}
		return result;
	}

	private void ShowErrorPopUp(CMD_Profile.CheckInputCharResult checkResult)
	{
		if (checkResult != CMD_Profile.CheckInputCharResult.FORBIDDEN_CHAR)
		{
			if (checkResult != CMD_Profile.CheckInputCharResult.LIMIT_OVER)
			{
				if (checkResult == CMD_Profile.CheckInputCharResult.FEW_CHAR)
				{
					AlertManager.ShowAlertDialog(null, "E-US07");
				}
			}
			else
			{
				AlertManager.ShowAlertDialog(null, "E-US18");
			}
		}
		else
		{
			AlertManager.ShowAlertDialog(null, "E-US17");
		}
	}

	private void OpenDialogSubmitConfirm(bool isUpdatedNickName, bool isUpdatedComment, Action<int> OnCloseConfirm)
	{
		string title = string.Empty;
		string info = string.Empty;
		if (isUpdatedNickName && isUpdatedComment)
		{
			title = StringMaster.GetString("MyProfile-11");
			info = StringMaster.GetString("MyProfile-12");
		}
		else if (isUpdatedComment)
		{
			title = StringMaster.GetString("MyProfile-07");
			info = StringMaster.GetString("MyProfile-08");
		}
		else if (isUpdatedNickName)
		{
			title = StringMaster.GetString("MyProfile-09");
			info = StringMaster.GetString("MyProfile-10");
		}
		CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(OnCloseConfirm, "CMD_Confirm", null) as CMD_Confirm;
		if (null != cmd_Confirm)
		{
			cmd_Confirm.Title = title;
			cmd_Confirm.Info = info;
		}
	}

	private void OnCloseNicknameUpdate(int selectButton)
	{
		if (selectButton == 0)
		{
			this.NicknameUpdate(delegate
			{
				this.<ClosePanel>__BaseCallProxy1(true);
			});
		}
		else
		{
			base.ClosePanel(true);
		}
	}

	private void OpenBlockListNicknameUpdate(int selectButton)
	{
		if (selectButton == 0)
		{
			this.NicknameUpdate(delegate
			{
				GUIMain.ShowCommonDialog(delegate(int index)
				{
					this.OnReturnBlockList();
				}, "CMD_BlockList", null);
			});
		}
		else
		{
			this.RefreshComponents();
			GUIMain.ShowCommonDialog(delegate(int index)
			{
				this.OnReturnBlockList();
			}, "CMD_BlockList", null);
		}
		this.characterCameraView.csRender3DRT.gameObject.transform.localScale = Vector3.zero;
	}

	private void NicknameUpdate(Action OnSuccess)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		APIRequestTask apirequestTask = new APIRequestTask();
		if (this.IsUpdateNickName())
		{
			apirequestTask.Add(Singleton<UserDataMng>.Instance.RequestUpdateNickName(this.inputNickName.value, false));
		}
		if (this.IsUpdateComment())
		{
			apirequestTask.Add(Singleton<UserDataMng>.Instance.RequestUpdateComment(this.inputComment.value, false));
		}
		base.StartCoroutine(apirequestTask.Run(delegate
		{
			RestrictionInput.EndLoad();
			this.inputNickName.value = this.inputNickName.value;
			this.inputComment.value = this.inputComment.value;
			GUIPlayerStatus.RefreshParams_S(false);
			OnSuccess();
		}, delegate(Exception nop)
		{
			RestrictionInput.EndLoad();
			this.RefreshComponents();
		}, null));
	}

	private new void OnClickedScreen()
	{
		this.googlePlay.EnableMenu(false);
		if (!this.isOpenScreen)
		{
			base.MoveTo(this.OPEN_BLOCK_LIST.transform.parent.gameObject, this.vPosSCR_BLOCK, 0.18f, iTween.EaseType.linear);
			base.MoveTo(this.playHistoryButton, this.vPosSCR_HISTORY, 0.18f, iTween.EaseType.linear);
		}
		else
		{
			base.MoveTo(this.OPEN_BLOCK_LIST.transform.parent.gameObject, this.vOrgSCR_BLOCK, 0.18f, iTween.EaseType.linear);
			base.MoveTo(this.playHistoryButton, this.vOrgSCR_HISTORY, 0.18f, iTween.EaseType.linear);
		}
		base.OnClickedScreen();
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		UIPanel uipanel = GUIMain.GetUIPanel();
		Vector2 windowSize = uipanel.GetWindowSize();
		this.vOrgSCR_BLOCK = this.OPEN_BLOCK_LIST.transform.parent.localPosition;
		this.vOrgSCR_HISTORY = this.playHistoryButton.transform.localPosition;
		this.vPosSCR_BLOCK = this.vOrgSCR_BLOCK;
		this.vPosSCR_BLOCK.x = windowSize.x;
		this.vPosSCR_HISTORY = this.vOrgSCR_HISTORY;
		this.vPosSCR_HISTORY.x = windowSize.x;
		this.vPosSCR_TITLE = this.vOrgSCR_TITLE;
		this.vPosSCR_TITLE.x = windowSize.x;
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (null != tutorialObserver)
		{
			GUIMain.BarrierON(null);
			TutorialObserver tutorialObserver2 = tutorialObserver;
			string tutorialName = "second_tutorial_profile";
			if (CMD_Profile.<>f__mg$cache0 == null)
			{
				CMD_Profile.<>f__mg$cache0 = new Action(GUIMain.BarrierOFF);
			}
			tutorialObserver2.StartSecondTutorial(tutorialName, CMD_Profile.<>f__mg$cache0, delegate
			{
				GUICollider.EnableAllCollider("CMD_Profile");
			});
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_Profile");
		}
	}

	private void OnBlockList()
	{
		this.inputNickName.value = this.TrimmingSpaceChar(this.inputNickName.value, "[ \n]");
		this.inputComment.value = this.TrimmingSpaceChar(this.inputComment.value, "[\n]");
		CMD_Profile.CheckInputCharResult checkInputCharResult = this.CheckInputChar(this.inputComment.value, 0, CMD_Profile.COMMENT_LENGTH_MAX);
		checkInputCharResult = ((checkInputCharResult != CMD_Profile.CheckInputCharResult.OK) ? checkInputCharResult : this.CheckInputChar(this.inputNickName.value, CMD_Profile.NICK_NAME_LENGTH_MIN, CMD_Profile.NICK_NAME_LENGTH_MAX));
		if (checkInputCharResult != CMD_Profile.CheckInputCharResult.OK)
		{
			this.ShowErrorPopUp(checkInputCharResult);
			this.RefreshComponents();
		}
		else
		{
			bool flag = this.IsUpdateNickName();
			bool flag2 = this.IsUpdateComment();
			if (flag || flag2)
			{
				this.OpenDialogSubmitConfirm(flag, flag2, new Action<int>(this.OpenBlockListNicknameUpdate));
			}
			else
			{
				CMD_Profile.instance = null;
				GUIMain.ShowCommonDialog(delegate(int index)
				{
					this.OnReturnBlockList();
				}, "CMD_BlockList", null);
				this.characterCameraView.csRender3DRT.gameObject.transform.localScale = Vector3.zero;
			}
		}
	}

	private void OnReturnBlockList()
	{
		if (null != this.characterCameraView.csRender3DRT)
		{
			this.characterCameraView.csRender3DRT.gameObject.transform.localScale = Vector3.one;
		}
	}

	private void OnplayHistory()
	{
		CMD_PlayHistory cmd_PlayHistory = GUIMain.ShowCommonDialog(null, "CMD_PlayHistory", null) as CMD_PlayHistory;
		cmd_PlayHistory.SetColosseumInfo(this.colosseumUserStatus);
	}

	protected override void SetColosseumUserStatus()
	{
	}

	private void OnTitleSelect()
	{
		GUIMain.ShowCommonDialog(null, "CMD_TitleSelect", null);
	}

	private enum CheckInputCharResult
	{
		OK,
		FORBIDDEN_CHAR,
		LIMIT_OVER,
		FEW_CHAR
	}
}
