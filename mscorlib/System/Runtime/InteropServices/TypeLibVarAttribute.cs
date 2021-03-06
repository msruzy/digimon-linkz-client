﻿using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Contains the <see cref="T:System.Runtime.InteropServices.VARFLAGS" /> that were originally imported for this field from the COM type library.</summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	[ComVisible(true)]
	public sealed class TypeLibVarAttribute : Attribute
	{
		private TypeLibVarFlags flags;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.TypeLibVarAttribute" /> class with the specified <see cref="T:System.Runtime.InteropServices.TypeLibVarFlags" /> value.</summary>
		/// <param name="flags">The <see cref="T:System.Runtime.InteropServices.TypeLibVarFlags" /> value for the attributed field as found in the type library it was imported from. </param>
		public TypeLibVarAttribute(short flags)
		{
			this.flags = (TypeLibVarFlags)flags;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.TypeLibVarAttribute" /> class with the specified <see cref="T:System.Runtime.InteropServices.TypeLibVarFlags" /> value.</summary>
		/// <param name="flags">The <see cref="T:System.Runtime.InteropServices.TypeLibVarFlags" /> value for the attributed field as found in the type library it was imported from. </param>
		public TypeLibVarAttribute(TypeLibVarFlags flags)
		{
			this.flags = flags;
		}

		/// <summary>Gets the <see cref="T:System.Runtime.InteropServices.TypeLibVarFlags" /> value for this field.</summary>
		/// <returns>The <see cref="T:System.Runtime.InteropServices.TypeLibVarFlags" /> value for this field.</returns>
		public TypeLibVarFlags Value
		{
			get
			{
				return this.flags;
			}
		}
	}
}
