﻿using Evolution;
using EvolutionSelect;
using Master;
using Monster;
using Picturebook;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class GUIListPartsEvolution : GUIListPartBS
{
	private GUIListPartsEvolution instance;

	[SerializeField]
	private GameObject goMONS_NEXT;

	[SerializeField]
	private List<GameObject> goITEMS;

	[SerializeField]
	private List<GameObject> goITEMS_NUM;

	[SerializeField]
	private GameObject goCAN_EVOLVE;

	[SerializeField]
	private UILabel ngTX_MONS_NEXT_NAME;

	[SerializeField]
	private UILabel ngTX_MONS_NEXT_SKILL;

	[SerializeField]
	private UILabel specificTypeName;

	[SerializeField]
	private UILabel ngTAG_NEED_CHIP;

	[SerializeField]
	private UILabel ngTX_NEED_CHIP;

	[SerializeField]
	private EvolutionSelectExecutionButton executionButton;

	[SerializeField]
	private Color iconTextColor = new Color(1f, 1f, 0f, 1f);

	private GUIMonsterIcon csNextMons;

	private string evolutionType;

	public const string NORMAL_EVOLVE = "1";

	public const string MODE_CHANGE_EVOLVE = "2";

	public const string JOGRESS = "3";

	public const string COMBINE = "4";

	public const string VERSION_UP = "5";

	private string notEnoughFormat;

	private string enoughFormat;

	private int needCluster;

	private int myCluster;

	private bool isShowedItemIcon;

	private EvolutionData.MonsterEvolveData data;

	private Action<EvolutionData.MonsterEvolveData, int> onEvolution;

	public EvolutionData.MonsterEvolveData Data
	{
		get
		{
			return this.data;
		}
		set
		{
			this.data = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.instance = this;
		this.SetupLocalize();
	}

	private void SetupLocalize()
	{
		this.notEnoughFormat = StringMaster.GetString("EvolutionNotEnoughFraction");
		this.enoughFormat = StringMaster.GetString("EvolutionEnoughFraction");
	}

	public void ChangeSprite(string sprName)
	{
		UISprite component = base.gameObject.GetComponent<UISprite>();
		if (component != null)
		{
			component.spriteName = sprName;
			component.MakePixelPerfect();
		}
	}

	private string GetEvolutionName()
	{
		string result = string.Empty;
		if ("2" == this.evolutionType)
		{
			result = StringMaster.GetString("EvolutionModeChange");
		}
		else
		{
			result = StringMaster.GetString("EvolutionTitle");
		}
		return result;
	}

	public void Initialize(Action<EvolutionData.MonsterEvolveData, int> evolutionAction)
	{
		this.onEvolution = evolutionAction;
		this.evolutionType = ClassSingleton<EvolutionData>.Instance.GetEvolutionEffectType(this.data.md.userMonster.monsterId, this.data.md_next.userMonster.monsterId);
		this.ngTAG_NEED_CHIP.text = StringMaster.GetString("SystemCost");
		this.myCluster = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney);
		if (this.evolutionType == "1" || this.evolutionType == "3" || this.evolutionType == "4")
		{
			this.needCluster = EvolutionData.CalcClusterForEvolve(this.data.md_next.monsterM.monsterId);
		}
		else if (this.evolutionType == "5")
		{
			this.needCluster = EvolutionData.CalcClusterForVersionUp(this.data.md_next.monsterM.monsterId);
		}
		else if (this.evolutionType == "2")
		{
			this.needCluster = EvolutionData.CalcClusterForModeChange(this.data.md_next.monsterM.monsterId);
		}
		this.ShowNextMonsterIcon();
		this.ShowNextMonsterData();
		this.executionButton.Initialize(new Action<CMD>(this.OnPushConfirmYesButton));
		UILabel component = this.goCAN_EVOLVE.GetComponent<UILabel>();
		if (this.CanEvolve())
		{
			if (this.evolutionType == "2")
			{
				if (this.myCluster < this.needCluster)
				{
					component.text = string.Format(StringMaster.GetString("EvolutionImpossible"), this.GetEvolutionName());
					this.SetDeactiveButton();
				}
				else
				{
					this.SetActiveButton();
				}
			}
			else if (this.evolutionType == "1" || this.evolutionType == "3" || this.evolutionType == "4" || this.evolutionType == "5")
			{
				if (this.myCluster < this.needCluster)
				{
					component.text = string.Format(StringMaster.GetString("EvolutionImpossible"), this.GetEvolutionName());
					this.SetDeactiveButton();
				}
				else
				{
					int num = int.Parse(this.data.md.userMonster.level);
					int num2 = int.Parse(this.data.md.monsterM.maxLevel);
					if (num >= num2)
					{
						this.SetActiveButton();
					}
					else
					{
						component.text = StringMaster.GetString("EvolutionNotEnoughLv");
						this.SetDeactiveButton();
					}
				}
			}
			else
			{
				global::Debug.LogError("進化タイプエラー.");
			}
		}
		else
		{
			component.text = string.Format(StringMaster.GetString("EvolutionImpossible"), this.GetEvolutionName());
			this.SetDeactiveButton();
		}
	}

	private void SetActiveButton()
	{
		UILabel component = this.goCAN_EVOLVE.GetComponent<UILabel>();
		component.text = string.Format(StringMaster.GetString("EvolutionPossible"), this.GetEvolutionName());
		component.color = ConstValue.DIGIMON_YELLOW;
	}

	private void SetDeactiveButton()
	{
		this.executionButton.SetImpossibleButton();
		UILabel component = this.goCAN_EVOLVE.GetComponent<UILabel>();
		component.color = ConstValue.DIGIMON_BLUE;
	}

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchBegan(touch, pos);
	}

	public override void OnTouchMoved(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchMoved(touch, pos);
	}

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchEnded(touch, pos, flag);
	}

	public bool IsPossessCluster()
	{
		return this.myCluster >= this.needCluster;
	}

	private void OnPushConfirmYesButton(CMD confirmPopup)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		confirmPopup.SetCloseAction(delegate(int noop)
		{
			this.onEvolution(this.data, this.needCluster);
		});
	}

	private void OnLongPushedMonsterIcon(MonsterData tappedMonsterData)
	{
		CMD_CharacterDetailed.DataChg = null;
		CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(null, "CMD_CharacterDetailed", null) as CMD_CharacterDetailed;
		cmd_CharacterDetailed.SetViewNextEvolutionMonster(tappedMonsterData.monsterM.monsterId, CMD_BaseSelect.DataChg.userMonster);
		cmd_CharacterDetailed.DisableEvolutionButton();
	}

	protected override void Update()
	{
		base.Update();
		if (!this.isShowedItemIcon)
		{
			this.ShowItemIcon();
			this.isShowedItemIcon = true;
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.instance = null;
	}

	private void ShowNextMonsterIcon()
	{
		Transform transform = this.goMONS_NEXT.transform;
		this.csNextMons = GUIMonsterIcon.MakePrefabByMonsterData(this.data.md_next, transform.localScale, transform.localPosition, transform.parent, true, false);
		this.goMONS_NEXT.SetActive(false);
		if (!MonsterPicturebookData.ExistPicturebook(this.data.md_next.monsterMG.monsterCollectionId))
		{
			this.csNextMons.SortMess = StringMaster.GetString("EvolutionUnkown");
			this.csNextMons.SetSortMessageColor(this.iconTextColor);
		}
		this.csNextMons.SetTouchAct_L(new Action<MonsterData>(this.OnLongPushedMonsterIcon));
		UIWidget component = this.goMONS_NEXT.GetComponent<UIWidget>();
		UIWidget component2 = this.csNextMons.gameObject.GetComponent<UIWidget>();
		if (component != null && component2 != null)
		{
			int add = component.depth - component2.depth;
			DepthController component3 = this.csNextMons.gameObject.GetComponent<DepthController>();
			component3.AddWidgetDepth(this.csNextMons.gameObject.transform, add);
		}
	}

	private void ShowNextMonsterData()
	{
		this.ngTX_MONS_NEXT_NAME.text = this.data.md_next.monsterMG.monsterName;
		this.ngTX_MONS_NEXT_SKILL.text = MonsterTribeData.GetTribeName(this.data.md_next.monsterMG.tribe);
		this.specificTypeName.text = MonsterSpecificTypeData.GetSpecificTypeName(this.data.md_next.monsterMG.monsterStatusId);
		int num = 0;
		if (this.evolutionType == "1" || this.evolutionType == "3" || this.evolutionType == "4")
		{
			string monsterId = this.data.md_next.monsterM.monsterId;
			num = EvolutionData.CalcClusterForEvolve(monsterId);
		}
		else if (this.evolutionType == "5")
		{
			string monsterId2 = this.data.md.monsterM.monsterId;
			num = EvolutionData.CalcClusterForVersionUp(monsterId2);
		}
		else if (this.evolutionType == "2")
		{
			string monsterId3 = this.data.md_next.monsterM.monsterId;
			num = EvolutionData.CalcClusterForModeChange(monsterId3);
		}
		this.ngTX_NEED_CHIP.text = StringFormat.Cluster(num);
		int num2 = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney);
		if (num2 < num)
		{
			this.ngTX_NEED_CHIP.color = Color.red;
		}
	}

	private bool CanEvolve()
	{
		List<EvolutionData.MonsterEvolveItem> itemList = this.data.itemList;
		for (int i = 0; i < itemList.Count; i++)
		{
			if (itemList[i].catId == ConstValue.EVOLVE_ITEM_MONS)
			{
				if (itemList[i].need_num > itemList[i].haveNum)
				{
					return false;
				}
			}
			else if (itemList[i].catId == ConstValue.EVOLVE_ITEM_SOUL)
			{
				if (itemList[i].need_num > itemList[i].haveNum)
				{
					return false;
				}
			}
			else
			{
				global::Debug.LogError("ここへは来るのか？");
			}
		}
		return true;
	}

	private void ShowItemIcon()
	{
		List<EvolutionData.MonsterEvolveItem> itemList = this.data.itemList;
		int i;
		for (i = 0; i < itemList.Count; i++)
		{
			UILabel component = this.goITEMS_NUM[i].GetComponent<UILabel>();
			string evolveItemIconPathByID = ClassSingleton<EvolutionData>.Instance.GetEvolveItemIconPathByID(itemList[i].sd_item.soulId);
			Vector3 localScale = this.goITEMS[i].transform.localScale;
			this.goITEMS[i].transform.localScale = Vector2.zero;
			this.LoadObjectASync(evolveItemIconPathByID, i, localScale);
			string soulId = itemList[i].sd_item.soulId;
			GUICollider component2 = this.goITEMS[i].GetComponent<GUICollider>();
			component2.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
			{
				this.ActCallBackDropItem(soulId);
			};
			if (itemList[i].haveNum < itemList[i].need_num)
			{
				component.text = string.Format(this.notEnoughFormat, itemList[i].haveNum, itemList[i].need_num);
			}
			else
			{
				component.text = string.Format(this.enoughFormat, itemList[i].haveNum, itemList[i].need_num);
			}
		}
		while (i < this.goITEMS.Count)
		{
			this.goITEMS[i].SetActive(false);
			this.goITEMS_NUM[i].SetActive(false);
			i++;
		}
	}

	private void LoadObjectASync(string path, int idx, Vector3 vS)
	{
		AssetDataMng.Instance().LoadObjectASync(path, delegate(UnityEngine.Object obj)
		{
			this.ShowDropItemsCB(obj, idx, vS);
		});
	}

	private void ShowDropItemsCB(UnityEngine.Object obj, int idx, Vector3 vS)
	{
		if (this.instance != null)
		{
			Texture2D mainTexture = obj as Texture2D;
			UITexture component = this.goITEMS[idx].GetComponent<UITexture>();
			component.mainTexture = mainTexture;
			List<EvolutionData.MonsterEvolveItem> itemList = this.data.itemList;
			if (itemList[idx].need_num > itemList[idx].haveNum)
			{
				component.color = new Color(0.6f, 0.6f, 0.6f, 1f);
			}
			ITweenFrameController component2 = this.goITEMS[idx].GetComponent<ITweenFrameController>();
			if (component2 == null)
			{
				this.goITEMS[idx].AddComponent<ITweenFrameController>();
			}
		}
	}

	private void ScaleEnd()
	{
	}

	private void ActCallBackDropItem(string soulId)
	{
		GameWebAPI.RespDataMA_GetSoulM.SoulM soulMaster = ClassSingleton<EvolutionData>.Instance.GetSoulMaster(soulId);
		CMD_QuestItemPOP.Create(soulMaster);
	}

	public string GetEvotuionType()
	{
		return this.evolutionType;
	}
}
