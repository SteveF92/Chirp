using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.IO;
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

        public void UploadProfilePicture(string a_userId, Stream a_data)
        {
            IAmazonS3 client;
            using (client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1))
            {
                Console.WriteLine("Uploading an object");
                try
                {
                    PutObjectRequest putRequest = new PutObjectRequest
                    {
                        BucketName = Options.Bucket,
                        Key = a_userId,
                        InputStream = a_data,
                        CannedACL = S3CannedACL.PublicRead
                    };

                    PutObjectResponse response = client.PutObject(putRequest);
                }
                catch (AmazonS3Exception amazonS3Exception)
                {
                    if (amazonS3Exception.ErrorCode != null &&
                        (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                        ||
                        amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    {
                        Console.WriteLine("Check the provided AWS Credentials.");
                        Console.WriteLine(
                            "For service sign up go to http://aws.amazon.com/s3");
                    }
                    else
                    {
                        Console.WriteLine(
                            "Error occurred. Message:'{0}' when writing an object"
                            , amazonS3Exception.Message);
                    }
                }
            }
        }
    }
}