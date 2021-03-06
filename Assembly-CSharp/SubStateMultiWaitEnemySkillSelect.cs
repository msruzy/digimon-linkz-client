﻿using System;
using System.Collections;
using UnityEngine;

public class SubStateMultiWaitEnemySkillSelect : SubStateMultiPlayerCharacterAndSkillSelectFunction
{
	public SubStateMultiWaitEnemySkillSelect(Action OnExit, Action<EventState> OnExitGotEvent) : base(OnExit, OnExitGotEvent)
	{
	}

	protected override void EnabledThisState()
	{
		base.EnabledThisState();
		base.stateManager.uiControl.SetHudCollider(false);
		base.stateManager.uiControlMulti.ShowLoading(true);
	}

	protected override IEnumerator MainRoutine()
	{
		base.stateManager.multiFunction.CurrentEnemyMyIndex = this.currentCharacter.myIndex;
		IEnumerator coroutine = base.MainRoutine();
		IEnumerator action = base.stateManager.multiFunction.WaitAllPlayers(TCPMessageType.Attack);
		bool isActiveForCoroutine = true;
		bool isActiveForAction = true;
		while (isActiveForCoroutine || isActiveForAction)
		{
			isActiveForCoroutine = coroutine.MoveNext();
			isActiveForAction = action.MoveNext();
			yield return null;
		}
		base.stateManager.uiControlMulti.StartSharedAPAnimation();
		base.stateManager.uiControlMulti.HideRemainingTurnRightDown();
		base.stateManager.targetSelect.TargetManualSelectAndApplyUIFunction(null);
		base.stateManager.uiControl.ApplyCurrentSelectArrow(false, default(Vector3), 0);
		base.stateManager.uiControl.SetHudCollider(false);
		yield break;
	}

	protected override void UpdateTarget()
	{
		if (this.currentCharacter.targetCharacter != null)
		{
			base.stateManager.targetSelect.SetTarget(this.currentCharacter, this.currentCharacter.targetCharacter);
		}
	}

	protected override void DisabledThisState()
	{
		base.DisabledThisState();
		base.stateManager.uiControlMulti.HideLoading();
	}
}
