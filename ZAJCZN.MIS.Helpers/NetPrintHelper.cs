using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Net.Sockets;
using System.Web;

namespace ZAJCZN.MIS.Helpers
{
    public class NetPrintHelper
    {
        #region 01.获得打印机1获得吧台打印机2获得后厨打印机

        /// <summary> 
        /// 获得打印机1获得吧台打印机2获得后厨打印机     
        /// </summary>  
        public TcpClient GetPrint(int n = 1)
        {
            var client = new System.Net.Sockets.TcpClient();
            var port = 9100;
            var ipPrint = "192.168.2.180";
            client.Connect(ipPrint, port);
            return client;
        }

        /// <summary> 
        /// 获得打印机1获得吧台打印机2获得后厨打印机     
        /// </summary>  
        public TcpClient GetPrint(string ip, int ipport)
        {
            var client = new System.Net.Sockets.TcpClient();
            var port = ipport;
            var ipPrint = ip;
            client.Connect(ipPrint, port);
            return client;
        }


        #endregion

        #region 02.初始化一个网路访问数据流

        /// <summary>    
        /// 初始化一个网路访问数据流    
        /// </summary>   
        /// <returns></returns>     
        public NetworkStream BuildStream()
        {
            System.Net.Sockets.NetworkStream stream = null;
            return stream;
        }

        #endregion

        #region 03.套接字和打印机通讯放回通讯流

        /// <summary>     
        /// 套接字和打印机通讯放回通讯流      
        /// </summary>   
        /// <returns></returns>   
        public NetworkStream GetStream(TcpClient client, NetworkStream stream)
        {
            byte[] chushihua = new byte[] { 27, 64 };//初始化打印机   
            byte[] ziti = new byte[] { 27, 77, 0 };//选择字体n =0,1,48,49  
            byte[] zitidaxiao = new byte[] { 29, 33, 0 };//选择字体大小 
            byte[] duiqifangshi = new byte[] { 27, 97, 1 };//选择对齐方式0,48左对齐1,49中间对齐2,50右对齐 
            stream = client.GetStream();             //是否支持写入  
            if (!stream.CanWrite)
            {
                stream = null;
            }
            stream.Write(chushihua, 0, chushihua.Length);//初始化  
            stream.Write(ziti, 0, ziti.Length);//设置字体   
            stream.Write(zitidaxiao, 0, zitidaxiao.Length);//设置字体大小--关键 
            stream.Write(duiqifangshi, 0, duiqifangshi.Length);//居中 
            return stream;
        }

        #endregion

        #region 04.把要打印的文字写入打印流

        /// <summary>
        /// 把要打印的文字写入打印流 
        /// </summary> 
        /// <param name="stream"></param>
        /// <param name="output"></param> 
        public void PrintText(NetworkStream stream, string output)
        {
            Byte[] data = System.Text.Encoding.Default.GetBytes(output);
            stream.Write(data, 0, data.Length);//输出文字 
        }

        #endregion

        #region 05.设置对齐方式0,48左对齐1,49中间对齐2,50右对齐

        /// <summary>   
        /// 设置对齐方式0,48左对齐1,49中间对齐2,50右对齐 
        /// </summary> 
        /// <param name="stream"></param>  
        /// <param name="n"></param>   
        public void SetDuiQi(NetworkStream stream, byte n)
        {
            byte[] duiqifangshi = new byte[] { 27, 97, n };//选择对齐方式0,48左对齐1,49中间对齐2,50右对齐  
            stream.Write(duiqifangshi, 0, duiqifangshi.Length);
        }

        #endregion

        #region 06.设置字体n =0,1,48,49

        /// <summary>   
        /// 设置字体n =0,1,48,49  
        /// </summary> 
        /// <param name="stream"></param>  
        /// <param name="n"></param>   
        public void SetFont(NetworkStream stream, byte n)
        {
            byte[] ziti = new byte[] { 27, 77, n };//选择字体n =0,1,48,49 
            stream.Write(ziti, 0, ziti.Length);
        }

        #endregion

        #region 07.设置加粗1加粗0还原

        /// <summary>  
        /// 设置加粗1加粗0还原  
        /// </summary>    
        /// <param name="stream"></param> 
        /// <param name="n"></param>   
        public void SetBold(NetworkStream stream, byte n)
        {
            byte[] jiacu = new byte[] { 27, 69, n };//选择加粗模式 
            stream.Write(jiacu, 0, jiacu.Length);
        }

        #endregion

        #region 08.设置字体大小0最小1,2,3

        /// <summary>
        /// 设置字体大小0最小1,2,3   
        /// </summary>   
        /// <param name="stream"></param> 
        /// <param name="n"></param>  
        public void SetFontSize(NetworkStream stream, byte n)
        {
            byte[] zitidaxiao = new byte[] { 29, 33, n };//选择字体大小    
            stream.Write(zitidaxiao, 0, zitidaxiao.Length);
        }

        #endregion

        #region 09.切纸

        /// <summary> 
        /// 切纸  
        /// </summary>  
        /// <param name="stream"></param> 
        /// <param name="n"></param>  
        public void QieZhi(NetworkStream stream)
        {
            byte[] qiezhi = new byte[] { 29, 86, 1, 49 };//切纸 
            stream.Write(qiezhi, 0, qiezhi.Length);
        }

        #endregion

        #region 10.释放资源

        /// <summary> 
        /// 释放资源 
        /// </summary> 
        /// <param name="client"></param> 
        /// <param name="stream"></param>  
        public void DiposeStreamClient(TcpClient client, NetworkStream stream)
        {
            if (stream != null)
            {
                stream.Close();
                stream.Dispose();
            }
            if (client != null)
            {
                client.Close();
            }
        }

        #endregion

        #region 11.样例展示

        /// <summary>
        /// 批量打印点菜单
        /// </summary>
        /// <param name="list"></param>
        /// <param name="printer"></param>
        /// <param name="type"></param>
        /// <param name="tabie"></param>
        public void Printeg(IList<tm_TabieDishesInfo> list, tm_Printer printer, int type, string tabie)
        {
            var p = new NetPrintHelper();
            var batai = p.GetPrint(printer.IP, printer.Port);//获得吧台打印机  
            var liunull = p.BuildStream();//初始化一个网络访问数据流  

            try
            {
                var liu = p.GetStream(batai, liunull);//获得通讯打印流  
                string title = "";
                if (type == 1)
                {
                    p.SetFontSize(liu, 2);//字体变大         
                    title = "点菜单";
                }
                else if (type == 2)
                {
                    p.SetFontSize(liu, 2);//字体变大         
                    title = "加菜单";
                }
                else
                {
                    p.SetFontSize(liu, 3);//字体变大         
                    title = "#退菜单#";
                }
                p.SetBold(liu, 1);//加粗 
                p.PrintText(liu, tabie + "\n");//打印文字  
                p.PrintText(liu, title + "\n\n");//打印文字   

                p.SetBold(liu, 0);//取消加粗             

                p.SetFontSize(liu, 0);

                p.SetDuiQi(liu, 0);
                tm_TabieUsingInfo entity = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(list[0].TabieUsingID);
                p.PrintText(liu, DateTime.Now.ToString() + "             客数：" + entity.Population + "\n");//打印文字      

                p.PrintText(liu, "------------------------------------------------\n");//打印文字

                p.SetFontSize(liu, 1);
                foreach (tm_TabieDishesInfo item in list)
                {
                    int length = 40 - item.DishesName.Length;
                    p.PrintText(liu, item.DishesName.PadRight(length) + item.DishesCount + " " + item.UnitName + "\n");//打印文字
                    item.IsPrint = 1;
                    Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().Update(item);
                }
                p.SetFontSize(liu, 0);
                p.PrintText(liu, "------------------------------------------------\n");//打印文字   

                p.PrintText(liu, "服务员：002           打印机：" + printer.PrinterName + "\n\n\n\n\n\n");//打印文字      

                p.QieZhi(liu);//切纸        

            }
            catch
            {
                //打印机缺纸或者纸匣打开时,不会出现异常,不用特殊代码判断,数据不会丢失.
            }

            finally
            {
                p.DiposeStreamClient(batai, liunull);//释放资源 
            }
        }

        /// <summary>
        /// 单个打印点菜单
        /// </summary>
        /// <param name="dishesName">菜品名称</param>
        /// <param name="dishesCount">菜品数量</param>
        /// <param name="unitName">菜品单位</param>
        /// <param name="custmerNO">客数</param>
        /// <param name="printer">打印机信息</param>
        /// <param name="type">单子类型</param>
        /// <param name="tabieName">餐台名称</param>
        public void Printeg(string dishesName, decimal dishesCount, string unitName, int custmerNO, tm_Printer printer, int type, string tabieName)
        {
            var p = new NetPrintHelper();
            var batai = p.GetPrint(printer.IP, printer.Port);//获得吧台打印机  
            var liunull = p.BuildStream();//初始化一个网络访问数据流  

            try
            {
                var liu = p.GetStream(batai, liunull);//获得通讯打印流  
                string title = "";
                if (type == 1)
                {
                    p.SetFontSize(liu, 2);//字体变大         
                    title = "点菜单(留存)";
                }
                if (type == 9)
                {
                    p.SetFontSize(liu, 2);//字体变大         
                    title = "点菜单(上菜)";
                }
                if (type == 2)
                {
                    p.SetFontSize(liu, 2);//字体变大         
                    title = "----叫菜单(留存)----";
                }
                if (type == 8)
                {
                    p.SetFontSize(liu, 2);//字体变大         
                    title = "----叫菜单(上菜)----";
                }
                if (type == 3)
                {
                    p.SetFontSize(liu, 3);//字体变大         
                    title = "####退菜单####";
                }
                p.SetBold(liu, 1);//加粗 
                p.PrintText(liu, tabieName + "\n");//打印文字  
                p.PrintText(liu, title + "\n\n");//打印文字   

                p.SetBold(liu, 0);//取消加粗             

                p.SetFontSize(liu, 0);

                p.SetDuiQi(liu, 0);
                //tm_TabieUsingInfo entity = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(dishesInfo.TabieUsingID);
                p.PrintText(liu, DateTime.Now.ToString() + "             客数：" + custmerNO + "\n");//打印文字      

                p.PrintText(liu, "------------------------------------------------\n");//打印文字

                p.SetFontSize(liu, 1);

                int length = 40 - dishesName.Length;
                p.PrintText(liu, dishesName.PadRight(length) + Math.Abs(dishesCount) + " " + unitName + "\n");//打印文字
                //dishesInfo.IsPrint = 1;
                //Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().Update(dishesInfo);

                p.SetFontSize(liu, 0);
                p.PrintText(liu, "------------------------------------------------\n");//打印文字   

                p.PrintText(liu, "服务员：002           打印机：" + printer.PrinterName + "\n\n\n\n\n\n");//打印文字      

                p.QieZhi(liu);//切纸        

            }
            catch
            {
                //打印机缺纸或者纸匣打开时,不会出现异常,不用特殊代码判断,数据不会丢失.
            }
            finally
            {
                p.DiposeStreamClient(batai, liunull);//释放资源 
            }
        }

        /// <summary>
        /// 打印美团单
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="printer"></param>
        /// <param name="custmerNO"></param>
        /// <param name="tabie"></param>
        public void Printeg(string groupName, tm_Printer printer, int custmerNO, string tabie, int type)
        {
            var p = new NetPrintHelper();
            var batai = p.GetPrint(printer.IP, printer.Port);//获得吧台打印机  
            var liunull = p.BuildStream();//初始化一个网络访问数据流  

            try
            {
                var liu = p.GetStream(batai, liunull);//获得通讯打印流  
                string title = "";

                p.SetFontSize(liu, 2);//字体变大    
                if (type == 4)
                {
                    p.SetFontSize(liu, 3);//字体变大         
                    title = "####点菜单(走菜)####";
                }
                if (type == 1)
                {
                    p.SetFontSize(liu, 3);//字体变大         
                    title = "美团单";
                }

                p.SetBold(liu, 1);//加粗 
                p.PrintText(liu, tabie + "\n");//打印文字  
                p.PrintText(liu, title + "\n\n");//打印文字   

                p.SetBold(liu, 0);//取消加粗             

                p.SetFontSize(liu, 0);

                p.SetDuiQi(liu, 0);
                p.PrintText(liu, DateTime.Now.ToString() + "             客数：" + custmerNO + "\n");//打印文字      

                p.PrintText(liu, "------------------------------------------------\n");//打印文字

                p.SetFontSize(liu, 1);

                int length = 40 - groupName.Length;
                p.PrintText(liu, groupName.PadRight(length) + 1 + " " + "份\n");//打印文字

                p.SetFontSize(liu, 0);
                p.PrintText(liu, "------------------------------------------------\n");//打印文字   

                p.PrintText(liu, "服务员：002           打印机：" + printer.PrinterName + "\n\n\n\n\n\n");//打印文字      

                p.QieZhi(liu);//切纸  
            }
            catch
            {
                //打印机缺纸或者纸匣打开时,不会出现异常,不用特殊代码判断,数据不会丢失. 
            }

            finally
            {
                p.DiposeStreamClient(batai, liunull);//释放资源  
            }
        }

        public void Printeg()
        {

            var p = new NetPrintHelper();
            var batai = p.GetPrint();//获得吧台打印机  
            var liunull = p.BuildStream();//初始化一个网络访问数据流  

            try
            {
                var liu = p.GetStream(batai, liunull);//获得通讯打印流  
                string title = "";

                p.SetFontSize(liu, 2);//字体变大         
                title = "点菜单";


                p.SetFontSize(liu, 0);
                p.PrintText(liu, "------------------------------------------------\n");//打印文字   

                p.PrintText(liu, "服务员：002           打印机：测试\n\n\n\n\n\n");//打印文字      

                p.QieZhi(liu);//切纸        

            }
            catch
            {

                //打印机缺纸或者纸匣打开时,不会出现异常,不用特殊代码判断,数据不会丢失.     

            }

            finally
            {

                p.DiposeStreamClient(batai, liunull);//释放资源       

            }

        }


        #endregion
    }
}