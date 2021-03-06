﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;
using UnityEngine.Serialization;

namespace UnityEngine.Events
{
	[UsedByNativeCode]
	[Serializable]
	public abstract class UnityEventBase : ISerializationCallbackReceiver
	{
		private InvokableCallList m_Calls;

		[FormerlySerializedAs("m_PersistentListeners")]
		[SerializeField]
		private PersistentCallGroup m_PersistentCalls;

		[SerializeField]
		private string m_TypeName;

		private bool m_CallsDirty = true;

		protected UnityEventBase()
		{
			this.m_Calls = new InvokableCallList();
			this.m_PersistentCalls = new PersistentCallGroup();
			this.m_TypeName = base.GetType().AssemblyQualifiedName;
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this.DirtyPersistentCalls();
			this.m_TypeName = base.GetType().AssemblyQualifiedName;
		}

		protected abstract MethodInfo FindMethod_Impl(string name, object targetObj);

		internal abstract BaseInvokableCall GetDelegate(object target, MethodInfo theFunction);

		internal MethodInfo FindMethod(PersistentCall call)
		{
			Type argumentType = typeof(Object);
			if (!string.IsNullOrEmpty(call.arguments.unityObjectArgumentAssemblyTypeName))
			{
				argumentType = (Type.GetType(call.arguments.unityObjectArgumentAssemblyTypeName, false) ?? typeof(Object));
			}
			return this.FindMethod(call.methodName, call.target, call.mode, argumentType);
		}

		internal MethodInfo FindMethod(string name, object listener, PersistentListenerMode mode, Type argumentType)
		{
			MethodInfo result;
			switch (mode)
			{
			case PersistentListenerMode.EventDefined:
				result = this.FindMethod_Impl(name, listener);
				break;
			case PersistentListenerMode.Void:
				result = UnityEventBase.GetValidMethodInfo(listener, name, new Type[0]);
				break;
			case PersistentListenerMode.Object:
				result = UnityEventBase.GetValidMethodInfo(listener, name, new Type[]
				{
					argumentType ?? typeof(Object)
				});
				break;
			case PersistentListenerMode.Int:
				result = UnityEventBase.GetValidMethodInfo(listener, name, new Type[]
				{
					typeof(int)
				});
				break;
			case PersistentListenerMode.Float:
				result = UnityEventBase.GetValidMethodInfo(listener, name, new Type[]
				{
					typeof(float)
				});
				break;
			case PersistentListenerMode.String:
				result = UnityEventBase.GetValidMethodInfo(listener, name, new Type[]
				{
					typeof(string)
				});
				break;
			case PersistentListenerMode.Bool:
				result = UnityEventBase.GetValidMethodInfo(listener, name, new Type[]
				{
					typeof(bool)
				});
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		public int GetPersistentEventCount()
		{
			return this.m_PersistentCalls.Count;
		}

		public Object GetPersistentTarget(int index)
		{
			PersistentCall listener = this.m_PersistentCalls.GetListener(index);
			return (listener == null) ? null : listener.target;
		}

		public string GetPersistentMethodName(int index)
		{
			PersistentCall listener = this.m_PersistentCalls.GetListener(index);
			return (listener == null) ? string.Empty : listener.methodName;
		}

		private void DirtyPersistentCalls()
		{
			this.m_Calls.ClearPersistent();
			this.m_CallsDirty = true;
		}

		private void RebuildPersistentCallsIfNeeded()
		{
			if (this.m_CallsDirty)
			{
				this.m_PersistentCalls.Initialize(this.m_Calls, this);
				this.m_CallsDirty = false;
			}
		}

		public void SetPersistentListenerState(int index, UnityEventCallState state)
		{
			PersistentCall listener = this.m_PersistentCalls.GetListener(index);
			if (listener != null)
			{
				listener.callState = state;
			}
			this.DirtyPersistentCalls();
		}

		protected void AddListener(object targetObj, MethodInfo method)
		{
			this.m_Calls.AddListener(this.GetDelegate(targetObj, method));
		}

		internal void AddCall(BaseInvokableCall call)
		{
			this.m_Calls.AddListener(call);
		}

		protected void RemoveListener(object targetObj, MethodInfo method)
		{
			this.m_Calls.RemoveListener(targetObj, method);
		}

		public void RemoveAllListeners()
		{
			this.m_Calls.Clear();
		}

		internal List<BaseInvokableCall> PrepareInvoke()
		{
			this.RebuildPersistentCallsIfNeeded();
			return this.m_Calls.PrepareInvoke();
		}

		protected void Invoke(object[] parameters)
		{
			List<BaseInvokableCall> list = this.PrepareInvoke();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].Invoke(parameters);
			}
		}

		public override string ToString()
		{
			return base.ToString() + " " + base.GetType().FullName;
		}

		public static MethodInfo GetValidMethodInfo(object obj, string functionName, Type[] argumentTypes)
		{
			Type type = obj.GetType();
			while (type != typeof(object) && type != null)
			{
				MethodInfo method = type.GetMethod(functionName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, argumentTypes, null);
				if (method != null)
				{
					ParameterInfo[] parameters = method.GetParameters();
					bool flag = true;
					int num = 0;
					foreach (ParameterInfo parameterInfo in parameters)
					{
						Type type2 = argumentTypes[num];
						Type parameterType = parameterInfo.ParameterType;
						flag = (type2.IsPrimitive == parameterType.IsPrimitive);
						if (!flag)
						{
							break;
						}
						num++;
					}
					if (flag)
					{
						return method;
					}
				}
				type = type.BaseType;
			}
			return null;
		}
	}
}
