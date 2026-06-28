using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Cityinfo.API.Controller
{
    [Route("api/files")]
    [ApiController]
    public class FilesCotrollers : ControllerBase
    {

        FileExtensionContentTypeProvider fileExtensionContentTypeProvider;

        public FilesCotrollers(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            this.fileExtensionContentTypeProvider = fileExtensionContentTypeProvider;
        }


        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {

            string pathTofile = "webapiBanner.rar";

            if (!System.IO.File.Exists(pathTofile))
            {
                return NotFound();
            }

            var bytes = System.IO.File.ReadAllBytes(pathTofile);


            if (!fileExtensionContentTypeProvider.TryGetContentType(pathTofile,
                out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return File(bytes, contentType, Path.GetFileName(pathTofile));
        }
       
    }
}
