using GS.SQL.DataSource;
using System;
using System.Data;
using Newtonsoft.Json;

namespace GS.SQLModel
{
    public class User : Base<User>
	{

		[JsonProperty(PropertyName = "name")]
		[Column("Name", DbType.String)]
		public string Name { get; set; }

		[JsonIgnore]
		[JsonProperty(PropertyName = "password")]
		[Column("Password", DbType.String)]
		public string Password { get; set; }

		[JsonProperty(PropertyName = "department")]
		[Column("Department", DbType.Int16)]
		public int Department { get; set; }

		[JsonProperty(PropertyName = "departmentName")]
		[Column("DepartmentName", DbType.String)]
		public string DepartmentName { get; set; }

		[JsonProperty(PropertyName = "age")]
		[Column("Age", DbType.DateTime)]
		public DateTime? Age { get; set; }

		[JsonProperty(PropertyName = "email")]
		[Column("Email", DbType.String)]
		public string Email { get; set; }

		[JsonProperty(PropertyName = "qq")]
		[Column("QQ", DbType.String)]
		public string QQ { get; set; }

		[JsonProperty(PropertyName = "phone")]
		[Column("Phone", DbType.String)]
		public string Phone { get; set; }

		[JsonProperty(PropertyName = "tel")]
		[Column("Tel", DbType.String)]
		public string Tel { get; set; }

		[JsonProperty(PropertyName = "sex")]
		[Column("Sex", DbType.Int32)]
		public int Sex { get; set; }

		[JsonProperty(PropertyName = "address")]
		[Column("Address", DbType.String)]
		public string Address { get; set; }

		[JsonProperty(PropertyName = "city")]
		[Column("City", DbType.String)]
		public string City { get; set; }

		[JsonProperty(PropertyName = "province")]
		[Column("Province", DbType.String)]
		public string Province { get; set; }

        [JsonProperty(PropertyName = "country")]
		[Column("Country", DbType.String)]
		public string Country { get; set; }

		[JsonProperty(PropertyName = "rankCode")]
		[Column("RankCode", DbType.String)]
		public string RankCode { get; set; }

		[JsonProperty(PropertyName = "professionalLevel")]
		[Column("ProfessionalLevel", DbType.Int16)]
		public int ProfessionalLevel { get; set; }

        [JsonProperty(PropertyName = "headSculpture")]
        [Column("HeadSculpture", DbType.String)]
        public string HeadSculpture { get; set; }
    }
}
