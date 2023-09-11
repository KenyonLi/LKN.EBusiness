using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Minio;
using Minio.DataModel;
using Minio.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using YDT.EBusiness.Models;

namespace YDT.EBusiness.Controllers
{
    /// <summary>
    /// 电商首页
    /// </summary>
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private MinioClient _minioClient;
        public ProductController(ILogger<ProductController> logger, IHttpClientFactory httpClientFactory, MinioClient minioClient)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _minioClient = minioClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 商品图片上传
        /// </summary>
        /// <returns></returns>
        public IActionResult FileUpload(IFormFile formFile)
        {
            #region 1、本地上传
            {
                /*// 1.1 遍历所有文件
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        // 1.2 文件上传目的地
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", formFile.FileName);

                        // 1.3 文件上传
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            formFile.CopyToAsync(stream).Wait();
                        }
                        _logger.LogInformation($"文件:{formFile.FileName}上传成功");
                    }
                }*/
            }
            #endregion

            #region 2、MinIO分布式文件系统上传
            {
                // 2.1 创建MinioClient客户端
                MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");

                // 2.2 创建文件桶
                if (!_minioClient.BucketExistsAsync("productpictures").Result)
                {
                    _minioClient.MakeBucketAsync("productpictures").Wait();
                }

                // 2.3 上传文件
                _minioClient.PutObjectAsync("productpictures", formFile.FileName, formFile.OpenReadStream(), formFile.Length).Wait();
                string url = _minioClient.PresignedGetObjectAsync("productpictures", formFile.FileName, 24 * 60 * 60).Result;
                _logger.LogInformation($"文件:{formFile.FileName}上传到MinIO成功");

                return new JsonResult(url);
                  
            }
            #endregion

            #region 3、MinIO分布式文件系统中心上传
            {
                /*// 3.1 遍历所有文件
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        HttpClient httpClient = _httpClientFactory.CreateClient();
                        MultipartFormDataContent content = new MultipartFormDataContent();
                        content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                        content.Add(new StreamContent(formFile.OpenReadStream(), (int)formFile.Length), "file", formFile.FileName);
                        var result = httpClient.PostAsync("http://localhost:5007/File/Upload", content).Result;
                    }
                }*/
            }
            #endregion

            return new JsonResult("success");
        }

        /// <summary>
        /// 商品图片上传连接
        /// </summary>
        /// <returns></returns>
        public IActionResult FileUploadPresigned(string fileName)
        {
            #region 1、本地上传
            {
                /*// 1.1 遍历所有文件
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        // 1.2 文件上传目的地
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", formFile.FileName);

                        // 1.3 文件上传
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            formFile.CopyToAsync(stream).Wait();
                        }
                        _logger.LogInformation($"文件:{formFile.FileName}上传成功");
                    }
                }*/
            }
            #endregion

            #region 2、MinIO分布式文件系统上传
            {
                // 2.1 创建MinioClient客户端
                MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");

                try
                {
                    // 2.2、创建上传连接地址
                    String url = minioClient.PresignedPutObjectAsync("productpictures", fileName, 60 * 60 * 24).Result;
                    Console.Out.WriteLine(url);

                    return new JsonResult(url);
                }
                catch (MinioException e)
                {
                    Console.Out.WriteLine("Error occurred: " + e);
                }

            }
            #endregion
            return new JsonResult("success");
        }


        /// <summary>
        /// 商品图片批量上传
        /// </summary>
        /// <returns></returns>
        public IActionResult FileUploadList(List<IFormFile> files)
        {
            #region 1、本地上传
            {
                /*// 1.1 遍历所有文件
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        // 1.2 文件上传目的地
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", formFile.FileName);

                        // 1.3 文件上传
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            formFile.CopyToAsync(stream).Wait();
                        }
                        _logger.LogInformation($"文件:{formFile.FileName}上传成功");
                    }
                }*/
            }
            #endregion

            #region 2、MinIO分布式文件系统上传
            {
                // 2.1 遍历所有文件
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        // 2.1 创建MinioClient客户端
                        MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");

                        // 2.2 创建文件桶
                        if (!_minioClient.BucketExistsAsync("productpictures").Result)
                        {
                            _minioClient.MakeBucketAsync("productpictures").Wait();
                        }

                        // 2.3 上传文件
                        _minioClient.PutObjectAsync("productpictures", formFile.FileName, formFile.OpenReadStream(), formFile.Length).Wait();
                        string url = _minioClient.PresignedGetObjectAsync("productpictures", formFile.FileName, 24 * 60 * 60).Result;
                        _logger.LogInformation($"文件:{formFile.FileName}上传到MinIO成功");

                        return new JsonResult(url);
                    }
                }
            }
            #endregion

            return new JsonResult("success");
        }

        /// <summary>
        /// 商品图片下载
        /// </summary>
        /// <returns></returns>
        public IActionResult FileDownload(string fileName)
        {
            // 文件流结果对象
            FileStreamResult fileStreamResult = null;
            #region 1、本地下载访问
            {
               /* // 1、上传图片
                var imgStream = new MemoryStream();
                // 1.1、创建文件路径
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", fileName);
                //从图片中读取流

                // 1.2 获取文件流
                var stream = System.IO.File.Create(filePath);
                // 1.3 输出文件流
                stream.CopyTo(imgStream);
                imgStream.Position = 0;
                fileStreamResult = new FileStreamResult(imgStream, "image/jpg");
                return fileStreamResult;*/
            }
            #endregion

            #region 2、MinIO分布式文件系统下载
            {
                try
                {
                    // 2.1、上传图片
                    MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");

                    var imgStream = new MemoryStream();
                    // 2.2、下载图片
                    minioClient.GetObjectAsync("productpictures", fileName, stream => stream.CopyTo(imgStream)).Wait();
                    imgStream.Position = 0;

                    fileStreamResult = new FileStreamResult(imgStream, "image/jpg");
                }
                catch (MinioException e)
                {

                    Console.WriteLine("Error: " + e);
                }
            }
            #endregion

            #region 3、MinIO分布式文件系统下载
            {
                /*// 3.1、下载图片
                HttpClient httpClient = _httpClientFactory.CreateClient();
                HttpContent httpContent = new StringContent(fileName);
                var result = httpClient.PostAsync("http://localhost:5007/File/Download", httpContent).Result;
                // 3.2、下载图片
                fileStreamResult = new FileStreamResult(result.Content.ReadAsStreamAsync().Result, "image/jpg");*/
            }
            #endregion

            return fileStreamResult;
        }

        /// <summary>
        /// 商品图片下载地址
        /// </summary>
        /// <returns></returns>
        public IActionResult FileDownloadPresigned(string fileName)
        {
            #region 2、MinIO分布式文件系统下载
            {
                try
                {
                    // 2.1、创建客户端
                    MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");

                    // 2.2、创建下载文件地址
                    String url = minioClient.PresignedGetObjectAsync("productpictures", fileName, 60 * 60 * 24).Result;
                    Console.Out.WriteLine(url);
                    return new JsonResult(url);
                }
                catch (MinioException e)
                {

                    Console.WriteLine("Error: " + e);
                }

            }
            #endregion
            return new JsonResult("success");

        }
        /// <summary>
        /// 商品图片删除
        /// </summary>
        /// <returns></returns>
        public IActionResult FileDelete(string fileName)
        {

            #region 2、MinIO分布式文件系统下载
            {
                try
                {
                    // 2.1、创建客户端
                    MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");

                    var imgStream = new MemoryStream();
                    // 2.2、单个图片删除
                    minioClient.RemoveObjectAsync("productpictures", fileName).Wait();
                }
                catch (MinioException e)
                {
                    Console.WriteLine("Error: " + e);
                }
               
            }
            #endregion


            return Ok("删除成功");
        }

        /// <summary>
        /// 商品图片批量删除
        /// </summary>
        /// <returns></returns>
        public IActionResult FileDeleteList(string[] fileNames)
        {

            #region 2、MinIO分布式文件系统下载
            {
                try
                {
                    // 2.1、创建客户端
                    MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");

                    var imgStream = new MemoryStream();
                    // 2.2、批量删除
                    minioClient.RemoveObjectAsync("productpictures", fileNames.ToList()).Wait();
                }
                catch (MinioException e)
                {
                    Console.WriteLine("Error: " + e);
                }

            }
            #endregion


            return Ok("删除成功");
        }

        /// <summary>
        /// 商品图片上传监听
        /// </summary>
        /// <returns></returns>
        public IActionResult FileUploadNotification(string[] fileNames)
        {

            #region 2、MinIO分布式文件系统下载
            {
                try
                {
                    // 2.1、创建客户端
                    MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");

                   BucketNotification notification = new BucketNotification();
                    /*Arn topicArn = new Arn("aws", "sns", "us-west-1", "412334153608", "topicminio");

                   TopicConfig topicConfiguration = new TopicConfig(topicArn);
                   List<EventType> events = new List<EventType>() { EventType.ObjectCreatedPut, EventType.ObjectCreatedCopy };
                   topicConfiguration.AddEvents(events);
                   topicConfiguration.AddFilterPrefix("images");
                   topicConfiguration.AddFilterSuffix("jpg");
                   notification.AddTopic(topicConfiguration);*/

                    QueueConfig queueConfiguration = new QueueConfig("arn:minio:sqs::_:mysql");
                    queueConfiguration.AddEvents(new List<EventType>() { EventType.ObjectCreatedAll });
                    notification.AddQueue(queueConfiguration);

                    minioClient.SetBucketNotificationsAsync("productpictures",
                                                        notification);
                    Console.Out.WriteLine("Notifications set for the bucket " + minioClient + " successfully");
                }
                catch (MinioException e)
                {
                    Console.WriteLine("Error: " + e);
                }

            }
            #endregion

            return Ok("删除成功");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
