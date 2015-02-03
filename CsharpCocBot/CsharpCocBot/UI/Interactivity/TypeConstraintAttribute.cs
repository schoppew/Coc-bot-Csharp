namespace CoC.Bot.UI.Interactivity
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class TypeConstraintAttribute : Attribute
	{
		public TypeConstraintAttribute(Type constraint)
		{
			Constraint = constraint;
		}

		public Type Constraint { get; private set; }
	}
}