using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3Services
{
    public interface IProfilePictureService
    {
        void UploadProfilePicture(string a_userId, string a_data);
    }
}
