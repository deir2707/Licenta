using System.IO;
using Microsoft.AspNetCore.Http;

namespace Domain
{
    public class UploadFile
    {
        public Stream Stream { get; }
        public string Name { get; }
        public string ContentType { get; }

        public string FileTypeName { get; set; }

        public UploadFile(Stream fileStream, IFormFile fileUploadModel)
        {
            Stream = fileStream;
            Name = fileUploadModel.FileName;
            ContentType = fileUploadModel.ContentType;
        }

        public UploadFile(Stream fileStream, string fileName)
        {
            Stream = fileStream;
            Name = fileName;
        }

        public void ResetToBeginning()
        {
            Stream.Position = 0;
        }
    }

    public class ImageFile : UploadFile
    {
        public ImageFile(Stream fileStream, IFormFile fileUploadModel) :
            base(fileStream, fileUploadModel)
        {
        }
    }
}