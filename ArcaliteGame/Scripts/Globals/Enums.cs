using Godot;
using System;

public partial class Enums : Node
{
    public enum itemType
    {
        necklace,
        shield,
        shard

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
}
