namespace Expense_Tracker.Services
{
    public class FileUploadService
    {
        private readonly IWebHostEnvironment _env;

        public FileUploadService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveAsync(IFormFile file, int userId)
        {
            var folder = Path.Combine(
                _env.ContentRootPath, "Uploads", "Receipts", userId.ToString());

            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(folder, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/Uploads/Receipts/{userId}/{fileName}";
        }
    }
}
