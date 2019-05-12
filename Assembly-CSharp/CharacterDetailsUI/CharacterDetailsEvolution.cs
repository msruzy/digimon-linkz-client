﻿using System;
using UnityEngine;

namespace CharacterDetailsUI
{
	public sealed class CharacterDetailsEvolution : ICharacterDetailsUIAnimation
	{
		private PartsUpperCutinController.AnimeType cutinType;

		private int cutinSortingOrder;

		private Transform cutinParentObject;

		private PartsUpperCutinController cutinController;

		private Action onEndCutin;

		private void OnFinishCutin()
		{
			if (this.onEndCutin != null)
			{
				this.onEndCutin();
			}
		}

		public void OnOpenWindow()
		{
			this.cutinController = PartsUpperCutinController.Create(this.cutinParentObject, this.cutinSortingOrder);
		}

		public void OnCloseWindow()
		{
		}

		public void OnOpenMenu()
		{
		}

		public void OnCloseMenu()
		{
		}

		public void StartAnimation()
		{
			this.cutinController.PlayAnimator(this.cutinType, new Action(this.OnFinishCutin));
		}

		public void Initialize(int windowSortingOrder, Transform windowRoot, string evolutionType, Action endCutin)
		{
			if (evolutionType != null && !(evolutionType == "1"))
			{
				if (evolutionType == "2")
				{
					this.cutinType = PartsUpperCutinController.AnimeType.ModeChangeComplete;
					goto IL_7C;
				}
				if (evolutionType == "3")
				{
					this.cutinType = PartsUpperCutinController.AnimeType.Jogress;
					goto IL_7C;
				}
				if (evolutionType == "4")
				{
					this.cutinType = PartsUpperCutinController.AnimeType.Combine;
					goto IL_7C;
				}
			}
			this.cutinType = PartsUpperCutinController.AnimeType.EvolutionComplete;
			IL_7C:
			this.cutinSortingOrder = windowSortingOrder + 1;
			this.onEndCutin = endCutin;
			this.cutinParentObject = windowRoot;
		}
	}
}
