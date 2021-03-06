﻿using System;

namespace UnityEngine
{
	[Obsolete("Built-in support for Substance Designer materials has been deprecated and will be removed in Unity 2018.1. To continue using Substance Designer materials in Unity 2018.1, you will need to install Allegorithmic's external importer from the Asset Store.", false)]
	public enum ProceduralPropertyType
	{
		Boolean,
		Float,
		Vector2,
		Vector3,
		Vector4,
		Color3,
		Color4,
		Enum,
		Texture,
		String
	}
}
