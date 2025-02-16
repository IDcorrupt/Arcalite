using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyControl : Node2D
{
    [Export]
    public string room;
    public int enemyAmount = 0;
    public bool playerInRange = false;
    public bool playerInRoom = false;
    private bool enemiesActive = false;
    private bool despawningInProgress = false;

    private List<Node2D> enemySpawnPoints = new List<Node2D>();
    private Timer SpawnTimer;

    public override void _Ready()
    {
        SpawnTimer = GetNode<Timer>("Timer");
        for (int i = 0; i < GetChildCount(); i++)
        {
            if (GetChildren()[i] is EnemySpawner)
            {
                enemyAmount++;
                enemySpawnPoints.Add((EnemySpawner) GetChildren()[i]);
            }
        }

    }

    public void AreaEntered(Area2D area)
    {
        if(area.GetCollisionLayerValue(9))
            playerInRange = true;
        else if (area.GetCollisionLayerValue(6))
            playerInRoom = true;
    }
    public void AreaExited(Area2D area)
    {
        if (area.GetCollisionLayerValue(9))
            playerInRange = false;
        else if (area.GetCollisionLayerValue(6))
            playerInRoom = false;
    }


    public void OnTimerTimeout()
    {
        foreach (Node2D node in enemySpawnPoints)
        {
            if (node is EnemySpawner spawner)
            {
                spawner.Despawn();
            }
            enemiesActive = false;
        }
        despawningInProgress = false;
    }


    public override void _Process(double delta)
    {
        if (playerInRange && !enemiesActive)
        {
            SpawnTimer.Stop();
            foreach (Node2D node in enemySpawnPoints)
            {
                if (node is EnemySpawner spawner)
                {
                    spawner.Spawn();
                }

            }
            enemiesActive = true;
        }
        else if (!playerInRange && !despawningInProgress && enemiesActive)
        {
            despawningInProgress = true;
            SpawnTimer.WaitTime = 2;
            SpawnTimer.Start();
        }
        else if (playerInRange && despawningInProgress)
        {
            //cancel despawn if player moves back in range
            SpawnTimer.Stop();
        }

        if (playerInRoom)
        {
            foreach(Node node in GetChildren())
            {

                if (node is LightMeele lightMelee)
                    lightMelee.isChasing = true;
                //[TBD] add all other enemy types when they start existing
            }
        }
        else
        {
            foreach (Node node in GetChildren())
            {
                if (node is LightMeele lightMelee)
                    lightMelee.isChasing = false;
                //[TBD] add all other enemy types when they start existing
            }
        }
    }
}