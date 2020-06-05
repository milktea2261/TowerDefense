using My.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// 可設定初始間隔、生成間隔
/// 專注於生成單一對象，任務完成後回收
/// </summary>
public class SoldierSpawner : MonoBehaviour
{
    public System.Action<SoldierSpawner> OnSpawnFinished;
    public System.Action<Soldier> OnSpawnEnemy;

    private bool startSpawn = false;//是否是初始空白期
    private float startTime = 0;

    private float spawnInterval = 0;//時間間隔
    [SerializeField, ReadOnly]
    private float timer = 0;
    private Vector3[] points = new Vector3[0];

    [SerializeField]private Transform target;//終點
    private Soldier enemy = null;
    [SerializeField, ReadOnly]
    private int counter = 0;
    private int level = 1;

    private void Update()
    {
        if (counter <=0)
        {
            OnSpawnFinished.Invoke(this);
            return;
        }

        if (timer <= 0)
        {
            if (!startSpawn)
            {
                startSpawn = true;
            }
            if (counter > 0)
            {
                Spawn();
            }
            timer = spawnInterval;
        }
        timer -= Time.deltaTime;
    }

    public void Init(Transform point, Transform target) 
    {
        transform.position = point.position;
        transform.rotation = point.rotation;

        this.target = target;
    }
    public void SetTimer(float startTime, float spawnInterval) 
    {
        this.startTime = startTime;
        this.spawnInterval = spawnInterval;
    }

    public void SetPath(Vector3[] points) {
        this.points = points;
    }
    public void RequireSpawn(Soldier enemy, int amount, int level = 1)
    {
        this.enemy = enemy;
        counter = amount;
        this.level = level;

        startSpawn = false;
        timer = startTime;
    }

    private void Spawn()
    {
        Soldier newEnemy = LevelManager.Instance.unitPool.SpawnUnit(enemy, transform.position, transform.rotation);
        newEnemy.level = level;
        newEnemy.Init();
        newEnemy.SetPathANDTarget(points, LevelManager.Instance.player.castle);
        
        counter--;
        OnSpawnEnemy(newEnemy);

        newEnemy.SetFaction(!LevelManager.Instance.player.isDarkFaction);
    }

}