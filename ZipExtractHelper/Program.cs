using System;
using System.IO;
using System.IO.Compression;

namespace ZipExtractHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string source, extract;
                source = args[0].Trim();
                extract = args[1].Trim();
                if (File.Exists(source) && Directory.Exists(extract))
                {
                    ZipFile.ExtractToDirectory(source, extract);
                }
                else
                {
                    Console.WriteLine("路径不存在");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("文件已存在");
            }
        }
    }
}
