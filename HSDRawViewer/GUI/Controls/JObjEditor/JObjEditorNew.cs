﻿using HSDRaw.Common;
using HSDRaw.Common.Animation;
using HSDRaw.Tools;
using HSDRawViewer.Converters;
using HSDRawViewer.GUI.Dialog;
using HSDRawViewer.Rendering;
using HSDRawViewer.Rendering.Animation;
using HSDRawViewer.Rendering.GX;
using HSDRawViewer.Rendering.Models;
using HSDRawViewer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace HSDRawViewer.GUI.Controls.JObjEditor
{
    public partial class JObjEditorNew : UserControl, IDrawable
    {
        public DrawOrder DrawOrder => DrawOrder.First;

        private DockablePropertyGrid _propertyGrid;

        private DockableJointTree _jointTree;

        private DockableMeshList _meshList;

        private DockableTextureEditor _textureEditor;

        private DockableTrackEditor _trackEditor;

        private DockableViewport _viewport;

        private RenderJObj RenderJObj;

        private HSD_JOBJ _root;

        public float Frame { get => _viewport.glViewport.Frame; }

        /// <summary>
        /// 
        /// </summary>
        public JObjEditorNew()
        {
            InitializeComponent();

            // initial theme
            dockPanel1.Theme = new VS2015LightTheme();

            // initialize viewport
            _viewport = new DockableViewport();
            _viewport.Show(dockPanel1, DockState.Document);

            // refresh render when viewport reloads
            _viewport.glViewport.Load += (r, a) =>
            {
                RenderJObj.Invalidate();
            };

            // initialize texture editor
            _trackEditor = new DockableTrackEditor();
            _trackEditor.Show(dockPanel1);
            _trackEditor.DockState = DockState.DockBottom;
            _trackEditor.Hide();

            _trackEditor.TracksUpdated += () =>
            {
                RenderJObj.RootJObj.ResetTransforms();
                ApplyEditorAnimation(Frame);
            };

            // initialize joint tree
            _jointTree = new DockableJointTree();
            _jointTree.Show(dockPanel1, DockState.DockLeft);

            _jointTree.SelectJObj += (name, jobj) =>
            {
                if (jobj.jobj.Flags.HasFlag(JOBJ_FLAG.PTCL))
                    _propertyGrid.SetObject(new JObjParticlePropertyAccessor(jobj.jobj));
                else
                if (jobj.jobj.Flags.HasFlag(JOBJ_FLAG.SPLINE))
                    _propertyGrid.SetObject(new JObjSplinePropertyAccessor(jobj.jobj));
                else
                    _propertyGrid.SetObject(new JObjPropertyAccessor(jobj.jobj));

                RenderJObj.SelectedJObj = jobj.jobj;

                _trackEditor.SetKeys(name, GraphEditor.AnimType.Joint, jobj.Tracks);
            };

            // initialize meshlist
            _meshList = new DockableMeshList();
            _meshList.Show(dockPanel1, DockState.DockLeft);

            _meshList.SelectDObj += (dobj, indices) =>
            {
                _propertyGrid.SetObjects(dobj);

                RenderJObj.ClearDObjSelection();
                foreach (var i in indices)
                    RenderJObj.SetDObjSelected(i, true);

                // set textures in editor
                if (dobj.Length == 1)
                    _textureEditor.SetTextures(dobj[0]);
                else
                    _textureEditor.SetTextures(null);
            };

            _meshList.VisibilityUpdated += () =>
            {
                UpdateVisibility();
            };

            _meshList.ListUpdated += () =>
            {
                RenderJObj.Invalidate();
                UpdateVisibility();

                // set materials in editor
                _textureEditor.SetTextures(null);
            };

            // initialize properties
            _propertyGrid = new DockablePropertyGrid();
            _propertyGrid.Show(dockPanel1);
            _propertyGrid.DockState = DockState.DockRight;

            _propertyGrid.PropertyValueUpdated += (sender, args) =>
            {
                // update all flags
                UpdateFlagsAll();

                // redraw mesh list
                _meshList.Invalidate();

                // update jobj if changed
                if (_propertyGrid.SelectedObject is JObjProxy j)
                {
                    RenderJObj.RootJObj?.GetJObjFromDesc(j.jobj).ResetTransforms();
                }

                // update jobj if changed
                if (_propertyGrid.SelectedObject is DObjProxy d)
                {
                    RenderJObj.ResetDefaultStateMaterial();
                }
            };

            // initialize texture editor
            _textureEditor = new DockableTextureEditor();
            _textureEditor.Show(dockPanel1);
            _textureEditor.DockState = DockState.DockLeft;

            _textureEditor.SelectTObj += (tobj) =>
            {
                _propertyGrid.SetObject(tobj);
            };

            _textureEditor.InvalidateTexture += () =>
            {
                RenderJObj.Invalidate();
            };

            // add to renderer
            _viewport.glViewport.AddRenderer(this);

            _viewport.glViewport.FrameChange += (f) =>
            {
                ApplyEditorAnimation(f);
            };

            // initialize joint manager
            RenderJObj = new RenderJObj();

            // setup params
            renderModeBox.ComboBox.DataSource = Enum.GetValues(typeof(RenderMode));
            viewModeBox.ComboBox.DataSource = Enum.GetValues(typeof(ObjectRenderMode));
            viewModeBox.ComboBox.SelectedIndex = 1;

            renderModeBox.SelectedIndex = 0;
            showBonesToolStripMenuItem.Checked = RenderJObj._settings.RenderBones;
            showBoneOrientationToolStripMenuItem.Checked = RenderJObj._settings.RenderOrientation;
            showSelectionOutlineToolStripMenuItem.Checked = RenderJObj._settings.OutlineSelected;
            showSplinesToolStripMenuItem.Checked = RenderJObj._settings.RenderSplines;
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateFlagsAll()
        {
            if (_root != null)
            {
                _root.UpdateFlags();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frame"></param>
        private void ApplyEditorAnimation(float frame)
        {
            // joints
            foreach (var p in _jointTree.EnumerateJoints())
            {
                RenderJObj.RootJObj.GetJObjFromDesc(p.jobj).ApplyAnimation(p.Tracks, frame);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jointAnim"></param>
        public void LoadAnimation(JointAnimManager animation)
        {
            // reset skeleton
            RenderJObj.RootJObj?.ResetTransforms();

            // add any missing nodes
            while (animation.Nodes.Count < RenderJObj.RootJObj.JointCount)
                animation.Nodes.Add(new AnimNode());

            // set animation
            for (int i = 0; i < animation.NodeCount; i++)
            {
                var pro = _jointTree.GetProxyAtIndex(i);

                if (pro != null)
                {
                    pro.Tracks.Clear();
                    pro.Tracks.AddRange(animation.Nodes[i].Tracks);
                }
            }

            // apply animation
            ApplyEditorAnimation(Frame);

            // bring joint tree to front
            _jointTree.Show();

            // enable animation view
            EnableAnimation();

            // clear track editor
            _trackEditor.SetKeys("", GraphEditor.AnimType.Joint, null);
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadAnimation(MatAnimManager matanim)
        {
            // set animation
            // TODO: MaterialAnimation = matanim;

            // enable animation view
            EnableAnimation();
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadAnimation(ShapeAnimManager shapeanim)
        {
            // set animation
            // TODO: ShapeAnimation = shapeanim;

            // enable animation view
            EnableAnimation();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearAnimation()
        {
            // clear animations
            foreach (var j in _jointTree.EnumerateJoints())
                j.Tracks.Clear();

            // reset joint animation
            RenderJObj?.RootJObj?.ResetTransforms();

            // disable viewport
            var vp = _viewport.glViewport;
            vp.AnimationTrackEnabled = false;

            // hide track editor
            _trackEditor.SetKeys("", GraphEditor.AnimType.Texture, null);
            _trackEditor.Hide();
        }

        /// <summary>
        /// 
        /// </summary>
        private void EnableAnimation()
        {
            // enable viewport
            var vp = _viewport.glViewport;

            // enable view if not already
            if (!vp.AnimationTrackEnabled)
            {
                vp.AnimationTrackEnabled = true;
                vp.Frame = 0;
                vp.MaxFrame = 0;

                // show track editor
                _trackEditor.Show();

                // hide texture panel
                _textureEditor.DockState = DockState.DockLeftAutoHide;
            }

            // calculate max frame count
            vp.MaxFrame = 0;

            vp.MaxFrame = Math.Max(vp.MaxFrame, _jointTree.EnumerateJoints().Max(e => e.Tracks.Count > 0 ? e.Tracks.Max(r => r.FrameCount) : 0));

            // TODO: find end frame
            //if (MaterialAnimation != null)
            //    vp.MaxFrame = Math.Max(vp.MaxFrame, MaterialAnimation.FrameCount);

            //if (ShapeAnimation != null)
            //    vp.MaxFrame = Math.Max(vp.MaxFrame, ShapeAnimation.FrameCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="windowWidth"></param>
        /// <param name="windowHeight"></param>
        public void Draw(Camera cam, int windowWidth, int windowHeight)
        {
            //RenderJObj.Frame = _viewport.glViewport.Frame;
            RenderJObj._settings.RenderSplines = showSplinesToolStripMenuItem.Checked;
            RenderJObj._settings.RenderBones = showBonesToolStripMenuItem.Checked;
            RenderJObj._settings.RenderOrientation = showBoneOrientationToolStripMenuItem.Checked;
            RenderJObj._settings.OutlineSelected = showSelectionOutlineToolStripMenuItem.Checked;
            RenderJObj.Render(cam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobj"></param>
        public void SetJOBJ(HSD_JOBJ jobj)
        {
            _root = jobj;
            _jointTree.SetJObj(jobj);
            _meshList.SetJObj(jobj);
            RenderJObj.LoadJObj(jobj);
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateVisibility()
        {
            int index = 0;
            Dictionary<int, int> dobjindex = new Dictionary<int, int>();
            foreach (var c in _meshList.EnumerateDObjs)
            {
                // show or hide
                RenderJObj.SetDObjVisible(index, c.Visible);

                // also update dobj index
                if (!dobjindex.ContainsKey(c.JOBJIndex))
                    dobjindex.Add(c.JOBJIndex, 0);

                // update dobj index?
                c.DOBJIndex = dobjindex[c.JOBJIndex];
                dobjindex[c.JOBJIndex] += 1;

                index += 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fog"></param>
        public void SetFog(HSD_FogDesc fog)
        {
            RenderJObj._fogParam.LoadFromHSD(fog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="camera"></param>
        public void SetCamera(HSD_Camera camera)
        {
            _viewport.glViewport.Camera.LoadFromHSD(camera);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewModeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RenderJObj != null)
            {
                RenderJObj._settings.RenderObjects = (ObjectRenderMode)(viewModeBox.SelectedIndex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void renderModeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RenderJObj != null)
            {
                RenderJObj.RenderMode = (RenderMode)renderModeBox.SelectedIndex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importModelFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelImporter.ReplaceModelFromFile(_root);
            SetJOBJ(_root);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportModelToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelExporter.ExportFile(_root, _jointTree._jointMap);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importSceneSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = FileIO.OpenFile("Scene (*.yaml)|*.yml");

            if (f != null)
                LoadSceneYAML(f);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportSceneSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = FileIO.SaveFile("Scene (*.yaml)|*.yml");

            if (f != null)
            {
                SceneSettings settings = new SceneSettings()
                {
                    Frame = _viewport.glViewport.Frame,
                    CSPMode = _viewport.glViewport.CSPMode,
                    ShowGrid = _viewport.glViewport.DisplayGrid,
                    ShowBackdrop = _viewport.glViewport.EnableBack,
                    Camera = _viewport.glViewport.Camera,
                    Lighting = RenderJObj._lightParam,
                    Settings = RenderJObj._settings,
                    // TODO: Animation = JointAnimation,
                    HiddenNodes = _meshList.EnumerateDObjs.Where(e => !e.Visible).Select((i, e) => e).ToArray(),
                };
                settings.Serialize(f);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public void LoadSceneYAML(string filePath)
        {
            var settings = SceneSettings.Deserialize(filePath);

            _viewport.glViewport.CSPMode = settings.CSPMode;

            if (settings.CSPMode && showSelectionOutlineToolStripMenuItem.Checked)
                showSelectionOutlineToolStripMenuItem.PerformClick();

            _viewport.glViewport.EnableBack = settings.ShowBackdrop;
            _viewport.glViewport.DisplayGrid = settings.ShowGrid;

            if (settings.Camera != null)
                _viewport.glViewport.Camera = settings.Camera;

            if (settings.Settings != null)
                RenderJObj._settings = settings.Settings;

            if (settings.Lighting != null)
                RenderJObj._lightParam = settings.Lighting;

            if (settings.Animation != null)
            {
                // load animations
                LoadAnimation(settings.Animation);

                // load material animation if exists
                var symbol = MainForm.SelectedDataNode.Text.Replace("_joint", "_matanim_joint");
                var matAnim = MainForm.Instance.GetSymbol(symbol);
                if (matAnim != null && matAnim is HSD_MatAnimJoint maj)
                {
                    var a = new MatAnimManager();
                    a.FromMatAnim(maj);
                    LoadAnimation(a);
                }

                // set frames
                _viewport.glViewport.Frame = settings.Frame;
            }

            if (settings.HiddenNodes != null)
            {
                int i = 0;
                foreach (var d in _meshList.EnumerateDObjs)
                {
                    d.Visible = !settings.HiddenNodes.Contains(i);
                    i++;
                }
                _meshList.Invalidate();
                UpdateVisibility();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displaySettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PropertyDialog d = new PropertyDialog("Light Settings", RenderJObj._lightParam))
                d.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fogSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PropertyDialog d = new PropertyDialog("Fog Settings", RenderJObj._fogParam))
                d.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var anim = JointAnimManager.LoadFromFile(_jointTree._jointMap);

            if (anim != null)
            {
                LoadAnimation(anim);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearAnimation();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private JointAnimManager ToJointAnim()
        {
            JointAnimManager m = new JointAnimManager();

            foreach (var n in _jointTree.EnumerateJoints())
                m.Nodes.Add(new AnimNode() { Tracks = n.Tracks });

            return m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ToJointAnim().ExportAsMayaAnim(_jointTree._jointMap);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ToJointAnim().ExportAsFigatree();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (_root != null)
                ToJointAnim().ExportAsAnimJoint(_root);
        }


        /// <summary>
        /// 
        /// </summary>
        public class FrameSpeedModifierSettings
        {
            public List<FrameSpeedMultiplier> Modifiers { get; set; } = new List<FrameSpeedMultiplier>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fSMApplyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_trackEditor.IsHidden)
                return;

            var mult = new FrameSpeedModifierSettings();

            using (PropertyDialog d = new PropertyDialog("Frame Speed Multipler Settings", mult))
                if (d.ShowDialog() == DialogResult.OK)
                {
                    foreach (var j in _jointTree.EnumerateJoints())
                        foreach (var i in j.Tracks)
                            i.ApplyFSMs(mult.Modifiers);

                    // recalculat frame count
                    EnableAnimation();

                    // re apply animation
                    ApplyEditorAnimation(Frame);
                }
        }


        /// <summary>
        /// 
        /// </summary>
        public class AnimationCreationSettings
        {
            public int FrameCount { get; set; } = 60;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settings = new AnimationCreationSettings();
            using (PropertyDialog d = new PropertyDialog("Create Animation", settings))
            {
                if (d.ShowDialog() == DialogResult.OK && settings.FrameCount > 0)
                {
                    // enable animation editing
                    EnableAnimation();

                    // set the new max frame
                    var vp = _viewport.glViewport;
                    vp.MaxFrame = settings.FrameCount;
                }
            }
        }
    }
}
