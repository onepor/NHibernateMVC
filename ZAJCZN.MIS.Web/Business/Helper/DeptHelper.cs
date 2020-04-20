using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public class DeptHelper
    {
        private static List<depts> _depts;

        public static List<depts> Depts
        {
            get
            {
                if (_depts == null)
                {
                    InitDepts();
                }
                return _depts;
            }
        }

        public static void Reload()
        {
            _depts = null;
        }

        private static void InitDepts()
        {
            _depts = new List<depts>();

            //List<depts> dbDepts = PageBase.DB.Depts.OrderBy(d => d.SortIndex).ToList();

            List<depts> dbDepts = Core.Container.Instance.Resolve<IServiceDepts>().GetAll().ToList().OrderBy(a => a.SortIndex).ToList();

            ResolveDeptCollection(dbDepts, 0, 0);
        }

        //private static int ResolveDeptCollection(List<depts> dbDepts, depts parentDept, int level)
        //{
        //    int count = 0;
        //    foreach (var dept in dbDepts.Where(d => d.Parent == parentDept))
        //    {
        //        count++;

        //        _depts.Add(dept);
        //        dept.TreeLevel = level;
        //        dept.IsTreeLeaf = true;
        //        dept.Enabled = true;

        //        level++;
        //        // 如果这个节点下没有子节点，则这是个终结节点
        //        int childCount = ResolveDeptCollection(dbDepts, dept, level);
        //        if (childCount != 0)
        //        {
        //            dept.IsTreeLeaf = false;
        //        }
        //        level--;

        //    }

        //    return count;
        //}


        private static int ResolveDeptCollection(List<depts> dbDepts, int parentDeptID, int level)
        {
            int count = 0;
            foreach (var dept in dbDepts.Where(d => d.ParentID == parentDeptID))
            {
                count++;

                _depts.Add(dept);
                dept.TreeLevel = level;
                dept.IsTreeLeaf = true;
                dept.Enabled = true;

                level++;
                // 如果这个节点下没有子节点，则这是个终结节点
                int childCount = ResolveDeptCollection(dbDepts, dept.ID, level);
                if (childCount != 0)
                {
                    dept.IsTreeLeaf = false;
                }
                level--;

            }

            return count;
        }
    }
}
