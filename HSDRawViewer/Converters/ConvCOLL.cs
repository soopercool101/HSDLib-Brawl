using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrawlLib.Internal;
using BrawlLib.SSBB.ResourceNodes;
using HSDRaw.Melee.Gr;

namespace HSDRawViewer.Converters
{
    class ConvCOLL
    {
        public static void CollDataToBrawl(string filename, SBM_Coll_Data colldata)
        {
            CollisionNode c = new CollisionNode();
            foreach (SBM_CollLineGroup g in colldata.LineGroups)
            {
                CollisionObject obj = new CollisionObject {Independent = true};
                for (int i = g.TopLineIndex; i < g.TopLineCount + g.TopLineIndex; i++)
                {
                    generateCollision(i, colldata, obj);
                }
                for (int i = g.BottomLineIndex; i < g.BottomLineCount + g.BottomLineIndex; i++)
                {
                    generateCollision(i, colldata, obj);
                }
                for (int i = g.LeftLineIndex; i < g.LeftLineCount + g.LeftLineIndex; i++)
                {
                    generateCollision(i, colldata, obj);
                }
                for (int i = g.RightLineIndex; i < g.RightLineCount + g.RightLineIndex; i++)
                {
                    generateCollision(i, colldata, obj);
                }
                obj.FixLedges();
                c.AddChild(obj);
            }
            c.Export(filename);
        }

        public static void generateCollision(int index, SBM_Coll_Data colldata, CollisionObject obj)
        {
            SBM_CollLine link = colldata.Links[index];
            Vector2 posL = new Vector2(colldata.Vertices[link.VertexIndex1].X,
                colldata.Vertices[link.VertexIndex1].Y);
            Vector2 posR = new Vector2(colldata.Vertices[link.VertexIndex2].X,
                colldata.Vertices[link.VertexIndex2].Y);
            CollisionLink l;
            CollisionLink r;
            if ((l = obj.FindLink(posL)) == null)
            {
                l = new CollisionLink(obj, posL);
            }
            if ((r = obj.FindLink(posR)) == null)
            {
                r = new CollisionLink(obj, posR);
            }
            CollisionPlane p = new CollisionPlane(obj, l, r);
            switch (link.Material)
            {
                case CollMaterial.Rock:
                    p._material = 1;
                    break;
                case CollMaterial.Grass:
                    p._material = 2;
                    break;
                case CollMaterial.Dirt:
                    p._material = 3;
                    break;
                case CollMaterial.Wood:
                    p._material = 4;
                    break;
                case CollMaterial.LightMetal:
                    p._material = 5;
                    break;
                case CollMaterial.GreatBay: // Closest
                case CollMaterial.HeavyMetal:
                    p._material = 6;
                    break;
                case CollMaterial.UnkFlatZone:
                case CollMaterial.FlatZone:
                    p._material = 0x0F;
                    break;
                case CollMaterial.AlienGoop:
                    p._material = 8;
                    break;
                case CollMaterial.Water:
                    p._material = 0x0A;
                    break;
                case CollMaterial.Checkered:
                    p._material = 0x11;
                    break;
                case CollMaterial.Basic:
                case CollMaterial.Glass: // No analogous
                case CollMaterial.Unknown9:
                case CollMaterial.Unknown11:
                case CollMaterial.Unknown14:
                case CollMaterial.Unknown15:
                case CollMaterial.Unknown17:
                default:
                    p._material = 0;
                    break;
            }
            p.IsFloor = (link.CollisionFlag & ~CollPhysics.Top) == 0;
            p.IsLeftWall = (link.CollisionFlag & ~CollPhysics.Left) == 0;
            p.IsRightWall = (link.CollisionFlag & ~CollPhysics.Right) == 0;
            p.IsCeiling = (link.CollisionFlag & ~CollPhysics.Bottom) == 0;
            p.IsCharacters = true;
            if (p.IsFloor)
            {
                p.IsFallThrough = (link.Flag & ~CollProperty.LedgeGrab) != 0;
                // Ledges are universal in melee, can be fixed afterwards
                p.IsLeftLedge = p.IsRightLedge = (link.Flag & ~CollProperty.DropThrough) != 0;
            }

            if (p.IsWall)
            {
                p.IsNoWalljump = (link.Flag & ~CollProperty.LedgeGrab) == 0;
            }
        }
    }
}
