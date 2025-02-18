using Godot;
using System;
using System.Diagnostics;

public partial class EnemySpawner : Node2D
{
    private enum EnemyClass
    {
        None,
        LightMelee,
        HeavyMelee,
        LightRanged,
        HeavyRanged,
        Elite
    }
    [Export] EnemyClass enemyClass;


    private PackedScene lightMeleeScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/light_meele.tscn");
    private PackedScene HeavyMeleeScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/heavy_melee.tscn");
    private PackedScene LightRangedScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/light_ranged.tscn");
    private PackedScene HeavyRangedScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/heavy_ranged.tscn");

    private PackedScene ActiveEnemyType;
    private Enemy ActiveEnemy;

    private EnemyControl parent;
    public override void _Ready()
    {
        parent = (EnemyControl)GetParent();
    }
    public bool Spawn()
    {
        switch (enemyClass)
        {
            case EnemyClass.LightMelee:
                ActiveEnemy = lightMeleeScene.Instantiate() as Enemy;
                ActiveEnemy.Name = "LightMelee" + this.Name.ToString()[Name.ToString().Length - 1];
                ActiveEnemy.GlobalPosition = Position;
                AddSibling(ActiveEnemy);
                break;
            case EnemyClass.HeavyMelee:
                ActiveEnemy = HeavyMeleeScene.Instantiate() as Enemy;
                ActiveEnemy.Name = "ActiveHeavyMelee" + this.Name.ToString()[Name.ToString().Length - 1];
                ActiveEnemy.GlobalPosition = Position;
                AddSibling(ActiveEnemy);
                break;
            case EnemyClass.LightRanged:
                ActiveEnemy = LightRangedScene.Instantiate() as Enemy;
                ActiveEnemy.Name = "LightRanged" + this.Name.ToString()[Name.ToString().Length - 1];
                ActiveEnemy.GlobalPosition = Position;
                AddSibling(ActiveEnemy);
                break;
            case EnemyClass.HeavyRanged:
                ActiveEnemy = HeavyRangedScene.Instantiate() as Enemy;
                ActiveEnemy.Name = "LightRanged" + this.Name.ToString()[Name.ToString().Length - 1];
                ActiveEnemy.GlobalPosition = Position;
                AddSibling(ActiveEnemy);
                break;
            case EnemyClass.Elite:
                break;
            default:
                break;
        }
        if (ActiveEnemy != null) return true;
        else return false;
    }
    public bool Despawn()
    {
        if (ActiveEnemy != null)
        {
            ActiveEnemy.QueueFree();
            return true;
        }
        else return false;
    }
}
