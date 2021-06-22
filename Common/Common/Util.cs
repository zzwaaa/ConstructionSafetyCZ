using Aliyun.OSS;
using Aspose.Words;
using Aspose.Words.Tables;
using MCUtil.Security;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ViewModels.ViewModels;

namespace Common.Common
{
   public class Util
    {
        public static IHttpClientFactory Http;


        /// <summary>
        /// 上传图片到服务器封装 获取上传到服务器文件的网络路径
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="_environment">根目录</param>
        /// <param name="request">根目录对应的网络路径</param>
        /// <param name="folderName">文件夹名字</param>
        /// <returns>返回文件的url</returns>
        public static string UploadFileToServer(IFormFile file, string webRootPath, HttpRequest request, string folderName)
        {
            Stream stream = file.OpenReadStream();
            string[] imgTypeList = new[] { "jpg","jpeg", "png", "bmp"
            , "ico"};
            string suffix = Path.GetExtension(file.FileName).Replace(".", "").ToLower();

            byte[] srcBuf = null;
            if (imgTypeList.Contains(suffix))
            {
                srcBuf = CompressionImage(stream, 15);
            }
            else
            {
                srcBuf = new byte[stream.Length];
                stream.Read(srcBuf, 0, srcBuf.Length);
                // 设置当前流的位置为流的开始
                stream.Seek(0, SeekOrigin.Begin);
            }

            var date = DateTime.Today.ToString("yyyy-MM-dd");
            string pathStr = "uploadFile" + Path.DirectorySeparatorChar + folderName + Path.DirectorySeparatorChar + date;
            string dir = webRootPath + Path.DirectorySeparatorChar + pathStr;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string fileName = file.FileName.ToLower();
            string newName = SecurityManage.GuidUpper() + "." + GetFileExtend(fileName);
            string path = $"{dir}{Path.DirectorySeparatorChar}{newName}";
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(srcBuf, 0, srcBuf.Length);

            }

            var baseUrl = GetBaseUrl(request);
            var url = baseUrl + "uploadFile/" + folderName + "/" + date + "/" + newName;
            return url;

        }

        /// <summary>
        /// 用远程地址获取文件流
        /// </summary>
        /// <param name="path">URL</param>
        /// <returns></returns>
        public static byte[] GetUrlMemoryStream(string path)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();

            List<byte> btlst = new List<byte>();
            int b = responseStream.ReadByte();
            while (b > -1)
            {
                btlst.Add((byte)b);
                b = responseStream.ReadByte();
            }
            byte[] bts = btlst.ToArray();
            return bts;
        }

        /// <summary>
        /// 形如 http://xxx.xxx.xxx/abc/
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetBaseUrl(HttpRequest request)
        {
            var host = request.Host;
            string http = "";
            string pathBase = string.IsNullOrWhiteSpace(request.PathBase) ? "" : request.PathBase + Path.DirectorySeparatorChar;
            if (request.IsHttps)
            {
                http = "https://";
            }
            else
            {
                http = "http://";
            }
            var baseUrl = $"{http}{host.Value}/{pathBase}";
            return baseUrl;
        }


        /// <summary>
        /// 删除oss服务器的单个文件
        /// </summary>
        /// <param name="ossFileSetting">oss对象</param>
        /// <param name="folderName">oss服务器的文件全路径</param>
        /// <returns></returns>
        public static void DelAnyFile(OssFileSetting ossFileSetting, string filePath)
        {
            var endpoint = ossFileSetting.Endpoint;
            var accessKeyId = ossFileSetting.AccessKeyId;
            var accessKeySecret = ossFileSetting.AccessKeySecret;
            string bucketName = ossFileSetting.BucketName;
            // 创建OSSClient实例。
            OssClient client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            string objectName = filePath.Replace("http://" + bucketName + "." + endpoint + "/", "");
            // 删除文件。
            client.DeleteObject(bucketName, objectName);
        }

        /// <summary>
        /// 删除oss服务器的多个个文件
        /// </summary>
        /// <param name="ossFileSetting">oss对象</param>
        /// <param name="folderName">oss服务器的文件全路径</param>
        /// <returns></returns>
        public static void DelAnyFileMore(OssFileSetting ossFileSetting, List<string> filePath)
        {
            if (filePath.Count > 0)
            {
                var endpoint = ossFileSetting.Endpoint;
                var accessKeyId = ossFileSetting.AccessKeyId;
                var accessKeySecret = ossFileSetting.AccessKeySecret;
                string bucketName = ossFileSetting.BucketName;
                // 创建OSSClient实例。
                OssClient client = new OssClient(endpoint, accessKeyId, accessKeySecret);
                filePath.ForEach(x =>
                {
                    string objectName = x.Replace("http://" + bucketName + "." + endpoint + "/", "");
                    // 删除文件。
                    client.DeleteObject(bucketName, objectName);
                });
            }
        }




        /// <summary>
        /// 下载并保存
        /// </summary>
        /// <param name="url">网络路径</param>
        /// <param name="savePath">保存本地的文件夹</param>
        public static void FileDownSave(string url, string savePath)
        {
            HttpClient httpClient = null;
            //if (!string.IsNullOrWhiteSpace(url))
            //{
            //    string[] strArry = url.Split('/');
            //    savePath = savePath + "/" + strArry[strArry.Length - 1];
            //}
            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }
            var t = httpClient.GetByteArrayAsync(url);
            t.Wait();
            Stream responseStream = new MemoryStream(t.Result);
            Stream stream = new FileStream(savePath, FileMode.Create);
            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, bArr.Length);
            }
            stream.Close();
            responseStream.Close();
        }

        /// <summary>
        /// 删除图片 释放服务器资源
        /// </summary>
        /// <param name="filePath">文件路径+文件名</param>
        /// <param name="request">请求</param>
        /// <param name="_environment"></param>
        //public static void DeleteFile(string filePath, HttpRequest request, IWebHostEnvironment _environment)
        //{
        //    var baseUrl = GetBaseUrl(request);
        //    string path = filePath.Replace(baseUrl, _environment.WebRootPath + "\\").Replace("/", "\\"); //网络路径转物理路径
        //    if (File.Exists(path))
        //    {
        //        File.Delete(path);
        //    }
        //}

        ///// <summary>
        ///// 上传文件封装
        ///// </summary>
        ///// <param name="_ossFileSetting">oss参数对象</param>
        ///// <param name="file">文件</param>
        ///// <param name="folderName">存储的文件夹名字</param>
        ///// <returns>返回完整的文件路径</returns>
        //public static string UploadAnyFile(OssFileSetting _ossFileSetting, IFormFile file, string folderName)
        //{
        //    var url = GetUploadOtherFilePath(_ossFileSetting, file, folderName);
        //    return url;
        //}


        /// <summary>
        /// 获取上传图片的新名称 此方法不再使用
        /// 请使用 GetUploadOtherFilePath
        /// </summary>
        /// <param name="file"></param>
        /// <param name="_environment"></param>
        /// <param name="request"></param>
        /// <param name="imgName"></param>
        /// <returns></returns>
        //[Obsolete]
        //public static string GetUploadFilePath(IFormFile file, IWebHostEnvironment _environment, HttpRequest request, string imgName)
        //{
        //    Stream stream = file.OpenReadStream();
        //    //byte[] srcBuf = new byte[stream.Length];
        //    //stream.Read(srcBuf, 0, srcBuf.Length);
        //    //stream.Seek(0, SeekOrigin.Begin);
        //    byte[] srcBuf = CompressionImage(stream, 15);

        //    //string imgName = "EntCode";
        //    string dir = _environment.WebRootPath + Path.DirectorySeparatorChar + imgName;
        //    if (!Directory.Exists(dir))
        //    {
        //        Directory.CreateDirectory(dir);
        //    }
        //    string fileName = file.FileName;
        //    string newName = SecurityManage.GuidUpper() + "." + GetFileExtend(fileName);
        //    string path = $"{dir}{Path.DirectorySeparatorChar}{newName}";
        //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
        //    {
        //        fs.Write(srcBuf, 0, srcBuf.Length);
        //    }
        //    return newName;
        //}



        /// <summary>
        /// 上传文件-返回文件的网络路径
        /// </summary>
        /// <param name="ossFileSetting">oss对象</param>
        /// <param name="file">文件</param>
        /// <param name="folderName">文件存储的文件夹名字</param>
        /// <returns></returns>
        public static string UploadAnyFile(OssFileSetting ossFileSetting, IFormFile file, string folderName)
        {
            //获取文件后缀
            string houzhui = Path.GetExtension(file.FileName);
            string newName = SecurityManage.GuidUpper() + houzhui;

            var endpoint = ossFileSetting.Endpoint;
            var accessKeyId = ossFileSetting.AccessKeyId;
            var accessKeySecret = ossFileSetting.AccessKeySecret;
            var bucketName = ossFileSetting.BucketName;
            var date = DateTime.Today.ToString("yyyy-MM-dd");
            var objectName = "uploadFile/" + folderName + "/" + date + "/" + newName;
            var localFilename = file.FileName;

            // 创建OSSClient实例。
            var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            // 判断空间是否存在，不存在，创建一个空间
            var exist = client.DoesBucketExist(bucketName);
            if (!exist)
            {
                client.CreateBucket(bucketName);
            }
            // 上传文件。
            var result = client.PutObject(bucketName, objectName, file.OpenReadStream());
            return "http://" + bucketName + "." + endpoint + "/" + objectName;
        }


        //public static string UploadVideo(OssFileSetting ossFileSetting, IFormFile file, string ext, string filename)
        //{

        //    //获取文件名后缀
        //    string newName = SecurityManage.GuidUpper() + ext;

        //    string date = DateTime.Today.ToString("yyyy-MM-dd");
        //    //创建oss实例
        //    var client = new OssClient(ossFileSetting.Endpoint, ossFileSetting.AccessKeyId, ossFileSetting.AccessKeySecret);
        //    //初始化分片上传
        //    var uploadId = "";
        //    var objectName = "uploadFile/" + filename + "/" + date + "/" + newName;
        //    var localFilename = file.FileName;
        //    try
        //    {
        //        //定义上传的文件及所属存储空间的名称。您可以在InitiateMultipartUploadRequest中设置ObjectMeta，但不必指定其中的ContentLength。
        //        var request = new InitiateMultipartUploadRequest(ossFileSetting.BucketName, objectName);
        //        var result = client.InitiateMultipartUpload(request);
        //        uploadId = result.UploadId;
        //        //打印UploadId
        //        Console.WriteLine(result.UploadId);

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Init multi part upload failed, {0}", ex.Message);

        //    }
        //    var partSize = 100 * 1024;
        //    var fi = new FileInfo(localFilename);
        //    var fileSize = fi.Length;
        //    var partCount = fileSize / partSize;
        //    if (fileSize % partSize != 0)
        //    {
        //        partCount++;
        //    }
        //    // 开始分片上传。PartETags是保存PartETag的列表，OSS收到用户提交的分片列表后，会逐一验证每个分片数据的有效性。当所有的数据分片通过验证后，OSS会将这些分片组合成一个完整的文件。
        //    var partETags = new List<PartETag>();
        //    try
        //    {
        //        using (var fs = File.Open(localFilename, FileMode.Open))
        //        {
        //            for (var i = 0; i < partCount; i++)
        //            {
        //                var skipBytes = (long)partSize * 1;
        //                // 定位到本次上传的起始位置。
        //                fs.Seek(skipBytes, 0);
        //                // 计算本次上传的分片大小，最后一片为剩余的数据大小。
        //                var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
        //                var request = new UploadPartRequest(ossFileSetting.BucketName, objectName, uploadId)
        //                {
        //                    InputStream=fs,
        //                    PartSize=size,
        //                    PartNumber=i+1
        //                };

        //                var result = client.UploadPart(request);
        //                partETags.Add(result.PartETag);
        //                Console.WriteLine("finish {0}/{1}", partETags.Count, partCount);
        //            }
        //            Console.WriteLine("Put multi part upload succeeded");
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine("Put multi part upload failed, {0}", ex.Message);
        //    }
        //    // 完成分片上传。
        //    try
        //    {
        //        var completeMultipartUploadRequest = new CompleteMultipartUploadRequest(ossFileSetting.BucketName, objectName, uploadId);
        //        foreach (var partETag in partETags)
        //        {
        //            completeMultipartUploadRequest.PartETags.Add(partETag);
        //        }
        //        var result = client.CompleteMultipartUpload(completeMultipartUploadRequest);
        //        Console.WriteLine("complete multi part succeeded");
        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine("complete multi part failed, {0}", ex.Message);
        //    }
        //}



        /// <summary>
        /// 上传文件到oss
        /// </summary>
        /// <param name="ossFileSetting"></param>
        /// <param name="bytes">字节</param>
        /// <param name="folderName">文件夹名</param>
        /// <param name="ext">.jpg</param>
        /// <returns></returns>
        public static string UploadAnyFile(OssFileSetting ossFileSetting, byte[] bytes, string folderName, string ext)
        {
            //获取文件后缀
            //string houzhui = Path.GetExtension(file.FileName);
            string newName = SecurityManage.GuidUpper() + ext;

            var endpoint = ossFileSetting.Endpoint;
            var accessKeyId = ossFileSetting.AccessKeyId;
            var accessKeySecret = ossFileSetting.AccessKeySecret;
            var bucketName = ossFileSetting.BucketName;
            var date = DateTime.Today.ToString("yyyy-MM-dd");
            var objectName = "uploadFile/" + folderName + "/" + date + "/" + newName;
            //var localFilename = file.FileName;

            // 创建OSSClient实例。
            var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            // 判断空间是否存在，不存在，创建一个空间
            var exist = client.DoesBucketExist(bucketName);
            if (!exist)
            {
                client.CreateBucket(bucketName);
            }
            // 上传文件。
            using (Stream stream = new MemoryStream(bytes))
            {
                var result = client.PutObject(bucketName, objectName, stream);
            }
            return "http://" + bucketName + "." + endpoint + "/" + objectName;
        }











        /// <summary>
        /// 
        /// </summary>
        /// <param name="ossFileSetting"></param>
        /// <param name="projectInfo"></param>
        /// <param name="folderName"></param>
        /// <param name="files"></param>
        /// <param name="positions"></param>
        /// <param name="suffx"></param>
        /// <returns></returns>
        public static string UploadAnyBigFiles(OssFileSetting ossFileSetting, string projectInfo, string folderName, Stream files, long positions, string suffx)
        {



            var client = new OssClient(ossFileSetting.Endpoint, ossFileSetting.AccessKeyId, ossFileSetting.AccessKeySecret);

            var bucketName = ossFileSetting.BucketName;
            var objectName = "uploadFile/" + folderName + "/" + projectInfo + "." + suffx;
            if (positions == 0)
            {
                client.DeleteObject(bucketName, objectName);
            }
            try
            {
                // 获取追加位置，再次追加文件。
                var request1 = new AppendObjectRequest(bucketName, objectName)
                {
                    ObjectMetadata = new ObjectMetadata(),
                    Content = files,
                    Position = positions// 设置文件的追加位置。
                };
                var result1 = client.AppendObject(request1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Append object failed, {0}", ex.Message);
            }
            return "http://" + bucketName + "." + ossFileSetting.Endpoint + "/" + objectName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectInfo">项目信息</param>
        /// <param name="folderName">文件夹名</param>
        /// <param name="request">request请求</param>
        /// <param name="files">文件流</param>
        /// <param name="positions">项目</param>
        /// <param name="suffx">后缀</param>
        /// <returns></returns>
        public static string UploadAnyFiles(string projectInfo, string folderName, string environment, HttpRequest request, Stream files, long positions, string suffix)
        {
            byte[] srcBuf = null;
            srcBuf = new byte[files.Length];
            files.Read(srcBuf, 0, srcBuf.Length);
            // 设置当前流的位置为流的开始
            //files.Seek(positions, SeekOrigin.Begin);
            var date = DateTime.Today.ToString("yyyy-MM-dd");
            //获取网络路径
            var objectName = "uploadFile" + Path.DirectorySeparatorChar + folderName + Path.DirectorySeparatorChar + date;
            var baseUrl = GetBaseUrl(request);
            var dir = environment + Path.DirectorySeparatorChar + objectName;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var fileName = projectInfo.ToLower();
            var newName = GetFileExtend(fileName) + "." + suffix;
            var path = $"{dir}{Path.DirectorySeparatorChar}{newName}";
            var urldir = "uploadFile/" + folderName + "/" + date;
            if (positions == 0)
            {
                DirectoryInfo di = new DirectoryInfo(urldir);
                var deletefile = "wwwroot/uploadFile/" + folderName + "/" + date + "/" + newName;
                if (di is DirectoryInfo)            //判断是否文件夹
                {
                    if (File.Exists(deletefile))
                    {
                        File.Delete(deletefile);
                    }
                }
            }
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                fs.Position = positions;
                fs.Write(srcBuf, 0, srcBuf.Length);
            }
            var url = baseUrl + "uploadFile/" + folderName + "/" + date + "/" + newName;
            return url;
        }

        //public static  string UploadBigFileToServer(IFormFile file, IWebHostEnvironment _environment, HttpRequest request, string folderName,string total,string fileName)
        //{
        //    try
        //    {
        //        string lastModified = Request.Form["lastModified"].ToString();

        //        var index = Request.Form["index"];

        //        string temporary = Path.Combine($"{Directory.GetCurrentDirectory()}/wwwroot/", lastModified);//临时保存分块的目录

        //            if (!Directory.Exists(temporary))
        //                Directory.CreateDirectory(temporary);
        //            string filePath = Path.Combine(temporary, index.ToString());
        //            if (!Convert.IsDBNull(file))
        //            {
        //                 Task.Run(() => {
        //                    FileStream fs = new FileStream(filePath, FileMode.Create);
        //                     file.CopyTo(fs);
        //                });
        //            }
        //            bool mergeOk = false;
        //            if (total == index)
        //            {
        //                mergeOk = FileMerge(lastModified, fileName);
        //            }

        //            Dictionary<string, object> result = new Dictionary<string, object>();
        //            result.Add("number", index);
        //            result.Add("mergeOk", mergeOk);
        //            return JsonConvert.SerializeObject(result);

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //public async Task<bool> FileMerge(string lastModified, string fileName)
        //{
        //    bool ok = false;
        //    try
        //    {
        //        var temporary = Path.Combine($"{Directory.GetCurrentDirectory()}/wwwroot/", lastModified);//临时文件夹
        //        fileName = Request.Form["fileName"];//文件名
        //        string fileExt = Path.GetExtension(fileName);//获取文件后缀
        //        var files = Directory.GetFiles(temporary);//获得下面的所有文件
        //        var finalPath = Path.Combine($"{Directory.GetCurrentDirectory()}/wwwroot/", DateTime.Now.ToString("yyMMddHHmmss") + fileExt);//最终的文件名（demo中保存的是它上传时候的文件名，实际操作肯定不能这样）
        //        var fs = new FileStream(finalPath, FileMode.Create);
        //        foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
        //        {
        //            var bytes = System.IO.File.ReadAllBytes(part);
        //            await fs.WriteAsync(bytes, 0, bytes.Length);
        //            bytes = null;
        //            System.IO.File.Delete(part);//删除分块
        //        }
        //        fs.Close();
        //        Directory.Delete(temporary);//删除文件夹
        //        ok = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return ok;

        /// <summary>
        /// 获取文件后缀名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileExtend(string fileName)
        {

            return fileName.Substring(fileName.LastIndexOf(".") + 1, (fileName.Length - fileName.LastIndexOf(".") - 1)).ToLower();
        }



        /// <summary>
        ///css- 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static string GetTime(string timeStamp)
        {
            //处理字符串,截取括号内的数字
            var strStamp = Regex.Matches(timeStamp, @"(?<=\()((?<gp>\()|(?<-gp>\))|[^()]+)*(?(gp)(?!))").Cast<Match>().Select(t => t.Value).ToArray()[0].ToString();
            //处理字符串获取+号前面的数字
            var str = Convert.ToInt64(strStamp.Substring(0, strStamp.IndexOf("+")));
            long timeTricks = new DateTime(1970, 1, 1).Ticks + str * 10000 + TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now, TimeZoneInfo.Local).Hour * 3600 * (long)10000000;
            return new DateTime(timeTricks).ToString("yyyy-MM-dd HH:mm:ss");

        }

        /// <summary>
        /// css- DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {

            System.DateTime startTime = new System.DateTime(1970, 1, 1);

            return (int)(time - startTime).TotalSeconds;

        }


        public class MyViewModel
        {
            public string Username { get; set; }
        }

        //private static Encoding GetEncoding(MultipartSection section)
        //{
        //    MediaTypeHeaderValue mediaType;
        //    var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out mediaType);
        //    // UTF-7 is insecure and should not be honored. UTF-8 will succeed in 
        //    // most cases.
        //    if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
        //    {
        //        return Encoding.UTF8;
        //    }
        //    return mediaType.Encoding;
        //}
        /// <summary>
        /// 上传大文件，先存到临时文件夹，最后再存到指定的目录下
        /// 此方法暂未处理成上传到OSS服务，所以此处暂时标记为弃用的
        /// </summary>
        /// <returns>http链接</returns>
        /// 
        //public static async Task<string> UploadBigFile(HttpRequest Request, FormOptions _defaultFormOptions, IWebHostEnvironment _environment, string imgName)
        //{
        //    if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
        //    {
        //        throw new Exception($"Expected a multipart request, but got {Request.ContentType}");//BadRequest();
        //    }
        //    var formAccumulator = new KeyValueAccumulator();
        //    string targetFilePath = null;


        //    var boundary = MultipartRequestHelper.GetBoundary(
        //MediaTypeHeaderValue.Parse(Request.ContentType),
        //_defaultFormOptions.MultipartBoundaryLengthLimit);
        //    var reader = new MultipartReader(boundary, Request.Body);

        //    var section = await reader.ReadNextSectionAsync();

        //    string fileName = null;

        //    while (section != null)
        //    {
        //        ContentDispositionHeaderValue contentDisposition;
        //        var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);

        //        if (hasContentDispositionHeader)
        //        {
        //            fileName = contentDisposition.FileName.Value;

        //            if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
        //            {
        //                targetFilePath = Path.GetTempFileName();
        //                using (var targetStream = System.IO.File.Create(targetFilePath))
        //                {
        //                    await section.Body.CopyToAsync(targetStream);

        //                    //_logger.LogInformation($"Copied the uploaded file '{targetFilePath}'");
        //                }
        //            }
        //            else if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
        //            {
        //                // Content-Disposition: form-data; name="key"
        //                //
        //                // value

        //                // Do not limit the key name length here because the 
        //                // multipart headers length limit is already in effect.
        //                var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name);
        //                var encoding = GetEncoding(section);
        //                using (var streamReader = new StreamReader(
        //                    section.Body,
        //                    encoding,
        //                    detectEncodingFromByteOrderMarks: true,
        //                    bufferSize: 1024,
        //                    leaveOpen: true))
        //                {
        //                    // The value length limit is enforced by MultipartBodyLengthLimit
        //                    var value = await streamReader.ReadToEndAsync();
        //                    if (String.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
        //                    {
        //                        value = String.Empty;
        //                    }
        //                    formAccumulator.Append(key.Value, value);

        //                    if (formAccumulator.ValueCount > _defaultFormOptions.ValueCountLimit)
        //                    {
        //                        throw new InvalidDataException($"Form key count limit {_defaultFormOptions.ValueCountLimit} exceeded.");
        //                    }
        //                }
        //            }
        //        }

        //        // Drains any remaining section body that has not been consumed and
        //        // reads the headers for the next section.
        //        section = await reader.ReadNextSectionAsync();
        //    }


        //    var target = _environment.WebRootPath + Path.DirectorySeparatorChar + imgName;
        //    if (!Directory.Exists(target))
        //    {
        //        Directory.CreateDirectory(target);
        //    }

        //    System.IO.FileInfo file = new FileInfo(targetFilePath);
        //    string newName = SecurityManage.GuidUpper() + "." + GetFileExtend(fileName ?? "");
        //    string newPath = target + Path.DirectorySeparatorChar + newName;
        //    file.MoveTo(newPath);

        //    var baseUrl = GetBaseUrl(Request);
        //    var url = baseUrl + imgName + "/" + newName;
        //    return url;

        //}

        /// <summary>
        /// Author:fuchi
        /// Date:2018/10/19
        /// Desc:高低位转换
        /// </summary>
        /// <param name="strValue">转换值</param>
        /// <returns>转换后的值</returns>
        public static string ByteConvert(string strValue)
        {
            int intLength = strValue.Length;
            string res = string.Empty;
            for (int i = 0; i < intLength / 2; i++)
            {
                res += strValue.Substring(intLength - 2 * (i + 1), 2);
            }
            return res;
        }


        private static byte[] CompressionImage(Stream fileStream, long quality)
        {
            using (Image img = Image.FromStream(fileStream))
            {

                using (Bitmap bitmap = new Bitmap(img))
                {
                    ImageCodecInfo CodecInfo = GetEncoder(img.RawFormat);
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, quality);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitmap.Save(ms, CodecInfo, myEncoderParameters);
                        myEncoderParameters.Dispose();
                        myEncoderParameter.Dispose();
                        return ms.ToArray();
                    }
                }
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                { return codec; }
            }
            return null;
        }

        /// <summary>
        /// css-将List转换成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static System.Data.DataTable ListToDataTable(IList list)
        {
            System.Data.DataTable result = new System.Data.DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    //获取类型
                    Type colType = pi.PropertyType;
                    //当类型为Nullable<>时
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    result.Columns.Add(pi.Name, colType);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取检查类型 根据type
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetCheckTypeDisplay(int? value)
        {
            if (value == 1)
            {
                return "其它";
            }
            else if (value == 2)
            {
                return "限期整改";
            }
            else if (value == 3)
            {
                return "需要停工";
            }
            return "其它";
        }

        /// <summary>
        /// 获取检查单编号
        /// </summary>
        /// <param name="recordNumber"></param>
        /// <returns></returns>
        public static string GetCheckFormNumber(string recordNumber)
        {
            if (string.IsNullOrWhiteSpace(recordNumber))
            {
                throw new ArgumentNullException(nameof(recordNumber));
            }
            return DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss") + "_" + recordNumber;
        }

        /// <summary>
        /// 获取通知书编号
        /// </summary>
        /// <returns></returns>
        public static string GetTzsNo()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// 生成word，并返回路径
        /// </summary>
        /// <param name="contentDic"></param>
        /// <param name="table"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public static string BuildWord(OssFileSetting ossFileSetting, Dictionary<string, string> contentDic, List<List<string>> table, string template, string folderName, string fileName = null)
        {
            Aspose.Words.Document doc = new Aspose.Words.Document(template);
            DocumentBuilder builder = new DocumentBuilder(doc);

            foreach (var item in contentDic.Keys)
            {
                if (doc.Range.Bookmarks[item] != null && contentDic[item] != null)
                {
                    Bookmark mark = doc.Range.Bookmarks[item];
                    mark.Text = contentDic[item] ?? "";
                }
            }

            if (doc.Range.Bookmarks["PO_regTable0"] != null)
            {
                builder.MoveToBookmark("PO_regTable0");// 定位到书签去
                var strTitleWidth = new int[] { 59, 87, 120, 115, 59, 59 };
                for (var m = 0; m < table.Count; m++)
                {
                    for (var n = 0; n < table[m].Count; n++)
                    {
                        builder.InsertCell();// 添加一个单元格
                        builder.CellFormat.Width = strTitleWidth[n];
                        builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                        builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                        builder.CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.None;
                        builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;//垂直居中对齐
                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;//水平居中对齐
                        if (table[m][n] != null)
                        {
                            builder.Write(table[m][n]);
                        }
                        else
                        {
                            builder.Write("");
                        }


                    }
                    builder.EndRow();
                    if (m == 0)
                    {
                        doc.Range.Bookmarks["PO_regTable0"].Text = "";    // 清掉标示
                    }

                }
            }

            MemoryStream outStream = new MemoryStream();
            doc.Save(outStream, SaveFormat.Pdf);
            byte[] docBytes = outStream.ToArray();
            Stream inStream = new MemoryStream(docBytes);
            var endpoint = ossFileSetting.Endpoint;
            var accessKeyId = ossFileSetting.AccessKeyId;
            var accessKeySecret = ossFileSetting.AccessKeySecret;
            var bucketName = ossFileSetting.BucketName;
            var date = DateTime.Today.ToString("yyyy-MM-dd");
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = SecurityManage.GuidUpper() + ".pdf";
            }
            var objectName = "uploadFile/" + folderName + "/" + date + "/" + fileName;
            var localFilename = fileName;

            // 创建OSSClient实例。
            var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            // 判断空间是否存在，不存在，创建一个空间
            var exist = client.DoesBucketExist(bucketName);
            if (!exist)
            {
                client.CreateBucket(bucketName);
            }
            // 上传文件。
            var result = client.PutObject(bucketName, objectName, inStream);
            return "http://" + bucketName + "." + endpoint + "/" + objectName;

            //doc.Save(buildSrc, SaveFormat.Pdf);
        }


        /// <summary>
        /// 推送绑定信息
        /// </summary>
        /// <param name="clientFactory"></param>
        /// <param name="viewModel"></param>
        /// <param name="aPIUrl"></param>
        public static async Task<BindReturnViewModel> SendBind(IHttpClientFactory clientFactory, BindViewModel viewModel, string aPIUrl)
        {
            try
            {
                var client = clientFactory.CreateClient();
                var content = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var url = aPIUrl;
                if (url.EndsWith("/"))
                {
                    url += "Bind";
                }
                else
                {
                    url += "/Bind";
                }
                var response = await client.PostAsync(url, content);
                var temp = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BindReturnViewModel>(temp);
                return result;
            }
            catch (Exception ex)
            {
                BindReturnViewModel model = new BindReturnViewModel()
                {
                    code = 300,
                    message = ex.Message
                };
                return model;
            }
        }

        public static DateTime GetHourTime(int hour)
        {
            return Convert.ToDateTime(DateTime.Now.ToString($"yyyy-MM-dd {hour}:00:00"));
        }
        /// <summary>
        /// 海康Ehome对接
        /// </summary>
        /// <param name="url"></param>
        /// <param name="datajson"></param>
        /// <returns></returns>
        public async Task<string> HttpClientPost(string url, object datajson)
        {
            HttpClient httpClient = new HttpClient();//http对象
            //表头参数
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            //转为链接需要的格式
            HttpContent httpContent = new JsonContent(datajson);
            //请求
            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;
            if (response.IsSuccessStatusCode)
            {
                Task<string> t = response.Content.ReadAsStringAsync();
                if (t != null)
                {
                    return t.Result;
                }
            }
            return "";
        }
        public class JsonContent : StringContent
        {
            public JsonContent(object obj) :
               base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
            { }
        }
        /// <summary>
        ///  指定Post地址使用Get 方式获取全部字符串
        /// </summary>
        /// <param name="url">请求后台地址</param>
        /// <param name="dic">参数</param>
        /// <param name="timeoutSeconds">延迟</param>
        /// <param name="contentType">类型</param>
        /// <returns></returns>
        public static async Task<string> Post(string url, object dic, int timeoutSeconds = 15, string contentType = "application/x-www-form-urlencoded", Dictionary<string, string> dicheader = null, string type = "POST")
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = type;
            req.ContentType = contentType;
            req.Timeout = timeoutSeconds * 1000;
            #region 添加Post 参数
            StringBuilder builder = new StringBuilder();
            if (contentType == "application/x-www-form-urlencoded")
            {
                int i = 0;
                foreach (var item in dic as Dictionary<string, string>)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
            }
            else if (contentType == "application/json")
            {
                builder.Append(JsonConvert.SerializeObject(dic));
            }
            if (dicheader != null)
            {
                foreach (var item in dicheader)
                {
                    req.Headers.Add(item.Key, item.Value);
                }
            }
            byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = await req.GetRequestStreamAsync())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
            WebResponse resp = await req.GetResponseAsync();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = await reader.ReadToEndAsync();
            }
            return result;
        }

        public static async Task<string> HttpGet(string Url, string postDataStr, Dictionary<string, string> dic = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            if (dic != null)
            {
                foreach (var item in dic)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = await myStreamReader.ReadToEndAsync();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        //public static async Task<string> ConfigHatRelay(HttpRequest request,safety_hat_dbContext dbContext,HatStocks hatStocks)
        //{
        //    try
        //    {
        //        var data =await dbContext.HatStocks.Where(w => w.DeviceId == hatStocks.DeviceId)
        //            .OrderByDescending(o => o.Id)
        //            .FirstOrDefaultAsync();
        //        var now = DateTime.Now;
        //        if (data == null)
        //        {
        //            hatStocks.CreateDate = now;
        //            hatStocks.UseUnit = "南京傲途软件公司";
        //            hatStocks.IsRelay = true;
        //            hatStocks.HatState = "转发入库";
        //            hatStocks.City = "南京";
        //            hatStocks.DeleteMark = 0;
        //            hatStocks.RelayUrl = GetBaseUrl(request) + "api/SmartHat/ReceiveHatRangeData";
        //            await dbContext.HatStocks.AddAsync(hatStocks);
        //            await dbContext.SaveChangesAsync();
        //            return "";
        //        }
        //        else
        //        {
        //            return "配置安全帽数据转发错误";
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.Error("配置安全帽数据转发错误：", ex);
        //        return "配置安全帽数据转发错误";
        //    }
        //}


        public static string Get(string uri)
        {
            //Web访问对象64
            string serviceUrl = uri;// string.Format("{0}/{1}", this.BaseUri, uri);

            //构造一个Web请求的对象
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
            // 获得接口返回值68
            //获取web请求的响应的内容
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();

            //通过响应流构造一个StreamReader
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            //string ReturnXml = HttpUtility.UrlDecode(reader.ReadToEnd());
            string ReturnXml = reader.ReadToEnd();
            reader.Close();
            myResponse.Close();
            return ReturnXml;
        }

        /// <summary>
        /// 根据base64去获取图片
        /// </summary>
        /// <param name="base64Str"></param>
        /// <param name="imgName"></param>
        /// <returns></returns>
        public static string Base64ToImage(string base64Str,string imgName)
        {
            try
            {
                string filename = "";//声明一个string类型的相对路径
                                     //取图片的后缀格式
                string suffix = "";
                if (base64Str.Contains("data:image"))
                {
                    suffix = base64Str.Split(',')[0].Split(';')[0].Split('/')[1]?.ToLower();
                }
               
                string base64 = "";
                if (!string.IsNullOrEmpty(suffix))
                {
                    if (!(suffix == "png" || suffix == "jpg" || suffix == "jpeg"))
                    {
                        return "";
                    }
                    base64= base64Str.Split(',')[1];
                }
                else
                {
                    base64 = base64Str;
                    suffix = "jpg";
                }
                byte[] imageBytes = Convert.FromBase64String(base64);
                //读入MemoryStream对象
                MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                memoryStream.Write(imageBytes, 0, imageBytes.Length);
                //filename = path + imgName + "." + hz;//所要保存的相对路径及名字
                string tmpRootDir = Directory.GetCurrentDirectory() + "//wwwroot//ImgPhoto"; //获取程序根目录 
                if (!Directory.Exists(tmpRootDir))
                {
                    Directory.CreateDirectory(tmpRootDir);
                }
                string imgUrl = tmpRootDir + "//" + imgName + "." + suffix; //转换成绝对路径 
                filename = "/ImgPhoto/"+ imgName + "." + suffix;
                //  转成图片
                Image image = Image.FromStream(memoryStream);

                image.Save(imgUrl);
                return filename;
            }
            catch (Exception ex)
            {
                return "";
            }
           
        }

        //    /// <summary>
        //    /// 获得国控点数据信息
        //    /// </summary>
        //    /// <param name="url"></param>
        //    public static List<CriterionDustHistory> GetGKDustInfo(string url)
        //{
        //    List<CriterionDustHistory> list = new List<CriterionDustHistory>();
        //    try
        //    {
        //        //判断是否存在 然后取出国控点数据
        //        if (RedisHelper.Exists("GKDUST"))
        //        {
        //            list = RedisHelper.Get<List<CriterionDustHistory>>("GKDUST");

        //        }
        //        else
        //        {
        //            //获得国控点数据
        //            string aesjson = Get(url);
        //            //解密
        //            string daesjson = DecryptByAES(aesjson, "9b367665-ec48-47");
        //            DustReal dustReal = JsonConvert.DeserializeObject<DustReal>(daesjson);
        //            DateTime now = DateTime.Now;
        //            foreach (var item in dustReal.data)
        //            {
        //                list.Add(
        //               new CriterionDustHistory()
        //               {
        //                   CreateDate = now,
        //                   CriterionDustHistoryKey = Guid.NewGuid().ToString("N").ToUpper(),
        //                   Aqi = item.aqi,
        //                   AqiType = item.aqidj,
        //                   No2 = item.no2,
        //                   NO2Aqi = item.no2_aqi,
        //                   No2AqiType = item.no2dj,
        //                   Pm10 = item.pm10,
        //                   Pm10Aqi = item.pm10_aqi,
        //                   Pm10Type = item.pm10dj,
        //                   Pm2dot5 = item.pm25,
        //                   Pm2dot5Aqi = item.pm25_aqi,
        //                   Pm2dot5Type = item.pm25dj,
        //                   AreaCityName = item.xzqmc,
        //                   ParentAreaCode = item.xzqdm,
        //                   AreaName = item.zdmc,
        //                   AreaId = item.id,
        //                   HealthInfluence = item.jkzs,
        //                   Temperature = item.wd + "",
        //                   Longitude = item.jd + "",
        //                   Proposal = item.cxzs,
        //                   Dimension = item.wd + "",
        //                   DeleteMark = 0,
        //                   UploadDate = DateTime.ParseExact(item.sj, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture),
        //               });

        //            }
        //            RedisHelper.Set("GKDUST", list, 60 * 20);
        //        }

        //        return list;
        //    }
        //    catch (Exception ex)
        //    {

        //        return new List<CriterionDustHistory>();
        //    }
        //}
        // <summary> 
        /// AES解密 
        /// </summary> 
        /// <param name="input">密文字节数组</param> 
        /// <param name="key">密钥（16位）</param> 
        /// <returns>返回解密后的字符串</returns> 
        public static string DecryptByAES(string input, string key)
        {
            byte[] inputBytes = Convert.FromBase64String(input);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 16));

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Padding = PaddingMode.Zeros;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Key = keyBytes;
                aesAlg.IV = keyBytes;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream(inputBytes))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srEncrypt = new StreamReader(csEncrypt))
                        {
                            return srEncrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }


    public static class Exchange<T, F>
    {

        public static T GetResylt(T temp, F copy)
        {
            var typeF = typeof(F);
            var typeT = typeof(T);
            foreach (PropertyInfo item in typeF.GetProperties())
            {
                var obj = item.GetValue(copy);
                var properT = typeT.GetProperty(item.Name);
                if (properT != null)
                {
                    properT.SetValue(temp, obj);
                }
            }
            return temp;
        }
    }


   
}
