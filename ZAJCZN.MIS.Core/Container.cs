using System;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Windsor;
using Castle.MicroKernel;

namespace ZAJCZN.MIS.Core
{
    public class Container
    {
        /// <summary>
        /// 拦截器
        /// </summary>
        private XmlInterpreter interpreter;
        /// <summary>
        /// IOC变量
        /// </summary>
        private WindsorContainer windsor;
        /// <summary>
        /// 单例模式
        /// </summary>
        private static readonly Container instance = new Container();
        public static Container Instance
        {
            get { return instance; }
        }
        public IKernel Ikernel { get; private set; }
        private Container()
        {
            try
            {
                XmlInterpreter interpreter; 
                //解决 在II7中使用集成环境错误；
                string path = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                interpreter = new XmlInterpreter(path + @"\Configuration\IServiceConfig.xml");
                //if (path.ToLower().Contains(@"Windows\SysWOW64\inetsrv"))
                //{
                //    interpreter = new XmlInterpreter("Configuration/castle.config");
                //}
                //else
                //{
                //    interpreter = new XmlInterpreter(path + @"\Configuration\castle.config");
                //} 

                //interpreter = new XmlInterpreter("Configuration/IServiceConfig.xml");
                windsor = new WindsorContainer(interpreter);
                Ikernel = windsor.Kernel;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        public void Dispose()
        {
            Ikernel.Dispose();
        }
        public T Resolve<T>()
        {
            return Ikernel.Resolve<T>();
        }
        public T Resolve<T>(string key)
        {
            return (T)Ikernel.Resolve<T>(key);
        }
    }
}
