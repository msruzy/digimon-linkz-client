﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace UniRx
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct CollectionAddEvent<T> : IEquatable<CollectionAddEvent<T>>
	{
		public CollectionAddEvent(int index, T value)
		{
			this = default(CollectionAddEvent<T>);
			this.Index = index;
			this.Value = value;
		}

		public int Index { get; private set; }

		public T Value { get; private set; }

		public override string ToString()
		{
			return string.Format("Index:{0} Value:{1}", this.Index, this.Value);
		}

		public override int GetHashCode()
		{
			return this.Index.GetHashCode() ^ EqualityComparer<T>.Default.GetHashCode(this.Value) << 2;
		}

		public bool Equals(CollectionAddEvent<T> other)
		{
			return this.Index.Equals(other.Index) && EqualityComparer<T>.Default.Equals(this.Value, other.Value);
		}
	}
}
