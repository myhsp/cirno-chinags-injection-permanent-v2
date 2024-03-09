using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using GS.Terminal.LogicShell.Interface;
using GS.Unitive.Framework.Core;
using IVisualBlock;
using SmartBoardViewModels;
using SmartBoardViewModels.Models.VisualBlock;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Media;

namespace CirnoBuiltins
{
    public class Utils
    {
        const string VM_ID_MAIN = "6E55B5B3-E977-4254-BA69-73E5EEB0C8E5"; // 主页面 ViewID
        const string VM_ID_FOOT = "CD154FA4-A56C-425B-9501-E442B6C86825"; // 底部栏 ViewID

        public static ShadowLanternControl SLControl;

        public static string ShadowLantern(IAddonContext context, string msg, DateTime start, DateTime end, out bool error)
        {
            string ret = string.Empty;

            if (SLControl == null)
            {
                SLControl = new ShadowLanternControl(context);
            }

            SLControl.ShadowLantern(msg, start, end, out error);
            ret = "Success";

            return ret;
        }

        public static void SetFootText(IAddonContext context, string msg)
        {
            FootViewModel vm = SimpleIoc.Default.GetInstance<FootViewModel>() ?? (FootViewModel)GetViewModel(context, VM_ID_FOOT);
            if (vm == null) return;

            DispatcherHelper.RunAsync(() => vm.CenterText = msg);
        }

        public static void SetFootVersion(IAddonContext context, string msg)
        {
            FootViewModel vm = SimpleIoc.Default.GetInstance<FootViewModel>() ?? (FootViewModel)GetViewModel(context, VM_ID_FOOT);
            if (vm == null) return;

            DispatcherHelper.RunAsync(() => vm.Version = msg);
        }

        public static void SetTerminalCode(IAddonContext context, string msg)
        {
            FootViewModel vm = SimpleIoc.Default.GetInstance<FootViewModel>() ?? (FootViewModel)GetViewModel(context, VM_ID_FOOT);
            if (vm == null) return;

            DispatcherHelper.RunAsync(() =>
            {
                vm.tCode = msg;
                vm.tCodeColor = new SolidColorBrush(Colors.Blue);
            });
        }

        public static void SetRemoteServerAddr(IAddonContext context, string addr)
        {
            dynamic service = context.GetFirstOrDefaultService("Cirno.ChinaGS.Injection.Permanent", "Cirno.ChinaGS.Injection.Permanent.Service");
            if (service != null)
            {
                service.SetHttpServerAddr(addr);
            }
        }

        public static void CreateTimelineTask(IAddonContext context, DateTime StartTime, DateTime EndTime,
            int Lvl, bool AllowParallel,
            Action<string, string> OnStart, Action<string, string> OnPause,
            Action<string, string> OnRestart, Action<string, string> OnStop,
            Action<string, string> OnTaskStateChanged,
            Action<string, string> OnTaskCreated,
            string taskname)
        {
            dynamic timelineService = context.GetFirstOrDefaultService("GS.Terminal.TimeLine", "GS.Terminal.TimeLine.Service");
            timelineService.CreateTimeLineTask(StartTime, EndTime, Lvl, AllowParallel, OnStart, OnPause, OnRestart, OnStop, OnTaskStateChanged, OnTaskCreated, taskname); ;
        }

        public static void CreateTimelineTask(IAddonContext context, DateTime StartTime, DateTime EndTime, int Lvl, Action<string, string> OnStart, Action<string, string> OnStop, string taskname)
        {
            CreateTimelineTask(context, StartTime, EndTime, Lvl, true, OnStart, null, null, OnStop, null, null, taskname);
        }

        private static IViewModel GetViewModel(IAddonContext context, string id)
        {
            IViewHelperService service = context.GetFirstOrDefaultService<IViewHelperService>("GS.Terminal.LogicShell");
            var vm = service.GetAllViewModel().FirstOrDefault(x => x.ViewID == id);

            return vm;
        }

        public static Guid AddPosterTemplate(IAddonContext context, string imageUri)
        {
            MainWindowViewModel vm = SimpleIoc.Default.GetInstance<MainWindowViewModel>() ?? (MainWindowViewModel)GetViewModel(context, VM_ID_MAIN);

            if (vm != null)
            {
                BlockTemplate template = new BlockTemplate()
                {
                    DisplayName = "内嵌多媒体",
                    TemplateType = BlockTemplateType.Media,
                    TemplateName = "内嵌多媒体",
                    Index = vm.TemplateList.Max((BlockTemplate ss) => ss.Index) + 1,
                    Tag = imageUri
                };

                BlockTemplate t = template;
                DispatcherHelper.RunAsync(() =>
                {
                    vm.AddTemplate(t);
                });

                return t.Id;
            }
            else
            {
                return Guid.Empty;
            }
        }

        public static void RemovePosterTemplate(IAddonContext context, Guid guid)
        {
            MainWindowViewModel vm = SimpleIoc.Default.GetInstance<MainWindowViewModel>() ?? (MainWindowViewModel)GetViewModel(context, VM_ID_MAIN);
            if (vm != null)
            {
                BlockTemplate t = vm.TemplateList.FirstOrDefault((BlockTemplate ss) => ss.Id == guid);
                if (t != null)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        vm.RemoveTemplate(t);
                    });
                }
            }
        }

        public static void AddMultiMediaVisualTemplate(IAddonContext context, string media_json_filename)
        {
            MainWindowViewModel vm = SimpleIoc.Default.GetInstance<MainWindowViewModel>() ?? (MainWindowViewModel)GetViewModel(context, VM_ID_MAIN);
            if (vm == null) return;

            IBlockService service = context.GetFirstOrDefaultService<IBlockService>("GS.Terminal.VisualBlock");
            if (service != null)
            {
                BlockTemplate template = new BlockTemplate()
                {
                    TemplateName = "ClassTemplateStyle",
                    DisplayName = "校园风采",
                    Index = 2,
                    TemplateType = BlockTemplateType.Theme
                };

                BaseBlock block = service.GetBlock("ClassMultiMedia");
                block.Init(GetDesignatedAddonContext(context, "GS.Terminal.SmartBoard.Logic"));
                IUpdate update = (IUpdate)block;

                if (!File.Exists(media_json_filename))
                {
                    media_json_filename = Path.Combine(GetCachePath(), "BlockCache", media_json_filename);
                }

                update.LoadLocalData(media_json_filename);

                template.Blocks = new List<VisualBlockItem>
                {
                    new VisualBlockItem
                    {
                        Id = Guid.NewGuid(),
                        BlockComponent = "班级风采",
                        BlockTypeName = ((IBlock)block).TypeName,
                        DataSource = "Services/SmartBoard/BlockClassMultiMedia/json",
                        NavTemplateName = "",
                        Width = 20,
                        Height = 10,
                        X = 1,
                        Y = 1,
                        DataContext = (BaseBlock)update
                    }
                };

                template.Previous = vm.TemplateList[1];
                template.Next = vm.TemplateList[3];

                BlockTemplate t = template;
                DispatcherHelper.RunAsync(() => vm.TemplateList[2] = t);
            }
        }

        public static void RemoveVisualTemplate(IAddonContext context, string template_name, bool ignore_first = true)
        {
            MainWindowViewModel vm = SimpleIoc.Default.GetInstance<MainWindowViewModel>() ?? (MainWindowViewModel)GetViewModel(context, VM_ID_MAIN);
            if (vm == null) return;

            BlockTemplate blockTemplate = vm.TemplateList.FirstOrDefault((BlockTemplate ss) => ss.TemplateName == template_name);
            if (blockTemplate != vm.TemplateList.First<BlockTemplate>() || !ignore_first)
            {
                vm.RemoveTemplate(blockTemplate);
            }
        }

        public static string GetCachePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cache");
        }

        public static void ClearAllPosterTemplate(IAddonContext context)
        {
            MainWindowViewModel vm = SimpleIoc.Default.GetInstance<MainWindowViewModel>() ?? (MainWindowViewModel)GetViewModel(context, VM_ID_MAIN);

            foreach (BlockTemplate t in vm.TemplateList)
            {
                if (t.TemplateType == BlockTemplateType.Media)
                {
                    DispatcherHelper.RunAsync(() => vm.RemoveTemplate(t));
                }
            }
        }

        public static void WriteJson(IAddonContext context, string filename, string json_b64)
        {
            string cache = GetCachePath();
            if (!filename.EndsWith(".json"))
            {
                filename = filename + ".json";
            }
            if (!Directory.Exists(Path.Combine(cache, "BlockCache")))
            {
                Directory.CreateDirectory(Path.Combine(cache, "BlockCache"));
            }
            byte[] byteB64 = Convert.FromBase64String(json_b64);
            string content = Encoding.UTF8.GetString(byteB64);

            File.WriteAllText(Path.Combine(cache, "BlockCache", filename), content, Encoding.UTF8);
        }

        public static void DownloadFile(IAddonContext context, string url, string savename)
        {
            string cache = GetCachePath();
            string savepath = cache;

            if (savename.EndsWith(".png") || savename.EndsWith(".jpg") || savename.EndsWith(".bmp"))
            {
                savepath = Path.Combine(cache, "image", savename);
            }
            else if (savename.EndsWith(".flv") || savename.EndsWith(".mp4"))
            {
                savepath = Path.Combine(cache, "video", savename);
            }
            else
            {
                savepath = Path.Combine(savepath, "utils");
                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }
                savepath = Path.Combine(savepath, savename);
            }

            using (WebClient web = new WebClient())
            {
                try
                {
                    web.DownloadFileAsync(new Uri(url), savepath);
                }
                catch (Exception)
                {
                }
            }
        }

        public static void SetTheme(IAddonContext context, string name)
        {
            context.GetFirstOrDefaultService("GS.Terminal.Theme", "GS.Terminal.Theme.Service").SetTheme(name);
        }

        public static void StartProcess(IAddonContext context, string process)
        {
            string cache = GetCachePath();
            if (!(process.EndsWith(".exe") || process.EndsWith(".bat") || process.EndsWith(".cmd")))
            {
                process = process + ".exe";
            }
            if (!Directory.Exists(Path.Combine(cache, "utils")))
            {
                Directory.CreateDirectory(Path.Combine(cache, "utils"));
            }
            if (!File.Exists(process))
            {
                if (File.Exists(Path.Combine(cache, "utils", process)))
                {
                    process = Path.Combine(cache, "utils", process);
                }
                else
                {
                    return;
                }
            }

            try
            {
                Process.Start(process);
            }
            catch
            {
            }
        }

        private static IAddonContext GetDesignatedAddonContext(IAddonContext context, string name)
        {
            IAddon addon = AddonRuntime.Instance.GetInstalledAddons().FirstOrDefault(x => x.SymbolicName == name);
            return addon.Context;
        }
    }
}
