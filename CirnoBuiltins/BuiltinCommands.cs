using CirnoFramework.Interface.Commands;
using GS.Unitive.Framework.Core;
using System;
using System.ComponentModel.Composition;

namespace CirnoBuiltins
{
    /// <summary>
    /// 检查是否在线
    /// </summary>
    [Export(typeof(ICommand))]
    [Command("Builtin.Ping")]
    public class Ping : ICommand
    {
        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="context"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="args">没有参数</param>
        /// <returns></returns>
        public string Execute(IAddonContext context, DateTime start, DateTime stop, string[] args)
        {
            return "Pong";
        }
    }

    /// <summary>
    /// 设置底部栏文本
    /// </summary>
    [Export(typeof(ICommand))]
    [Command("Builtin.SetFoot")]
    public class SetFoot : ICommand
    {
        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="context"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="args">要设置的文本</param>
        /// <returns></returns>
        public string Execute(IAddonContext context, DateTime start, DateTime stop, string[] args)
        {
            Utils.SetFootText(context, args[0]);
            return "Success";
        }
    }

    /// <summary>
    /// 设置版本信息
    /// </summary>
    [Export(typeof(ICommand))]
    [Command("Builtin.SetVersion")]
    public class SetVersion : ICommand
    {
        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="context"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="args">要设置的文本</param>
        /// <returns></returns>
        public string Execute(IAddonContext context, DateTime start, DateTime stop, string[] args)
        {
            Utils.SetFootVersion(context, args[0]);
            return "Success";
        }
    }

    /// <summary>
    /// 设置显示的 TerminalCode
    /// </summary>
    [Export(typeof(ICommand))]
    [Command("Builtin.SetTerminalCode")]
    public class SetTerminalCode : ICommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="args">要设置的值</param>
        /// <returns></returns>
        public string Execute(IAddonContext context, DateTime start, DateTime stop, string[] args)
        {
            Utils.SetTerminalCode(context, args[0]);
            return "Success";
        }
    }

    /// <summary>
    /// 设置远程 http 服务器地址
    /// </summary>
    [Export(typeof(ICommand))]
    [Command("Builtin.SetRemoteServerAddr")]
    public class SetRemoteServerAddr : ICommand
    {
        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="context"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="args">要设置的地址</param>
        /// <returns></returns>
        public string Execute(IAddonContext context, DateTime start, DateTime stop, string[] args)
        {
            string result = "Success";
            try
            {
                if (args[0].EndsWith("/")) args[0].Substring(0, args[0].Length - 1);
                Utils.SetRemoteServerAddr(context, args[0]);
            }
            catch (Exception ex)
            {
                result = "Exception encountered! " + ex.Message;
            }
            return result;
        }
    }

    /// <summary>
    /// 播放跑马灯
    /// </summary>
    [Export(typeof(ICommand))]
    [Command("Builtin.ShadowLantern")]
    public class ShadowLantern : ICommand
    {
        /// <summary>
        /// 执行指令（需要指定时间）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            Utils.ShadowLantern(context, args[0], start, end, out bool error);
            return error ? "Failed" : "Success";
        }
    }

    /// <summary>
    /// 添加大图
    /// </summary>
    [Export(typeof(ICommand))]
    [Command("Builtin.AddPosterTemplate")]
    public class AddPosterTemplate : ICommand
    {
        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="context"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="args">远程图片 url</param>
        /// 
        /// <returns></returns>
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            try
            {
                result = Utils.AddPosterTemplate(context, args[0]).ToString();
            }
            catch (Exception ex)
            {
                result = "Exception encountered! " + ex.Message;
            }
            return result;
        }
    }

    /// <summary>
    /// 添加大图（并指定时间）
    /// </summary>
    [Export(typeof(ICommand))]
    [Command("Builtin.AddPosterTemplateScheduled")]
    public class AddPosterTemplateScheduled : ICommand
    {
        /// <summary>
        /// 执行指令（需指定时间）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="args">远程图片 url </param>
        /// <remarks>建议使用校园网内网服务器上图片</remarks>
        /// <returns></returns>
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            try
            {
                Guid guid= Guid.Empty;
                Utils.CreateTimelineTask(context, start, end, 99, 
                    (s, id) => { guid = Utils.AddPosterTemplate(context, args[0]); }, 
                    (s, id) => { Utils.RemovePosterTemplate(context, guid); }, 
                    "PT_" + Guid.NewGuid().ToString());
                result = "Success";
            }
            catch (Exception ex)
            {
                result = "Exception encountered! " + ex.Message;
            }
            return result;
        }
    }

    /// <summary>
    /// 移除大图
    /// </summary>
    [Export(typeof(ICommand))]
    [Command("Builtin.RemovePosterTemplate")]
    public class RemovePosterTemplate : ICommand
    {
        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="context"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="args">要移除的大图的 GUID</param>
        /// <returns></returns>
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            try
            {
                Utils.RemovePosterTemplate(context, Guid.Parse(args[0]));
            }
            catch (Exception ex)
            {
                result = "Exception encountered! " + ex.Message;
            }
            return result;
        }
    }

    /// <summary>
    /// 添加多媒体
    /// </summary>
    [Export(typeof(ICommand))]
    [Command("Builtin.AddMultiMediaVisualTemplate")]
    public class AddMultiMediaVisualTemplate : ICommand
    {
        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="context"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="args">本地 VisualBlock 内容 Json</param>
        /// <returns></returns>
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            try
            {
                Utils.AddMultiMediaVisualTemplate(context, args[0]);
            }
            catch (Exception ex)
            {
                result = "Exception encountered! " + ex.Message;
            }
            return result;
        }
    }

    /// <summary>
    /// 清除所有大图
    /// </summary>
    [Export(typeof(ICommand))]
    [Command("Builtin.ClearAllPosterTemplate")]
    public class ClearAllPosterTemplate : ICommand
    {
        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="context"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="args">没有参数</param>
        /// <returns></returns>
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            try
            {
                Utils.ClearAllPosterTemplate(context);
            }
            catch (Exception ex)
            {
                result = "Exception encountered! " + ex.Message;
            }
            return result;
        }
    }

    /// <summary>
    /// 移除指定 VisualTemplate
    /// </summary>
    [Export(typeof(ICommand))]
    [Command("Builtin.RemoveVisualTemplate")]
    public class RemoveVisualTemplate : ICommand
    {
        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="context"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="args">VisualTemplate 名称</param>
        /// <returns></returns>
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            Utils.RemoveVisualTemplate(context, args[0]);
            return result;
        }
    }

    /// <summary>
    /// 写入一段 Json 到 BlockCache 文件夹
    /// </summary>
    [Export(typeof(ICommand))]
    [Command("Builtin.WriteJson")]
    public class WriteJson : ICommand
    {
        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="context"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="args">参数 0：写入文件名；参数 1：Json 内容的 base64 字符串</param>
        /// <returns></returns>
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            Utils.WriteJson(context, args[0], args[1]);
            return result;
        }
    }

    /// <summary>
    /// 下载文件到 cache 文件夹
    /// </summary>
    [Export(typeof(ICommand))]
    [Command("Builtin.DownloadFile")]
    public class DownloadFile : ICommand
    {
        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="context"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="args">参数 0：保存的文件名；参数 1：远程文件 url</param>
        /// <remarks>建议使用校园网内网上资源</remarks>
        /// <returns></returns>
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            Utils.DownloadFile(context, args[0], args[1]);
            return result;
        }
    }

    /// <summary>
    /// 设置主题
    /// </summary>
    [Export(typeof(ICommand))]
    [Command("Builtin.SetTheme")]
    public class SetTheme : ICommand
    {
        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="context"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="args">主题名称</param>
        /// <returns></returns>
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            Utils.SetTheme(context, args[0]);
            return result;
        }
    }

    /// <summary>
    /// 启动进程
    /// </summary>
    [Export(typeof(ICommand))]
    [Command("Builtin.StartProcess")]
    public class StartProcess : ICommand
    {
        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="context"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="args">要启动进程的文件路径</param>
        /// <returns></returns>
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            Utils.StartProcess(context, args[0]);
            return result;
        }
    }
}
