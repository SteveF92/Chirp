using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace S3Services
{
    public interface IProfilePictureService
    {
        void UploadProfilePicture(string a_userId, Stream a_data);
    }
}
