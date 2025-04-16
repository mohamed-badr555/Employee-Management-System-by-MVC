using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helpers
{
    public class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            //to make it UnIQUE
            string fileName = $"{Guid.NewGuid()}{file.FileName}";
            string filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return fileName;
        }
        public static void DeleteFile(string fileName, string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        public static string UpdateFile(IFormFile file, string folderName, string oldFileName)
        {
            // If no new file is uploaded, keep the old file
            if (file == null || file.Length == 0)
                return oldFileName;

            // Otherwise delete the old file and upload the new one
            DeleteFile(oldFileName, folderName);
            return UploadFile(file, folderName);
        }
    }
    }
