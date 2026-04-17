namespace StockManager.API.Interfaces.CatalogInterfaces
{
    public interface IImageService
    {
        Task<string> UploadImage(Stream file, string name);
    }
}
