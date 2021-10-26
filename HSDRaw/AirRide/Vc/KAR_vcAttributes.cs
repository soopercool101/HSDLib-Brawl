﻿namespace HSDRaw.AirRide.Vc
{
    public class KAR_vcAttributes : HSDAccessor
    {
        public override int TrimmedSize => 0x1F0;

        public int KirbySitBoneIndex { get => _s.GetInt32(0x0); set => _s.SetInt32(0x0, value); }

        public int KirbyAdditionalBoneIndex { get => _s.GetInt32(0x4); set => _s.SetInt32(0x4, value); }

        public float ModelScaling { get => _s.GetFloat(0x8); set => _s.SetFloat(0x8, value); }

        public float BaseOffense { get => _s.GetFloat(0xc); set => _s.SetFloat(0xc, value); }

        public float StartCameraDistance { get => _s.GetFloat(0x10); set => _s.SetFloat(0x10, value); }

        public float Unknown1 { get => _s.GetFloat(0x14); set => _s.SetFloat(0x14, value); }

        public float ShadowLengthFrontAndBehind { get => _s.GetFloat(0x18); set => _s.SetFloat(0x18, value); }

        public float ShadowLengthSides { get => _s.GetFloat(0x1c); set => _s.SetFloat(0x1c, value); }

        public float ShadowWidthTurning { get => _s.GetFloat(0x20); set => _s.SetFloat(0x20, value); }

        public int Unknown2 { get => _s.GetInt32(0x24); set => _s.SetInt32(0x24, value); }

        public float Unknown3 { get => _s.GetFloat(0x28); set => _s.SetFloat(0x28, value); }

        public float AnglePointerDistance { get => _s.GetFloat(0x2c); set => _s.SetFloat(0x2c, value); }

        public float Unknown4 { get => _s.GetFloat(0x30); set => _s.SetFloat(0x30, value); }
        public float Unknown5 { get => _s.GetFloat(0x34); set => _s.SetFloat(0x34, value); }
        public float HitKnockback { get => _s.GetFloat(0x38); set => _s.SetFloat(0x38, value); }
        public float Unknown6 { get => _s.GetFloat(0x3c); set => _s.SetFloat(0x3c, value); }
        public float Unknown7 { get => _s.GetFloat(0x40); set => _s.SetFloat(0x40, value); }
        public float Unknown8 { get => _s.GetFloat(0x44); set => _s.SetFloat(0x44, value); }
        public float MaxAngularDisplacementPerfectLand { get => _s.GetFloat(0x48); set => _s.SetFloat(0x48, value); }
        public float Unknown9 { get => _s.GetFloat(0x4c); set => _s.SetFloat(0x4c, value); }
        public int Unknown10 { get => _s.GetInt32(0x50); set => _s.SetInt32(0x50, value); }
        public int Unknown11 { get => _s.GetInt32(0x54); set => _s.SetInt32(0x54, value); }
        public float Unknown12 { get => _s.GetFloat(0x58); set => _s.SetFloat(0x58, value); }
        public float Unknown13 { get => _s.GetFloat(0x5c); set => _s.SetFloat(0x5c, value); }
        public int Unknown14 { get => _s.GetInt32(0x60); set => _s.SetInt32(0x60, value); }
        public float Unknown15 { get => _s.GetFloat(0x64); set => _s.SetFloat(0x64, value); }
        public int Unknown16 { get => _s.GetInt32(0x68); set => _s.SetInt32(0x68, value); }
        public float BaseHP { get => _s.GetFloat(0x6c); set => _s.SetFloat(0x6c, value); }
        public float SizeOfHitbox { get => _s.GetFloat(0x70); set => _s.SetFloat(0x70, value); }
        public float HitboxDistanceX { get => _s.GetFloat(0x74); set => _s.SetFloat(0x74, value); }
        public float Unknown17 { get => _s.GetFloat(0x78); set => _s.SetFloat(0x78, value); }
        public float Unknown18 { get => _s.GetFloat(0x7c); set => _s.SetFloat(0x7c, value); }
        public float Unknown19 { get => _s.GetFloat(0x80); set => _s.SetFloat(0x80, value); }
        public float FlySpeedPerfectLand { get => _s.GetFloat(0x84); set => _s.SetFloat(0x84, value); }
        public float Unknown20 { get => _s.GetFloat(0x88); set => _s.SetFloat(0x88, value); }
        public float BaseDefense { get => _s.GetFloat(0x8c); set => _s.SetFloat(0x8c, value); }
        public float TopSpeedGround { get => _s.GetFloat(0x90); set => _s.SetFloat(0x90, value); }
        public float SpeedMultiplierUpSlope { get => _s.GetFloat(0x94); set => _s.SetFloat(0x94, value); }
        public float SpeedMultiplierDownSlope { get => _s.GetFloat(0x98); set => _s.SetFloat(0x98, value); }
        public float ChargeSpeed { get => _s.GetFloat(0x9c); set => _s.SetFloat(0x9c, value); }
        public float ChargeSpeedTurning { get => _s.GetFloat(0xa0); set => _s.SetFloat(0xa0, value); }
        public float ChargeDepleteSpeed { get => _s.GetFloat(0xa4); set => _s.SetFloat(0xa4, value); }
        public float SpeedGainUnchargedBoost { get => _s.GetFloat(0xa8); set => _s.SetFloat(0xa8, value); }
        public float SpeedGainHalfChargedBoost { get => _s.GetFloat(0xac); set => _s.SetFloat(0xac, value); }
        public float Unknown21 { get => _s.GetFloat(0xb0); set => _s.SetFloat(0xb0, value); }
        public float SpeedGainFullchargedBoost { get => _s.GetFloat(0xb4); set => _s.SetFloat(0xb4, value); }
        public float Unknown22 { get => _s.GetFloat(0xb8); set => _s.SetFloat(0xb8, value); }
        public float Unknown23 { get => _s.GetFloat(0xbc); set => _s.SetFloat(0xbc, value); }
        public float Unknown24 { get => _s.GetFloat(0xc0); set => _s.SetFloat(0xc0, value); }
        public float SpeedGainThirdQuadBoost { get => _s.GetFloat(0xc4); set => _s.SetFloat(0xc4, value); }
        public float Unknown25 { get => _s.GetFloat(0xc8); set => _s.SetFloat(0xc8, value); }
        public float Unknown26 { get => _s.GetFloat(0xcc); set => _s.SetFloat(0xcc, value); }
        public float Unknown27 { get => _s.GetFloat(0xd0); set => _s.SetFloat(0xd0, value); }
        public float Unknown28 { get => _s.GetFloat(0xd4); set => _s.SetFloat(0xd4, value); }
        public float SpeedGainAnyBoost { get => _s.GetFloat(0xd8); set => _s.SetFloat(0xd8, value); }
        public float SpeedGainSlidingBoost { get => _s.GetFloat(0xdc); set => _s.SetFloat(0xdc, value); }
        public float TurnHandling { get => _s.GetFloat(0xe0); set => _s.SetFloat(0xe0, value); }
        public float Unknown29 { get => _s.GetFloat(0xe4); set => _s.SetFloat(0xe4, value); }
        public float Unknown30 { get => _s.GetFloat(0xe8); set => _s.SetFloat(0xe8, value); }
        public float Unknown31 { get => _s.GetFloat(0xec); set => _s.SetFloat(0xec, value); }
        public float Unknown32 { get => _s.GetFloat(0xf0); set => _s.SetFloat(0xf0, value); }
        public float Unknown33 { get => _s.GetFloat(0xf4); set => _s.SetFloat(0xf4, value); }
        public float Unknown34 { get => _s.GetFloat(0xf8); set => _s.SetFloat(0xf8, value); }
        public float Unknown35 { get => _s.GetFloat(0xfc); set => _s.SetFloat(0xfc, value); }
        public float Unknown36 { get => _s.GetFloat(0x100); set => _s.SetFloat(0x100, value); }
        public float Unknown37 { get => _s.GetFloat(0x104); set => _s.SetFloat(0x104, value); }
        public float Unknown38 { get => _s.GetFloat(0x108); set => _s.SetFloat(0x108, value); }
        public float Unknown39 { get => _s.GetFloat(0x10c); set => _s.SetFloat(0x10c, value); }
        public float Unknown40 { get => _s.GetFloat(0x110); set => _s.SetFloat(0x110, value); }
        public float Unknown41 { get => _s.GetFloat(0x114); set => _s.SetFloat(0x114, value); }
        public float Unknown42 { get => _s.GetFloat(0x118); set => _s.SetFloat(0x118, value); }
        public float LandingHitboxSize { get => _s.GetFloat(0x11c); set => _s.SetFloat(0x11c, value); }
        public float LandingHitboxDistanceX { get => _s.GetFloat(0x120); set => _s.SetFloat(0x120, value); }
        public float Unknown43 { get => _s.GetFloat(0x124); set => _s.SetFloat(0x124, value); }
        public float Unknown44 { get => _s.GetFloat(0x128); set => _s.SetFloat(0x128, value); }
        public float Unknown45 { get => _s.GetFloat(0x12c); set => _s.SetFloat(0x12c, value); }
        public float Unknown46 { get => _s.GetFloat(0x130); set => _s.SetFloat(0x130, value); }
        public float QuickSpinTornadoSize { get => _s.GetFloat(0x134); set => _s.SetFloat(0x134, value); }
        public float TurnSpeedOnSlope { get => _s.GetFloat(0x138); set => _s.SetFloat(0x138, value); }
        public float InitialTakeOffSpeed { get => _s.GetFloat(0x13c); set => _s.SetFloat(0x13c, value); }
        public float Unknown47 { get => _s.GetFloat(0x140); set => _s.SetFloat(0x140, value); }
        public float Unknown48 { get => _s.GetFloat(0x144); set => _s.SetFloat(0x144, value); }
        public float Unknown49 { get => _s.GetFloat(0x148); set => _s.SetFloat(0x148, value); }
        public float StraightAirAcceleration { get => _s.GetFloat(0x14c); set => _s.SetFloat(0x14c, value); }
        public float AirTurnSharpness { get => _s.GetFloat(0x150); set => _s.SetFloat(0x150, value); }
        public float Unknown50 { get => _s.GetFloat(0x154); set => _s.SetFloat(0x154, value); }
        public float Unknown51 { get => _s.GetFloat(0x158); set => _s.SetFloat(0x158, value); }
        public float Unknown52 { get => _s.GetFloat(0x15c); set => _s.SetFloat(0x15c, value); }
        public float FullChargeMidairSpeed { get => _s.GetFloat(0x160); set => _s.SetFloat(0x160, value); }
        public float Unknown53 { get => _s.GetFloat(0x164); set => _s.SetFloat(0x164, value); }
        public float Unknown54 { get => _s.GetFloat(0x168); set => _s.SetFloat(0x168, value); }
        public float Unknown55 { get => _s.GetFloat(0x16c); set => _s.SetFloat(0x16c, value); }
        public float Unknown56 { get => _s.GetFloat(0x170); set => _s.SetFloat(0x170, value); }
        public float GlidePointUpSpeed { get => _s.GetFloat(0x174); set => _s.SetFloat(0x174, value); }
        public float GlidePointUpAmount { get => _s.GetFloat(0x178); set => _s.SetFloat(0x178, value); }
        public float GlidePointDownSpeed { get => _s.GetFloat(0x17c); set => _s.SetFloat(0x17c, value); }
        public float GlidePointDownAmount { get => _s.GetFloat(0x180); set => _s.SetFloat(0x180, value); }
        public float Unknown57 { get => _s.GetFloat(0x184); set => _s.SetFloat(0x184, value); }
        public float MidairUpSnapbackSpeed { get => _s.GetFloat(0x188); set => _s.SetFloat(0x188, value); }
        public float MidairTurnSpeedUp { get => _s.GetFloat(0x18c); set => _s.SetFloat(0x18c, value); }
        public float MidairTurnSpeedStraight { get => _s.GetFloat(0x190); set => _s.SetFloat(0x190, value); }
        public float Unknown58 { get => _s.GetFloat(0x194); set => _s.SetFloat(0x194, value); }
        public float MidairSideSnapbackSpeed { get => _s.GetFloat(0x198); set => _s.SetFloat(0x198, value); }
        public float Unknown59 { get => _s.GetFloat(0x19c); set => _s.SetFloat(0x19c, value); }
        public float Unknown60 { get => _s.GetFloat(0x1a0); set => _s.SetFloat(0x1a0, value); }
        public float Unknown61 { get => _s.GetFloat(0x1a4); set => _s.SetFloat(0x1a4, value); }
        public float Unknown62 { get => _s.GetFloat(0x1a8); set => _s.SetFloat(0x1a8, value); }
        public float Unknown63 { get => _s.GetFloat(0x1ac); set => _s.SetFloat(0x1ac, value); }
        public float Unknown64 { get => _s.GetFloat(0x1b0); set => _s.SetFloat(0x1b0, value); }
        public float Unknown65 { get => _s.GetFloat(0x1b4); set => _s.SetFloat(0x1b4, value); }
        public float Unknown66 { get => _s.GetFloat(0x1b8); set => _s.SetFloat(0x1b8, value); }
        public float Unknown67 { get => _s.GetFloat(0x1bc); set => _s.SetFloat(0x1bc, value); }
        public float RailStationTurnSpeed { get => _s.GetFloat(0x1c0); set => _s.SetFloat(0x1c0, value); }
        public float Unknown68 { get => _s.GetFloat(0x1c4); set => _s.SetFloat(0x1c4, value); }
        public float Unknown69 { get => _s.GetFloat(0x1c8); set => _s.SetFloat(0x1c8, value); }
        public float Unknown70 { get => _s.GetFloat(0x1cc); set => _s.SetFloat(0x1cc, value); }
        public float Unknown71 { get => _s.GetFloat(0x1d0); set => _s.SetFloat(0x1d0, value); }
        public float Unknown72 { get => _s.GetFloat(0x1d4); set => _s.SetFloat(0x1d4, value); }
        public float Unknown73 { get => _s.GetFloat(0x1d8); set => _s.SetFloat(0x1d8, value); }
        public float Unknown74 { get => _s.GetFloat(0x1dc); set => _s.SetFloat(0x1dc, value); }
        public float Unknown75 { get => _s.GetFloat(0x1e0); set => _s.SetFloat(0x1e0, value); }
        public float Unknown76 { get => _s.GetFloat(0x1e4); set => _s.SetFloat(0x1e4, value); }
        public float Unknown77 { get => _s.GetFloat(0x1e8); set => _s.SetFloat(0x1e8, value); }
        public float Unknown78 { get => _s.GetFloat(0x1ec); set => _s.SetFloat(0x1ec, value); }
    }
}
