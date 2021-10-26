﻿using HSDRaw.Common.Animation;

namespace HSDRaw.AirRide.Vc
{
    /// <summary>
    /// Animation container for Star Vehicles
    /// </summary>
    public class KAR_vcAnimationStar : HSDAccessor
    {
        public override int TrimmedSize => 0x7C;

        public HSD_FigaTree MovingAnim { get => _s.GetReference<HSD_FigaTree>(0x0); set => _s.SetReference(0x0, value); }
        public HSD_MatAnimJoint MovingMatAnim { get => _s.GetReference<HSD_MatAnimJoint>(0x4); set => _s.SetReference(0x4, value); }
        public HSD_FigaTree Unk1Anim { get => _s.GetReference<HSD_FigaTree>(0x8); set => _s.SetReference(0x8, value); }
        public HSD_MatAnimJoint Unk1MatAnim { get => _s.GetReference<HSD_MatAnimJoint>(0xc); set => _s.SetReference(0xc, value); }
        public HSD_FigaTree Unk2Anim { get => _s.GetReference<HSD_FigaTree>(0x10); set => _s.SetReference(0x10, value); }
        public HSD_MatAnimJoint Unk2MatAnim { get => _s.GetReference<HSD_MatAnimJoint>(0x14); set => _s.SetReference(0x14, value); }
        public HSD_FigaTree BoostAnim { get => _s.GetReference<HSD_FigaTree>(0x18); set => _s.SetReference(0x18, value); }
        public HSD_MatAnimJoint BoostMatAnim { get => _s.GetReference<HSD_MatAnimJoint>(0x1c); set => _s.SetReference(0x1c, value); }
        public HSD_FigaTree ChargeAnim { get => _s.GetReference<HSD_FigaTree>(0x20); set => _s.SetReference(0x20, value); }
        public HSD_MatAnimJoint ChargeMatAnim { get => _s.GetReference<HSD_MatAnimJoint>(0x24); set => _s.SetReference(0x24, value); }
        public HSD_FigaTree StopAnim { get => _s.GetReference<HSD_FigaTree>(0x28); set => _s.SetReference(0x28, value); }
        public HSD_MatAnimJoint StopMatAnim { get => _s.GetReference<HSD_MatAnimJoint>(0x2c); set => _s.SetReference(0x2c, value); }


        public int UnkParticleSpawn1 { get => _s.GetInt32(0x30); set => _s.SetInt32(0x30, value); }

        public int UnkParticleSpawn2 { get => _s.GetInt32(0x34); set => _s.SetInt32(0x34, value); }


        public int MovingParticleSpawn1 { get => _s.GetInt32(0x38); set => _s.SetInt32(0x38, value); }

        public int MovingParticleSpawn2 { get => _s.GetInt32(0x3c); set => _s.SetInt32(0x3c, value); }


        public int BoostingParticleSpawn1 { get => _s.GetInt32(0x40); set => _s.SetInt32(0x40, value); }

        public int BoostingParticleSpawn2 { get => _s.GetInt32(0x44); set => _s.SetInt32(0x44, value); }

        public int BoostingParticleSpawn3 { get => _s.GetInt32(0x48); set => _s.SetInt32(0x48, value); }


        public int ParticleSpawnBone1 { get => _s.GetInt32(0x4c); set => _s.SetInt32(0x4c, value); }

        public int ParticleSpawnBone2 { get => _s.GetInt32(0x50); set => _s.SetInt32(0x50, value); }

        public int ParticleSpawnBone3 { get => _s.GetInt32(0x54); set => _s.SetInt32(0x54, value); }


        public int Flags { get => _s.GetInt32(0x58); set => _s.SetInt32(0x58, value); }

        
        public float Particle1SpawnSpeed1 { get => _s.GetFloat(0x5c); set => _s.SetFloat(0x5c, value); }

        public float Particle1SpawnSpeed2 { get => _s.GetFloat(0x60); set => _s.SetFloat(0x60, value); }

        public float Particle1SpawnSpeed3 { get => _s.GetFloat(0x64); set => _s.SetFloat(0x64, value); }


        public float Particle2SpawnSpeed1 { get => _s.GetFloat(0x68); set => _s.SetFloat(0x68, value); }

        public float Particle2SpawnSpeed2 { get => _s.GetFloat(0x6c); set => _s.SetFloat(0x6c, value); }

        public float Particle2SpawnSpeed3 { get => _s.GetFloat(0x70); set => _s.SetFloat(0x70, value); }


        public int BoostSoundID { get => _s.GetInt32(0x74); set => _s.SetInt32(0x74, value); }

        public int AfterBoostSoundID { get => _s.GetInt32(0x78); set => _s.SetInt32(0x78, value); }
    }
}
