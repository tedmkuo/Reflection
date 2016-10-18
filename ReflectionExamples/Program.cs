using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

//You cannot generate custom compiler warnings from attributes. Some attributes like System.ObsoleteAttribute
//will generate a warning or error, but this is hard-coded into the C# compiler.
//http://stackoverflow.com/questions/17566623/force-the-usage-of-an-attribute-on-properties-if-they-already-have-another-attr

namespace ReflectionExamples
{
	internal class Program
	{
		private const string CalcAssemblyPath =
			@"C:\Training\Reflection\ReflectionExamples\CalcAssembly\bin\Debug\CalcAssembly.dll";

		private const double Tolerance = 0.001;
		private const double Pi = 3.14;

		private static void Main(string[] args)
		{
			//Create a DateTime instance with Reflection
			CreateInstance();

			// dynamically load assembly from file Test.dll
			Assembly calcAssembly = Assembly.LoadFile(CalcAssemblyPath);

			// create a Calculator instance from the class type off the assembly
			Type calcType = calcAssembly.GetType("CalcAssembly.Calculator");
			var calculator = Activator.CreateInstance(calcType, new object[] {55.00});

			GetSetProperties(calcType, calculator);

			InvokeInstanceMethods(calcType, calculator);

			GetUserRoleAttributeInfo(calcAssembly);

			Console.ReadKey();
		}

		private static void InvokeInstanceMethods(Type calcType, object calculator)
		{
			PropertyInfo numberPropertyInfo = calcType.GetProperty("Number");

			// invoke public Clear method
			calcType.InvokeMember("Clear",
				BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
				null, calculator, null);

			// set the Number property of the Calculator instance
			numberPropertyInfo.SetValue(calculator, 10.0, null);

			// invoke private Add method
			var value = (double) calcType.InvokeMember("Add",
				BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic,
				null, calculator, new object[] {20.0});
			Debug.Assert(Math.Abs(value - 30.0) < Tolerance);

			// invoke static GetPi method
			var piValue = (double) calcType.InvokeMember("GetPi",
				BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public,
				null, null, null);
			Debug.Assert(Math.Abs(piValue - Pi) < Tolerance);
		}

		private static void GetSetProperties(Type calcType, object calculator)
		{
			// get Calculator property
			PropertyInfo numberPropertyInfo = calcType.GetProperty("Number");
			double value = (double) numberPropertyInfo.GetValue(calculator, null);
			Debug.Assert(Math.Abs(value - 55.00) < Tolerance);

			// set Calculator property
			numberPropertyInfo.SetValue(calculator, 10.0, null);
			value = (double) numberPropertyInfo.GetValue(calculator, null);
			Debug.Assert(Math.Abs(value - 10.0) < Tolerance);

			// get static Calculator property
			PropertyInfo piPropertyInfo = calcType.GetProperty("Pi");
			double piValue = (double) piPropertyInfo.GetValue(null, null);
			Debug.Assert(Math.Abs(piValue - Pi) < Tolerance);

			// get value of private field
			value = (double) calcType.InvokeMember("_number",
				BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic,
				null, calculator, null);
			Debug.Assert(Math.Abs(value - 10.0) < Tolerance);
		}

		private static void CreateInstance()
		{
			// create instance of class DateTime
			var dateTime1 = (DateTime) Activator.CreateInstance(typeof (DateTime));
			//dateTime1 = DateTime.Now;
			Console.WriteLine("dateTime1 is {0}", dateTime1);

			// create instance of DateTime, use constructor with parameters (year, month, day)
			var dateTime2 = (DateTime) Activator.CreateInstance(typeof (DateTime), new object[] {2015, 1, 8});
			Console.WriteLine("dateTime2 is {0}", dateTime2);
		}

		private static void GetUserRoleAttributeInfo(Assembly calcAssembly)
		{
			Type userType = calcAssembly.GetType("CalcAssembly.User");
			var roleMembers = userType.GetMember("Role");
			var roleMember = roleMembers.FirstOrDefault();
			if (roleMember != null)
			{
				roleMember.CustomAttributes.ToList().ForEach(ca =>
				{
					Console.WriteLine("Custom attribute name = {0}", ca.AttributeType.Name);

					if (ca.ConstructorArguments.Any())
					{
						ca.ConstructorArguments.ToList().ForEach(arg=>Console.WriteLine("Constructor argument: {0}", arg.Value));
					}

					if (ca.NamedArguments != null)
					{
						ca.NamedArguments.ToList().ForEach(na =>
						{
							Console.WriteLine("Attribute member name = {0}, value = {1}", na.MemberName, na.TypedValue.Value);
						});
					}
				});
			}
		}
	}
}
