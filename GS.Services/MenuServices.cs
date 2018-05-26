using GS.SQL.DataSource;
using GS.SQLModel;
using GS.View.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.Services
{
    public interface IMenuServices {
        int AddMenu(Menu item);
        int Delete(Menu item);
        int Delete(IEnumerable<Menu> item);
        int Update(Menu item);
        SQLPage<Menu> GetMenu(int pageSize, int currentPage);
        SQLPage<Menu> GetMenuByName(string name, int pageSize, int currentPage);
        IEnumerable<VMenu> ConvertToVMenu(IEnumerable<Menu> item);
        IEnumerable<VMenu> GetVMenu();
        IEnumerable<Menu> GetMenu();
    }

    public class MenuServices: IMenuServices
    {
		private readonly Menu _menu;
		public MenuServices()
		{
			_menu = new Menu();
		}
		public int AddMenu(Menu item)
		{
			if (item.Parent == -1)
                item.Parent = null;
            return _menu.Add(item);
		}
		public int Delete(Menu item)
		{
			return _menu.Delete(item);
		}
		public int Delete(IEnumerable<Menu> item)
		{
			return _menu.Delete(item);
		}
		public SQLPage<Menu> GetMenu(int pageSize, int currentPage)
		{
			var sql = "select b.*,c.Name as ParentName from (select row_number()over(order by tempColumn)rownumber, * from(select top {0} tempColumn = 0, * from {1})a )b left join {1} as c on b.Parent = c.Id where rownumber > {2}";
			return _menu.Query(pageSize, currentPage, sql: sql);
		}
		public SQLPage<Menu> GetMenuByName(string name, int pageSize, int currentPage)
		{
			var condition = new SQLCondition();
			condition.Expression = "where Name like '%" + name + "%'";
			var sql = "select b.*,c.Name as ParentName from (select row_number()over(order by tempColumn)rownumber, * from(select top {0} tempColumn = 0, * from {1})a )b left join [Menu] as c on b.Parent = c.Id where rownumber > {2}";
			return _menu.Query(pageSize, currentPage, condition, sql: sql);
		}
		public int Update(Menu item)
		{
			if (item.Parent == -1)
                item.Parent = null;
            return _menu.Update(item);
		}
		public IEnumerable<VMenu> ConvertToVMenu(IEnumerable<Menu> item)
		{
			var parent = item.Where(t => t.Parent == null || t.Parent == -1).OrderBy(t => t.Sort).ToList();
			return parent.Select(t =>
			{
				var vMenu = new VMenu();
                vMenu.Id = t.Id;
                vMenu.Url = t.Url;
				vMenu.Type = t.Type;
				vMenu.Menu = item.Where(v => v.Parent == t.Id);
				vMenu.Name = t.Name;
				vMenu.EName = t.Icon;
				return vMenu;
			});
		}
		public IEnumerable<VMenu> GetVMenu()
		{
			return ConvertToVMenu(GetMenu());
		}
		public IEnumerable<Menu> GetMenu()
		{
			return _menu.Query().OrderBy(t => t.Sort);
		}
	}
}
