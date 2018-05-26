using GS.Api.Model;
using GS.Services;
using GS.SQL.DataSource;
using GS.SQLModel.Form;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GS.Api.Controllers
{
    [RoutePrefix("api/dyncData")]
    public class DyncDataController : BaseController
    {
        public readonly IDynamicDataservices _dynamicDataservices;
        public DyncDataController(IDynamicDataservices dynamicDataservices)
        {
            _dynamicDataservices = dynamicDataservices;
        }

        [HttpPost]
        [Route("exp/Dropdown")]
        public IEnumerable<SQLDynamicItem> GetDyncExpValue(DyncExp exp)
        {
            var dyncTable = _dynamicDataservices.GetDyncFormByCode(exp.Table);
            if (dyncTable == null)
                throw new Exception($"不能找到code 为{exp.Table}的数据表");
            return _dynamicDataservices.GetDyncValueByDyncParam(dyncTable.Name, exp.FiledName, exp.Param);
        }
    }
}
