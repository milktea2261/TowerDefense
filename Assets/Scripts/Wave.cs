using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public List<SpawnOrder> spawnOrders = new List<SpawnOrder>();

    public Wave()
    {
        spawnOrders = new List<SpawnOrder>(); ;
    }
}

[System.Serializable]
public class SpawnOrder
{
    public float startTime = 0;//初始等待時間
    public float spawnInterval = 1;//時間間隔

    public Soldier enemy = null;
    public int amount = 1;
    public PathIndicator path = null;

    public SpawnOrder(Soldier enemy, int amount, PathIndicator path, float startTime = 0, float spawnInterval = 1)
    {
        this.startTime = startTime;
        this.spawnInterval = spawnInterval;
        this.enemy = enemy;
        this.amount = amount;
        this.path = path;
    }
}
