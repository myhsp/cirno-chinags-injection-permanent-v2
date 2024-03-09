using GS.Unitive.Framework.Core;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Controls;

namespace Cirno.ChinaGS.Injection.Permanent
{
    /// <summary>
    /// 杂项
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// 获取一个配置项。如果为空，则返回给定的默认值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="dictname">配置字典名</param>
        /// <param name="keyname">配置项键名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T GetConfigOrDefaultValue<T>(string dictname, string keyname, T defaultValue)
        {
            try
            {
                string conf = Program.AddonContext.DictionaryValue(dictname, keyname);
                if (!string.IsNullOrEmpty(conf))
                {
                    if (typeof(T) == typeof(bool))
                    {
                        return (T)(object)bool.Parse(conf);
                    }
                    else if (typeof(T) == typeof(int))
                    {
                        return (T)(object)int.Parse(conf);
                    }
                    else if (typeof(T) == typeof(double))
                    {
                        return (T)(object)double.Parse(conf);
                    }
                    else if (typeof(T) == typeof(string))
                    {
                        return (T)(object)conf;
                    }
                    else
                    {
                        return defaultValue;
                    }
                }
                else
                {
                    return defaultValue;
                }
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 获得机器当前状态
        /// </summary>
        /// <returns>机器状态</returns>
        public static string GetMachineCurrentStatus()
        {
            dynamic logicService = Program.AddonContext.GetFirstOrDefaultService("GS.Terminal.SmartBoard.Logic",
                "GS.Terminal.SmartBoard.Logic.Core.Service");
            string status = logicService.GetState();

            return status;
        }

        public static string GetTerminalCode()
        {
            return Program.AddonContext.IntercativeData("TerminalCode");
        }

        /// <summary>
        /// 获得 webpath
        /// </summary>
        /// <returns> webpath </returns>
        public static string GetMachineWebPath()
        {
            IAddonContext logicContext = AddonRuntime.Instance.GetInstalledAddons()
                .FirstOrDefault((IAddon ss) => ss.SymbolicName == "GS.Terminal.SmartBoard.Logic").Context;

            bool success = false;
            string webPath = logicContext.GlobalSetting("WebPath", ref success);
            if (!success)
            {
                Program.WriteLog("Method Utils.GetMachineWebPath can't fetch attribute WebPath through GlobalSetting - returned empty string.");
            }
            else
            {
                Program.WriteLog("Method Utils.GetMachineWebPath is called and fetched attribute WebPath!");
            }

            return webPath;
        }

        /// <summary>
        /// 获取 IP 地址
        /// </summary>
        /// <returns>IP 地址</returns>
        public static string GetMachineMacAddr()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 获得当前机器 ID
        /// </summary>
        /// <returns>机器 ID</returns>
        public static string GetMachineID()
        {
            IAddonContext logicContext = AddonRuntime.Instance.GetInstalledAddons()
                .FirstOrDefault((IAddon ss) => ss.SymbolicName == "GS.Terminal.SmartBoard.Logic").Context;

            bool success = false;
            string machineId = logicContext.GlobalSetting("tCode", ref success);
            if (!success)
            {
                Program.WriteLog("Method Utils.GetMachineID can't fetch attribute tCode through GlobalSetting - returned empty string.");
            }
            else
            {
                Program.WriteLog("Method Utils.GetMachineID is called and fetched attribute tCode!");
            }

            return machineId;
        }

        /// <summary>
        /// 添加用户控件
        /// </summary>
        /// <param name="control">要添加的控件</param>
        /// <param name="left">左边距</param>
        /// <param name="top">上边距</param>
        /// <returns>控件管理 GUID</returns>
        public static string AddGarnitureControl(UserControl control, double left, double top)
        {
            dynamic uiService = Program.AddonContext.GetFirstOrDefaultService("GS.Terminal.MainShell",
                "GS.Terminal.MainShell.Services.UIService");

            string guid = uiService.AddGarnitureControl(control, top, left);
            return guid;
        }

        /// <summary>
        /// 创建时间线任务
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="Lvl"></param>
        /// <param name="AllowParallel"></param>
        /// <param name="OnStart"></param>
        /// <param name="OnPause"></param>
        /// <param name="OnRestart"></param>
        /// <param name="OnStop"></param>
        /// <param name="OnTaskStateChanged"></param>
        /// <param name="OnTaskCreated"></param>
        /// <param name="taskname"></param>
        public static void CreateTimelineTask(DateTime StartTime, DateTime EndTime,
            int Lvl, bool AllowParallel,
            Action<string, string> OnStart, Action<string, string> OnPause,
            Action<string, string> OnRestart, Action<string, string> OnStop,
            Action<string, string> OnTaskStateChanged,
            Action<string, string> OnTaskCreated,
            string taskname)
        {
            dynamic timelineService = Program.AddonContext.GetFirstOrDefaultService("GS.Terminal.TimeLine", "GS.Terminal.TimeLine.Service");
            timelineService.CreateTimeLineTask(StartTime, EndTime, Lvl, AllowParallel, OnStart, OnPause, OnRestart, OnStop, OnTaskStateChanged, OnTaskCreated, taskname); ;
        }

        public static void CreateTimelineTask(DateTime StartTime, DateTime EndTime, int Lvl, Action<string, string> OnStart, Action<string, string> OnStop, string taskname)
        {
            ///<summary>
            /// 创建时间线任务
            ///<returns></returns>
            ///</summary>
            CreateTimelineTask(StartTime, EndTime, Lvl, true, OnStart, null, null, OnStop, null, null, taskname);
        }

        /// <summary>
        /// 创建定时任务
        /// </summary>
        /// <param name="ExecuteTime">执行时间</param>
        /// <param name="TaskAction">任务行为</param>
        /// <param name="Callback">回调</param>
        /// <param name="taskname">任务名称</param>
        public static void CreateTask(DateTime ExecuteTime, Action<string> TaskAction, AsyncCallback Callback, string taskname)
        {
            dynamic timelineService = Program.AddonContext.GetFirstOrDefaultService("GS.Terminal.TimeLine", "GS.Terminal.TimeLine.Service");
            timelineService.CreateTask(ExecuteTime, TaskAction, Callback, taskname);
        }

        /// <summary>
        /// 弹出弹窗
        /// </summary>
        /// <param name="msg">消息</param>
        public static void ShowPopup(string msg)
        {
            dynamic uiAddon = Program.AddonContext.
                GetFirstOrDefaultService("GS.Terminal.MainShell", "GS.Terminal.MainShell.Services.UIService");

            uiAddon.ShowPrompt(msg, 3);
        }

        /// <summary>
        /// 转换远程指令
        /// </summary>
        /// <param name="e">UDP 接收事件参数</param>
        /// <param name="protocolVersion">返回数据协议版本</param>
        /// <returns></returns>
        public static RemoteCommandGeneric ParseCommand(UdpTransmissionEventArgs e, ref CommandProtocolVersion protocolVersion)
        {
            try
            {
                if (e.Message.Contains("command_protocol_version"))
                {
                    try
                    {
                        RemoteCommandv2 commandv2 = JsonConvert.DeserializeObject<RemoteCommandv2>(e.Message);
                        RemoteCommandGeneric generic = new RemoteCommandGeneric
                        {
                            args = commandv2.args,
                            start_time = commandv2.start_time,
                            end_time = commandv2.end_time,
                            command_name = commandv2.command_name,
                            command_protocol_version = commandv2.command_protocol_version
                        };
                        protocolVersion = CommandProtocolVersion.Cirno_RCMD_v2;
                        return generic;
                    }
                    catch (Exception)
                    {
                        protocolVersion = CommandProtocolVersion.Unknown;
                        return null;
                    }
                }
                else
                {
                    RemoteCommandv1 commandv1 = JsonConvert.DeserializeObject<RemoteCommandv1>(e.Message);
                    protocolVersion = CommandProtocolVersion.Cirno_RCMD_v1;
                    return new RemoteCommandGeneric
                    {
                        args = commandv1.args.Split(';').ToList(),
                        start_time = commandv1.start_time,
                        end_time = commandv1.end_time,
                        command_name = commandv1.command_name,
                        command_protocol_version = "cirno-rcmd-v1"
                    };
                }
            }
            catch
            {
                protocolVersion = CommandProtocolVersion.Unknown;
                return null;
            }
        }
    }
}
