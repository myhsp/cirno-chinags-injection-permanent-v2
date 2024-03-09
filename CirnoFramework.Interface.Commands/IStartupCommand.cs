using GS.Unitive.Framework.Core;

namespace CirnoFramework.Interface.Commands
{
    public interface IStartupCommand
    {
        string Execute(IAddonContext context, string[] args);
    }
}
