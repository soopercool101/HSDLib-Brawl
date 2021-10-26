using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrawlLib.Internal;
using BrawlLib.SSBB.ResourceNodes;
using HSDRaw.Melee.Gr;
using HSDRawViewer.ContextMenus;

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
                    generateCollision(i, colldata, obj, false);
                }
                for (int i = g.BottomLineIndex; i < g.BottomLineCount + g.BottomLineIndex; i++)
                {
                    generateCollision(i, colldata, obj, false);
                }
                for (int i = g.LeftLineIndex; i < g.LeftLineCount + g.LeftLineIndex; i++)
                {
                    generateCollision(i, colldata, obj, false);
                }
                for (int i = g.RightLineIndex; i < g.RightLineCount + g.RightLineIndex; i++)
                {
                    generateCollision(i, colldata, obj, false);
                }
                obj.FixLedges();
                if (obj._planes.Count > 0)
                {
                    c.AddChild(obj);
                }

                obj = new CollisionObject {Independent = true};
                for (int i = g.DynamicLineIndex; i < g.DynamicLineCount + g.DynamicLineIndex; i++)
                {
                    generateCollision(i, colldata, obj, true);
                }
                if (obj._planes.Count > 0)
                {
                    c.AddChild(obj);
                }
            }
            c.Export(filename);
        }

        public static void generateCollision(int index, SBM_Coll_Data colldata, CollisionObject obj, bool dynamic)
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
                    p._material = 1; // Rock
                    break;
                case CollMaterial.Grass:
                    p._material = 2; // Grass
                    break;
                case CollMaterial.Dirt:
                    p._material = 3; // Soil
                    break;
                case CollMaterial.Wood:
                    p._material = 4; // Wood
                    break;
                case CollMaterial.LightMetal:
                    p._material = 5; // Light Metal
                    break;
                case CollMaterial.HeavyMetal:
                    p._material = 6; // Heavy Metal
                    break;
                case CollMaterial.Cloth:
                case CollMaterial.Felt:
                    p._material = 7; // Carpet
                    break;
                case CollMaterial.FlatZone:
                    p._material = 0x0F; // Flat Zone
                    break;
                case CollMaterial.AlienGoop:
                    p._material = 8; // Alien
                    break;
                case CollMaterial.Water:
                    p._material = 0x0A; // Water
                    break;
                case CollMaterial.Cardboard:
                    p._material = 0x11; // Checkered (matches same plat)
                    break;
                case CollMaterial.Snow:
                    p._material = 0x0D; // Snow
                    break;
                case CollMaterial.Ice:
                    p._material = 0x15; // Ice
                    break;
                default:
                    p._material = 0;
                    break;
            }

            if (dynamic)
            {
                p.IsFloor = true;
                p.IsRotating = true;
                p.IsCharacters = true;
                p.IsFallThrough = (link.Flag & ~CollProperty.LedgeGrab) != 0;
                // Ledges are universal in melee, can be fixed afterwards
                p.IsLeftLedge = p.IsRightLedge = (link.Flag & ~CollProperty.DropThrough) != 0;
                p.IsNoWalljump = (link.Flag & ~CollProperty.LedgeGrab) != 0;
            }
            else
            {
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
                    p.IsNoWalljump = (link.Flag & ~CollProperty.LedgeGrab) != 0;
                }
            }
        }
    }
}
