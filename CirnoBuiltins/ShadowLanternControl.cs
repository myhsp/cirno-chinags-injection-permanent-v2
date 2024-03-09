using GalaSoft.MvvmLight.Threading;
using GS.Unitive.Framework.Core;
using System;
using System.Windows.Controls;

namespace CirnoBuiltins
{
    public class ShadowLanternControl
    {
        public IAddonContext Context { get; set; }
        public Action<object> Handle { get; set; }

        public ShadowLanternControl(IAddonContext context)
        {
            DispatcherHelper.RunAsync(() =>
            {
                Context = context;

                Action<object> controlHandle = null;
                FindControlByKey(context, "BannerMessage", ref controlHandle);
                Handle = controlHandle;
            });
        }

        public void ShadowLantern(string msg, DateTime start, DateTime end, out bool error)
        {
            Utils.CreateTimelineTask(Context, start, end, 1,
                (string s, string id) => AddShadowLantern(msg),
                (string s, string id) => RemoveShadowLantern(msg),
                "SL_" + Guid.NewGuid().ToString()
                );

            error = Handle == null;
        }

        private void AddShadowLantern(string msg)
        {
            if (Handle != null)
            {
                DispatcherHelper.RunAsync(() => Handle("Command.Add" + msg));
            }
        }

        private void RemoveShadowLantern(string msg)
        {
            if (Handle != null)
            {
                DispatcherHelper.RunAsync(() => Handle("Command.Remove" + msg));
            }
        }

        private static UserControl FindControlByKey(IAddonContext context, string key, ref Action<object> controlHandle)
        {
            dynamic service = context.GetFirstOrDefaultService("GS.Terminal.GarnitureControl", "GS.Terminal.GarnitureControl.Service");
            if (service != null)
            {
                return service.FindControlByKey(key, ref controlHandle);
            }
            else
            {
                return null;
            }
        }

        private static string AddGarnitureControl(IAddonContext context, UserControl userControl, double top, double left)
        {
            dynamic service = context.GetFirstOrDefaultService("GS.Terminal.GarnitureControl", "GS.Terminal.GarnitureControl.Service");
            if (service != null)
            {
                return service.AddGarnitureControl(userControl, top, left);
            }
            else
            {
                return null;
            }
        }
    }
}
