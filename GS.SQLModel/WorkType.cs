using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.SQLModel
{
	// 工作类别
	public class WorkType:Base<WorkType>
	{
		public string Title { get; set; }
		public string Code { get; set; }
	}
}
