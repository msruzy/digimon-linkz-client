﻿using System;

namespace UniRx.Operators
{
	public delegate TR CombineLatestFunc<T1, T2, T3, T4, T5, TR>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
}
