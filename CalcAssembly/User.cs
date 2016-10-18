using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcAssembly
{
	public class User
	{
		public string LastName { get; set; }
		public string FirstName { get; set; }

		[UserRole(10, Role="Accountant", Rank="Senior", Age = 40)]
		public string Role { get; set; }
	}
}
