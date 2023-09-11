//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Minio;
//using Minio.Exceptions;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//namespace YDT.FileService.Controllers
//{
//    /// <summary>
//    /// 商品控制器
//    /// </summary>
//    [ApiController]
//    [Route("[controller]")]
//    public class ProductFileController : ControllerBase
//    {

//        private readonly ILogger<ProductFileController> _logger;

//        public ProductFileController(ILogger<ProductFileController> logger)
//        {
//            _logger = logger;
//        }

//        /// <summary>
//        /// 文件上传
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost("Upload")]
//        public IActionResult Upload(IFormFile formFile)
//        {
//            // _logger.LogInformation. 数据分析。
//            // 2.1 创建MinioClient客户端
//            MinioClient minioClient = new MinioClient("127.0.0.1:9001", "minioadmin", "minioadmin");

//            // 2.2 创建文件桶(数据库)
//            if (!minioClient.BucketExistsAsync("product").Result)
//            {
//                minioClient.MakeBucketAsync("product").Wait();
//            }

//            // 2.3 上传文件(最大上传5TB的数据)
//            minioClient.PutObjectAsync("product", formFile.FileName, formFile.OpenReadStream(), formFile.Length).Wait();

//            _logger.LogInformation($"文件:{formFile.FileName}上传到MinIO成功");

//            return new JsonResult("上传成功");
//        }

//        /// <summary>
//        /// 文件上传(文件分片上传)默认支持，5TB大小文件
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost("UploadShard")]
//        public IActionResult UploadShard(IFormFile formFile)
//        {
//            // 2.1 创建MinioClient客户端
//            MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");

//            // 2.2 创建文件桶(数据库)
//            if (!minioClient.BucketExistsAsync("product").Result)
//            {
//                minioClient.MakeBucketAsync("product").Wait();
//            }

//            // 2.3 上传文件(最大上传5TB的数据)
//            minioClient.PutObjectAsync("product", formFile.FileName, formFile.OpenReadStream(),formFile.Length).Wait();
            
//            minioClient.PutObjectAsync("product", formFile.FileName, "D://12312.txt").Wait();

//            _logger.LogInformation($"文件:{formFile.FileName}上传到MinIO成功");

//            return new JsonResult("上传成功");
//        }

//        /// <summary>
//        /// 文件上传(文件分片上传)默认支持，5TB大小文件
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost("UploadBigFile")]
//        public IActionResult UploadBigFile(IFormFile formFile)
//        {
//            // 2.1 创建MinioClient客户端
//            //MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");
//            MinioClient minioClient = new MinioClient("127.0.0.1:9000", "tony", "tonyzjh123");
//            //minioClient.WithSSL();
//            // 2.2 创建文件桶(数据库)
//            if (!minioClient.BucketExistsAsync("product").Result)
//            {
//                minioClient.MakeBucketAsync("product").Wait();
//            }

//            // 2.3 上传文件(最大上传5TB的数据)
//            //minioClient.SetPolicyAsync
//            string url = minioClient.PresignedPutObjectAsync("product", formFile.FileName,24*60*60).Result;

//            _logger.LogInformation($"文件上传路径:{url}上传到MinIO成功");

//            return new JsonResult(url);
//        }

//        /// <summary>
//        /// 文件上传
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost("UploadList")]
//        public IActionResult UploadList(IFormFile[] files)
//        {
//            // 2.1 遍历所有文件
//            foreach (var formFile in files)
//            {
//                if (formFile.Length > 0)
//                {
//                    // 2.1 创建MinioClient客户端
//                    MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");

//                    // 2.2 创建文件桶
//                    if (!minioClient.BucketExistsAsync("product").Result)
//                    {
//                        minioClient.MakeBucketAsync("product").Wait();
//                    }

//                    // 2.3 上传文件
//                    minioClient.PutObjectAsync("product", formFile.FileName, formFile.OpenReadStream(), formFile.Length).Wait();

//                    _logger.LogInformation($"文件:{formFile.FileName}上传到MinIO成功");
//                }
//            }

//            return new JsonResult("上传成功");
//        }

//        /// <summary>
//        /// 商品图片下载
//        /// </summary>
//        /// <returns></returns>
//        [HttpGet("Download")]
//        public IActionResult Download(string fileName)
//        {
//            FileStreamResult fileStreamResult = null;
//            try
//            {
//                // 1、创建MioIO客户端
//                MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");

//                var imgStream = new MemoryStream();
//                // 2、下载图片
//                minioClient.GetObjectAsync("product", fileName, stream => stream.CopyTo(imgStream)).Wait();
//                imgStream.Position = 0;

//                fileStreamResult = new FileStreamResult(imgStream, "image/jpg");
                
//            }
//            catch (MinioException e)
//            {

//                Console.WriteLine("Error: " + e);
//            }

//            return fileStreamResult;
//        }

//        /// <summary>
//        /// 商品图片下载(分段下载)
//        /// </summary>
//        /// <returns></returns>
//        [HttpGet("DownloadShard")]
//        public IActionResult DownloadShard(string fileName)
//        {
//            FileStreamResult fileStreamResult = null;
//            try
//            {
//                // 1、创建MioIO客户端
//                MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");

//                var imgStream = new MemoryStream();
//                // 2、下载图片
//                //minioClient.GetObjectAsync("product", fileName, stream => stream.CopyTo(imgStream)).Wait();
//                minioClient.GetObjectAsync("product", fileName, 0,1000,stream => stream.CopyTo(imgStream)).Wait();
//                minioClient.GetObjectAsync("product", fileName, 1001, 10000, stream => stream.CopyTo(imgStream)).Wait();
//                minioClient.GetObjectAsync("product", fileName, 10001, 100000, stream => stream.CopyTo(imgStream)).Wait();

//                //分片 4%2
//                //文件分片  100000000/3 = 300000
//                // for(3){}
//                // 0 ---- 300000 *1
//                // 300000 ----- 600000 *2
//                // 600000 ----- 100000000*3
//                imgStream.Position = 0;

//                fileStreamResult = new FileStreamResult(imgStream, "image/jpg");

//            }
//            catch (MinioException e)
//            {

//                Console.WriteLine("Error: " + e);
//            }

//            return fileStreamResult;
//        }

//        /// <summary>
//        /// 商品图片删除
//        /// </summary>
//        /// <returns></returns>
//        [HttpDelete]
//        public IActionResult FileDelete(string fileName)
//        {

//            #region 1、商品图片删除
//            {
//                try
//                {
//                    // 2.1、创建客户端
//                    MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");

//                    var imgStream = new MemoryStream();
//                    // 2.2、单个图片删除
//                    minioClient.RemoveObjectAsync("product", fileName).Wait();
//                }
//                catch (MinioException e)
//                {
//                    Console.WriteLine("Error: " + e);
//                }

//            }
//            #endregion

//            return Ok("删除成功");
//        }

//        /// <summary>
//        /// 批量商品图片删除
//        /// </summary>
//        /// <returns></returns>
//        [HttpDelete("DeleteList")]
//        public IActionResult FileDeleteList(string[] fileNames)
//        {
//            #region 2、MinIO分布式文件系统下载
//            {
//                try
//                {
//                    // 2.1、创建客户端
//                    MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");

//                    var imgStream = new MemoryStream();
//                    // 2.2、批量删除
//                    minioClient.RemoveObjectAsync("product", fileNames.ToList()).Wait();
//                }
//                catch (MinioException e)
//                {
//                    Console.WriteLine("Error: " + e);
//                }

//            }
//            #endregion

//            return Ok("删除成功");
//        }

//        /// <summary>
//        /// 商品图片复制
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost("FileCopy")]
//        public IActionResult FileCopy(string fileName, string destFileName)
//        {
//            #region 2、MinIO分布式文件系统下载
//            {
//                try
//                {
//                    // 2.1、创建客户端
//                    MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");

//                    // 2.2、新图片bulk创建
//                    if (!minioClient.BucketExistsAsync("productnew").Result)
//                    {
//                        minioClient.MakeBucketAsync("productnew").Wait();
//                    }

//                    // 2.3、批量复制
//                    minioClient.CopyObjectAsync("product", fileName, "productnew", destFileName).Wait();
//                }
//                catch (MinioException e)
//                {
//                    Console.WriteLine("Error: " + e);
//                }

//            }
//            #endregion

//            return Ok("复制成功");
//        }
//    }
//}
