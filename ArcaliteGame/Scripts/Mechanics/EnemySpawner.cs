using Godot;
using System;
using System.Diagnostics;

public partial class EnemySpawner : Node2D
{
    
    [Export] Enums.EnemyClass EnemyClass;


    private PackedScene lightMeleeScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/light_meele.tscn");
    private PackedScene HeavyMeleeScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/heavy_melee.tscn");
    private PackedScene LightRangedScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/light_ranged.tscn");
    private PackedScene HeavyRangedScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/heavy_ranged.tscn");
    
    
    private PackedScene BossUndeadScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/Bosses/boss_undead.tscn");

    private PackedScene ActiveEnemyType;
    private Enemy ActiveEnemy;

    private EnemyControl parent;
    public override void _Ready()
    {
        parent = (EnemyControl)GetParent();
    }
    public bool Spawn()
    {
        switch (EnemyClass)
        {
            case Enums.EnemyClass.LightMelee:
                ActiveEnemy = lightMeleeScene.Instantiate() as Enemy;
                ActiveEnemy.Name = "LightMelee" + this.Name.ToString()[Name.ToString().Length - 1];
                ActiveEnemy.GlobalPosition = Position;
                AddSibling(ActiveEnemy);
                break;
            case Enums.EnemyClass.HeavyMelee:
                ActiveEnemy = HeavyMeleeScene.Instantiate() as Enemy;
                ActiveEnemy.Name = "ActiveHeavyMelee" + this.Name.ToString()[Name.ToString().Length - 1];
                ActiveEnemy.GlobalPosition = Position;
                AddSibling(ActiveEnemy);
                break;
            case Enums.EnemyClass.LightRanged:
                ActiveEnemy = LightRangedScene.Instantiate() as Enemy;
                ActiveEnemy.Name = "LightRanged" + this.Name.ToString()[Name.ToString().Length - 1];
                ActiveEnemy.GlobalPosition = Position;
                AddSibling(ActiveEnemy);
                break;
            case Enums.EnemyClass.HeavyRanged:
                ActiveEnemy = HeavyRangedScene.Instantiate() as Enemy;
                ActiveEnemy.Name = "LightRanged" + this.Name.ToString()[Name.ToString().Length - 1];
                ActiveEnemy.GlobalPosition = Position;
                AddSibling(ActiveEnemy);
                break;
            case Enums.EnemyClass.Elite:
                ActiveEnemy = BossUndeadScene.Instantiate() as Enemy;
                ActiveEnemy.Name = "BossUndead" + this.Name.ToString()[Name.ToString().Length - 1];
                ActiveEnemy.GlobalPosition = Position;
                AddSibling(ActiveEnemy);
                break;
            default:
                break;
        }
        if (ActiveEnemy != null) return true;
        else return false;
    }
    public bool Despawn()
    {
        if (ActiveEnemy != null && IsInstanceValid(ActiveEnemy))
        {
            GD.Print("current enemy: " + ActiveEnemy);
            ActiveEnemy.Free();
            ActiveEnemy = null;
            return true;
        }
        else
        {
            ActiveEnemy = null;
            return false;
        }

    }
}
