using Godot;
using System;

public partial class Enums : Node
{
    public enum itemType
    {
        empty = 0,
        necklace = 1,
        shield = 2,
        shard = 3,

    }
    public enum EnemyClass
    {
        None,
        LightMelee,
        HeavyMelee,
        LightRanged,
        HeavyRanged,
        Elite
    }
    public enum BossClass
    {
        None,
        Mech,
    }
    public enum SaveState
    {
        Existing,
        Created,
        None
    }
}
