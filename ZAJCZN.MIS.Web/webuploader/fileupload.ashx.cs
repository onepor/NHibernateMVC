using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ZAJCZN.MIS.Web.webuploader
{
    /// <summary>
    /// fileupload 的摘要说明
    /// </summary>
    public class fileupload : IHttpHandler, IRequiresSessionState
    {

        private static readonly string WEBUPLOADER_FIXED_SESSION_NAME = "webuploader.webuploader_fixed";

        private void ResponseError(HttpContext context)
        {
            // 出错了
            context.Response.StatusCode = 500;
            context.Response.Write("No file");
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string owner = context.Request.Form["owner"];

            if (context.Request.Files.Count == 0)
            {
                ResponseError(context);
                return;
            }

            if (String.IsNullOrEmpty(owner))
            {
                ResponseError(context);
                return;
            }

            HttpPostedFile postedFile = context.Request.Files[0];
            // 文件名完整路径
            string fileName = postedFile.FileName;
            // 文件名保存的服务器路径
            string savedFileName = GetSavedFileName(fileName);
            postedFile.SaveAs(context.Server.MapPath("~/upload/" + savedFileName));

            string shortFileName = GetFileName(fileName);
            string fileType = GetFileType(fileName);
            int fileSize = postedFile.ContentLength;

            if (owner.StartsWith(WEBUPLOADER_FIXED_SESSION_NAME))
            {
                // 固定文件上传页面专用
                string fileId = owner.Substring(WEBUPLOADER_FIXED_SESSION_NAME.Length + 1);

                JArray source = context.Session[WEBUPLOADER_FIXED_SESSION_NAME] as JArray;
                JObject file = GetFileObject(source, fileId);
                if (file == null)
                {
                    ResponseError(context);
                    return;
                }

                file["name"] = shortFileName;
                file["type"] = fileType;
                file["savedName"] = savedFileName;
                file["size"] = fileSize;
                file["status"] = "uploaded";

                context.Response.Write("Success");
            }
            else
            {
                JObject fileObj = new JObject();
                string fileId = Guid.NewGuid().ToString();

                fileObj.Add("name", shortFileName);
                fileObj.Add("type", fileType);
                fileObj.Add("savedName", savedFileName);
                fileObj.Add("size", fileSize);
                fileObj.Add("id", fileId);

                SaveToDatabase(context, owner, fileObj);

                context.Response.Write("Success");
            }

        }

        private JObject GetFileObject(JArray source, string fileId)
        {
            for (int i = 0, count = source.Count; i < count; i++)
            {
                JObject item = source[i] as JObject;

                if (item.Value<string>("id") == fileId)
                {
                    return item;
                }
            }
            return null;
        }


        // 模拟在服务器端保存数据
        // 特别注意：在真实的开发环境中，不要在Session放置大量数据，否则会严重影响服务器性能
        private void SaveToDatabase(HttpContext context, string sessionName, JObject fileObj)
        {
            if (context.Session[sessionName] == null)
            {
                context.Session[sessionName] = new JArray();
            }

            JArray source = context.Session[sessionName] as JArray;
            source.Add(fileObj);

            context.Session[sessionName] = source;
        }


        private string GetFileType(string fileName)
        {
            string fileType = String.Empty;
            int lastDotIndex = fileName.LastIndexOf(".");
            if (lastDotIndex >= 0)
            {
                fileType = fileName.Substring(lastDotIndex + 1).ToLower();
            }

            return fileType;
        }

        private string GetFileName(string fileName)
        {
            string shortFileName = fileName;
            int lastSlashIndex = shortFileName.LastIndexOf("\\");
            if (lastSlashIndex >= 0)
            {
                shortFileName = shortFileName.Substring(lastSlashIndex + 1);
            }

            return shortFileName;
        }

        private string GetSavedFileName(string fileName)
        {
            fileName = fileName.Replace(":", "_").Replace(" ", "_").Replace("\\", "_").Replace("/", "_");
            fileName = DateTime.Now.Ticks.ToString() + "_" + fileName;

            return fileName;
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}