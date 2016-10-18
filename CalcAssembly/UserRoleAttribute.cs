using System;

namespace CalcAssembly
{
	/*
	[AttributeUsage(AttributeTargets.Class |			//valid on a class
	                AttributeTargets.Constructor |		//valid on a constructor
	                AttributeTargets.Field |			//valid on a field
	                AttributeTargets.Method |			//valid on a method
	                AttributeTargets.Property,			//valid on a property
		AllowMultiple = true,							//can apply multiple times to the target, default is false
		Inherited = false)]								//apply to the target's child, default is false
	 */

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class UserRoleAttribute : Attribute
	{
		public UserRoleAttribute(int userId)
		{
			UserId = userId;
		}

		public int UserId { get; private set; }

		public int Age { get; set; }
		public string Role { get; set; }
		public string Rank { get; set; }
	}
}