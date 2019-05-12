﻿using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	[RequireComponent(typeof(RectTransform))]
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	public abstract class LayoutGroup : UIBehaviour, ILayoutElement, ILayoutController, ILayoutGroup
	{
		[SerializeField]
		protected RectOffset m_Padding = new RectOffset();

		[SerializeField]
		[FormerlySerializedAs("m_Alignment")]
		protected TextAnchor m_ChildAlignment;

		[NonSerialized]
		private RectTransform m_Rect;

		protected DrivenRectTransformTracker m_Tracker;

		private Vector2 m_TotalMinSize = Vector2.zero;

		private Vector2 m_TotalPreferredSize = Vector2.zero;

		private Vector2 m_TotalFlexibleSize = Vector2.zero;

		[NonSerialized]
		private List<RectTransform> m_RectChildren = new List<RectTransform>();

		protected LayoutGroup()
		{
			if (this.m_Padding == null)
			{
				this.m_Padding = new RectOffset();
			}
		}

		public RectOffset padding
		{
			get
			{
				return this.m_Padding;
			}
			set
			{
				this.SetProperty<RectOffset>(ref this.m_Padding, value);
			}
		}

		public TextAnchor childAlignment
		{
			get
			{
				return this.m_ChildAlignment;
			}
			set
			{
				this.SetProperty<TextAnchor>(ref this.m_ChildAlignment, value);
			}
		}

		protected RectTransform rectTransform
		{
			get
			{
				if (this.m_Rect == null)
				{
					this.m_Rect = base.GetComponent<RectTransform>();
				}
				return this.m_Rect;
			}
		}

		protected List<RectTransform> rectChildren
		{
			get
			{
				return this.m_RectChildren;
			}
		}

		public virtual void CalculateLayoutInputHorizontal()
		{
			this.m_RectChildren.Clear();
			List<Component> list = ListPool<Component>.Get();
			for (int i = 0; i < this.rectTransform.childCount; i++)
			{
				RectTransform rectTransform = this.rectTransform.GetChild(i) as RectTransform;
				if (!(rectTransform == null) && rectTransform.gameObject.activeInHierarchy)
				{
					rectTransform.GetComponents(typeof(ILayoutIgnorer), list);
					if (list.Count == 0)
					{
						this.m_RectChildren.Add(rectTransform);
					}
					else
					{
						for (int j = 0; j < list.Count; j++)
						{
							ILayoutIgnorer layoutIgnorer = (ILayoutIgnorer)list[j];
							if (!layoutIgnorer.ignoreLayout)
							{
								this.m_RectChildren.Add(rectTransform);
								break;
							}
						}
					}
				}
			}
			ListPool<Component>.Release(list);
			this.m_Tracker.Clear();
		}

		public abstract void CalculateLayoutInputVertical();

		public virtual float minWidth
		{
			get
			{
				return this.GetTotalMinSize(0);
			}
		}

		public virtual float preferredWidth
		{
			get
			{
				return this.GetTotalPreferredSize(0);
			}
		}

		public virtual float flexibleWidth
		{
			get
			{
				return this.GetTotalFlexibleSize(0);
			}
		}

		public virtual float minHeight
		{
			get
			{
				return this.GetTotalMinSize(1);
			}
		}

		public virtual float preferredHeight
		{
			get
			{
				return this.GetTotalPreferredSize(1);
			}
		}

		public virtual float flexibleHeight
		{
			get
			{
				return this.GetTotalFlexibleSize(1);
			}
		}

		public virtual int layoutPriority
		{
			get
			{
				return 0;
			}
		}

		public abstract void SetLayoutHorizontal();

		public abstract void SetLayoutVertical();

		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetDirty();
		}

		protected override void OnDisable()
		{
			this.m_Tracker.Clear();
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
			base.OnDisable();
		}

		protected override void OnDidApplyAnimationProperties()
		{
			this.SetDirty();
		}

		protected float GetTotalMinSize(int axis)
		{
			return this.m_TotalMinSize[axis];
		}

		protected float GetTotalPreferredSize(int axis)
		{
			return this.m_TotalPreferredSize[axis];
		}

		protected float GetTotalFlexibleSize(int axis)
		{
			return this.m_TotalFlexibleSize[axis];
		}

		protected float GetStartOffset(int axis, float requiredSpaceWithoutPadding)
		{
			float num = requiredSpaceWithoutPadding + (float)((axis != 0) ? this.padding.vertical : this.padding.horizontal);
			float num2 = this.rectTransform.rect.size[axis];
			float num3 = num2 - num;
			float num4;
			if (axis == 0)
			{
				num4 = (float)(this.childAlignment % TextAnchor.MiddleLeft) * 0.5f;
			}
			else
			{
				num4 = (float)(this.childAlignment / TextAnchor.MiddleLeft) * 0.5f;
			}
			return (float)((axis != 0) ? this.padding.top : this.padding.left) + num3 * num4;
		}

		protected void SetLayoutInputForAxis(float totalMin, float totalPreferred, float totalFlexible, int axis)
		{
			this.m_TotalMinSize[axis] = totalMin;
			this.m_TotalPreferredSize[axis] = totalPreferred;
			this.m_TotalFlexibleSize[axis] = totalFlexible;
		}

		protected void SetChildAlongAxis(RectTransform rect, int axis, float pos, float size)
		{
			if (rect == null)
			{
				return;
			}
			this.m_Tracker.Add(this, rect, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.SizeDeltaX | DrivenTransformProperties.SizeDeltaY);
			rect.SetInsetAndSizeFromParentEdge((axis != 0) ? RectTransform.Edge.Top : RectTransform.Edge.Left, pos, size);
		}

		private bool isRootLayoutGroup
		{
			get
			{
				Transform parent = base.transform.parent;
				return parent == null || base.transform.parent.GetComponent(typeof(ILayoutGroup)) == null;
			}
		}

		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			if (this.isRootLayoutGroup)
			{
				this.SetDirty();
			}
		}

		protected virtual void OnTransformChildrenChanged()
		{
			this.SetDirty();
		}

		protected void SetProperty<T>(ref T currentValue, T newValue)
		{
			if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
			{
				return;
			}
			currentValue = newValue;
			this.SetDirty();
		}

		protected void SetDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
		}
	}
}
