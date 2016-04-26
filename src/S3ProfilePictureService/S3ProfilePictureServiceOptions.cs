using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3Services
{
    public class S3ProfilePictureServiceOptions
    {
        public string AWSProfileName { get; set; }
        public string Bucket { get; set; }
    }
}
