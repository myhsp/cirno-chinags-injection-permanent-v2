using GS.Unitive.Framework.Core;
using System;

namespace CirnoFramework.Interface.Commands
{
    public interface ICommand
    {
        string Execute(IAddonContext context, DateTime start, DateTime stop, string[] args);
    }
}
