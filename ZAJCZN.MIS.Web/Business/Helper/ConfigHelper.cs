using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using NHibernate.Criterion;

namespace ZAJCZN.MIS.Web
{
    public class ConfigHelper
    {
        #region fields & constructor

        private static List<configs> _configs;

        private static List<String> changedKeys = new List<string>();

        public static List<configs> Configs
        {
            get
            {
                if (_configs == null)
                {
                    InitConfigs();
                }
                return _configs;
            }
        }

        public static void Reload()
        {
            _configs = null;
        }

        public static void InitConfigs()
        {
            _configs = Core.Container.Instance.Resolve<IServiceConfigs>().GetAll().ToList();
        }

        #endregion

        #region methods

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ConfigKey", key));
            configs obj = Core.Container.Instance.Resolve<IServiceConfigs>().GetEntityByFields(qryList);
            return obj != null ? obj.ConfigValue : ""; ;
            //return Configs.Where(c => c.ConfigKey == key).Select(c => c.ConfigValue).FirstOrDefault();
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetValue(string key, string value)
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ConfigKey", key));
            configs config = Core.Container.Instance.Resolve<IServiceConfigs>().GetEntityByFields(qryList);

            //Config config = Configs.Where(c => c.ConfigKey == key).FirstOrDefault();
            if (config != null)
            {
                if (config.ConfigValue != value)
                {
                    //changedKeys.Add(key);
                    config.ConfigValue = value;
                    Core.Container.Instance.Resolve<IServiceConfigs>().Update(config);
                }
            }
        }

        ///// <summary>
        ///// 保存所有更改的配置项
        ///// </summary>
        //public static void SaveAll()
        //{ 
        //    var changedConfigs = PageBase.DB.Configs.Where(c => changedKeys.Contains(c.ConfigKey));
        //    foreach (var changed in changedConfigs)
        //    {
        //        changed.ConfigValue = GetValue(changed.ConfigKey);
        //    }

        //    PageBase.DB.SaveChanges();

        //    Reload();
        //}

        #endregion

        #region properties

        /// <summary>
        /// 网站标题
        /// </summary>
        public static string Title
        {
            get
            {
                return GetValue("Title");
            }
            set
            {
                SetValue("Title", value);
            }
        }

        /// <summary>
        /// 列表每页显示的个数
        /// </summary>
        public static int PageSize
        {
            get
            {
                return Convert.ToInt32(GetValue("PageSize"));
            }
            set
            {
                SetValue("PageSize", value.ToString());
            }
        }

        /// <summary>
        /// 帮助下拉列表
        /// </summary>
        public static string HelpList
        {
            get
            {
                return GetValue("HelpList");
            }
            set
            {
                SetValue("HelpList", value);
            }
        }


        /// <summary>
        /// 菜单样式
        /// </summary>
        public static string MenuType
        {
            get
            {
                return GetValue("MenuType");
            }
            set
            {
                SetValue("MenuType", value);
            }
        }


        /// <summary>
        /// 网站主题
        /// </summary>
        public static string Theme
        {
            get
            {
                return GetValue("Theme");
            }
            set
            {
                SetValue("Theme", value);
            }
        }

        /// <summary>
        /// 店铺名称
        /// </summary>
        public static string StoreName
        {
            get
            {
                return GetValue("StoreName");
            }
            set
            {
                SetValue("StoreName", value);
            }
        }

        /// <summary>
        /// 联系电话
        /// </summary>
        public static string ContractPhone
        {
            get
            {
                return GetValue("ContractPhone");
            }
            set
            {
                SetValue("ContractPhone", value);
            }
        }

        /// <summary>
        /// 安装售后
        /// </summary>
        public static string InstallPhone
        {
            get
            {
                return GetValue("InstallPhone");
            }
            set
            {
                SetValue("InstallPhone", value);
            }
        }

        /// <summary>
        /// 量尺设计
        /// </summary>
        public static string DesignPhone
        {
            get
            {
                return GetValue("DesignPhone");
            }
            set
            {
                SetValue("DesignPhone", value);
            }
        }

        /// <summary>
        /// 投诉电话
        /// </summary>
        public static string ComplaintPhone
        {
            get
            {
                return GetValue("ComplaintPhone");
            }
            set
            {
                SetValue("ComplaintPhone", value);
            }
        }

        /// <summary>
        /// 店铺地址
        /// </summary>
        public static string Address
        {
            get
            {
                return GetValue("Address");
            }
            set
            {
                SetValue("Address", value);
            }
        }
        #endregion
    }
}
