using GalaSoft.MvvmLight.Ioc;
using GS.Terminal.LogicShell.Interface;
using GS.Unitive.Framework.Core;
using SmartBoardViewModels;
using SmartBoardViewModels.Models.VisualBlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;
using IVisualBlock;

namespace CirnoModifier
{
    public class Utils
    {
        const string VM_ID_MAIN = "6E55B5B3-E977-4254-BA69-73E5EEB0C8E5";

        public static IViewModel GetViewModel(IAddonContext context, string id)
        {
            IViewHelperService service = context.GetFirstOrDefaultService<IViewHelperService>("GS.Terminal.LogicShell");
            var vm = service.GetAllViewModel().FirstOrDefault(x => x.ViewID == id);

            return vm;
        }

        public static List<BlockTemplate> GetBlockTemplates(IAddonContext context)
        {
            var vm = SimpleIoc.Default.GetInstance<MainWindowViewModel>() ?? (MainWindowViewModel)GetViewModel(context, VM_ID_MAIN);
            return vm.TemplateList.ToList();
        }

        public static BlockTemplate GetBlockTemplate(IAddonContext context, string name)
        {
            return GetBlockTemplates(context).FirstOrDefault((t) => t.TemplateName == name);
        }

        public static List<VisualBlockItem> GetVisualBlockItems(BlockTemplate template)
        {
            return template.Blocks.ToList();
        }

        public static void SetVisualBlockItemJsonData(VisualBlockItem blockItem, string json_filename)
        {
            if (!File.Exists(json_filename))
            {
                json_filename = Path.Combine(GetCachePath(), "BlockCache", json_filename);
            }
            IUpdate update = (IUpdate)blockItem.DataContext;
            update.LoadLocalData(json_filename);
            blockItem.DataContext = (BaseBlock)update;
        }

        public static void RegistBackgroundCommand(IAddonContext context, string command, Action action)
        {
            dynamic service = context.GetFirstOrDefaultService("GS.Terminal.MainShell", "GS.Terminal.MainShell.Services.UIService");
            service.RegistBackgroundCommand(command, action);
        }

        public static string AddGarnitureControl(IAddonContext context, UserControl control, double left, double top)
        {
            ///<summary>
            /// 添加用户控件
            ///<returns></returns>
            ///</summary>
            dynamic uiService = context.GetFirstOrDefaultService("GS.Terminal.MainShell",
                "GS.Terminal.MainShell.Services.UIService");

            string guid = uiService.AddGarnitureControl(control, top, left);
            return guid;
        }

        public static void RemoveGarnitureControl(IAddonContext context, string guid)
        {
            ///<summary>
            /// 移除用户控件
            ///<returns></returns>
            ///</summary>
            dynamic uiService = context.GetFirstOrDefaultService("GS.Terminal.MainShell",
                "GS.Terminal.MainShell.Services.UIService");

            uiService.RemoveGarnitureControl(guid);
        }

        public static string GetCachePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cache");
        }
    }
}
