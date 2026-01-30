using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace fitapp_plodik_MVC.Auto_img;

public static class ImageHelper
{
    public static async Task<string?> SaveImageAsync(IFormFile? file, string subFolder, IWebHostEnvironment env)
    {
        if (file == null || file.Length == 0)
            return null;

        string folder = Path.Combine(env.WebRootPath, $"img/uploads/{subFolder}");
        Directory.CreateDirectory(folder);

        string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        string filePath = Path.Combine(folder, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/img/uploads/{subFolder}/{fileName}";
    }
}
