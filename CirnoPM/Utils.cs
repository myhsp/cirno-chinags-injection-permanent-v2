using GS.Unitive.Framework.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace CirnoPM
{
    public enum PackageDownloadResult
    {
        Sucess,
        FailedWithBadHash,
        FailedWithBadFile,
        Failed
    }

    public class Utils
    {
        public static PackageDownloadResult Verify(string path, string hash = "", bool verify_hash = false)
        {
            FileInfo info = new FileInfo(path);

            if (!info.Exists)
            {
                return PackageDownloadResult.Failed;
            }

            if (info.Length == 0)
            {
                return PackageDownloadResult.FailedWithBadFile;
            }

            if (verify_hash && !string.IsNullOrEmpty(hash))
            {
                byte[] fileHash;
                try
                {
                    using (var md5 = MD5.Create())
                    {
                        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            fileHash = md5.ComputeHash(fs);
                            fs.Close();
                        }
                        md5.Dispose();
                    }
                }
                catch (Exception)
                {
                    return PackageDownloadResult.FailedWithBadHash;
                }

                if (fileHash != null)
                {
                    if (BitConverter.ToString(fileHash).ToString() == hash)
                    {
                        return PackageDownloadResult.Sucess;
                    }
                    else
                    {
                        return PackageDownloadResult.FailedWithBadHash;
                    }
                }
            }

            return PackageDownloadResult.Sucess;
        }

        public static string DownloadString(string url)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }

        public static void DownloadFile(string url, string savefilename)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFileAsync(new Uri(url), savefilename);
            }
        }

        public static string ListInstalledPackageInfo(IAddonContext context)
        {
            dynamic service = context.GetFirstOrDefaultService("Cirno.ChinaGS.Injection.Permanent",
               "Cirno.ChinaGS.Injection.Permanent.Service");
            List<string> list = service.GetInstalledPackageInfo();
            return string.Join(";", list);
        }

        public static PackageInfo ParsePackageName(string package)
        {
            if (package.Contains("="))
            {
                return new PackageInfo
                {
                    PackageName = package.Split('=')[0].Trim(),
                    PackageVersion = package.Split('=')[1].Trim()
                };
            }
            else
            {
                return new PackageInfo
                {
                    PackageName = package.Trim(),
                    PackageVersion = "default"
                };
            }
        }

        public static string GetRemotePackageFilename(PackageInfo package)
        {
            if (package.PackageVersion.ToLower() == "default")
            {
                return package.PackageName + ".dll.cpm";
            }
            else
            {
                return package.PackageName + "_" + package.PackageVersion + ".dll.cpm";
            }
        }

        public static List<PackageInfo> ParseManifest(string manifest)
        {
            List<PackageInfo> result = new List<PackageInfo>();
            foreach (string i in manifest.Split(';'))
            {
                string item = i.Trim();
                if (item.Contains("="))
                {
                    result.Add(new PackageInfo
                    {
                        PackageName = item.Split('=')[0].Trim(),
                        PackageVersion = item.Split('=')[1].Trim()
                    });
                }
                else
                {
                    result.Add(new PackageInfo
                    {
                        PackageName = item.Trim(),
                        PackageVersion = "default"
                    });
                }
            }
            return result;
        }
    }
}
