using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyControl : Node2D
{
    public int enemyAmount = 0;
    public bool playerInRange = false;
    private bool enemiesActive = false;
    private bool despawningInProgress = false;

    private List<Node2D> enemySpawnPoints = new List<Node2D>();
    private Timer SpawnTimer;
    public override void _Ready()
    {
        SpawnTimer = GetNode<Timer>("Timer");
        for (int i = 0; i < GetChildCount(); i++)
        {
            if (GetChildren()[i].HasMeta("Type"))
            {
                if ((string)GetChildren()[i].GetMeta("Type") == "EnemySpawner")
                {
                    enemyAmount++;
                    enemySpawnPoints.Add((EnemySpawner)GetChildren()[i]);
                }
            }
        }
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
        if (!playerInRange && !despawningInProgress && enemiesActive)
        {
            despawningInProgress = true;
            SpawnTimer.WaitTime = 2;
            SpawnTimer.Start();
        }
    }
}
