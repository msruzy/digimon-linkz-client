﻿using Master;
using Monster;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_InheritCheck : CMD
{
	[Header("タイトルのラベル")]
	[SerializeField]
	private UILabel titleLabel;

	[Header("デジモンの継承スキル")]
	[SerializeField]
	private MonsterLearnSkill monsterSuccessionSkill;

	[Header("消費クラスタのラベル")]
	[SerializeField]
	private UILabel useClusterLabel;

	[Header("基本的なメッセージのラベル")]
	[SerializeField]
	private UILabel normalMessageLabel;

	[Header("警告のメッセージのラベル")]
	[SerializeField]
	private UILabel warningMessageLabel;

	[Header("警告ダイアログの窓本体")]
	[SerializeField]
	private GameObject dialogPlate;

	[SerializeField]
	[Header("警告ダイアログのボタングループ")]
	private GameObject btnGroup;

	public void SetParams(List<MonsterData> selectedMonsterDataList, string useCluster, int baseDigimonSkillNumber, int partnerDigimonSkillNumber)
	{
		if (partnerDigimonSkillNumber == 1)
		{
			this.monsterSuccessionSkill.SetCommonSkill(selectedMonsterDataList[0]);
		}
		else
		{
			this.monsterSuccessionSkill.SetCommonSkill2(selectedMonsterDataList[0]);
		}
		bool flag = MonsterUserDataMng.AnyChipEquipMonster(selectedMonsterDataList);
		bool flag2 = MonsterUserDataMng.AnyHighGrowStepMonster(selectedMonsterDataList);
		List<string> list = new List<string>();
		bool flag3 = false;
		bool flag4 = false;
		foreach (MonsterData monsterData in selectedMonsterDataList)
		{
			bool flag5 = MonsterStatusData.IsArousal(monsterData.GetMonsterMaster().Simple.rare);
			bool flag6 = MonsterStatusData.IsVersionUp(monsterData.GetMonsterMaster().Simple.rare);
			if (flag6)
			{
				flag4 = true;
			}
			else if (flag5)
			{
				flag3 = true;
			}
		}
		if (flag3)
		{
			list.Add(StringMaster.GetString("SuccessionCautionArousal"));
		}
		if (flag4)
		{
			list.Add(StringMaster.GetString("SuccessionCautionVersionUp"));
		}
		if (flag)
		{
			list.Add(StringMaster.GetString("CautionDisappearChip"));
		}
		if (flag2)
		{
			string growStep = ConstValue.GROW_STEP_HIGH.ToString();
			string growStepName = MonsterGrowStepData.GetGrowStepName(growStep);
			list.Add(string.Format(StringMaster.GetString("SuccessionCautionGrowth"), growStepName));
		}
		if (list.Count >= 2)
		{
			int num = (this.warningMessageLabel.fontSize + this.warningMessageLabel.spacingY) * (list.Count - 1);
			this.warningMessageLabel.transform.SetLocalY(this.warningMessageLabel.transform.localPosition.y - (float)(num / 2));
			this.dialogPlate.transform.SetLocalY(this.dialogPlate.transform.localPosition.y - (float)(num / 2));
			this.dialogPlate.GetComponent<UIWidget>().height += num;
			this.btnGroup.transform.SetLocalY(this.btnGroup.transform.localPosition.y - (float)num);
		}
		this.warningMessageLabel.text = string.Join("\n", list.ToArray());
		this.useClusterLabel.text = useCluster;
		string @string = StringMaster.GetString("SuccessionConfirmInfo");
		this.normalMessageLabel.text = string.Format(@string, baseDigimonSkillNumber);
	}

	protected override void Awake()
	{
		base.Awake();
		this.titleLabel.text = StringMaster.GetString("SuccessionConfirmTitle");
	}
}
