using Microsoft.Extensions.Options;
using PetFamily.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.Interfaces;
using PetFamily.Api.Extensions;
using PetFamily.Application.FileProvider;

namespace PetFamily.Api.Controllers
{
    public class FilesController : ApplicationController
    {
        private const string BUCKET_NAME = "photos";
        private readonly IFilesProvider _filesProvider;

        public FilesController(
            IFilesProvider filesProvider)
        {
            _filesProvider = filesProvider;
        }

        [HttpPost]
        public async Task<ActionResult> UploadFile(
            IFormFile file,
            CancellationToken cancellationToken)
        {
            await using var stream = file.OpenReadStream();

            var path = Guid.NewGuid().ToString();

            var fileData = new FileData(stream, BUCKET_NAME, path);    

            var result = await _filesProvider.UploadFile(fileData, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpDelete("{name:guid}")]
        public async Task<ActionResult> DeleteFile(
            [FromRoute]Guid name, CancellationToken cancellationToken)
        {
            var fileMeta = new FileMeta(BUCKET_NAME, name.ToString());

            var result = await _filesProvider.DeleteFile(fileMeta, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpGet("{name:guid}")]
        public async Task<ActionResult> PresignedGetFile(
            [FromRoute]Guid name, CancellationToken cancellationToken)
        {
            var fileMeta = new FileMeta(BUCKET_NAME, name.ToString());

            var result = await _filesProvider.PresignedGetObject(fileMeta, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}
