﻿using System;
using System.IO;
using System.Text;

namespace Rokt.Tests
{
    public class BaseTest
    {
        public FileInfo CreateTestFile(string content)
        {
            var emtptyFilePath = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString() + ".txt");
            var fileInfo = new FileInfo(emtptyFilePath);
            using (var fs = File.Create(emtptyFilePath))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(content);
                fs.Write(info, 0, info.Length);
            }
            return fileInfo;
        }
    }
}