﻿using System;
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
        private readonly string _folderName = Environment.GetEnvironmentVariable("EnvironmentName");

        public void UploadDataToFtp(string data, string fileName)
        {
            var request = (FtpWebRequest) WebRequest.Create(_ftpAddress + "/" + _folderName + "/" + fileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UseBinary = false;
            request.EnableSsl = true;

            request.Credentials = new NetworkCredential(_ftpUsername, _ftpPassword);

            try
            {

                byte[] bytes = Encoding.UTF8.GetBytes(data);

                var ftpStream = request.GetRequestStream();
                ftpStream.Write(bytes, 0, bytes.Length);
                ftpStream.Close();
            }
            catch (WebException e)
            {
                String status = ((FtpWebResponse)e.Response).StatusDescription;
            }



        }

    }
}
