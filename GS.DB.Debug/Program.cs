using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS.Services;
using GS.SQLModel;

namespace GS.DB.Debug
{
	class Program
	{
		static void Main(string[] args)
		{
            List<int> a = null;
            
            Console.WriteLine(a?.Count>0);
            Console.Read();
		}
	}
}
