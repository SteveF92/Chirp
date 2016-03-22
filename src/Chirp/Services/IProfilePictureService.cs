using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public interface IProfilePictureService
    {
        void UploadProfilePicture(string a_userId, byte[] a_data);
    }
}
