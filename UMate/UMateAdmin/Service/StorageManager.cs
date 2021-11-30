using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace UMateAdmin.Service
{
    public class StorageManager
    {
        public static StorageManager Shared = new StorageManager();

        private StorageManager()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appSettings.json");
            var config = builder.Build();
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        public static CloudBlobContainer GetContainer(string containerName)
        {
            IConfiguration config = StorageManager.Shared.Configuration;

            var credentials = new StorageCredentials("umateblob", config.GetConnectionString("StorageAccessKey"));
            var account = new CloudStorageAccount(credentials, true);
            var client = account.CreateCloudBlobClient();

            var container = client.GetContainerReference(containerName);

            return container;
        }


        public async Task<string> UploadFile(CloudBlobContainer container, IFormFile file)
        {
            if (container != null)
            {
                try
                {
                    // 새로운 파일의 이름, 빈 껍데기 파일 만들기
                    // 이름 중복을 피하고, 지원하지 않는 문자 사용을 피하기 위해 랜덤 문자열 + 확장자
                    var fileName = Guid.NewGuid().ToString() + file.FileName.Substring(file.FileName.LastIndexOf("."));
                    var blob = container.GetBlockBlobReference(fileName);

                    // 비어있는 파일에 업로드
                    await blob.UploadFromStreamAsync(file.OpenReadStream());

                    // 업로드된 최종 url 리턴
                    return blob.Uri.AbsoluteUri;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return null;
        }

        public async Task DeleteFile(CloudBlobContainer container, string path)
        {
            if (container != null)
            {
                try
                {
                    // 마지막 slash 다음 문자부터 끝까지
                    var id = path.Substring(path.LastIndexOf("/") + 1);
                    var blob = container.GetBlockBlobReference(id);

                    // 존재한다면 삭제
                    await blob.DeleteIfExistsAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public static implicit operator CloudBlobContainer(StorageManager v)
        {
            throw new NotImplementedException();
        }
    }
}
