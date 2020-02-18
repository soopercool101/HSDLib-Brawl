﻿using OpenTK;
using System;
using System.Collections.Generic;
using HSDRawViewer.Tools;
using HSDRaw;

namespace HSDRawViewer.GUI.Plugins.Melee
{
    /// <summary>
    /// Handles and processes subaction data for rendering
    /// </summary>
    public class SubactionProcessor
    {
        public class Command
        {
            public Subaction Action;

            public int[] Parameters;

            public HSDStruct Reference;

            public List<Command> ReferenceCommands = new List<Command>();
        }

        private List<Command> Commands = new List<Command>();

        public List<Hitbox> Hitboxes { get; internal set; } = new List<Hitbox>();

        public Vector3 OverlayColor { get; internal set; } = Vector3.One;

        public bool CharacterInvisibility { get; internal set; } = false;

        public int BodyCollisionState { get; internal set; } = 0;

        public Dictionary<int, int> BoneCollisionStates = new Dictionary<int, int>();

        private HSDStruct Struct;

        /// <summary>
        /// 
        /// </summary>
        public SubactionProcessor()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetState()
        {
            BodyCollisionState = 0;
            BoneCollisionStates.Clear();
            OverlayColor = Vector3.One;
            CharacterInvisibility = false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        public void SetStruct(HSDStruct str)
        {
            Struct = str;
            Commands = GetCommands(Struct);
            ResetState();
            SetFrame(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private List<Command> GetCommands(HSDStruct str, Dictionary<HSDStruct, List<Command>> structToComman = null)
        {
            if(structToComman == null)
                structToComman = new Dictionary<HSDStruct, List<Command>>();

            if (structToComman.ContainsKey(str))
                return structToComman[str];

            var data = str.GetData();
            
            var Commands = new List<Command>();
            structToComman.Add(str, Commands);

            for (int i = 0; i < data.Length;)
            {
                var sa = SubactionManager.GetSubaction(data[i]);

                var cmd = new Command();

                foreach (var r in str.References)
                {
                    if (r.Key >= i && r.Key < i + sa.ByteSize)
                        if (cmd.Reference != null)
                            throw new NotSupportedException("Multiple References not supported");
                        else
                        {
                            if(r.Value != str) // prevent self reference
                            {
                                cmd.Reference = r.Value;
                                cmd.ReferenceCommands = GetCommands(cmd.Reference, structToComman);
                            }
                        }
                }

                var sub = new byte[sa.ByteSize];

                for (int j = 0; j < sub.Length; j++)
                    sub[j] = data[i + j];

                cmd.Parameters = sa.GetParameters(sub);
                cmd.Action = sa;
                Commands.Add(cmd);

                i += sa.ByteSize;
            }

            return Commands;
        }


        // prevent recursion...
        private HashSet<List<Command>> CommandHashes = new HashSet<List<Command>>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="frame"></param>
        public void SetFrame(float frame)
        {
            Hitboxes.Clear();
            BoneCollisionStates.Clear();
            BodyCollisionState = 0;
            if (frame == 0)
                ResetState();
            CommandHashes.Clear();
            SetFrame(frame, 0, Commands);
        }

        /// <summary>
        /// 
        /// </summary>
        private float SetFrame(float frame, float time, List<Command> commands)
        {
            int loopAmt = 0;
            int loopPos = 0;
            for (int i = 0; i < commands.Count; i++)
            {
                var cmd = commands[i];
                switch (cmd.Action.Code)
                {
                    case 0 << 2: //end script
                        time = int.MaxValue;
                        break;
                    case 1 << 2: //synchronous
                        time += cmd.Parameters[0];
                        break;
                    case 2 << 2: //asynchronus
                        time = cmd.Parameters[0];
                        break;
                    case 3 << 2: //start loop
                        loopAmt = cmd.Parameters[0];
                        loopPos = i;
                        break;
                    case 4 << 2: //end loop
                        loopAmt -= 1;
                        if(loopAmt > 0)
                            i = loopPos;
                        break;
                    case 5 << 2: //subroutine
                        time = SetFrame(frame, time, cmd.ReferenceCommands);
                        break;
                    case 6 << 2: //return?
                        return time;
                    case 7 << 2: //goto
                        time = SetFrame(frame, time, cmd.ReferenceCommands);
                        break;
                    case 11 << 2: // Create Hitbox
                        // remove the current hitbox with this id
                        Hitboxes.RemoveAll(e => e.ID == cmd.Parameters[0]);
                        // add hitbox
                        Hitboxes.Add(new Hitbox()
                        {
                            ID = cmd.Parameters[0],
                            BoneID = cmd.Parameters[2],
                            Size = ((short)cmd.Parameters[5] >> 7) / 2,
                            Point1 = new Vector3(cmd.Parameters[6] >> 7, cmd.Parameters[7] >> 7, cmd.Parameters[8] >> 7),
                            Element = cmd.Parameters[15]
                        });
                        break;
                    case 13 << 2: // adjust size
                        {
                            var hb = Hitboxes.Find(e=>e.ID == cmd.Parameters[0]);
                            if(hb != null)
                            {
                                hb.Size = (short)cmd.Parameters[1] / 150f;
                            }
                        }
                        break;
                    case 15 << 2:
                        Hitboxes.RemoveAll(e=>e.ID == cmd.Parameters[0]);
                        break;
                    case 16 << 2:
                        Hitboxes.Clear();
                        break;
                    case 26 << 2:
                        BodyCollisionState = cmd.Parameters[1];
                        break;
                    case 27 << 2:
                        // i don't really know how many bone to assume...
                        for(int j = 0; j < 100; j++)
                        {
                            if (BoneCollisionStates.ContainsKey(j))
                                BoneCollisionStates[j] = cmd.Parameters[0];
                            else
                                BoneCollisionStates.Add(j, cmd.Parameters[0]);
                        }
                        break;
                    case 28 << 2:
                        if(cmd.Parameters.Length > 1)
                        {
                            if (BoneCollisionStates.ContainsKey(cmd.Parameters[0]))
                                BoneCollisionStates[cmd.Parameters[0]] = cmd.Parameters[1];
                            else
                                BoneCollisionStates.Add(cmd.Parameters[0], cmd.Parameters[1]);
                        }
                        break;
                    case 37 << 2:
                        CharacterInvisibility = cmd.Parameters[1] == 1;
                        break;
                    case 46 << 2: //overlay color
                        if(cmd.Parameters[0] == 1)
                        {
                            OverlayColor = new Vector3(cmd.Parameters[1] / 255f, cmd.Parameters[2] / 255f, cmd.Parameters[3] / 255f);
                        }
                        break;
                }

                if (time > frame)
                    break;
            }
            return time;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class Hitbox
    {
        public int ID;
        public int BoneID;
        public float Size;
        public int Element;
        public Vector3 Point1;
        public Vector3 Point2;
    }
}