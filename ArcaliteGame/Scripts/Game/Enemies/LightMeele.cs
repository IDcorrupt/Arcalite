using Godot;
using System;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

public partial class LightMeele : Enemy
{

    public override void _Ready()
    {
        base._Ready();
        maxHP = 50;
        currentHP = maxHP;
        damage = 5;
        atkCooldown.WaitTime = 1;
    }
}
