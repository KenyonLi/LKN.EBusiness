using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Minio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YDT.EBusiness.Index.Models;

namespace YDT.EBusiness.Index.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(List<IFormFile> files)
        {
            // 2.1 遍历所有文件
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // 2.1 创建MinioClient客户端
                    MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");

                    // 2.2 创建文件桶
                    if (!minioClient.BucketExistsAsync("productpictures").Result)
                    {
                        minioClient.MakeBucketAsync("productpictures").Wait();
                    }

                    // 2.3 上传文件
                    minioClient.PutObjectAsync("productpictures", formFile.FileName, formFile.OpenReadStream(), formFile.Length).Wait();

                    string url = minioClient.PresignedGetObjectAsync("productpictures", formFile.FileName, 24 * 60 * 60).Result;
                    _logger.LogInformation($"文件:{formFile.FileName}上传到MinIO成功");

                    return new JsonResult(url);
                }
            }
            return View();
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
