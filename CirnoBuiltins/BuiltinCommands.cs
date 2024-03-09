using CirnoFramework.Interface.Commands;
using GS.Unitive.Framework.Core;
using System;
using System.ComponentModel.Composition;

namespace CirnoBuiltins
{
    [Export(typeof(ICommand))]
    [Command("Builtin.Ping")]
    public class Ping : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime stop, string[] args)
        {
            return "Pong";
        }
    }

    [Export(typeof(ICommand))]
    [Command("Builtin.SetFoot")]
    public class SetFoot : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime stop, string[] args)
        {
            Utils.SetFootText(context, args[0]);
            return "Success";
        }
    }

    [Export(typeof(ICommand))]
    [Command("Builtin.SetVersion")]
    public class SetVersion : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime stop, string[] args)
        {
            Utils.SetFootVersion(context, args[0]);
            return "Success";
        }
    }

    [Export(typeof(ICommand))]
    [Command("Builtin.SetTerminalCode")]
    public class SetTerminalCode : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime stop, string[] args)
        {
            Utils.SetTerminalCode(context, args[0]);
            return "Success";
        }
    }

    [Export(typeof(ICommand))]
    [Command("Builtin.SetRemoteServerAddr")]
    public class SetRemoteServerAddr : ICommand
    {
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


    [Export(typeof(ICommand))]
    [Command("Builtin.ShadowLantern")]
    public class ShadowLantern : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            Utils.ShadowLantern(context, args[0], start, end, out bool error);
            return error ? "Failed" : "Success";
        }
    }

    [Export(typeof(ICommand))]
    [Command("Builtin.AddPosterTemplate")]
    public class AddPosterTemplate : ICommand
    {
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

    [Export(typeof(ICommand))]
    [Command("Builtin.RemovePosterTemplate")]
    public class RemovePosterTemplate : ICommand
    {
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

    [Export(typeof(ICommand))]
    [Command("Builtin.AddMultiMediaVisualTemplate")]
    public class AddMultiMediaVisualTemplate : ICommand
    {
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

    [Export(typeof(ICommand))]
    [Command("Builtin.ClearAllPosterTemplate")]
    public class ClearAllPosterTemplate : ICommand
    {
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

    [Export(typeof(ICommand))]
    [Command("Builtin.RemoveVisualTemplate")]
    public class RemoveVisualTemplate : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            Utils.RemoveVisualTemplate(context, args[0]);
            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Builtin.WriteJson")]
    public class WriteJson : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            Utils.WriteJson(context, args[0], args[1]);
            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Builtin.DownloadFile")]
    public class DownloadFile : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            Utils.DownloadFile(context, args[0], args[1]);
            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Builtin.SetTheme")]
    public class SetTheme : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            Utils.SetTheme(context, args[0]);
            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Builtin.StartProcess")]
    public class StartProcess : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            Utils.StartProcess(context, args[0]);
            return result;
        }
    }
}
