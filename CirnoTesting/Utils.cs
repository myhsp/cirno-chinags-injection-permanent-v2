using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using GS.Terminal.LogicShell.Interface;
using GS.Unitive.Framework.Core;
using IVisualBlock;
using SmartBoardViewModels;
using SmartBoardViewModels.Models.VisualBlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace CirnoTesting
{
    public class Utils
    {
        const string VM_ID_MAIN = "6E55B5B3-E977-4254-BA69-73E5EEB0C8E5"; // 主页面 ViewID
        const string VM_ID_FOOT = "CD154FA4-A56C-425B-9501-E442B6C86825"; // 底部栏 ViewID

        public static void TestMultiMediaVisualTemplate(IAddonContext current_context, string json_filename)
        {
            IAddonContext target_context = GetDesignatedAddonContext(current_context, "GS.Terminal.SmartBoard.Logic");
            DispatcherHelper.RunAsync(() =>
            {
                BlockTemplate blockTemplate = new BlockTemplate
                {
                    TemplateName = "ClassTemplateStyle",
                    DisplayName = "校园风采",
                    Index = 2,
                    TemplateType = BlockTemplateType.Theme,
                };
                VisualBlockItem blockItem = GetBlock(
                    current_context,
                    target_context,
                    "ClassMultiMedia",
                    json_filename,
                    "班级风采",
                    20, 10, 1, 1,
                    "Services/SmartBoard/BlockClassMultiMedia/json",
                    "");
                if (blockItem != null)
                {
                    current_context.Logger.Info("visualblockitem get");
                }
                else
                {
                    current_context.Logger.Info("vbi not get");
                }
                blockTemplate.Blocks = new List<VisualBlockItem> { blockItem };

                var vm = (MainWindowViewModel)(ViewModelBase)SimpleIoc.Default.GetInstance<MainWindowViewModel>();
                IViewModel vm2 = GetViewModel(current_context, VM_ID_MAIN);

                if (vm2 == null)
                {
                    current_context.Logger.Info("IViewHelperService 未获取到 VM");
                }
                if (vm == null)
                {
                    current_context.Logger.Info("SimpleIoc 未获取到 VM");
                    if (vm2 == null)
                    {
                        current_context.Logger.Info("IViewHelperService 未获取到 VM");
                        return;
                    }
                    else
                    {
                        current_context.Logger.Info("IViewHelperService 获取到 VM");
                    }
                    vm = (MainWindowViewModel)GetViewModel(current_context, VM_ID_MAIN);
                }
                else
                {
                    current_context.Logger.Info("SimpleIoc 获取到 VM");
                }
                blockTemplate.Previous = vm.TemplateList[1];
                blockTemplate.Next = vm.TemplateList[3];

                vm.TemplateList[2] = blockTemplate;
            }
            ).Wait();
        }

        public static IViewModel GetViewModel(IAddonContext context, string id)
        {
            IViewHelperService service = context.GetFirstOrDefaultService<IViewHelperService>("GS.Terminal.LogicShell");
            return service.GetAllViewModel().FirstOrDefault(x => x.ViewID == id);
        }

        public static void FootBarTesting(IAddonContext context, string text)
        {
            var vm = SimpleIoc.Default.GetInstance<FootViewModel>();
            IViewModel vm2 = GetViewModel(context, VM_ID_FOOT);

            if (vm2 == null)
            {
                context.Logger.Info("IViewHelperService 未获取到 VM");
            }

            if (vm == null)
            {
                context.Logger.Info("SimpleIoc 未获取到 VM");
                if (vm2 == null)
                {
                    context.Logger.Info("IViewHelperService 未获取到 VM");
                    return;
                }
                else
                {
                    context.Logger.Info("IViewHelperService 获取到 VM");
                }
                vm = (FootViewModel)GetViewModel(context, VM_ID_MAIN);
            }
            else
            {
                context.Logger.Info("SimpleIoc 获取到 VM");
            }

            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                SetFootbarText(vm, text);
                SetFootBarColor(vm, new SolidColorBrush(Colors.SkyBlue));
            });
        }

        public static void SetFootbarText(FootViewModel vm, string text)
        {
            vm.CenterText = text;
        }

        public static void SetFootBarColor(FootViewModel vm, Brush brush)
        {
            vm.tCodeColor = brush;
        }

        public static IAddonContext GetDesignatedAddonContext(IAddonContext context, string name)
        {
            IAddon addon = AddonRuntime.Instance.GetInstalledAddons().FirstOrDefault(x => x.SymbolicName == name);
            if (addon == null)
            {
                context.Logger.Info("未获取到指定的 addon @ GetDesignatedAddonContext()");
            }
            else
            {
                context.Logger.Info("获取到了 addon，symbolic name: " + addon.SymbolicName);
            }
            return addon.Context;
        }

        public static VisualBlockItem GetBlock(IAddonContext current_context, IAddonContext target_context,
            string block_name, string json_filename, string component, int width, int height, int x, int y,
            string data_source = "", string nav_template_name = "")
        {
            IBlockService firstOrDefaultService = current_context.GetFirstOrDefaultService<IBlockService>("GS.Terminal.VisualBlock");
            // 获取 VisualBlock 有关服务
            BaseBlock block = firstOrDefaultService.GetBlock(block_name);
            if (block == null)
            {
                current_context.Logger.Info("未获取到指定的 block @ GetBlock()");
            }
            else
            {
                current_context.Logger.Info("获取到了 block，block typename: " + ((IBlock)block).TypeName);
            }
            block.Init(target_context);

            IUpdate update = (IUpdate)block;
            update.LoadLocalData(json_filename);


            return new VisualBlockItem
            {
                Id = Guid.NewGuid(),
                BlockComponent = component,
                BlockTypeName = ((IBlock)block).TypeName,
                DataSource = data_source,
                NavTemplateName = nav_template_name,
                Width = width,
                Height = height,
                X = x,
                Y = y,
                DataContext = (BaseBlock)update
            };
        }
    }
}
