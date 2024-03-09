using System.Collections.Generic;

namespace Cirno.ChinaGS.Injection.Permanent
{
    /// <summary>
    /// 标准远程指令接口
    /// </summary>
    public interface IRemoteCommandGeneric
    {
        string command_protocol_version { get; set; }
        string command_name { get; set; }
        string start_time { get; set; }
        string end_time { get; set; }
        List<string> args { get; set; }
    }

    /// <summary>
    /// 标准远程指令
    /// </summary>
    public class RemoteCommandGeneric : IRemoteCommandGeneric
    {
        public string command_protocol_version { get; set; }
        public string command_name { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public List<string> args { get; set; }
    }

    /// <summary>
    /// 向下兼容的远程指令
    /// </summary>
    public class RemoteCommandv1
    {
        public string command_name { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string args { get; set; }
    }

    /// <summary>
    /// 新版远程指令
    /// </summary>
    public class RemoteCommandv2 : IRemoteCommandGeneric
    {
        public string command_protocol_version { get; set; }
        public string command_name { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public List<string> args { get; set; }
    }

    /// <summary>
    /// 返回数据格式接口
    /// </summary>
    public interface IResult
    {
        string machine_id { get; set; }
        string machine_ip { get; set; }
        string msg { get; set; }
        int errcode { get; set; }
        string time { get; set; }
    }

    /// <summary>
    /// 返回数据格式
    /// </summary>
    public class Result : IResult
    {
        public string machine_id { get; set; }
        public string machine_ip { get; set; }
        public string msg { get; set; }
        public int errcode { get; set; }
        public string time { get; set; }
    }

    /// <summary>
    /// 远程指令数据协议版本
    /// </summary>
    public enum CommandProtocolVersion
    {
        Unknown,
        Cirno_RCMD_v1,
        Cirno_RCMD_v2
    }
}
