using System;
using System.IO;
using System.Windows.Forms;
using BrawlLib.SSBB;
using HSDRaw.Melee.Gr;

namespace HSDRawViewer.ContextMenus
{
    class GeneralPointsContextMenu : CommonContextMenu
    {
        public override Type[] SupportedTypes { get; } = new Type[] { typeof(SBM_GeneralPoints) };

        public GeneralPointsContextMenu() : base()
        {
            MenuItem ExportBrawlLib = new MenuItem("Export As Stage Position");
            ExportBrawlLib.Click += (sender, args) =>
            {
                using (SaveFileDialog sd = new SaveFileDialog())
                {
                    sd.Filter = SupportedFilesHandler.GetCompleteFilter("mdl0");
                    sd.FileName = Path.GetFileNameWithoutExtension(MainForm.Instance.FilePath);

                    if (sd.ShowDialog() == DialogResult.OK)
                    {
                        Converters.ConvStgPos.GeneralPointsToStgPos(sd.FileName, MainForm.SelectedDataNode.Accessor as SBM_GeneralPoints);
                    }
                }
            };
            MenuItems.Add(ExportBrawlLib);
        }
    }
}
