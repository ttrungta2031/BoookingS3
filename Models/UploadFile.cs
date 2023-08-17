using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoookingS3.Models
{
    public class UploadFile
    {
        public IFormFile File
        {
            get;
            set;
        }
}
}
