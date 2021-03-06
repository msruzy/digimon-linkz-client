﻿using System;
using System.Collections;
using System.IO;

namespace System.Globalization
{
	[Serializable]
	internal class CCGregorianEraHandler
	{
		private SortedList _Eras;

		public CCGregorianEraHandler()
		{
			this._Eras = new SortedList();
		}

		public int[] Eras
		{
			get
			{
				int[] array = new int[this._Eras.Count];
				for (int i = 0; i < this._Eras.Count; i++)
				{
					array[i] = ((CCGregorianEraHandler.Era)this._Eras.GetByIndex(i)).Nr;
				}
				return array;
			}
		}

		public void appendEra(int nr, int rd_start, int rd_end)
		{
			CCGregorianEraHandler.Era era = new CCGregorianEraHandler.Era(nr, rd_start, rd_end);
			this._Eras[nr] = era;
		}

		public void appendEra(int nr, int rd_start)
		{
			this.appendEra(nr, rd_start, CCFixed.FromDateTime(DateTime.MaxValue));
		}

		public int GregorianYear(int year, int era)
		{
			return ((CCGregorianEraHandler.Era)this._Eras[era]).GregorianYear(year);
		}

		public int EraYear(out int era, int date)
		{
			IList valueList = this._Eras.GetValueList();
			foreach (object obj in valueList)
			{
				CCGregorianEraHandler.Era era2 = (CCGregorianEraHandler.Era)obj;
				if (era2.Covers(date))
				{
					return era2.EraYear(out era, date);
				}
			}
			throw new ArgumentOutOfRangeException("date", "Time value was out of era range.");
		}

		public void CheckDateTime(DateTime time)
		{
			int date = CCFixed.FromDateTime(time);
			if (!this.ValidDate(date))
			{
				throw new ArgumentOutOfRangeException("time", "Time value was out of era range.");
			}
		}

		public bool ValidDate(int date)
		{
			IList valueList = this._Eras.GetValueList();
			foreach (object obj in valueList)
			{
				if (((CCGregorianEraHandler.Era)obj).Covers(date))
				{
					return true;
				}
			}
			return false;
		}

		public bool ValidEra(int era)
		{
			return this._Eras.Contains(era);
		}

		[Serializable]
		private struct Era
		{
			private int _nr;

			private int _start;

			private int _gregorianYearStart;

			private int _end;

			private int _maxYear;

			public Era(int nr, int start, int end)
			{
				if (nr == 0)
				{
					throw new ArgumentException("Era number shouldn't be zero.");
				}
				this._nr = nr;
				if (start > end)
				{
					throw new ArgumentException("Era should start before end.");
				}
				this._start = start;
				this._end = end;
				this._gregorianYearStart = CCGregorianCalendar.year_from_fixed(this._start);
				int num = CCGregorianCalendar.year_from_fixed(this._end);
				this._maxYear = num - this._gregorianYearStart + 1;
			}

			public int Nr
			{
				get
				{
					return this._nr;
				}
			}

			public int GregorianYear(int year)
			{
				if (year < 1 || year > this._maxYear)
				{
					StringWriter stringWriter = new StringWriter();
					stringWriter.Write("Valid Values are between {0} and {1}, inclusive.", 1, this._maxYear);
					throw new ArgumentOutOfRangeException("year", stringWriter.ToString());
				}
				return year + this._gregorianYearStart - 1;
			}

			public bool Covers(int date)
			{
				return this._start <= date && date <= this._end;
			}

			public int EraYear(out int era, int date)
			{
				if (!this.Covers(date))
				{
					throw new ArgumentOutOfRangeException("date", "Time was out of Era range.");
				}
				int num = CCGregorianCalendar.year_from_fixed(date);
				era = this._nr;
				return num - this._gregorianYearStart + 1;
			}
		}
	}
}
