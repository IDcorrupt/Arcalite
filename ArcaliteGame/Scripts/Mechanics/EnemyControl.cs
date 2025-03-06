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
    private bool roomCleared = false;

    private List<Node2D> enemySpawnPoints = new List<Node2D>();
    private Timer SpawnTimer;
    private CameraController camera;

    public override void _Ready()
    {
        SpawnTimer = GetNode<Timer>("Timer");
        camera = GetParent().GetParent().GetNode("CameraController") as CameraController;
        for (int i = 0; i < GetChildCount(); i++)
        {
            if (GetChildren()[i] is EnemySpawner)
            {
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
        DespawnEnemies();
    }
    public void SpawnEnemies()
    {
        SpawnTimer.Stop();
        foreach (Node2D node in enemySpawnPoints)
        {
            if (node is EnemySpawner spawner)
            {
                if(spawner.Spawn())
                    enemyAmount++;
            }

        }
        enemiesActive = true;
    }
    public void DespawnEnemies()
    {
        foreach (Node2D node in enemySpawnPoints)
        {
            if (node is EnemySpawner spawner)
            {
                if(spawner.Despawn())
                    enemyAmount--;
            }
            enemiesActive = false;
        }
        despawningInProgress = false;
    }

    public void SetRoomCleared(bool cleared)
    {
        roomCleared = cleared;
    }
    public override void _Process(double delta)
    {
        if (!roomCleared)
        {
            if (Globals.player.GetIsDead())
            {
                playerInRoom = false;
            }
            if (playerInRange && !enemiesActive)
            {
                SpawnEnemies();
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


            //fight mode
            if (playerInRoom)
            {
                camera.LockPlayer(true);
            }
            if (enemyAmount == 0 && playerInRoom)
            {
                roomCleared = true;
            }
        }
        else
            camera.LockPlayer(false);
    }
}