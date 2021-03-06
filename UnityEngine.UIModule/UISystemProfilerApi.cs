﻿using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class UISystemProfilerApi
	{
		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void BeginSample(UISystemProfilerApi.SampleType type);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void EndSample(UISystemProfilerApi.SampleType type);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void AddMarker(string name, Object obj);

		public enum SampleType
		{
			Layout,
			Render
		}
	}
}
