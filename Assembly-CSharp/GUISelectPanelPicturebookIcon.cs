﻿using Master;
using MonsterIcon;
using MonsterPicturebook;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public sealed class GUISelectPanelPicturebookIcon : GUISelectPanelBSPartsUD
{
	[SerializeField]
	private int PARTS_CT_MN;

	[SerializeField]
	private Color iconTextColor = new Color(1f, 1f, 0f, 1f);

	private void SetIconMonsterData(GUIMonsterIcon monsterIcon, PicturebookMonster monsterData, Action<PicturebookMonster> actionShortPress)
	{
		monsterIcon.SetMonsterIcon(monsterData.monsterMaster.Simple.iconId, monsterData.monsterMaster.Simple.rare, monsterData.monsterMaster.Group.growStep);
		if (monsterData.isUnknown)
		{
			MonsterIconGrayout.SetGrayout(monsterIcon.gameObject, GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			monsterIcon.SortMess = StringMaster.GetString("EvolutionUnkown");
			monsterIcon.SetSortMessageColor(this.iconTextColor);
			monsterIcon.SetTouchAct_S(null);
		}
		else
		{
			monsterIcon.SetTouchAct_S(delegate(MonsterData noop)
			{
				actionShortPress(monsterData);
			});
		}
		monsterIcon.SetTouchAct_L(null);
	}

	public IEnumerator AllBuild(List<PicturebookMonster> monsterDataList, Vector3 partsObjectScale, Action<PicturebookMonster> actionTap)
	{
		base.InitBuild();
		this.partsCount = monsterDataList.Count;
		int phct = this.partsCount / this.PARTS_CT_MN;
		if (this.partsCount % this.PARTS_CT_MN > 0)
		{
			phct++;
		}
		GUISelectPanelBSPartsUD.PanelBuildData pbd = base.CalcBuildData(this.PARTS_CT_MN, phct, partsObjectScale.x, partsObjectScale.y);
		float ypos = pbd.startY;
		float xpos = pbd.startX;
		Stopwatch sw = new Stopwatch();
		sw.Start();
		long nextReturnMiliSecond = 0L;
		int lineCount = 0;
		for (int i = 0; i < this.partsCount; i++)
		{
			GameObject parts = base.AddBuildPart();
			parts.SetActive(true);
			parts.transform.parent = base.transform;
			DepthController dc = parts.GetComponent<DepthController>();
			if (null != dc)
			{
				dc.AddWidgetDepth(10);
			}
			partsObjectScale.z = 1f;
			parts.transform.localScale = partsObjectScale;
			GUIMonsterIcon monsterIcon = parts.GetComponent<GUIMonsterIcon>();
			if (null != monsterIcon)
			{
				float x = xpos + pbd.pitchW * (float)(lineCount % this.PARTS_CT_MN);
				monsterIcon.SetOriginalPos(new Vector3(x, ypos, -5f));
				this.SetIconMonsterData(monsterIcon, monsterDataList[i], actionTap);
			}
			lineCount++;
			if (lineCount % this.PARTS_CT_MN == 0)
			{
				ypos -= pbd.pitchH;
			}
			if (sw.ElapsedMilliseconds > nextReturnMiliSecond)
			{
				nextReturnMiliSecond += 100L;
				yield return null;
			}
		}
		base.height = pbd.lenH;
		base.InitMinMaxLocation(-1, 0f);
		yield break;
	}
}
