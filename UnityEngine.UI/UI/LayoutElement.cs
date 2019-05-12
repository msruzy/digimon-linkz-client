﻿using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("Layout/Layout Element", 140)]
	[ExecuteInEditMode]
	public class LayoutElement : UIBehaviour, ILayoutElement, ILayoutIgnorer
	{
		[SerializeField]
		private bool m_IgnoreLayout;

		[SerializeField]
		private float m_MinWidth = -1f;

		[SerializeField]
		private float m_MinHeight = -1f;

		[SerializeField]
		private float m_PreferredWidth = -1f;

		[SerializeField]
		private float m_PreferredHeight = -1f;

		[SerializeField]
		private float m_FlexibleWidth = -1f;

		[SerializeField]
		private float m_FlexibleHeight = -1f;

		protected LayoutElement()
		{
		}

		public virtual bool ignoreLayout
		{
			get
			{
				return this.m_IgnoreLayout;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<bool>(ref this.m_IgnoreLayout, value))
				{
					this.SetDirty();
				}
			}
		}

		public virtual void CalculateLayoutInputHorizontal()
		{
		}

		public virtual void CalculateLayoutInputVertical()
		{
		}

		public virtual float minWidth
		{
			get
			{
				return this.m_MinWidth;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<float>(ref this.m_MinWidth, value))
				{
					this.SetDirty();
				}
			}
		}

		public virtual float minHeight
		{
			get
			{
				return this.m_MinHeight;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<float>(ref this.m_MinHeight, value))
				{
					this.SetDirty();
				}
			}
		}

		public virtual float preferredWidth
		{
			get
			{
				return this.m_PreferredWidth;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<float>(ref this.m_PreferredWidth, value))
				{
					this.SetDirty();
				}
			}
		}

		public virtual float preferredHeight
		{
			get
			{
				return this.m_PreferredHeight;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<float>(ref this.m_PreferredHeight, value))
				{
					this.SetDirty();
				}
			}
		}

		public virtual float flexibleWidth
		{
			get
			{
				return this.m_FlexibleWidth;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<float>(ref this.m_FlexibleWidth, value))
				{
					this.SetDirty();
				}
			}
		}

		public virtual float flexibleHeight
		{
			get
			{
				return this.m_FlexibleHeight;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<float>(ref this.m_FlexibleHeight, value))
				{
					this.SetDirty();
				}
			}
		}

		public virtual int layoutPriority
		{
			get
			{
				return 1;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetDirty();
		}

		protected override void OnTransformParentChanged()
		{
			this.SetDirty();
		}

		protected override void OnDisable()
		{
			this.SetDirty();
			base.OnDisable();
		}

		protected override void OnDidApplyAnimationProperties()
		{
			this.SetDirty();
		}

		protected override void OnBeforeTransformParentChanged()
		{
			this.SetDirty();
		}

		protected void SetDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			LayoutRebuilder.MarkLayoutForRebuild(base.transform as RectTransform);
		}
	}
}
