using CirnoFramework.Interface.Commands;
using GS.Unitive.Framework.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;

namespace CirnoPM
{
    [Export(typeof(IPackageInfo))]
    public class CirnoPackageManagerInfo : IPackageInfo
    {
        public string PackageName { get; set; } = "CirnoPM";
        public string PackageVersion { get; set; } = "1.0.0.0";

        public string GetPackageInfo()
        {
            return string.Concat(PackageName, "=", PackageVersion);
        }
    }

    [Export(typeof(ICommand))]
    [Command("Cpm.Install")]
    public class InstallPackage : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            /// <summary>
            /// arg0: 包镜像源
            /// arg1: 包名称（包含版本则为 PackageName=PackageVersion）
            ///       下载一个 .dll.cpm 文件到本地，下一次启动时装载
            ///       可以下载已存在的包的新版本，可以自动替换
            ///       远程服务器上名称 /static/cpm/[PackageName]_[PackageVersion].dll.cpm 或 [PackageName].dll.cpm
            /// </summary>
            string result = string.Empty;
            try
            {
                PackageInfo info = Utils.ParsePackageName(args[1]);
                string url = args[0] + "/static/cpm/" + Utils.GetRemotePackageFilename(info);
                string savename = info.PackageName + ".dll.cpm";
                string path = Path.Combine(context.Addon.Location, "CommandLib", savename);
                Utils.DownloadFile(url, path);

                var verify = Utils.Verify(path);
                if (verify == PackageDownloadResult.FailedWithBadFile)
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (Exception)
                    {
                    }
                }

                result = "Package installed: " + args[1] + "; with result: " + verify.ToString();
            }
            catch (Exception)
            {
                result = "Fail to install package: " + args[1];
            }

            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Cpm.InstallVerifyHash")]
    public class InstallPackageVerifyHash : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            /// <summary>
            /// arg0: 包镜像源
            /// arg1: 包名称（包含版本则为 PackageName=PackageVersion）
            ///       下载一个 .dll.cpm 文件到本地，下一次启动时装载
            ///       可以下载已存在的包的新版本，可以自动替换
            ///       远程服务器上名称 /static/cpm/[PackageName]_[PackageVersion].dll.cpm 或 [PackageName].dll.cpm
            /// arg2: 文件 hash
            /// </summary>
            string result = string.Empty;
            try
            {
                PackageInfo info = Utils.ParsePackageName(args[1]);
                string hash = args[2];
                string url = args[0] + "/static/cpm/" + Utils.GetRemotePackageFilename(info);
                string savename = info.PackageName + ".dll.cpm";
                string path = Path.Combine(context.Addon.Location, "CommandLib", savename);
                Utils.DownloadFile(url, path);

                var verify = Utils.Verify(path, hash, true);
                if (verify == PackageDownloadResult.FailedWithBadFile || verify == PackageDownloadResult.FailedWithBadHash)
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (Exception)
                    {
                    }
                }

                result = "Package installed: " + args[1] + "; with result: " + verify.ToString();
            }
            catch (Exception)
            {
                result = "Fail to install package: " + args[1];
            }

            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Cpm.InstallMany")]
    public class InstallPackageMany : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            /// <summary>
            /// arg0: 包镜像源
            /// arg1: 包名称1（包含版本则为 PackageName=PackageVersion）;包名称2;包名称3;...
            /// </summary>
            string result = string.Empty;
            try
            {
                List<PackageInfo> infos = Utils.ParseManifest(args[1]);
                string url, savename, path;
                foreach (PackageInfo info in infos)
                {
                    try
                    {
                        url = args[0] + "/static/cpm/" + Utils.GetRemotePackageFilename(info);
                        savename = info.PackageName + ".dll.cpm";
                        path = Path.Combine(context.Addon.Location, "CommandLib", savename);
                        Utils.DownloadFile(url, path);

                        var verify = Utils.Verify(path);
                        if (verify == PackageDownloadResult.FailedWithBadFile)
                        {
                            try
                            {
                                File.Delete(path);
                            }
                            catch (Exception)
                            {
                            }
                        }

                        result += ("Package installed: " + info.PackageName + "; with result: " + verify.ToString() + ";\n");
                    }
                    catch (Exception)
                    {
                        result += ("Fail to install package: " + info.PackageName + ";\n");
                    }
                }
            }
            catch (Exception)
            {
                result = "Failed to parse manifest!";
            }
            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Cpm.InstallFromManifest")]
    public class InstallFromManifest : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            /// <summary>
            /// arg0: 包镜像源
            /// arg1: manifest 字符串请求路径 url（相当于 InstallMany）
            /// </summary>
            string result = string.Empty;
            try
            {
                List<PackageInfo> infos = Utils.ParseManifest(Utils.DownloadString(args[1]));
                string url, savename, path;
                foreach (PackageInfo info in infos)
                {
                    try
                    {
                        url = args[0] + "/static/cpm/" + Utils.GetRemotePackageFilename(info);
                        savename = info.PackageName + ".dll.cpm";
                        path = Path.Combine(context.Addon.Location, "CommandLib", savename);
                        Utils.DownloadFile(url, path);

                        var verify = Utils.Verify(path);
                        if (verify == PackageDownloadResult.FailedWithBadFile)
                        {
                            try
                            {
                                File.Delete(path);
                            }
                            catch (Exception)
                            {
                            }
                        }

                        result += ("Package installed: " + info.PackageName + "; with result: " + verify.ToString() + ";\n");
                    }
                    catch (Exception)
                    {
                        result += ("Fail to install package: " + info.PackageName + ";\n");
                    }
                }
            }
            catch (Exception)
            {
                result = "Failed to parse manifest!";
            }
            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Cpm.Update")]
    public class Update : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            result = "Not implemented";
            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Cpm.UpdateAll")]
    public class UpdateAll : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            result = "Not implemented";
            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Cpm.UpdateFromManifest")]
    public class UpdateFromManifest : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            result = "Not implemented";
            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Cpm.Uninstall")]
    public class UninstallPackage : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            result = "Not implemented";
            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Cpm.ListPackages")]
    public class ListPackages : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = Utils.ListInstalledPackageInfo(context);
            return result;
        }
    }
}
