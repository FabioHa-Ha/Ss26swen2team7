using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using tourplannerBackend.DTOs;
using tourplannerBackend.Services;

namespace tourplannerBackend.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class ImportExportController : ControllerBase
    {
        private readonly IImportExportService _importExportService;
        
        public ImportExportController(IImportExportService importExportService)
        {
            this._importExportService = importExportService;
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportUserData()
        {
            var userId = GetUserId();
            byte[]? exportFile = await _importExportService.ExportDatabaseForUser(userId);
            if (exportFile == null)
            {
                return NotFound();
            }
            return File(exportFile, "application/zip", "user_data.zip");
        }

        [HttpPost("import")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportUserData([FromForm] ImportUserDto importUserDto)
        {
            var userId = GetUserId();
            await _importExportService.ImportDatabaseForUser(importUserDto.importFile, userId);
            return Ok();
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(claim!);
        }
    }
}
