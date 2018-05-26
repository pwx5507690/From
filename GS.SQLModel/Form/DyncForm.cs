using GS.SQL.DataSource;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.SQLModel.Form
{
	//{"FRMNM":"表单名称","DESC":"","LANG":"cn","LBLAL":"T",
	//"CFMTYP":"T","CFMMSG":"提交成功。",
	//"SDMAIL":"0","CAPTCHA":"1","IPLMT":"0",
	//"SCHACT":"0","INSTR":"0","ISPUB":"1","GID":"","HEIGHT":758}
	public class DyncForm : DyncBaseForm<DyncForm>
	{
		[JsonProperty(PropertyName = "GID")]
		[Column("Code", DbType.String)]
		public string Code { get; set; }

		[JsonProperty(PropertyName = "FRMNM")]
		[Column("Name", DbType.String)]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "DESC")]
		[Column("Remark", DbType.String)]
		public string Remark { get; set; }

		[JsonProperty(PropertyName = "HEIGHT")]
		[Column("Height", DbType.Int32)]
		public int Height { get; set; }

		[JsonProperty(PropertyName = "LBLAL")]
		[Column("Lblal", DbType.String)]
		public string Lblal { get; set; }

		[JsonProperty(PropertyName = "CFMTYP")]
		[Column("Cfmtyp", DbType.String)]
		public string Cfmtyp { get; set; }

		[JsonProperty(PropertyName = "CFMURL")]
		[Column("Cfmurl", DbType.String)]
		public string Cfmurl { get; set; }

		[JsonProperty(PropertyName = "CFMMSG")]
		[Column("Cfmmsg", DbType.String)]
		public string Cfmmsg { get; set; }

		[JsonProperty(PropertyName = "LANG")]
		[Column("Lang", DbType.String)]
		public string Lang { get; set; }

		[JsonProperty(PropertyName = "Role")]
		[Column("Role", DbType.String)]
		public string Role { get; set; }
		
	}
}
