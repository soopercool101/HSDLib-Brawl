﻿using HSDRaw.Melee.Gr;
using System;
using System.Windows.Forms;

namespace HSDRawViewer.ContextMenus
{
    public class MapHeadContextMenu : CommonContextMenu
    {
        public override Type[] SupportedTypes { get; } = new Type[] { typeof(SBM_Map_Head) };

        public MapHeadContextMenu() : base()
        {
            MenuItem OpenAsAJ = new MenuItem("Import Model Group");
            OpenAsAJ.Click += (sender, args) =>
            {
                if (MainForm.SelectedDataNode.Accessor is SBM_Map_Head)
                {
                    MainForm.SelectedDataNode.ImportModelGroup();
                }
            };
            MenuItems.Add(OpenAsAJ);
        }
    }
}
