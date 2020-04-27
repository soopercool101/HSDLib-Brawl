﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HSDRaw.MEX;
using HSDRaw.Common;
using HSDRaw.MEX.Characters;
using HSDRaw;
using System.IO;
using HSDRaw.Melee.Pl;

namespace HSDRawViewer.GUI.MEX.Controls
{
    public partial class MEXFighterControl : UserControl, IMEXControl
    {
        /// <summary>
        /// 
        /// </summary>
        public MEX_Data MexData
        {
            get
            {
                var c = Parent;
                while (c != null && !(c is MexDataEditor)) c = c.Parent;
                if (c is MexDataEditor e) return e._data;
                return null;
            }
        }

        public MEXFighterEntry SelectedEntry { get => fighterList.SelectedItem as MEXFighterEntry; }

        public int SelectedIndex { get => fighterList.SelectedIndex; }

        /// <summary>
        /// 
        /// </summary>
        public int NumberOfEntries
        {
            get => FighterEntries.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        public BindingList<MEXFighterEntry> FighterEntries = new BindingList<MEXFighterEntry>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetControlName()
        {
            return "Fighter";
        }

        /// <summary>
        /// 
        /// </summary>
        public MEXFighterControl()
        {
            InitializeComponent();

            fighterList.DataSource = FighterEntries;

            FighterEntries.ListChanged += (sender, args) =>
            {
                MEXConverter.internalIDValues.Clear();
                MEXConverter.internalIDValues.Add("None");
                MEXConverter.internalIDValues.AddRange(FighterEntries.Select(e => e.NameText));
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void LoadData(MEX_Data data)
        {
            FighterEntries.Clear();
            for (int i = 0; i < data.MetaData.NumOfInternalIDs; i++)
            {
                FighterEntries.Add(new MEXFighterEntry().LoadData(data, i, MEXIdConverter.ToExternalID(i, data.MetaData.NumOfInternalIDs)));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void SaveData(MEX_Data d)
        {
            d.MetaData.NumOfExternalIDs = NumberOfEntries;
            d.MetaData.NumOfInternalIDs = NumberOfEntries;
            d.FighterData.NameText.Array = new HSD_String[0];
            d.FighterData.CharFiles.Array = new MEX_CharFileStrings[0];
            d.FighterData.CostumeIDs.Array = new MEX_CostumeIDs[0];
            d.FighterData.CostumeFileSymbols.Array = new MEX_CostumeFileSymbolTable[0];
            d.FighterData.AnimFiles.Array = new HSD_String[0];
            d.FighterData.AnimCount.Array = new MEX_AnimCount[0];
            d.FighterData.InsigniaIDs.Array = new HSD_Byte[0];
            d.FighterData.ResultAnimFiles.Array = new HSD_String[0];
            d.FighterData.ResultScale.Array = new HSD_Float[0];
            d.FighterData.VictoryThemeIDs.Array = new HSD_Int[0];
            d.FighterData.FtDemo_SymbolNames.Array = new MEX_FtDemoSymbolNames[0];
            d.FighterData.AnnouncerCalls.Array = new HSD_Int[0];
            d.FighterData.SSMFileIDs.Array = new MEX_CharSSMFileID[0];
            d.FighterData.EffectIDs.Array = new HSD_Byte[0];
            d.FighterData.CostumePointers.Array = new MEX_CostumeRuntimePointers[0];
            d.FighterData.DefineIDs.Array = new MEX_CharDefineIDs[0];
            d.FighterData.WallJump.Array = new HSD_Byte[0];
            d.FighterData.RstRuntime.Array = new MEX_RstRuntime[0];
            d.FighterData.FighterItemLookup.Array = new MEX_ItemLookup[0];
            d.FighterData.FighterEffectLookup.Array = new MEX_EffectTypeLookup[0];

            d.FighterData.TargetTestStageLookups = new HSDArrayAccessor<HSD_UShort>();
            d.FighterData.TargetTestStageLookups.Array = new HSD_UShort[0];

            d.FighterFunctions.OnLoad.Array = new HSD_UInt[0];
            d.FighterFunctions.OnDeath.Array = new HSD_UInt[0];
            d.FighterFunctions.OnUnknown.Array = new HSD_UInt[0];
            d.FighterFunctions.MoveLogic.Array = new HSDArrayAccessor<MEX_MoveLogic>[0];
            d.FighterFunctions.SpecialN.Array = new HSD_UInt[0];
            d.FighterFunctions.SpecialNAir.Array = new HSD_UInt[0];
            d.FighterFunctions.SpecialHi.Array = new HSD_UInt[0];
            d.FighterFunctions.SpecialHiAir.Array = new HSD_UInt[0];
            d.FighterFunctions.SpecialLw.Array = new HSD_UInt[0];
            d.FighterFunctions.SpecialLwAir.Array = new HSD_UInt[0];
            d.FighterFunctions.SpecialS.Array = new HSD_UInt[0];
            d.FighterFunctions.SpecialSAir.Array = new HSD_UInt[0];
            d.FighterFunctions.OnAbsorb.Array = new HSD_UInt[0];
            d.FighterFunctions.onItemPickup.Array = new HSD_UInt[0];
            d.FighterFunctions.onMakeItemInvisible.Array = new HSD_UInt[0];
            d.FighterFunctions.onMakeItemVisible.Array = new HSD_UInt[0];
            d.FighterFunctions.onItemDrop.Array = new HSD_UInt[0];
            d.FighterFunctions.onItemCatch.Array = new HSD_UInt[0];
            d.FighterFunctions.onUnknownItemRelated.Array = new HSD_UInt[0];
            d.FighterFunctions.onUnknownCharacterModelFlags1.Array = new HSD_UInt[0];
            d.FighterFunctions.onUnknownCharacterModelFlags2.Array = new HSD_UInt[0];
            d.FighterFunctions.onHit.Array = new HSD_UInt[0];
            d.FighterFunctions.onUnknownEyeTextureRelated.Array = new HSD_UInt[0];
            d.FighterFunctions.onFrame.Array = new HSD_UInt[0];
            d.FighterFunctions.onActionStateChange.Array = new HSD_UInt[0];
            d.FighterFunctions.onRespawn.Array = new HSD_UInt[0];
            d.FighterFunctions.onModelRender.Array = new HSD_UInt[0];
            d.FighterFunctions.onShadowRender.Array = new HSD_UInt[0];
            d.FighterFunctions.onUnknownMultijump.Array = new HSD_UInt[0];
            d.FighterFunctions.onActionStateChangeWhileEyeTextureIsChanged.Array = new HSD_UInt[0];
            d.FighterFunctions.onTwoEntryTable.Array = new HSD_UInt[0];
            d.FighterFunctions.onLand.Array = new HSD_UInt[0];

            d.FighterFunctions.onSmashDown.Array = new HSD_UInt[0];
            d.FighterFunctions.onSmashUp.Array = new HSD_UInt[0];
            d.FighterFunctions.onSmashForward.Array = new HSD_UInt[0];

            d.FighterFunctions.enterFloat.Array = new HSD_UInt[0];
            d.FighterFunctions.enterSpecialDoubleJump.Array = new HSD_UInt[0];
            d.FighterFunctions.enterTether.Array = new HSD_UInt[0];

            d.KirbyData.CapFiles.Array = new MEX_KirbyCapFiles[0];
            d.KirbyData.KirbyCostumes.Array = new MEX_KirbyCostume[0];
            d.KirbyData.EffectIDs.Array = new HSD_Byte[0];
            d.KirbyFunctions.OnAbilityGain.Array = new HSD_UInt[0];
            d.KirbyFunctions.OnAbilityLose.Array = new HSD_UInt[0];
            d.KirbyFunctions.KirbySpecialN.Array = new HSD_UInt[0];
            d.KirbyFunctions.KirbySpecialNAir.Array = new HSD_UInt[0];
            d.KirbyFunctions.KirbyOnHit.Array = new HSD_UInt[0];
            d.KirbyFunctions.KirbyOnItemInit.Array = new HSD_UInt[0];

            // runtime fighter pointer struct
            d.FighterData._s.GetReference<HSDAccessor>(0x40)._s.Resize(NumberOfEntries * 8);

            // kirby runtimes
            d.KirbyData.CapFileRuntime._s = new HSDStruct(4 * NumberOfEntries);
            d.KirbyData.CostumeRuntime._s = new HSDStruct(4);

            // dump data
            int index = 0;
            foreach (var v in FighterEntries)
            {
                v.SaveData(d, index, MEXIdConverter.ToExternalID(index, NumberOfEntries));
                index++;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void ResetDataBindings()
        {
            FighterEntries.ResetBindings();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="internalID"></param>
        public void RemoveFighterEntry(int internalID)
        {
            FighterEntries.RemoveAt(internalID);
        }

        /// <summary>
        /// Adds new entry to fighter list
        /// </summary>
        /// <param name="e"></param>
        /// <returns>internal ID</returns>
        public int AddEntry(MEXFighterEntry e)
        {
            FighterEntries.Insert(FighterEntries.Count - 6, e);
            return FighterEntries.Count - 6;
        }

        /// <summary>
        /// Removes item index from fighters and adjust indices to accommodate 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveItem(int index)
        {
            foreach (var v in FighterEntries)
            {
                foreach (var s in v.MEXItems)
                {
                    if (s.Value ==index)
                        s.Value = 0;
                    if (s.Value > index)
                        s.Value -= 1;
                }
            }
        }

        /// <summary>
        /// Removes effect index from fighters and adjust indices to accommodate
        /// </summary>
        /// <param name="index"></param>
        public void RemoveEffect(int index)
        {
            foreach (var v in FighterEntries)
            {
                if (v.EffectIndex == index)
                    v.EffectIndex = 0;
                if (v.EffectIndex > index)
                    v.EffectIndex--;
                if (v.KirbyEffectID == index)
                    v.KirbyEffectID = 0;
                if (v.KirbyEffectID > index)
                    v.KirbyEffectID--;
            }
        }

        /// <summary>
        /// Checks if effect index is currently in use by fighter
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool EffectInUse(int index)
        {
            return FighterEntries.Any(e => e.EffectIndex == index || e.KirbyEffectID == index);
        }

        /// <summary>
        /// Returns true if any fighter already uses given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool NameExists(string name)
        {
            foreach (var v in FighterEntries)
            {
                if (v.NameText.Equals(name))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if fighter at this index is an extended fighter
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsExtendedFighter(int index)
        {
            return (index >= 0x21 - 6 && index < FighterEntries.Count - 6);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsSpecialFighter(int index)
        {
            return (index >= FighterEntries.Count - 6);
        }



        #region Events
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveFightersButton_Click(object sender, EventArgs e)
        {
            SaveData(MexData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void propertyGrid2_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void fighterPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            FighterEntries.ResetBindings();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cloneButton_Click(object sender, EventArgs e)
        {
            if (fighterList.SelectedItem is MEXFighterEntry me)
            {
                var clone = ObjectExtensions.Copy(me);
                // give unique name
                int clnIndex = 0;
                if (NameExists(clone.NameText))
                {
                    while (NameExists(clone.NameText + " " + clnIndex.ToString())) clnIndex++;
                    clone.NameText = clone.NameText + " " + clnIndex;
                }
                AddEntry(clone);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportFighter_Click(object sender, EventArgs e)
        {
            if (fighterList.SelectedItem is MEXFighterEntry mex)
            {
                var f = Tools.FileIO.SaveFile("YAML (*.yaml)|*.yaml", mex.NameText + ".yaml");
                if (f != null)
                {
                    mex.Serialize(f);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importFighter_Click(object sender, EventArgs e)
        {
            var f = Tools.FileIO.OpenFile("YAML (*.yaml)|*.yaml");
            if (f != null)
            {
                FighterEntries.Insert(FighterEntries.Count - 6, MEXFighterEntry.DeserializeFile(f));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteFighter_Click(object sender, EventArgs e)
        {
            var selected = fighterList.SelectedIndex;
            if (IsExtendedFighter(selected))
            {
                FighterEntries.RemoveAt(selected);
                return;
            }
            MessageBox.Show("Unable to delete base game fighters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fighterList_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                e.DrawBackground();

                var brush = ApplicationSettings.SystemWindowTextColorBrush;

                if (IsExtendedFighter(e.Index))
                    brush = Brushes.DarkViolet;

                if (IsSpecialFighter(e.Index))
                    brush = ApplicationSettings.SystemWindowTextRedColorBrush;

                e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(),
                e.Font, brush, e.Bounds, StringFormat.GenericDefault);

                e.DrawFocusRectangle();
            }
            catch
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCopyMoveLogic_Click(object sender, EventArgs e)
        {
            if (fighterList.SelectedItem is MEXFighterEntry fighter)
            {
                var moveLogic = fighter.Functions.MoveLogic;

                var ftDataFile = Path.Combine(Path.GetDirectoryName(MainForm.Instance.FilePath), fighter.FighterDataPath);

                SBM_FighterData fighterData = null;

                if (File.Exists(ftDataFile))
                    fighterData = new HSDRawFile(ftDataFile).Roots[0].Data as SBM_FighterData;

                StringBuilder table = new StringBuilder();

                int index = 341;
                foreach (var m in moveLogic)
                {
                    table.AppendLine($"\t// State: {index} - " + (fighterData != null && m.AnimationID != -1 && fighterData.SubActionTable.Subactions[m.AnimationID].Name != null ? System.Text.RegularExpressions.Regex.Replace(fighterData.SubActionTable.Subactions[m.AnimationID].Name.Replace("_figatree", ""), @"Ply.*_Share_ACTION_", "") : "Animation: " + m.AnimationID.ToString("X")));
                    index++;
                    table.AppendLine(string.Format(
                        "\t{{" +
                        "\n\t\t{0, -12}// AnimationID" +
                        "\n\t\t0x{1, -10}// StateFlags" +
                        "\n\t\t0x{2, -10}// AttackID" +
                        "\n\t\t0x{3, -10}// BitFlags" +
                        "\n\t\t0x{4, -10}// AnimationCallback" +
                        "\n\t\t0x{5, -10}// IASACallback" +
                        "\n\t\t0x{6, -10}// PhysicsCallback" +
                        "\n\t\t0x{7, -10}// CollisionCallback" +
                        "\n\t\t0x{8, -10}// CameraCallback" +
                        "\n\t}},",
                m.AnimationID + ",",
                m.StateFlags.ToString("X") + ",",
                m.AttackID.ToString("X") + ",",
                m.BitFlags.ToString("X") + ",",
                m.AnimationCallBack.ToString("X") + ",",
                m.IASACallBack.ToString("X") + ",",
                m.PhysicsCallback.ToString("X") + ",",
                m.CollisionCallback.ToString("X") + ",",
                m.CameraCallback.ToString("X") + ","
                ));
                }

                Clipboard.SetText(
                    @"__attribute__((used))
static struct MoveLogic move_logic[] = {
" + table.ToString() + @"}; ");

                MessageBox.Show("Move Logic Table Copied to Clipboard");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fighterList_SelectedIndexChanged(object sender, EventArgs e)
        {
            fighterPropertyGrid.SelectedObject = fighterList.SelectedItem;
            functionPropertyGrid.SelectedObject = (fighterList.SelectedItem as MEXFighterEntry).Functions;
        }

        #endregion

    }
}
