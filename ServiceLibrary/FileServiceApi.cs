using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Models;
using System.Threading.Tasks;

namespace Common
{
    public class FileServiceApi : RestApiAbstract
    {
        public IConfiguration Configuration { get; }

        public FileServiceApi(IHttpContextAccessor HttpContextAccessor, IConfiguration configuration) : base(HttpContextAccessor)
        {
            ApiAddress = $"{configuration["FileServiceUrl"]}/";
            Configuration = configuration;
        }

        public async Task<string> FileSave(FileDto File, string OldFileUrl="")
        {
            if(File?.Status == FileDto.StatusEnum.Added)
            {
                var result = await Post<ServiceDto<string>>
                   ("File/InsertSave", new File.AddModel()
                   {
                       File = File,
                       ToDeleteFileUrl = OldFileUrl
                   });

                return result.Data;
            }
            else if (File?.Status == FileDto.StatusEnum.Deleted && string.IsNullOrEmpty(OldFileUrl) == false)
            {
                _ = await Post<ServiceDto>("File/DeleteSave", OldFileUrl);
                return "";
            }
            return OldFileUrl;
        }

        public async Task<ServiceDto> FileRemove(string FileUrl = "")
        {
            if (string.IsNullOrEmpty(FileUrl)) return new ServiceDto();
            return await this.Post<ServiceDto>("File/DeleteSave", FileUrl);
        }


        public string FileUrlGenerate(string FileName = "")
        {
            if (string.IsNullOrEmpty(FileName)) return "";
            return  $"{Configuration["FileServiceUrl"]}/File/Download/{FileName}";
        }
    }
}