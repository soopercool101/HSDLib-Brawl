﻿using HSDRawViewer.Converters;
using System;
using HSDRaw.Common;
using OpenTK;

namespace HSDRawViewer.Rendering
{
    /// <summary>
    /// 
    /// </summary>
    public class MotAnimManager : JointAnimManager
    {
        private MOT_FILE _motFile;
        private short[] _motJointTable;

        public override Matrix4 GetAnimatedState(float frame, int boneIndex, HSD_JOBJ jobj)
        {
            float TX = jobj.TX;
            float TY = jobj.TY;
            float TZ = jobj.TZ;
            float RX = jobj.RX;
            float RY = jobj.RY;
            float RZ = jobj.RZ;
            float SX = jobj.SX;
            float SY = jobj.SY;
            float SZ = jobj.SZ;

            Quaternion rotationOverride = Math3D.FromEulerAngles(RZ, RY, RX);

            var joints = _motFile.Joints.FindAll(e => e.BoneID >= 0 && e.BoneID < _motJointTable.Length && _motJointTable[e.BoneID] == boneIndex);

            foreach (var j in joints)
            {
                var key = j.GetKey(frame / 60f);

                if (j.TrackFlag.HasFlag(MOT_FLAGS.TRANSLATE))
                {
                    TX += key.X;
                    TY += key.Y;
                    TZ += key.Z;
                }
                if (j.TrackFlag.HasFlag(MOT_FLAGS.SCALE))
                {
                    SX += key.X;
                    SY += key.Y;
                    SZ += key.Z;
                }
                if (j.TrackFlag.HasFlag(MOT_FLAGS.ROTATE))
                {
                    rotationOverride = Math3D.FromEulerAngles(RZ, RY, RX);

                    var dir = new Vector3(key.X, key.Y, key.Z);
                    var angle = key.W;

                    float rot_angle = (float)Math.Acos(Vector3.Dot(Vector3.UnitX, dir));
                    if (Math.Abs(rot_angle) > 0.000001f)
                    {
                        Vector3 rot_axis = Vector3.Cross(Vector3.UnitX, dir).Normalized();
                        rotationOverride *= Quaternion.FromAxisAngle(rot_axis, rot_angle);
                    }

                    rotationOverride *= Quaternion.FromEulerAngles(angle * (float)Math.PI / 180, 0, 0);
                }
            }

            return Matrix4.CreateScale(SX, SY, SZ) *
                Matrix4.CreateFromQuaternion(rotationOverride) *
                Matrix4.CreateTranslation(TX, TY, TZ);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jointTable"></param>
        /// <param name="file"></param>
        public void SetMOT(short[] jointTable, MOT_FILE file)
        {
            _motFile = file;
            _motJointTable = jointTable;
            FrameCount = (int)Math.Ceiling(_motFile.EndTime * 60);
        }

    }
}
