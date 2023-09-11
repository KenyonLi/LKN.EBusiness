using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.Exceptions;
using System.Text.Unicode;

namespace LKN.EBusiness.Controllers
{
    /// <summary>
    /// 商品图片控制器
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProductFileController : ControllerBase
    {

        private readonly ILogger<ProductFileController> _logger;

        public ProductFileController(ILogger<ProductFileController> logger)
        {
            _logger = logger;
        }

  //      /// <summary>
  //      /// 文件上传
  //      /// </summary>
  //      /// <returns></returns>
  //      [HttpPost("Upload")]
  //      public IActionResult Upload(IFormFile formFile)
  //      {
  //          // 2.1 创建MinioClient客户端
  //          MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");

  //          // 2.2 创建文件桶
  //          if (!minioClient.BucketExistsAsync("product").Result)
  //          {
  //              minioClient.MakeBucketAsync("product").Wait();
  //          }

  //          // 2.3 上传文件
  //          minioClient.PutObjectAsync("product", formFile.FileName, formFile.OpenReadStream(), formFile.Length).Wait();

  //          _logger.LogInformation($"文件:{formFile.FileName}上传到MinIO成功");

  //          return new JsonResult("上传成功");
  //      }

  //      /// <summary>
  //      /// 批量商品上传
  //      /// </summary>
  //      /// <returns></returns>
  //      [HttpPost("UploadList")]
  //      public IActionResult UploadList(IFormFile[] files)
  //      {
  //          // 2.1 遍历所有文件
  //          foreach (var formFile in files)
  //          {
  //              if (formFile.Length > 0)
  //              {
  //                  // 2.1 创建MinioClient客户端
  //                  MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");

  //                  // 2.2 创建文件桶
  //                  if (!minioClient.BucketExistsAsync("product").Result)
  //                  {
  //                      minioClient.MakeBucketAsync("product").Wait();
  //                  }

  //                  // 2.3 上传文件
  //                  minioClient.PutObjectAsync("product", formFile.FileName, formFile.OpenReadStream(), formFile.Length).Wait();

  //                  _logger.LogInformation($"文件:{formFile.FileName}上传到MinIO成功");
  //              }
  //          }

  //          return new JsonResult("上传成功");
  //      }

  //      /// <summary>
  //      /// 商品图片下载
  //      /// </summary>
  //      /// <returns></returns>
  //      [HttpPost("Download")]
  //      [HttpGet("Download")]
  //      public IActionResult Download(string fileName)
  //      {
  //          FileStreamResult fileStreamResult = null;
  //          try
  //          {
  //              // 1、创建MioIO客户端
  //              MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");

  //              var imgStream = new MemoryStream();
  //              // 2、下载图片
  //              minioClient.GetObjectAsync("product", fileName, stream => stream.CopyTo(imgStream)).Wait();
  //              imgStream.Position = 0;

  //              fileStreamResult = new FileStreamResult(imgStream, "image/jpg");

  //          }
  //          catch (MinioException e)
  //          {

  //              Console.WriteLine("Error: " + e);
  //          }

  //          return fileStreamResult;
  //      }

  //      /// <summary>
  //      /// 商品图片删除
  //      /// </summary>
  //      /// <returns></returns>
  //      [HttpDelete]
  //      public IActionResult FileDelete(string fileName)
  //      {
  //          try
  //          {
  //              // 2.1、创建客户端
  //              MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin",
  //"minioadmin");
  //              var imgStream = new MemoryStream();
  //              // 2.2、单个图片删除
  //              minioClient.RemoveObjectAsync("product", fileName).Wait();
  //          }
  //          catch (MinioException e)
  //          {
  //              Console.WriteLine("Error: " + e);
  //          }
  //          return Ok("删除成功");
  //      }

        /// <summary>
        /// 批量商品图片删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("DeleteList")]
        public IActionResult FileDeleteList(string[] fileNames)
        {

            #region 2、MinIO分布式文件系统下载
            {
                try
                {
                    // 2.1、创建客户端
                    //MinioClient minioClient = new MinioClient("127.0.0.1:9000", "minioadmin", "minioadmin");
                    MinioClient minioClient = new MinioClient();
                    minioClient.WithEndpoint("127.0.0.1", 9000);
                    minioClient.WithCredentials("minioadmin", "minioadmin");
                    minioClient.Build();

                    var removeObjectArgs = new RemoveObjectsArgs();
                    removeObjectArgs.WithBucket("product");
                    removeObjectArgs.WithObjects(fileNames.ToList());

                    minioClient.RemoveObjectsAsync(removeObjectArgs).ConfigureAwait(false);

                    //var imgStream = new MemoryStream();
                    // 2.2、批量删除
                   // minioClient.RemoveObjectAsync("product", fileNames.ToList()).Wait();
                }
                catch (MinioException e)
                {
                    Console.WriteLine("Error: " + e);
                }
            }
            #endregion

            return Ok("删除成功");
        }
    }
}
