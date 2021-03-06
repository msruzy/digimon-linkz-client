﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class CMD_ColosseumBonus : CMD
{
	[SerializeField]
	private UILabel messageLabel;

	[SerializeField]
	private TextMeshPro titleTextMeshPro;

	[SerializeField]
	private GameObject rollInnerCircle;

	[SerializeField]
	private List<RewardIconRoot> rewardIconRootList = new List<RewardIconRoot>();

	private bool isInitialized;

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		GUICollider.DisableAllCollider("CMD_ColosseumBonus");
		base.HideDLG();
		base.ShowDLG();
		base.Show(f, sizeX, sizeY, aT);
		GUICollider.EnableAllCollider("CMD_ColosseumBonus");
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		SoundMng.Instance().TryPlaySE("SEInternal/Common/se_305", 0f, false, true, null, -1);
	}

	protected override void Update()
	{
		base.Update();
		if (this.rollInnerCircle != null)
		{
			this.rollInnerCircle.transform.Rotate(new Vector3(0f, 0f, -1f));
		}
		if (this.isInitialized && Input.GetMouseButtonUp(0))
		{
			this.ClosePanel(true);
		}
	}

	public void SetReward(string title, GameWebAPI.ColosseumReward[] rewardList)
	{
		RewardIconRoot rewardIconRoot = this.rewardIconRootList[rewardList.Length - 1];
		rewardIconRoot.SetRewardList(rewardList);
		this.messageLabel.text = rewardIconRoot.itemName;
		this.titleTextMeshPro.text = title;
		CountrySetting.ConvertTMProText(ref this.titleTextMeshPro);
		this.isInitialized = true;
	}
}
