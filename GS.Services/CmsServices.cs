using GS.SQL.DataSource;
using GS.SQLModel.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.Services
{
    public interface ICmsServices
    {
        
        SQLPage<Site> GetSite(int pageSize, int currentPage);
        SQLPage<Site> GetSiteByName(string name, int pageSize, int currentPage);
        IEnumerable<Site> GetSiteByName(string name);
        IEnumerable<Site> GetSite();
        IEnumerable<SiteTemp> GetSiteTempBySiteId(int siteId);
        IEnumerable<SiteTemp> GetSiteTempByName(string name);
        IEnumerable<SiteResource> GetSiteSiteResourceBySiteId(int siteId);
        SiteTemp GetSiteTempById(int Id);
        Site GetSiteById(int id);
        int AddTemp(SiteTemp siteTemp);
        int AddResource(SiteResource siteResource);
        int AddSite(Site site);
        int Update(SiteTemp siteTemp);
        int Update(Site site);
        int Remove(SiteTemp siteTemp);
        int Remove(Site site);
        int Remove(SiteResource siteResource);
    }

    public class CmsServices : ICmsServices
    {
        public readonly Site _site;
        public readonly SiteTemp _siteTemp;
        public readonly SiteResource _siteResource;
        public CmsServices()
        {
            _site = new Site();
            _siteTemp = new SiteTemp();
            _siteResource = new SiteResource();
        }
        public int Update(Site site)
        {
            site.Updatetime = DateTime.UtcNow;
            return _site.Update(site);
        }
        public int AddSite(Site site)
        {
            site.Updatetime = DateTime.UtcNow;
            return _site.Add(site);
        }
        public int Remove(Site site)
        {
            return _site.Delete(site);
        }
        public SQLPage<Site> GetSite(int pageSize, int currentPage)
        {
            var condition = new SQLCondition();
            condition.Expression = "where IsDelete=0 order by Updatetime desc";
            return _site.Query(pageSize, currentPage, condition);
        }
        public IEnumerable<Site> GetSiteByName(string name)
        {
            var condition = new SQLCondition();
            condition.Expression = $"where name = '{name}' and IsDelete=0 order by Updatetime desc";
            return _site.Query(condition);
        }
        public SQLPage<Site> GetSiteByName(string name, int pageSize, int currentPage)
        {
            var condition = new SQLCondition();
            condition.Expression = $"where name like '%{name}%' and IsDelete=0 order by Updatetime desc";
            return _site.Query(pageSize, currentPage, condition);
        }
        public Site GetSiteById(int id)
        {
            var condition = new SQLCondition();
            condition.Expression = $"where Id  = {id}";
            return _site.Query(condition).SingleOrDefault();
        }
        public int AddTemp(SiteTemp siteTemp)
        {
            _siteTemp.Updatetime = DateTime.UtcNow;
            return _siteTemp.Add(siteTemp);
        }
        public int Update(SiteTemp siteTemp)
        {
            _siteTemp.Updatetime = DateTime.UtcNow;
            return _siteTemp.Update(siteTemp);
        }
        public int Remove(SiteTemp siteTemp)
        {
            return _siteTemp.Delete(siteTemp); 
        }
        public IEnumerable<SiteTemp> GetSiteTempByName(string name)
        {
            var condition = new SQLCondition();
            condition.Expression = $"where TempName = '{name}' and IsDelete=0 order by Updatetime desc";
            return _siteTemp.Query(condition);
        }
        public IEnumerable<SiteTemp> GetSiteTempBySiteId(int siteId)
        {
            var condition = new SQLCondition();
            condition.Expression = $"where SiteId = {siteId} and IsDelete=0 order by Updatetime desc";
            return _siteTemp.Query(condition);
        }
        public IEnumerable<Site> GetSite()
        {
            return _site.Query();
        }
        public SiteTemp GetSiteTempById(int Id)
        {
            var condition = new SQLCondition();
            condition.Expression = $"where Id = {Id} and IsDelete=0 ";
            return _siteTemp.Query(condition).FirstOrDefault();
        }
        public int AddResource(SiteResource siteResource)
        {
            siteResource.Updatetime = DateTime.UtcNow;
            return _siteResource.Add(siteResource);
        }
        public int Remove(SiteResource siteResource)
        {
            return _siteResource.Delete(siteResource);
        }
        public IEnumerable<SiteResource> GetSiteSiteResourceBySiteId(int siteId)
        {
            var condition = new SQLCondition();
            condition.Expression = $"where SiteId = {siteId} and IsDelete=0 order by Updatetime desc";
            return _siteResource.Query(condition);
        }
    }
}
