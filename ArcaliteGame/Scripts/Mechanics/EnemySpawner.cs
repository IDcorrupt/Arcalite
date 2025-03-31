using Godot;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;

public partial class EnemySpawner : Node2D
{
    
    [Export] Enums.EnemyClass EnemyClass;
    [Export] bool BossSpawner = false;
    [Export] Enums.BossClass BossClass;
    public bool playerInRoom = false;

    private PackedScene lightMeleeScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/light_meele.tscn");
    private PackedScene HeavyMeleeScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/heavy_melee.tscn");
    private PackedScene LightRangedScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/light_ranged.tscn");
    private PackedScene HeavyRangedScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/heavy_ranged.tscn");
    
    
    private PackedScene BossUndeadScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/Bosses/boss_undead.tscn");
    private PackedScene BossMechScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/Bosses/boss_mech.tscn");

    private Enemy ActiveEnemy;
    private BossMech ActiveBoss;        // if more bosses are added, they might have a core class like "Enemy", but currently this variable has the "BossMech" class directly

    private EnemyControl parent;
    public override void _Ready()
    {
        parent = (EnemyControl)GetParent();
    }
    public bool Spawn()
    {
        if (BossSpawner)
        {
            switch (BossClass)
            {
                case Enums.BossClass.Mech:
                    ActiveBoss = BossMechScene.Instantiate() as BossMech;
                    ActiveBoss.Name = "BossMech";
                    AddSibling(ActiveBoss);
                    ActiveBoss.GlobalPosition = GlobalPosition;
                    break;
                default:
                    break;
            }
            if (ActiveBoss != null) return true;
            else return false;
        }
        else
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
    }
    public bool Despawn()
    {
        if (BossSpawner)
        {
            if (ActiveBoss != null && IsInstanceValid(ActiveBoss))
            {
                ActiveBoss.Free();
                ActiveBoss = null;
                return true;
            }
            else
            {
                ActiveBoss = null;
                return false;
            }
        }
        else
        {
            if (ActiveEnemy != null && IsInstanceValid(ActiveEnemy))
            {
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

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (playerInRoom)
            if(ActiveBoss != null)
                ActiveBoss.playerInRoom = true;

    }
}
