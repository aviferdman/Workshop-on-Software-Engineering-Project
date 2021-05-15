using System.Net;

using Microsoft.AspNetCore.Mvc;

namespace TradingSystem.WebApi
{
    public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        public ActionResult InternalServerError(object? value = null)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, value);
        }

        protected async System.Threading.Tasks.Task<System.Text.Json.JsonDocument?> ParseJsonFromFile(Microsoft.Extensions.FileProviders.IFileProvider fileProvider, string fileName)
        {
            using System.IO.Stream stream = fileProvider.GetFileInfo(fileName).CreateReadStream();
            byte[] buffer = new byte[stream.Length];
            _ = await stream.ReadAsync(buffer, 0, buffer.Length);
            try
            {
                return System.Text.Json.JsonDocument.Parse(new System.Buffers.ReadOnlySequence<byte>(buffer));
            }
            catch (System.Text.Json.JsonException)
            {
                return null;
            }
        }
    }
}
