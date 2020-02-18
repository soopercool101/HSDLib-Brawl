﻿using HSDRaw.Common.Animation;
using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace HSDRawViewer.GUI.Plugins
{
    public partial class AOBJEditor : DockContent, EditorBase
    {
        public DockState DefaultDockState => DockState.Document;

        public Type[] SupportedTypes => new Type[] { typeof(HSD_AOBJ) };

        public DataNode Node { get => _node;
            set
            {
                if (value.Accessor is HSD_AOBJ aobj)
                {
                    _node = value;
                    this.aobj = aobj;
                    RefreshList();
                }
            }
        }
        private DataNode _node;
        private HSD_AOBJ aobj;

        private KeyEditor keyEditor;

        public AOBJEditor()
        {
            InitializeComponent();

            keyEditor = new KeyEditor();
            keyEditor.Dock = DockStyle.Fill;

            cbAnimationType.DataSource = Enum.GetValues(typeof(JointTrackType));

            groupBox2.Controls.Add(keyEditor);
            keyEditor.BringToFront();
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshList()
        {
            treeView1.Nodes.Clear();
            foreach(var fobjDesc in aobj.FObjDesc.List)
            {
                treeView1.Nodes.Add(new TreeNode()
                {
                    Text = fobjDesc.AnimationType.ToString(),
                    Tag = fobjDesc
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(e.Node?.Tag is HSD_FOBJDesc desc)
            {
                cbAnimationType.SelectedItem = desc.AnimationType;
                keyEditor.SetKeys(desc.GetDecodedKeys());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is HSD_FOBJDesc desc)
            {
                desc.SetKeys(keyEditor.GetFOBJKeys(), (JointTrackType)(cbAnimationType.SelectedItem));
                treeView1.SelectedNode.Text = desc.AnimationType.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var FOBJDesc = new HSD_FOBJDesc();
            FOBJDesc.SetKeys(new System.Collections.Generic.List<HSDRaw.Tools.FOBJKey>()
            {
                new HSDRaw.Tools.FOBJKey()
                {
                    Frame = 0,
                    InterpolationType = GXInterpolationType.HSD_A_OP_CON
                },
                new HSDRaw.Tools.FOBJKey()
                {
                    Frame = aobj.EndFrame,
                    InterpolationType = GXInterpolationType.HSD_A_OP_CON
                }
            }, JointTrackType.HSD_A_J_NODE);

            if (aobj.FObjDesc == null)
                aobj.FObjDesc = FOBJDesc;
            else
                aobj.FObjDesc.Add(FOBJDesc);

            RefreshList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;

            var selected = treeView1.SelectedNode.Tag as HSD_FOBJDesc;

            if (aobj.FObjDesc == selected)
                aobj.FObjDesc = selected.Next;
            else
            {
                foreach (var v in aobj.FObjDesc.List)
                {
                    if (v.Next == selected)
                    {
                        v.Next = selected.Next;
                        break;
                    }
                }
            }

            RefreshList();
        }
    }
}