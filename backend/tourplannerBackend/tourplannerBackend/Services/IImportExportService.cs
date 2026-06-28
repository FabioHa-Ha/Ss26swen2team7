namespace tourplannerBackend.Services
{
    public interface IImportExportService
    {
        Task<byte[]?> ExportDatabaseForUser(int userId);
        Task<bool> ImportDatabaseForUser(IFormFile zipFile, int userId);
    }
}
