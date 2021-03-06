﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx
{
	[Serializable]
	public class Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple, IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>>
	{
		private T1 item1;

		private T2 item2;

		private T3 item3;

		private T4 item4;

		private T5 item5;

		private T6 item6;

		private T7 item7;

		private TRest rest;

		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest)
		{
			this.item1 = item1;
			this.item2 = item2;
			this.item3 = item3;
			this.item4 = item4;
			this.item5 = item5;
			this.item6 = item6;
			this.item7 = item7;
			this.rest = rest;
			if (!(rest is ITuple))
			{
				throw new ArgumentException("rest", "The last element of an eight element tuple must be a Tuple.");
			}
		}

		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}

		public T2 Item2
		{
			get
			{
				return this.item2;
			}
		}

		public T3 Item3
		{
			get
			{
				return this.item3;
			}
		}

		public T4 Item4
		{
			get
			{
				return this.item4;
			}
		}

		public T5 Item5
		{
			get
			{
				return this.item5;
			}
		}

		public T6 Item6
		{
			get
			{
				return this.item6;
			}
		}

		public T7 Item7
		{
			get
			{
				return this.item7;
			}
		}

		public TRest Rest
		{
			get
			{
				return this.rest;
			}
		}

		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>))
			{
				throw new ArgumentException("other");
			}
			Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple = (Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>)other;
			int num = comparer.Compare(this.item1, tuple.item1);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.item2, tuple.item2);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.item3, tuple.item3);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.item4, tuple.item4);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.item5, tuple.item5);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.item6, tuple.item6);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.item7, tuple.item7);
			if (num != 0)
			{
				return num;
			}
			return comparer.Compare(this.rest, tuple.rest);
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (!(other is Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>))
			{
				return false;
			}
			Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple = (Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>)other;
			return comparer.Equals(this.item1, tuple.item1) && comparer.Equals(this.item2, tuple.item2) && comparer.Equals(this.item3, tuple.item3) && comparer.Equals(this.item4, tuple.item4) && comparer.Equals(this.item5, tuple.item5) && comparer.Equals(this.item6, tuple.item6) && comparer.Equals(this.item7, tuple.item7) && comparer.Equals(this.rest, tuple.rest);
		}

		public override int GetHashCode()
		{
			EqualityComparer<T1> @default = EqualityComparer<T1>.Default;
			EqualityComparer<T2> default2 = EqualityComparer<T2>.Default;
			EqualityComparer<T3> default3 = EqualityComparer<T3>.Default;
			EqualityComparer<T4> default4 = EqualityComparer<T4>.Default;
			EqualityComparer<T5> default5 = EqualityComparer<T5>.Default;
			EqualityComparer<T6> default6 = EqualityComparer<T6>.Default;
			EqualityComparer<T7> default7 = EqualityComparer<T7>.Default;
			EqualityComparer<TRest> default8 = EqualityComparer<TRest>.Default;
			int num = @default.GetHashCode(this.item1);
			num = ((num << 5) + num ^ default2.GetHashCode(this.item2));
			int num2 = default3.GetHashCode(this.item3);
			num2 = ((num2 << 5) + num2 ^ default4.GetHashCode(this.item4));
			num = ((num << 5) + num ^ num2);
			num2 = default5.GetHashCode(this.item5);
			num2 = ((num2 << 5) + num2 ^ default6.GetHashCode(this.item6));
			int num3 = default7.GetHashCode(this.item7);
			num3 = ((num3 << 5) + num3 ^ default8.GetHashCode(this.rest));
			num2 = ((num2 << 5) + num2 ^ num3);
			return (num << 5) + num ^ num2;
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			int num = comparer.GetHashCode(this.item1);
			num = ((num << 5) + num ^ comparer.GetHashCode(this.item2));
			int num2 = comparer.GetHashCode(this.item3);
			num2 = ((num2 << 5) + num2 ^ comparer.GetHashCode(this.item4));
			num = ((num << 5) + num ^ num2);
			num2 = comparer.GetHashCode(this.item5);
			num2 = ((num2 << 5) + num2 ^ comparer.GetHashCode(this.item6));
			int num3 = comparer.GetHashCode(this.item7);
			num3 = ((num3 << 5) + num3 ^ comparer.GetHashCode(this.rest));
			num2 = ((num2 << 5) + num2 ^ num3);
			return (num << 5) + num ^ num2;
		}

		string ITuple.ToString()
		{
			return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", new object[]
			{
				this.item1,
				this.item2,
				this.item3,
				this.item4,
				this.item5,
				this.item6,
				this.item7,
				((ITuple)((object)this.rest)).ToString()
			});
		}

		public override string ToString()
		{
			return "(" + ((ITuple)this).ToString() + ")";
		}

		public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> other)
		{
			EqualityComparer<T1> @default = EqualityComparer<T1>.Default;
			EqualityComparer<T2> default2 = EqualityComparer<T2>.Default;
			EqualityComparer<T3> default3 = EqualityComparer<T3>.Default;
			EqualityComparer<T4> default4 = EqualityComparer<T4>.Default;
			EqualityComparer<T5> default5 = EqualityComparer<T5>.Default;
			EqualityComparer<T6> default6 = EqualityComparer<T6>.Default;
			EqualityComparer<T7> default7 = EqualityComparer<T7>.Default;
			EqualityComparer<TRest> default8 = EqualityComparer<TRest>.Default;
			return @default.Equals(this.item1, other.Item1) && default2.Equals(this.item2, other.Item2) && default3.Equals(this.item3, other.Item3) && default4.Equals(this.item4, other.Item4) && default5.Equals(this.item5, other.Item5) && default6.Equals(this.item6, other.Item6) && default7.Equals(this.item7, other.Item7) && default8.Equals(this.rest, other.rest);
		}
	}
}
