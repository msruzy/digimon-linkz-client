﻿using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.Rendering
{
	public abstract class RenderPipelineAsset : ScriptableObject, IRenderPipelineAsset
	{
		private readonly List<IRenderPipeline> m_CreatedPipelines = new List<IRenderPipeline>();

		public void DestroyCreatedInstances()
		{
			foreach (IRenderPipeline renderPipeline in this.m_CreatedPipelines)
			{
				renderPipeline.Dispose();
			}
			this.m_CreatedPipelines.Clear();
		}

		public IRenderPipeline CreatePipeline()
		{
			IRenderPipeline renderPipeline = this.InternalCreatePipeline();
			if (renderPipeline != null)
			{
				this.m_CreatedPipelines.Add(renderPipeline);
			}
			return renderPipeline;
		}

		public virtual Material GetDefaultMaterial()
		{
			return null;
		}

		public virtual Material GetDefaultParticleMaterial()
		{
			return null;
		}

		public virtual Material GetDefaultLineMaterial()
		{
			return null;
		}

		public virtual Material GetDefaultTerrainMaterial()
		{
			return null;
		}

		public virtual Material GetDefaultUIMaterial()
		{
			return null;
		}

		public virtual Material GetDefaultUIOverdrawMaterial()
		{
			return null;
		}

		public virtual Material GetDefaultUIETC1SupportedMaterial()
		{
			return null;
		}

		public virtual Material GetDefault2DMaterial()
		{
			return null;
		}

		public virtual Shader GetDefaultShader()
		{
			return null;
		}

		protected abstract IRenderPipeline InternalCreatePipeline();

		protected IEnumerable<IRenderPipeline> CreatedInstances()
		{
			return this.m_CreatedPipelines;
		}

		private void OnValidate()
		{
			this.DestroyCreatedInstances();
		}

		private void OnDisable()
		{
			this.DestroyCreatedInstances();
		}
	}
}
