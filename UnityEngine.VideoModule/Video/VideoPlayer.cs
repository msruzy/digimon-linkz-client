﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Video
{
	[RequireComponent(typeof(Transform))]
	public sealed class VideoPlayer : Behaviour
	{
		public extern VideoSource source { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern string url { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern VideoClip clip { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern VideoRenderMode renderMode { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern Camera targetCamera { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern RenderTexture targetTexture { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern Renderer targetMaterialRenderer { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern string targetMaterialProperty { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern VideoAspectRatio aspectRatio { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float targetCameraAlpha { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern Video3DLayout targetCamera3DLayout { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern Texture texture { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public void Prepare()
		{
			VideoPlayer.INTERNAL_CALL_Prepare(this);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Prepare(VideoPlayer self);

		public extern bool isPrepared { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern bool waitForFirstFrame { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool playOnAwake { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public void Play()
		{
			VideoPlayer.INTERNAL_CALL_Play(this);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Play(VideoPlayer self);

		public void Pause()
		{
			VideoPlayer.INTERNAL_CALL_Pause(this);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Pause(VideoPlayer self);

		public void Stop()
		{
			VideoPlayer.INTERNAL_CALL_Stop(this);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Stop(VideoPlayer self);

		public extern bool isPlaying { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern bool canSetTime { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern double time { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern long frame { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool canStep { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void StepForward();

		public extern bool canSetPlaybackSpeed { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern float playbackSpeed { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool isLooping { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool canSetTimeSource { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern VideoTimeSource timeSource { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern VideoTimeReference timeReference { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern double externalReferenceTime { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool canSetSkipOnDrop { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern bool skipOnDrop { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern ulong frameCount { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern float frameRate { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern ushort audioTrackCount { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string GetAudioLanguageCode(ushort trackIndex);

		public ushort GetAudioChannelCount(ushort trackIndex)
		{
			return VideoPlayer.INTERNAL_CALL_GetAudioChannelCount(this, trackIndex);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ushort INTERNAL_CALL_GetAudioChannelCount(VideoPlayer self, ushort trackIndex);

		public static extern ushort controlledAudioTrackMaxCount { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern ushort controlledAudioTrackCount { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public void EnableAudioTrack(ushort trackIndex, bool enabled)
		{
			VideoPlayer.INTERNAL_CALL_EnableAudioTrack(this, trackIndex, enabled);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_EnableAudioTrack(VideoPlayer self, ushort trackIndex, bool enabled);

		public bool IsAudioTrackEnabled(ushort trackIndex)
		{
			return VideoPlayer.INTERNAL_CALL_IsAudioTrackEnabled(this, trackIndex);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_IsAudioTrackEnabled(VideoPlayer self, ushort trackIndex);

		public extern VideoAudioOutputMode audioOutputMode { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool canSetDirectAudioVolume { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public float GetDirectAudioVolume(ushort trackIndex)
		{
			return VideoPlayer.INTERNAL_CALL_GetDirectAudioVolume(this, trackIndex);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float INTERNAL_CALL_GetDirectAudioVolume(VideoPlayer self, ushort trackIndex);

		public void SetDirectAudioVolume(ushort trackIndex, float volume)
		{
			VideoPlayer.INTERNAL_CALL_SetDirectAudioVolume(this, trackIndex, volume);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetDirectAudioVolume(VideoPlayer self, ushort trackIndex, float volume);

		public bool GetDirectAudioMute(ushort trackIndex)
		{
			return VideoPlayer.INTERNAL_CALL_GetDirectAudioMute(this, trackIndex);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_GetDirectAudioMute(VideoPlayer self, ushort trackIndex);

		public void SetDirectAudioMute(ushort trackIndex, bool mute)
		{
			VideoPlayer.INTERNAL_CALL_SetDirectAudioMute(this, trackIndex, mute);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetDirectAudioMute(VideoPlayer self, ushort trackIndex, bool mute);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern AudioSource GetTargetAudioSource(ushort trackIndex);

		public void SetTargetAudioSource(ushort trackIndex, AudioSource source)
		{
			VideoPlayer.INTERNAL_CALL_SetTargetAudioSource(this, trackIndex, source);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetTargetAudioSource(VideoPlayer self, ushort trackIndex, AudioSource source);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event VideoPlayer.EventHandler prepareCompleted;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event VideoPlayer.EventHandler loopPointReached;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event VideoPlayer.EventHandler started;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event VideoPlayer.EventHandler frameDropped;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event VideoPlayer.ErrorEventHandler errorReceived;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event VideoPlayer.EventHandler seekCompleted;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event VideoPlayer.TimeEventHandler clockResyncOccurred;

		public extern bool sendFrameReadyEvents { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event VideoPlayer.FrameReadyEventHandler frameReady;

		[RequiredByNativeCode]
		private static void InvokePrepareCompletedCallback_Internal(VideoPlayer source)
		{
			if (source.prepareCompleted != null)
			{
				source.prepareCompleted(source);
			}
		}

		[RequiredByNativeCode]
		private static void InvokeFrameReadyCallback_Internal(VideoPlayer source, long frameIdx)
		{
			if (source.frameReady != null)
			{
				source.frameReady(source, frameIdx);
			}
		}

		[RequiredByNativeCode]
		private static void InvokeLoopPointReachedCallback_Internal(VideoPlayer source)
		{
			if (source.loopPointReached != null)
			{
				source.loopPointReached(source);
			}
		}

		[RequiredByNativeCode]
		private static void InvokeStartedCallback_Internal(VideoPlayer source)
		{
			if (source.started != null)
			{
				source.started(source);
			}
		}

		[RequiredByNativeCode]
		private static void InvokeFrameDroppedCallback_Internal(VideoPlayer source)
		{
			if (source.frameDropped != null)
			{
				source.frameDropped(source);
			}
		}

		[RequiredByNativeCode]
		private static void InvokeErrorReceivedCallback_Internal(VideoPlayer source, string errorStr)
		{
			if (source.errorReceived != null)
			{
				source.errorReceived(source, errorStr);
			}
		}

		[RequiredByNativeCode]
		private static void InvokeSeekCompletedCallback_Internal(VideoPlayer source)
		{
			if (source.seekCompleted != null)
			{
				source.seekCompleted(source);
			}
		}

		[RequiredByNativeCode]
		private static void InvokeClockResyncOccurredCallback_Internal(VideoPlayer source, double seconds)
		{
			if (source.clockResyncOccurred != null)
			{
				source.clockResyncOccurred(source, seconds);
			}
		}

		public delegate void EventHandler(VideoPlayer source);

		public delegate void ErrorEventHandler(VideoPlayer source, string message);

		public delegate void FrameReadyEventHandler(VideoPlayer source, long frameIdx);

		public delegate void TimeEventHandler(VideoPlayer source, double seconds);
	}
}
