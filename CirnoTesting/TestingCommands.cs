using CirnoFramework.Interface.Commands;
using GS.Unitive.Framework.Core;
using System;
using System.ComponentModel.Composition;

namespace CirnoTesting
{
    public class TestingCommands
    {
        [Export(typeof(ICommand))]
        [Command("Test.VT")]
        public class FutureVT : ICommand
        {
            public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
            {
                string ret = string.Empty;
                Utils.TestMultiMediaVisualTemplate(context, args[0]);
                return ret;
            }
        }

        [Export(typeof(ICommand))]
        [Command("Test.Foot")]
        public class FutureFoot : ICommand
        {
            public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
            {
                string ret = string.Empty;
                Utils.FootBarTesting(context, args[0]);
                return ret;
            }
        }
    }
}
