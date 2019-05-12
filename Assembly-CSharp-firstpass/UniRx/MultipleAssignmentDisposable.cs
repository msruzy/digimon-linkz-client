﻿using System;

namespace UniRx
{
	public sealed class MultipleAssignmentDisposable : IDisposable, ICancelable
	{
		private static readonly BooleanDisposable True = new BooleanDisposable(true);

		private object gate = new object();

		private IDisposable current;

		public bool IsDisposed
		{
			get
			{
				object obj = this.gate;
				bool result;
				lock (obj)
				{
					result = (this.current == MultipleAssignmentDisposable.True);
				}
				return result;
			}
		}

		public IDisposable Disposable
		{
			get
			{
				object obj = this.gate;
				IDisposable result;
				lock (obj)
				{
					result = ((this.current != MultipleAssignmentDisposable.True) ? this.current : UniRx.Disposable.Empty);
				}
				return result;
			}
			set
			{
				bool flag = false;
				object obj = this.gate;
				lock (obj)
				{
					flag = (this.current == MultipleAssignmentDisposable.True);
					if (!flag)
					{
						this.current = value;
					}
				}
				if (flag && value != null)
				{
					value.Dispose();
				}
			}
		}

		public void Dispose()
		{
			IDisposable disposable = null;
			object obj = this.gate;
			lock (obj)
			{
				if (this.current != MultipleAssignmentDisposable.True)
				{
					disposable = this.current;
					this.current = MultipleAssignmentDisposable.True;
				}
			}
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}
}
