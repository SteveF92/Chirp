using Amazon.S3;
using Amazon.S3.Model;
using System;

namespace Chirp.Services
{
    public class S3ProfilePictureService : IProfilePictureService
    {
        private IAmazonS3 m_client;

        public S3ProfilePictureService()
        {

        }

        public void UploadProfilePicture(string a_userId, string a_data)
        {
            using (m_client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1))
            {
                try
                {
                    PutObjectRequest putRequest1 = new PutObjectRequest
                    {
                        BucketName = "chirp-profile-pictures",
                        Key = a_userId,
                        ContentBody = a_data
                    };

                    PutObjectResponse response1 = m_client.PutObject(putRequest1);
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