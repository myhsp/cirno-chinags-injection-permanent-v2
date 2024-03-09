using CirnoFramework.Interface.Commands;
using System;
using System.Collections.Generic;

namespace Cirno.ChinaGS.Injection.Permanent
{
    /// <summary>
    /// 服务类
    /// </summary>
    public class Service
    {
        /// <summary>
        /// 设置 http 后台地址
        /// </summary>
        /// <param name="addr">http 后台地址</param>
        public static void SetHttpServerAddr(string addr)
        {
            Program.Config.RemoteHttpConfig.HttpServerAddr = addr;
        }

        /// <summary>
        /// 获取所有安装的命令扩展包的信息
        /// </summary>
        /// <returns></returns>
        public static List<string> GetInstalledPackageInfo()
        {
            List<string> result = new List<string>();
            foreach (IPackageInfo info in Program.Factory.packageInfos)
            {
                result.Add(info.GetPackageInfo());
            }
            return result;
        }

        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="command">指令名称</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="args">参数</param>
        /// <param name="errorCode">返回的错误码</param>
        /// <returns></returns>
        public static string ExecuteCommand(string command, DateTime start, DateTime end, string[] args, ref int errorCode)
        {
            return Program.Factory.ExecuteCommand(command, start, end, args, ref errorCode);
        }
    }
}
