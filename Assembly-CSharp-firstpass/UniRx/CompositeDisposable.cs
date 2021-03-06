﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx
{
	public sealed class CompositeDisposable : ICollection<IDisposable>, IDisposable, ICancelable, IEnumerable, IEnumerable<IDisposable>
	{
		private readonly object _gate = new object();

		private bool _disposed;

		private List<IDisposable> _disposables;

		private int _count;

		private const int SHRINK_THRESHOLD = 64;

		public CompositeDisposable()
		{
			this._disposables = new List<IDisposable>();
		}

		public CompositeDisposable(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			this._disposables = new List<IDisposable>(capacity);
		}

		public CompositeDisposable(params IDisposable[] disposables)
		{
			if (disposables == null)
			{
				throw new ArgumentNullException("disposables");
			}
			this._disposables = new List<IDisposable>(disposables);
			this._count = this._disposables.Count;
		}

		public CompositeDisposable(IEnumerable<IDisposable> disposables)
		{
			if (disposables == null)
			{
				throw new ArgumentNullException("disposables");
			}
			this._disposables = new List<IDisposable>(disposables);
			this._count = this._disposables.Count;
		}

		public int Count
		{
			get
			{
				return this._count;
			}
		}

		public void Add(IDisposable item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			bool flag = false;
			object gate = this._gate;
			lock (gate)
			{
				flag = this._disposed;
				if (!this._disposed)
				{
					this._disposables.Add(item);
					this._count++;
				}
			}
			if (flag)
			{
				item.Dispose();
			}
		}

		public bool Remove(IDisposable item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			bool flag = false;
			object gate = this._gate;
			lock (gate)
			{
				if (!this._disposed)
				{
					int num = this._disposables.IndexOf(item);
					if (num >= 0)
					{
						flag = true;
						this._disposables[num] = null;
						this._count--;
						if (this._disposables.Capacity > 64 && this._count < this._disposables.Capacity / 2)
						{
							List<IDisposable> disposables = this._disposables;
							this._disposables = new List<IDisposable>(this._disposables.Capacity / 2);
							foreach (IDisposable disposable in disposables)
							{
								if (disposable != null)
								{
									this._disposables.Add(disposable);
								}
							}
						}
					}
				}
			}
			if (flag)
			{
				item.Dispose();
			}
			return flag;
		}

		public void Dispose()
		{
			IDisposable[] array = null;
			object gate = this._gate;
			lock (gate)
			{
				if (!this._disposed)
				{
					this._disposed = true;
					array = this._disposables.ToArray();
					this._disposables.Clear();
					this._count = 0;
				}
			}
			if (array != null)
			{
				foreach (IDisposable disposable in array)
				{
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
		}

		public void Clear()
		{
			IDisposable[] array = null;
			object gate = this._gate;
			lock (gate)
			{
				array = this._disposables.ToArray();
				this._disposables.Clear();
				this._count = 0;
			}
			foreach (IDisposable disposable in array)
			{
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		public bool Contains(IDisposable item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			object gate = this._gate;
			bool result;
			lock (gate)
			{
				result = this._disposables.Contains(item);
			}
			return result;
		}

		public void CopyTo(IDisposable[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0 || arrayIndex >= array.Length)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			object gate = this._gate;
			lock (gate)
			{
				List<IDisposable> list = new List<IDisposable>();
				foreach (IDisposable disposable in this._disposables)
				{
					if (disposable != null)
					{
						list.Add(disposable);
					}
				}
				Array.Copy(list.ToArray(), 0, array, arrayIndex, array.Length - arrayIndex);
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public IEnumerator<IDisposable> GetEnumerator()
		{
			List<IDisposable> list = new List<IDisposable>();
			object gate = this._gate;
			lock (gate)
			{
				foreach (IDisposable disposable in this._disposables)
				{
					if (disposable != null)
					{
						list.Add(disposable);
					}
				}
			}
			return list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public bool IsDisposed
		{
			get
			{
				return this._disposed;
			}
		}
	}
}
