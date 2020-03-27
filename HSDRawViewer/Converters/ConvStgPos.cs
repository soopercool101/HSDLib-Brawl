using System.Collections.Generic;
using BrawlLib.Internal;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Models;
using HSDRaw.Common;
using HSDRaw.Melee.Gr;

namespace HSDRawViewer.Converters
{
    class ConvStgPos
    {
        public static void GeneralPointsToStgPos(string filename, SBM_GeneralPoints points)
        {
            List<HSD_JOBJ> jobj = points.JOBJReference.BreathFirstList;

            MDL0Node m = new MDL0Node {Name = "stagePosition"};
            if (m._boneGroup == null)
            {
                MDL0GroupNode g = m._boneGroup;
                if (g == null)
                {
                    m.AddChild(g = new MDL0GroupNode(MDLResourceType.Bones), true);
                    m._boneGroup = g;
                    m._boneList = g.Children;
                }
            }

            MDL0BoneNode b = new MDL0BoneNode {Name = "stagePosition", Scale = new Vector3(1, 1, 1)};
            m._boneGroup.AddChild(b);
            foreach (SBM_GeneralPointInfo point in points.Points)
            {
                HSD_JOBJ original = jobj[point.JOBJIndex];
                MDL0BoneNode newBone = new MDL0BoneNode();
                newBone.Scale = new Vector3(original.SX, original.SY, original.SZ);
                newBone.Translation = new Vector3(original.TX, original.TY, original.TZ);
                newBone.Rotation = new Vector3(original.RX, original.RY, original.RZ);
                MDL0BoneNode newCopy = newBone.Clone();
                switch (point.Type)
                {
                    case PointType.DeltaAngleCamera:
                        newBone.Name = "CamCtrlN";
                        break;
                    case PointType.TopLeftBoundary:
                    case PointType.BottomRightBoundary:
                        newBone.Name = $"CamLimit{(int) point.Type - 149}N";
                        break;
                    case PointType.TopLeftBlastZone:
                    case PointType.BottomRightBlastZone:
                        newBone.Name = $"Dead{(int)point.Type - 151}N";
                        break;
                    case PointType.ItemSpawn1:
                    case PointType.ItemSpawn2:
                    case PointType.ItemSpawn3:
                    case PointType.ItemSpawn4:
                    case PointType.ItemSpawn5:
                    case PointType.ItemSpawn6:
                    case PointType.ItemSpawn7:
                    case PointType.ItemSpawn8:
                    case PointType.ItemSpawn9:
                    case PointType.ItemSpawn10:
                        newBone.Name = $"Item{(int)point.Type - 127}E";
                        newCopy.Name = $"Item{(int)point.Type - 127}N";
                        break;
                    case PointType.Player1Spawn:
                    case PointType.Player2Spawn:
                    case PointType.Player3Spawn:
                    case PointType.Player4Spawn:
                        newBone.Name = $"Player{(int)point.Type}E";
                        newCopy.Name = $"Player{(int)point.Type}N";
                        break;
                    case PointType.Player1Respawn:
                    case PointType.Player2Respawn:
                    case PointType.Player3Respawn:
                    case PointType.Player4Respawn:
                        newBone.Name = $"Rebirth{(int)point.Type - 4}E";
                        newCopy.Name = $"Rebirth{(int)point.Type - 4}N";
                        break;
                    //default:
                    //    newBone.Name = point.Type.ToString();
                    //    break;
                }

                if (!string.IsNullOrEmpty(newBone.Name) && !newBone.Name.Equals("<null>"))
                {
                    b.AddChild(newBone);
                }

                if (!string.IsNullOrEmpty(newCopy.Name) && !newCopy.Name.Equals("<null>"))
                {
                    b.AddChild(newCopy);
                }
            }
            m.Export(filename);
        }
    }
}
