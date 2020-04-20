using System;
using System.Collections.Generic;
using System.Web;

using System.Linq;
using System.Data.Entity;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using NHibernate.Criterion;

namespace ZAJCZN.MIS.Web
{
    public class MenuHelper
    {
        private static List<menus> _menus;

        public static List<menus> Menus
        {
            get
            {
                if (_menus == null)
                {
                    InitMenus();
                }
                return _menus;
            }
        }

        public static void Reload()
        {
            _menus = null;
        }

        private static void InitMenus()
        {
            _menus = new List<menus>();

            List<menus> dbMenus = Core.Container.Instance.Resolve<IServiceMenus>().GetAll().ToList().OrderBy(a => a.SortIndex).ToList();
            // List<Menu> dbMenus = PageBase.DB.Menus.Include(m => m.ViewPower).OrderBy(m => m.SortIndex).ToList();
            foreach (menus menu in dbMenus)
            {
                if (menu.ViewPowerID > 0)
                {
                    powers entity = Core.Container.Instance.Resolve<IServicePowers>().GetEntity(menu.ViewPowerID);
                    menu.ViewPowerName = entity != null ? entity.Name : "";
                }
            }
            ResolveMenuCollection(dbMenus, 0, 0);
        }


        private static int ResolveMenuCollection(List<menus> dbMenus, int parentMenuID, int level)
        {
            int count = 0;

            foreach (var menu in dbMenus.Where(m => m.ParentID == parentMenuID))
            {
                count++;

                _menus.Add(menu);
                menu.TreeLevel = level;
                menu.IsTreeLeaf = true;
                menu.Enabled = true;

                level++;
                int childCount = ResolveMenuCollection(dbMenus, menu.ID, level);
                if (childCount != 0)
                {
                    menu.IsTreeLeaf = false;
                }
                level--;
            }

            return count;
        }

    }
}
