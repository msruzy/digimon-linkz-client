﻿using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Identifies a list of interfaces that are exposed as COM event sources for the attributed class.</summary>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class ComSourceInterfacesAttribute : Attribute
	{
		private string internalValue;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.ComSourceInterfacesAttribute" /> class with the name of the event source interface.</summary>
		/// <param name="sourceInterfaces">A null-delimited list of fully qualified event source interface names. </param>
		public ComSourceInterfacesAttribute(string sourceInterfaces)
		{
			this.internalValue = sourceInterfaces;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.ComSourceInterfacesAttribute" /> class with the type to use as a source interface.</summary>
		/// <param name="sourceInterface">The <see cref="T:System.Type" /> of the source interface. </param>
		public ComSourceInterfacesAttribute(Type sourceInterface)
		{
			this.internalValue = sourceInterface.ToString();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.ComSourceInterfacesAttribute" /> class with the types to use as source interfaces.</summary>
		/// <param name="sourceInterface1">The <see cref="T:System.Type" /> of the default source interface. </param>
		/// <param name="sourceInterface2">The <see cref="T:System.Type" /> of a source interface. </param>
		public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2)
		{
			this.internalValue = sourceInterface1.ToString() + sourceInterface2.ToString();
		}

		/// <summary>Initializes a new instance of the ComSourceInterfacesAttribute class with the types to use as source interfaces.</summary>
		/// <param name="sourceInterface1">The <see cref="T:System.Type" /> of the default source interface. </param>
		/// <param name="sourceInterface2">The <see cref="T:System.Type" /> of a source interface. </param>
		/// <param name="sourceInterface3">The <see cref="T:System.Type" /> of a source interface. </param>
		public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2, Type sourceInterface3)
		{
			this.internalValue = sourceInterface1.ToString() + sourceInterface2.ToString() + sourceInterface3.ToString();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.ComSourceInterfacesAttribute" /> class with the types to use as source interfaces.</summary>
		/// <param name="sourceInterface1">The <see cref="T:System.Type" /> of the default source interface. </param>
		/// <param name="sourceInterface2">The <see cref="T:System.Type" /> of a source interface. </param>
		/// <param name="sourceInterface3">The <see cref="T:System.Type" /> of a source interface. </param>
		/// <param name="sourceInterface4">The <see cref="T:System.Type" /> of a source interface. </param>
		public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2, Type sourceInterface3, Type sourceInterface4)
		{
			this.internalValue = sourceInterface1.ToString() + sourceInterface2.ToString() + sourceInterface3.ToString() + sourceInterface4.ToString();
		}

		/// <summary>Gets the fully qualified name of the event source interface.</summary>
		/// <returns>The fully qualified name of the event source interface.</returns>
		public string Value
		{
			get
			{
				return this.internalValue;
			}
		}
	}
}
