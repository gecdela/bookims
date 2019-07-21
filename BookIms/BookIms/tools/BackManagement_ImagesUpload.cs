//using System;
//using System.Collections;
//using System.Configuration;
//using System.Data;
//using System.Linq;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
//using System.IO;
//using System.Net;
//using System.Text.RegularExpressions;
/////

///// FileUpload1.HasFile  如果是true，则表示该控件有文件要上传
///// FileUpload1.FileName  返回要上传文件的名称，不包含路径信息
///// FileUpload1.FileContent  返回一个指向上传文件的流对象
///// FileUpload1.PostedFile   返回已经上传文件的引用
///// FileUpload1.PostedFile.ContentLength  返回上传文件的按字节表示的文件大小
///// FileUpload1.PostedFile.ContentType    返回上传文件的MIME内容类型，也就是文件类型，如返回"image/jpg"
///// FileUpload1.PostedFile.FileName       返回文件在客户端的完全路径（包括文件名全称）
///// FileUpload1.PostedFile.InputStream    返回一个指向上传文件的流对象
///// FileInfo对象表示磁盘或网络位置上的文件。提供文件的路径，就可以创建一个FileInfo对象：
/////
//namespace BookIms.Dapper {
//    public partial class BackManagement_ImagesUpload : System.Web.UI.Page
//    {
//        public string treePath = "";
//        public int imageW = 100;
//        public int imageH = 100;
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            this.Button5.Attributes.Add("Onclick", "window.close();"); //在本地关闭当前页，而不需要发送到服务器去关闭当前页时
//            if (!Page.IsPostBack)
//            {
//                Label2.Text = Server.MapPath("/");
//                TextBox3.Text = "ImageUpload";
//                treePath = Server.MapPath("/") + TextBox3.Text.Trim() + "/";
//                TextBox4.Text = imageW.ToString();
//                TextBox5.Text = imageH.ToString();
//            }
//        }
//        protected void btnload_Click(object sender, EventArgs e)
//        {
//            //如果保存图片的目录不存在，由创建它
//            treePath = Server.MapPath("/") + TextBox3.Text.Trim() + "/";
//            imageW = Convert.ToInt32(TextBox4.Text.ToString());
//            imageH = Convert.ToInt32(TextBox5.Text.ToString());
//            if (!File.Exists(treePath + "images"))   //如果/ImageUpload/images不存在，则创建/ImageUpload/images，用于存放源图片
//            {
//                System.IO.Directory.CreateDirectory(treePath + "images");
//            }
//            if (!File.Exists(treePath + "thumbnails"))   //如果/ImageUpload/thumbnails不存在，则创建/ImageUpload/thumbnails，用于存放缩略图片
//            {
//                System.IO.Directory.CreateDirectory(treePath + "thumbnails");
//            }
//            if (!File.Exists(treePath + "textImages"))   //如果/ImageUpload/textImages不存在，则创建/ImageUpload/textImages，用于存文字水印图片
//            {
//                System.IO.Directory.CreateDirectory(treePath + "textImages");
//            }
//            if (!File.Exists(treePath + "waterImages"))   //如果/ImageUpload/waterImages不存在，则创建/ImageUpload/waterImages
//                                                          //用于存图形水印图片
//            {
//                System.IO.Directory.CreateDirectory(treePath + "waterImages");
//            }
//            if (FileUpload1.HasFile)   //如果是true，则表示该控件有文件要上传
//            {
//                string fileContentType = FileUpload1.PostedFile.ContentType;
//                if (fileContentType == "image/bmp" || fileContentType == "image/gif" || fileContentType == "image/pjpeg")
//                {
//                    string name = FileUpload1.PostedFile.FileName;         //返回文件在客户端的完全路径（包括文件名全称）
//                    FileInfo file = new FileInfo(name);          //FileInfo对象表示磁盘或网络位置上的文件。提供文件的路径，就可以创建一个FileInfo对象：
//                    string fileName = file.Name;                     // 文件名称
//                    string fileName_s = "x_" + file.Name;     // 缩略图文件名称
//                    string fileName_sy = "text_" + file.Name;                              // 水印图文件名称（文字）
//                    string fileName_syp = "water_" + file.Name;                            // 水印图文件名称（图片）
//                    string webFilePath = treePath + "images/" + fileName;          // 服务器端文件路径
//                    string webFilePath_s = treePath + "thumbnails/" + fileName_s;    // 服务器端缩略图路径
//                    string webFilePath_sy = treePath + "textImages/" + fileName_sy;   // 服务器端带水印图路径(文字)
//                    string webFilePath_syp = treePath + "waterImages/" + fileName_syp; // 服务器端带水印图路径(图片)
//                    string webFilePath_sypf = Server.MapPath("../images/tzwhx.png");               // 服务器端水印图路径(图片)

//                    if (!File.Exists(webFilePath))
//                    {
//                        try
//                        {
//                            FileUpload1.SaveAs(webFilePath);                                // 使用 SaveAs 方法保存文件
//                            if (CheckBox1.Checked)                                          //是否生成文字水印图
//                            {
//                                AddWater(webFilePath, webFilePath_sy);
//                            }
//                            if (CheckBox2.Checked)                                          //是否生成图形水印图
//                            {
//                                AddWaterPic(webFilePath, webFilePath_syp, webFilePath_sypf);
//                            }
//                            MakeThumbnail(webFilePath, webFilePath_s, imageW, imageH, "Cut");     // 生成缩略图方法
//                            Label1.Text = "提示：文件“" + fileName + "”成功上传，并生成“" + fileName_s + "”缩略图，文件类型为：" + FileUpload1.PostedFile.ContentType + "，文件大小为：" + FileUpload1.PostedFile.ContentLength + "B";
//                            Image1.ImageUrl = "/" + TextBox3.Text.ToString() + "/images/" + fileName;
//                            TextBox1.Text = webFilePath;
//                            TextBox2.Text = "/" + TextBox3.Text.ToString() + "/images/" + fileName;
//                        }
//                        catch (Exception ex)
//                        {
//                            Label1.Text = "提示：文件上传失败，失败原因：" + ex.Message;
//                        }
//                    }
//                    else
//                    {
//                        Label1.Text = "提示：文件已经存在，请重命名后上传";
//                    }
//                }
//                else
//                {
//                    Label1.Text = "提示：文件类型不符";
//                }
//            }
//        }
//    }