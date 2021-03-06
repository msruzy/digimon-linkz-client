﻿using Master;
using Monster;
using System;
using UnityEngine;

public sealed class MonsterStatusChangeValueList : MonoBehaviour
{
	[SerializeField]
	private UILabel hpLabel;

	[SerializeField]
	private UILabel attackLabel;

	[SerializeField]
	private UILabel defenseLabel;

	[SerializeField]
	private UILabel magicAttackLabel;

	[SerializeField]
	private UILabel magicDefenseLabel;

	[SerializeField]
	private UILabel speedLabel;

	[SerializeField]
	private UILabel luckLabel;

	[SerializeField]
	private UILabel friendshipLabel;

	[SerializeField]
	private Color colorDefault = new Color(0.2274f, 0.2274f, 0.2274f, 1f);

	[SerializeField]
	private Color colorUp = new Color(0f, 0.5882f, 0f, 1f);

	[SerializeField]
	private Color colorDown = new Color(1f, 0f, 0f, 1f);

	public void SetValues(StatusValue beforeStatus, StatusValue afterStatus)
	{
		this.SetParameter(this.hpLabel, afterStatus.hp - beforeStatus.hp);
		this.SetParameter(this.attackLabel, afterStatus.attack - beforeStatus.attack);
		this.SetParameter(this.defenseLabel, afterStatus.defense - beforeStatus.defense);
		this.SetParameter(this.magicAttackLabel, afterStatus.magicAttack - beforeStatus.magicAttack);
		this.SetParameter(this.magicDefenseLabel, afterStatus.magicDefense - beforeStatus.magicDefense);
		this.SetParameter(this.speedLabel, afterStatus.speed - beforeStatus.speed);
		this.SetParameter(this.luckLabel, afterStatus.luck - beforeStatus.luck);
		this.SetParameter(this.friendshipLabel, afterStatus.friendship - beforeStatus.friendship);
	}

	public void SetValues(MonsterData monsterData)
	{
		StatusValue statusValue = MonsterStatusData.GetStatusValue(monsterData.userMonster.monsterId, monsterData.userMonster.level);
		StatusValue status = monsterData.GetStatus();
		this.SetPlusParameter(this.hpLabel, status.hp - statusValue.hp);
		this.SetPlusParameter(this.attackLabel, status.attack - statusValue.attack);
		this.SetPlusParameter(this.defenseLabel, status.defense - statusValue.defense);
		this.SetPlusParameter(this.magicAttackLabel, status.magicAttack - statusValue.magicAttack);
		this.SetPlusParameter(this.magicDefenseLabel, status.magicDefense - statusValue.magicDefense);
		this.SetPlusParameter(this.speedLabel, status.speed - statusValue.speed);
	}

	private void SetParameter(UILabel label, int value)
	{
		label.text = value.ToString(StringMaster.GetString("MSChangeValueList_txt"));
		if (value > 0)
		{
			label.color = this.colorUp;
		}
		else if (value < 0)
		{
			label.color = this.colorDown;
		}
		else
		{
			label.color = this.colorDefault;
		}
	}

	private void SetPlusParameter(UILabel label, int value)
	{
		if (value != 0)
		{
			this.SetParameter(label, value);
		}
		else
		{
			label.text = StringMaster.GetString("CharaStatus-01");
		}
	}

	public void SetEggStatusValues()
	{
		this.hpLabel.text = StringMaster.GetString("CharaStatus-01");
		this.attackLabel.text = StringMaster.GetString("CharaStatus-01");
		this.defenseLabel.text = StringMaster.GetString("CharaStatus-01");
		this.magicAttackLabel.text = StringMaster.GetString("CharaStatus-01");
		this.magicDefenseLabel.text = StringMaster.GetString("CharaStatus-01");
		this.speedLabel.text = StringMaster.GetString("CharaStatus-01");
		this.luckLabel.text = StringMaster.GetString("CharaStatus-01");
		this.friendshipLabel.text = StringMaster.GetString("CharaStatus-01");
	}
}
