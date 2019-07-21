using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace BookIms.tools
{
    public static class PostDataServer
    {

        public static string HttpPostData(string url, MsMultiPartFormData form, string filePath, string fileKeyName = "file", int timeOut = 360000)
        {
            string result = "";
            WebRequest request = WebRequest.Create(url);
            request.Timeout = timeOut;
            FileStream file = new FileStream(filePath, FileMode.Open);
            byte[] bb = new byte[file.Length];
            file.Read(bb, 0, (int)file.Length);
            file.Close();
            string fileName = Path.GetFileName(filePath);
            form.AddStreamFile(fileKeyName, fileName, bb);
            form.PrepareFormData();
            request.ContentType = "multipart/form-data; boundary=" + form.Boundary;
            request.Method = "POST";
            Stream stream = request.GetRequestStream();
            foreach (var b in form.GetFormData())
            {
                stream.WriteByte(b);
            }
            stream.Close();
            WebResponse response = request.GetResponse();

            using (var httpStreamReader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
            {
                result = httpStreamReader.ReadToEnd();
            }
            response.Close();
            request.Abort();
            return result;
        }
    }

}