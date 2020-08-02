﻿using HSDRaw.Melee.Gr;
using HSDRawViewer.Converters.Melee;
using System;
using System.IO;
using System.Windows.Forms;
using BrawlLib.SSBB;

namespace HSDRawViewer.ContextMenus
{
    public class CollDataContextMenu : CommonContextMenu
    {
        public override Type[] SupportedTypes { get; } = new Type[] { typeof(SBM_Coll_Data) };

        public CollDataContextMenu() : base()
        {
            MenuItem Export = new MenuItem("Export As SVG");
            Export.Click += (sender, args) =>
            {
                using (SaveFileDialog sd = new SaveFileDialog())
                {
                    sd.Filter = "Scalable Vector Graphics (.svg)|*.svg";

                    if (sd.ShowDialog() == DialogResult.OK)
                    {
                        Converters.ConvSVG.CollDataToSVG(sd.FileName, MainForm.SelectedDataNode.Accessor as SBM_Coll_Data);
                    }
                }
            };
            MenuItems.Add(Export);


            MenuItem ImportSSF = new MenuItem("Import SSF");
            ImportSSF.Click += (sender, args) =>
            {
                using (OpenFileDialog sd = new OpenFileDialog())
                {
                    sd.Filter = "Smash Stage File (.ssf)|*.ssf";

                    if (sd.ShowDialog() == DialogResult.OK)
                        SSFConverter.ImportCollDataFromSSF(MainForm.SelectedDataNode.Accessor as SBM_Coll_Data, SSF.Open(sd.FileName));
                }
            };
            MenuItems.Add(ImportSSF);

            MenuItem ExportSSF = new MenuItem("Export SSF");
            ExportSSF.Click += (sender, args) =>
            {
                SSFConverter.ExportCollDataToSSF(MainForm.SelectedDataNode.Accessor as SBM_Coll_Data);
            };
            MenuItems.Add(ExportSSF);

            MenuItem ExportBrawlLib = new MenuItem("Export As COLL");
            ExportBrawlLib.Click += (sender, args) =>
            {
                using (SaveFileDialog sd = new SaveFileDialog())
                {
                    sd.Filter = FileFilters.CollisionDef;
                    sd.FileName = Path.GetFileNameWithoutExtension(MainForm.Instance.FilePath);

                    if (sd.ShowDialog() == DialogResult.OK)
                    {
                        Converters.ConvCOLL.CollDataToBrawl(sd.FileName, MainForm.SelectedDataNode.Accessor as SBM_Coll_Data);
                    }
                }
            };
            MenuItems.Add(ExportBrawlLib);
        }
    }
}
