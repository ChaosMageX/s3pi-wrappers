using System;

namespace s3piwrappers.EffectCloner.Swarm
{
    public enum VisualEffectType : byte
    {
        Particle = 0x01,
        Metaparticle = 0x02,
        Decal = 0x03,
        Sequence = 0x04,
        Sound = 0x05,
        Shake = 0x06,
        Camera = 0x07,
        Model = 0x08,
        Screen = 0x09,
        Game = 0x0B,
        FastParticle = 0x0C,
        Distribute = 0x0D,
        Ribbon = 0x0E,
        Sprite = 0x0F
    }

    public enum VisualResourceType : byte
    {
        Map = 0x00,
        Material = 0x01
    }
}
