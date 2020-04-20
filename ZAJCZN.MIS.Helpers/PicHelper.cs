using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ZAJCZN.MIS.Helpers
{
    /// <summary>
    /// 图片操作相关的类
    /// </summary>
    public class PicHelper
    {
        /// <summary>
        /// 根据图片位置判断，如没有则返回默认图片
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public string GetImageStr(string imagePath)
        {
            string serverPath = HttpContext.Current.Server.MapPath(imagePath);
            if (File.Exists(serverPath))
                return imagePath;
            return "/SysImg/noPic.jpg";
        }

        /// <summary>
        /// 根据文件短名称确认是否为图片
        /// </summary>
        /// <param name="fileShortName"></param>
        /// <returns></returns>
        public static bool CheckFileIsCorrect(string fileShortName)
        {
            string filename = fileShortName.Substring(fileShortName.LastIndexOf(".") + 1);//取得后缀
            if (filename.ToLower() != "jpg" && filename.ToLower() != "gif" && filename.ToLower() != "png" && filename.ToLower() != "bmp")//判断类型
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检查图片裁剪支持的类型
        /// </summary>
        /// <param name="fileupload"></param>
        /// <returns></returns>
        public string CheckFileIsCorrect(FileUpload fileupload)
        {
            if (!fileupload.HasFile) return "未选择文件";

            string filename = fileupload.FileName;//取得文件名
            filename = filename.Substring(filename.LastIndexOf(".") + 1);//取得后缀
            if (filename.ToLower() != "jpg" && filename.ToLower() != "gif" && filename.ToLower() != "png" && filename.ToLower() != "bmp")//判断类型
            {
                return "图片格式只支持jpg、gif、png、bmp";
            }
            if (fileupload.PostedFile.ContentLength > ConfigHelper.GetConfigInt("picUpSizeKB") * 1024)
            {
                return "上传图片尺寸过大，图片大小不得超过" + ConfigHelper.GetConfigInt("picUpSizeKB") + "KB";
            }

            return null;
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="sourcePicPath"></param>
        /// <param name="savePicPath"></param>
        /// <param name="xLength"></param>
        /// <param name="yLength"></param>
        /// <returns></returns>
        private bool SavePic(string sourcePicPath, string savePicPath, int xLength, int yLength)
        {
            try
            {
                using (System.Drawing.Image imgPhoto = System.Drawing.Image.FromFile(sourcePicPath))
                {
                    //获取图片宽高
                    int Width = imgPhoto.Width;
                    int Height = imgPhoto.Height;

                    //获取图片水平和垂直的分辨率
                    float dpiX = imgPhoto.HorizontalResolution;
                    float dpiY = imgPhoto.VerticalResolution;

                    //缩略图在画布上的X放向起始点
                    int MarginX = 0;
                    //缩略图在画布上的Y放向起始点
                    int MarginY = 0;

                    int dw = 0;
                    int dh = 0;

                    if ((double)Width / (double)Height > (double)xLength / (double)yLength)
                    {
                        //宽比高大，以宽为准  
                        dw = imgPhoto.Width * xLength / imgPhoto.Width;
                        dh = imgPhoto.Height * yLength / imgPhoto.Width;
                        MarginX = 0;
                        MarginY = (yLength - dh) / 2;
                    }
                    else
                    {
                        //高比宽大，以高为准  
                        dw = imgPhoto.Width * xLength / imgPhoto.Height;
                        dh = imgPhoto.Height * yLength / imgPhoto.Height;
                        MarginX = (xLength - dw) / 2;
                        MarginY = 0;
                    }

                    using (Bitmap bmPhoto = new Bitmap(xLength, yLength, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
                    {
                        //设置位图文件的水平和垂直分辨率  与Img一致
                        bmPhoto.SetResolution(dpiX, dpiY);

                        using (Graphics gbmPhoto = Graphics.FromImage(bmPhoto))
                        {
                            //设置高质量插值法  
                            gbmPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                            //设置高质量,低速度呈现平滑程度  
                            gbmPhoto.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            //清空画布并以白色背景色填充  
                            gbmPhoto.Clear(Color.White);

                            //在指定位置并且按指定大小绘制原图片的指定部分  
                            gbmPhoto.DrawImage(imgPhoto, new Rectangle(MarginX, MarginY, dw, dh),
                             new Rectangle(0, 0, Width, Height),
                             GraphicsUnit.Pixel);

                            ////在位图文件上填充一个矩形框
                            //System.Drawing.Rectangle Rec = new System.Drawing.Rectangle(0, 0, xLength, yLength);
                            ////定义一个白色的画刷
                            //SolidBrush mySolidBrush = new SolidBrush(System.Drawing.Color.White);
                            ////Grp.Clear(Color.White);
                            ////将矩形框填充为白色
                            //gbmPhoto.FillRectangle(mySolidBrush, Rec);

                            ////向矩形框内填充Img
                            ////gbmPhoto.DrawImage(imgPhoto
                            ////    , MarginX, MarginY, Rec, GraphicsUnit.Pixel);

                            ////gbmPhoto.DrawImage(imgPhoto
                            ////    ,Rec
                            ////    , MarginX, MarginY, imgPhoto.Width, imgPhoto.Height, GraphicsUnit.Pixel);

                            //gbmPhoto.DrawImage(imgPhoto
                            //    , new Rectangle(0, 0, xLength, yLength)
                            //    , 0, 0
                            //    , imgPhoto.Width, imgPhoto.Height
                            //    , GraphicsUnit.Pixel);

                            //保存图片到服务器 以Jpeg格式保存缩略图(KB最小)  
                            bmPhoto.Save(savePicPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                            ////生成文件流回传  
                            //MemoryStream ms2 = new MemoryStream();
                            //bmPhoto.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);

                            imgPhoto.Dispose();
                            bmPhoto.Dispose();
                            gbmPhoto.Dispose();
                            GC.Collect();
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #region - image 与 base64 -

        /// <summary>
        /// 将Base64字符串转换为图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private System.Drawing.Image Base64ToImage(string base64Str)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64Str);
                MemoryStream memStream = new MemoryStream(bytes);
                //BinaryFormatter binFormatter = new BinaryFormatter();
                //System.Drawing.Image img = (System.Drawing.Image)binFormatter.Deserialize(memStream);
                System.Drawing.Image img = System.Drawing.Image.FromStream(memStream);
                return img;
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex.Message, "Base64ToImage");
                return null;
            }
        }

        /// <summary>
        /// 将Base64字符串转换为图片，返回图片保存路径
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public string Base64ToImageReturnSavePath(string base64Str)
        {
            try
            {
                System.Drawing.Image img = Base64ToImage(base64Str);
                if (null == img)
                {
                    return null;
                }

                string curDay = DateTime.Now.ToString("yyyyMMdd");
                string curTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string FolderPath = ConfigHelper.GetConfigString("picSavePath") + curDay + "/";
                FileHelper.CheckFolder(FolderPath);

                string savePath = FolderPath + curTime + ".jpg";
                img.Save(savePath);
                string returnPath = ConfigHelper.GetConfigString("picShowPath") + curDay + "/" + curTime + ".jpg";
                return returnPath;
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex.Message, "Base64ToImageReturnSavePath");
                return null;
            }
        }

        public bool Base64ToImageSaveWithInputPath(string base64Str, string fileSaveAllPath)
        {
            try
            {
                System.Drawing.Image img = Base64ToImage(base64Str);
                if (null == img)
                {
                    return false;
                }
                img.Save(fileSaveAllPath);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex.Message, "Base64ToImageReturnSavePath");
                return false;
            }
        }

        #endregion

        /// <summary>
        /// 获取文件保存路径（显示）
        /// </summary>
        /// <param name="imgShortName"></param>
        /// <returns></returns>
        public static string GetShowPicPath(string fileEx, DateTime cutTime)
        {
            string curDay = cutTime.ToString("yyyyMMdd");
            string curTime = cutTime.ToString("yyyyMMddHHmmssfff");
            string returnPath = ConfigHelper.GetConfigString("picShowPath") + curDay + "/" + curTime + "." + fileEx;
            return returnPath;
        }

        /// <summary>
        /// 获取文件保存的物理路径
        /// </summary>
        /// <param name="fileEx"></param>
        /// <returns></returns>
        public static string GetRealSavePath(string fileEx, DateTime cutTime)
        {
            string curDay = cutTime.ToString("yyyyMMdd");
            string curTime = cutTime.ToString("yyyyMMddHHmmssfff");
            string FolderPath = ConfigHelper.GetConfigString("picSavePath") + curDay + "/";
            FileHelper.CheckFolder(FolderPath);

            return FolderPath + curTime + "." + fileEx;
        }

        /// <summary>
        /// 获取默认空图片保存的物理路径
        /// </summary>
        /// <param name="fileEx"></param>
        /// <returns></returns>
        public static string GetEmptyPicPath()
        {
            string FolderPath = ConfigHelper.GetConfigString("picShowPath") + "default/nopic.jpg";
            //FileHelper.CheckFolder(FolderPath);
            return FolderPath;
        }

        /// <summary>
        /// 获取保存图片的长高
        /// </summary>
        /// <param name="realPicPath"></param>
        /// <returns></returns>
        public static int[] GetRealPicWidth(string realPicPath)
        {
            System.Drawing.Image pic = System.Drawing.Image.FromFile(realPicPath);//realPicPath是该图片的绝对路径

            int intWidth = pic.Width;//长度像素值
            int intHeight = pic.Height;//高度像素值 

            return new int[2] { intWidth, intHeight };
        }

    }

    /// <summary>
    /// 图片设置类，从webConfig获取
    /// </summary>
    public static class PicSets
    {
        public static int SmallPicSizeX { get { return ConfigHelper.GetConfigInt("SmallPicSizeX"); } }
        public static int SmallPicSizeY { get { return ConfigHelper.GetConfigInt("SmallPicSizeY"); } }
        public static int MidPicSizeX { get { return ConfigHelper.GetConfigInt("MidPicSizeX"); } }
        public static int MidPicSizeY { get { return ConfigHelper.GetConfigInt("MidPicSizeY"); } }
        public static int BigPicSizeX { get { return ConfigHelper.GetConfigInt("BigPicSizeX"); } }
        public static int BigPicSizeY { get { return ConfigHelper.GetConfigInt("BigPicSizeY"); } }
    }

}
