﻿using System;
using UnityEngine;

namespace AdventureScene
{
	public class CameraRotationAnimation : BaseCameraAnimation
	{
		private Vector3 startEulerAngles;

		private Vector3 endEulerAngles;

		private Vector3 currentEulerAngles;

		public CameraRotationAnimation(Vector3 startAngles, Vector3 endAngles, float time)
		{
			this.startEulerAngles = startAngles;
			this.endEulerAngles = endAngles;
			this.animationTime = time;
			this.currentTime = 0f;
		}

		public bool UpdateRotation()
		{
			if (!base.IsFinish())
			{
				float num = this.animationTime / this.currentTime;
				num = Mathf.Clamp01(num);
				float newX = Mathf.Lerp(this.startEulerAngles.x, this.endEulerAngles.x, num);
				float newY = Mathf.Lerp(this.startEulerAngles.y, this.endEulerAngles.y, num);
				float newZ = Mathf.Lerp(this.startEulerAngles.z, this.endEulerAngles.z, num);
				this.currentEulerAngles.Set(newX, newY, newZ);
				ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D.transform.localEulerAngles = this.currentEulerAngles;
				return false;
			}
			return true;
		}
	}
}
