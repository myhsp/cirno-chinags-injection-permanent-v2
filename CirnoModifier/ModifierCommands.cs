using CirnoFramework.Interface.Commands;
using GS.Unitive.Framework.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CirnoModifier
{
    [Export(typeof(IPackageInfo))]
    public class PackageInfo : IPackageInfo
    {
        public string PackageName { get; set; } = "CirnoModifier";
        public string PackageVersion { get; set; } = "1.0.0.0";

        public string GetPackageInfo()
        {
            return string.Concat(PackageName, "=", PackageVersion);
        }
    }

    [Export(typeof(IStartupCommand))]
    public class ModifierFloatingWindowRegister : IStartupCommand
    {
        public string guid;
        public ModifierPanel panel;
        public IAddonContext context;

        public string Execute(IAddonContext context, string[] args)
        {
            try
            {
                this.context = context;
                panel = new ModifierPanel(this, context);

                Utils.RegistBackgroundCommand(
                    context, "192608",
                    () => 
                    {
                        panel.LoadLists();
                        guid = Utils.AddGarnitureControl(context, panel, 960 - panel.Width / 2, 540 - panel.Height / 2);
                    }
                    );
            }
            catch (Exception ex)
            {
                context.Logger.Info("[CirnoModifier] Failed to execute startup command!", ex);
            }
            return string.Empty;
        }

        public void HideModifierPanel()
        {
            Utils.RemoveGarnitureControl(context, guid);
        }
    }
}
