using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.SQLModel.Form
{
	public abstract class DyncBaseForm<T> : Base<T>
	{
		public DyncBaseForm() : base("DyncData")
		{
			 
		}
	}
}
