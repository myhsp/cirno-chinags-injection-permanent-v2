using GS.Unitive.Framework.Core;
using SmartBoardViewModels.Models.VisualBlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CirnoModifier
{
    /// <summary>
    /// ModifierPanel.xaml 的交互逻辑
    /// </summary>
    public partial class ModifierPanel : UserControl
    {
        ModifierFloatingWindowRegister Register = null;
        IAddonContext context = null;
        VisualBlockItem selectedBlock = null;

        public ModifierPanel(ModifierFloatingWindowRegister register, IAddonContext context)
        {
            Register = register;
            this.context = context;
            InitializeComponent();
        }

        public void LoadLists()
        {
            try
            {
                LoadVisualTemplateList();
            }
            catch (Exception ex)
            {
                context.Logger.Info("[CirnoModifier] Failed to load lists!", ex);
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            if (Register != null)
            {
                Register.HideModifierPanel();
            }
        }

        private void Btn_RefreshVTL_Click(object sender, RoutedEventArgs e)
        {
            LoadVisualTemplateList();
        }

        private void LoadVisualTemplateList()
        {
            VisualTemplateList.Items.Clear();
            string[] blockTemplateNames = Utils.GetBlockTemplates(this.context)
                    .Select(t => t.TemplateName).ToArray();

            foreach (string name in blockTemplateNames)
            {
                ListBoxItem item = new ListBoxItem();
                item.Name = name;
                item.Content = name;
                VisualTemplateList.Items.Add(item);
            }
        }

        private void LoadVisualBlockList()
        {
            VisualBlockList.Items.Clear();
            string visualTemplateName = ((ListBoxItem)VisualTemplateList.SelectedItem).Content.ToString();
            string[] blockNames = Utils.GetVisualBlockItems(
                Utils.GetBlockTemplate(
                    context, visualTemplateName
                    )
                )
                .Select(b => b.BlockTypeName).ToArray();

            foreach (string name in blockNames)
            {
                ListBoxItem item = new ListBoxItem();
                item.Name = name;
                item.Content = name;
                item.Tag = visualTemplateName;
                VisualBlockList.Items.Add(item);
            }
        }

        private void GetSelectedBlock()
        {
            string blockName = ((ListBoxItem)VisualBlockList.SelectedItem).Content.ToString();
            string templateName = ((ListBoxItem)VisualBlockList.SelectedItem).Tag.ToString();
            selectedBlock = Utils.GetBlockTemplate(context, templateName).Blocks.FirstOrDefault(b => b.BlockTypeName == blockName);
            SelectedVisualBlockLabel.Content = blockName;
        }

        private void Btn_RefreshVBL_Click(object sender, RoutedEventArgs e)
        {
            if (VisualTemplateList.SelectedItem != null)
            {
                LoadVisualBlockList();
            }
        }

        private void Btn_SelectVB_Click(object sender, RoutedEventArgs e)
        {
            if (VisualBlockList.SelectedItem != null)
            {
                GetSelectedBlock();
            }
        }

        private void Btn_SelectJson_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog() 
            {
                Filter = "Json files (.json)|*.txt|All files (*.*)|*.*"
            };

            bool result = dialog.ShowDialog() == true;
            if (result == true)
            {
                JsonSource.Text = dialog.FileName;
            }
        }

        private void Btn_LoadJson_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(JsonSource.Text))
            {
                Utils.SetVisualBlockItemJsonData(selectedBlock, JsonSource.Text);
            }
        }

        private void VisualTemplateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadVisualTemplateList();
        }

        private void VisualBlockList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadVisualBlockList();
        }
    }
}
