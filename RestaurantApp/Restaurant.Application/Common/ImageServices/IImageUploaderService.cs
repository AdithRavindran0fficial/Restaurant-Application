using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Common.ImageServices
{
    public  interface IImageUploaderService
    {
        Task<string> UploadImageAsync(byte[] imageData, string fileName,string tenantId,string folderName, string contentType);   
    }
}
