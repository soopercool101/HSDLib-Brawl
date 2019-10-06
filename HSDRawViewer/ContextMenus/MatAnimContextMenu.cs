﻿using HSDRaw.Common.Animation;
using System;
using System.Windows.Forms;

namespace HSDRawViewer.ContextMenus
{
    public class MatAnimContextMenu : CommonContextMenu
    {
        public override Type[] SupportedTypes { get; } = new Type[] { typeof(HSD_MatAnim) };

        public MatAnimContextMenu() : base()
        {
            MenuItem OpenAsAJ = new MenuItem("Add Texture Anim");
            OpenAsAJ.Click += (sender, args) =>
            {
                if (MainForm.SelectedDataNode.Accessor is HSD_MatAnim matanim)
                {
                    matanim.TextureAnimation = new HSD_TexAnim();
                    matanim.TextureAnimation.AnimationObject = new HSD_AOBJ();
                    matanim.TextureAnimation.AnimationObject.FObjDesc = new HSD_FOBJDesc();
                    MainForm.SelectedDataNode.Refresh();
                }
            };
            MenuItems.Add(OpenAsAJ);
        }
    }
}
