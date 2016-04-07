using Amazon.S3;
using Amazon.S3.Model;
using System;
using Microsoft.Extensions.OptionsModel;

namespace S3Services
{
    public class S3ProfilePictureService : IProfilePictureService
    {
        private IAmazonS3 m_client;

        public S3ProfilePictureService(IOptions<S3ProfilePictureServiceOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public S3ProfilePictureServiceOptions Options { get; }  // set only via Secret Manager

        public void UploadProfilePicture(string a_userId, string a_data)
        {
            throw new NotImplementedException();
        }
    }
}