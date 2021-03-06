﻿using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[UsedByNativeCode]
	public struct UIVertex
	{
		public Vector3 position;

		public Vector3 normal;

		public Color32 color;

		public Vector2 uv0;

		public Vector2 uv1;

		public Vector2 uv2;

		public Vector2 uv3;

		public Vector4 tangent;

		private static readonly Color32 s_DefaultColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		private static readonly Vector4 s_DefaultTangent = new Vector4(1f, 0f, 0f, -1f);

		public static UIVertex simpleVert = new UIVertex
		{
			position = Vector3.zero,
			normal = Vector3.back,
			tangent = UIVertex.s_DefaultTangent,
			color = UIVertex.s_DefaultColor,
			uv0 = Vector2.zero,
			uv1 = Vector2.zero,
			uv2 = Vector2.zero,
			uv3 = Vector2.zero
		};
	}
}
