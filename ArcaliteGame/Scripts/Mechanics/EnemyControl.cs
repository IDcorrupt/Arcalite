using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyControl : Node2D
{
    public int enemyAmount = 0;
    public bool playerInRange = false;
    public bool playerInRoom = false;
    private bool enemiesActive = false;
    private bool despawningInProgress = false;
    private bool roomCleared = false;
    private bool camState = false;

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
        {
            playerInRoom = true;
        }
    }
    public void AreaExited(Area2D area)
    {
        if (area.GetCollisionLayerValue(9))
            playerInRange = false;
        else if (area.GetCollisionLayerValue(6))
        {
            playerInRoom = false;
        }
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
        //despawn mechBoss spikes
        playerInRoom = false;
        foreach (Node node in GetChildren())
        {
            if(node is Spike spike)   
                spike.QueueFree();
        }
        foreach (Node2D node in enemySpawnPoints)
        {
            if (node is EnemySpawner spawner)
            {
                if(spawner.Despawn())
                    enemyAmount--;
            }
            enemiesActive = false;
        }
        camState = false;
        despawningInProgress = false;
    }

    public void SetRoomCleared(bool cleared)
    {
        playerInRoom = false;
        roomCleared = cleared;
    }
    public override void _Process(double delta)
    {
        if (!roomCleared)
        {
            //stop chase if player dead
            if (Globals.player.GetIsDead())
            {
                playerInRoom = false;
            }
            //spawn enemies if player close
            if (playerInRange && !enemiesActive)
            {
                SpawnEnemies();
            }
            //start despawning if player too far
            else if (!playerInRange && !despawningInProgress && enemiesActive)
            {
                despawningInProgress = true;
                SpawnTimer.WaitTime = 2;
                SpawnTimer.Start();
            }
            //cancel despawn if player moves back in range
            else if (playerInRange && despawningInProgress)
            {
                SpawnTimer.Stop();
            }

            //fight mode
            if (playerInRoom)
            {
                if (!camState)
                {
                    camera.LockPlayer(true);
                    camState = true;
                }
                foreach(EnemySpawner spawner in enemySpawnPoints)
                {
                    spawner.playerInRoom = true;
                }
            }
            if (enemyAmount == 0 && playerInRoom)
            {
                roomCleared = true;
            }
        }
        else if (camState)
        {
            camera.LockPlayer(false);
            camState = false;
        }
    }

    public bool isRoomCleared() { return roomCleared; }
}