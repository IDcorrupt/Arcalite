using Godot;
using System;
using System.Diagnostics;

public partial class EnemySpawner : Node2D
{
    [Export] bool LightMelee;
    [Export] bool HeavyMelee;
    [Export] bool Caster;
    [Export] bool EliteMelee;
    [Export] bool EliteCaster;


    private PackedScene lightMeleeScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/light_meele.tscn");
    private PackedScene HeavyMeleeScene;
    private PackedScene CasterScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/light_ranged.tscn");
    private PackedScene EliteMeleeScene;
    private PackedScene EliteCasterScene;

    private PackedScene ActiveEnemyType;
    private Node ActiveEnemy;

    private EnemyControl parent;
    public override void _Ready()
    {
        parent = (EnemyControl)GetParent();
        if (LightMelee)
            ActiveEnemyType = lightMeleeScene;
        else if (HeavyMelee)
            ActiveEnemyType = HeavyMeleeScene;
        else if (Caster)
            ActiveEnemyType = CasterScene;
        else if (EliteMelee)
            ActiveEnemyType = EliteMeleeScene;
        else if (EliteCaster)
            ActiveEnemyType = EliteCasterScene;
    }
    public void Spawn()
    {
        ActiveEnemy = ActiveEnemyType.Instantiate();
        if(ActiveEnemy is LightMeele ActiveLightMelee)
        {
            ActiveLightMelee.Name = "LightMelee" + this.Name.ToString()[Name.ToString().Length-1];
            ActiveLightMelee.GlobalPosition = Position;
            ActiveEnemy = ActiveLightMelee;
        }else if(ActiveEnemy is LightRanged ActiveLightRanged)
        {
            ActiveLightRanged.Name = "LightRanged" + this.Name.ToString()[Name.ToString().Length - 1];
            ActiveLightRanged.GlobalPosition = Position;
            ActiveEnemy = ActiveLightRanged;
        }

        //other type handling with else if-s
        AddSibling(ActiveEnemy);
        
    }
    public void Despawn()
    {
        ActiveEnemy.QueueFree();
    }
}
