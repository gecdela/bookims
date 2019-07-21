using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BookIms.tools
{
    public class SendFileHandler 
    {

        /// <summary>
        /// 存放文件的物理根路径
        /// </summary>
        string RootFolder = "files";
        string UPLOADFILE_SERVICE_URL = "http://.....";//最终接收文件上传的服务接口
        public void ProcessRequest(HttpContext context)
        {
            object result = null;
            string temSavePath = context.Server.MapPath(@"~\") + RootFolder;

            if (!Directory.Exists(temSavePath))
            {
                Directory.CreateDirectory(temSavePath);
            }
            if (context.Request.Files != null && context.Request.Files.Count > 0)
            {
                HttpPostedFile file = context.Request.Files[0];
                string saveFileName = String.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddhhmmssffff"), Path.GetExtension(file.FileName));//保存文件名称
                string fileName = String.Format(@"{0}\{1}", temSavePath, saveFileName);
                file.SaveAs(@"D:\backup\develop\vsSpace\BookIms\BookIms\images\" + fileName);
                MsMultiPartFormData form = new MsMultiPartFormData();
                string decodeName = HttpUtility.UrlDecode(Path.GetFileName(fileName));//最终服务器会按原文件名保存文件，所以需要将文件名编码下，防止中文乱码
                string rst = PostDataServer.HttpPostData(UPLOADFILE_SERVICE_URL, form, fileName, decodeName);
                context.Response.Write(rst);//输出上传文件返回结果
                context.Response.End();
            }

        }
    }
}