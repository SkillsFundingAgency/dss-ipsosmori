using System;
using System.IO;
using System.Net;
using System.Text;

namespace NCS.DSS.IpsosMori.Helpers
{
    public class FtpHelper : IFtpHelper
    {
        private readonly string _ftpUsername = Environment.GetEnvironmentVariable("FtpUsername");
        private readonly string _ftpPassword = Environment.GetEnvironmentVariable("FtpPassword");
        private readonly string _ftpAddress = Environment.GetEnvironmentVariable("FtpAddress");


        public void UploadDataToFtp(string data, string fileName)
        {
            var request = (FtpWebRequest) WebRequest.Create(_ftpAddress + fileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UseBinary = false;
            request.EnableSsl = true;

            request.Credentials = new NetworkCredential(_ftpUsername, _ftpPassword);

            using (var memoryStream = new MemoryStream(Encoding.Default.GetBytes(data), true))
            {
                using (var requestStream = request.GetRequestStream())
                {
                    memoryStream.CopyTo(requestStream);
                    requestStream.Close();
                }
                memoryStream.Close();
            }

        }

    }
}
