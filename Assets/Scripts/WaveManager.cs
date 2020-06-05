using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生成器負責一個路徑上的一種怪物，任務完成後回收
/// 多個怪物多/同路徑、同/不同時間出現的實現: 利用多個生成器來實現
/// 
/// 下次個波次的發動條件:
/// 前一個波次必需生成完畢，才能開始倒數
/// 提前波次條件:
/// 場上沒有存活任何敵人
/// </summary>
public class WaveManager : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private int waveIndex = 0;//波數計數
    [SerializeField, ReadOnly]
    private bool isSpawnFinished = false;//該波生成結束
    private List<SoldierSpawner> spawners = new List<SoldierSpawner>();
    [SerializeField, ReadOnly]
    private float timer = 0f;//計時器
    [Space()]
    public int waveInterval = 30;//等生產完畢再計算時間OR不等待

    [Header("Skip Wave")]
    public bool autoSkip = false;
    [SerializeField, ReadOnly]
    private bool allowSkip = false;
    [SerializeField, ReadOnly]
    private int enemyExistCounter = 0;//計數活在場上的敵人

    [Header("Data")]
    public int soldierLevel = 1;
    public SoldierSpawner spawnerPrefab;
    public PathIndicator[] paths;
    public Soldier[] enemiesDB;
    [Header("Wave")]
    public List<Wave> waves;


    public int CurrentWave { get { return waveIndex; } }
    public int EnemyExist { get { return enemyExistCounter; } }

    private void Update()
    {
        if (waveIndex >= waves.Count)
        {
            return;
        }

        if (timer <= 0)
        {
            NextWave();
        }

        allowSkip = isSpawnFinished && enemyExistCounter == 0;
        if (autoSkip)
        {
            if (allowSkip)
            {
                NextWave();
            }
        }

        if (isSpawnFinished)
        {
            timer -= Time.deltaTime;
        }
    }

    private void NextWave()
    {
        if (waveIndex < waves.Count)
        {
            Debug.Log("Wave " + waveIndex);
            for (int i = 0; i < waves[waveIndex].spawnOrders.Count; i++)
            {
                SoldierSpawner newSpawner = Instantiate(spawnerPrefab);
                //assign event
                newSpawner.OnSpawnFinished += OnSpawnerFinish;
                newSpawner.OnSpawnEnemy += OnSpawnEnemy;
                newSpawner.Init(waves[waveIndex].spawnOrders[i].path.transform, LevelManager.Instance.player.castle.transform);
                newSpawner.SetTimer(waves[waveIndex].spawnOrders[i].startTime, waves[waveIndex].spawnOrders[i].spawnInterval);

                Vector3[] path = new Vector3[0];
                foreach(PathIndicator spawnPoint in paths) {
                    if(spawnPoint.gameObject == waves[waveIndex].spawnOrders[i].path.gameObject) {
                        path = spawnPoint.points;
                    }
                }
                newSpawner.SetPath(path);

                newSpawner.RequireSpawn(waves[waveIndex].spawnOrders[i].enemy, waves[waveIndex].spawnOrders[i].amount, soldierLevel);

                spawners.Add(newSpawner);
                isSpawnFinished = false;
            }

            waveIndex++;

            timer = waveInterval;
        }
        else
        {
            Debug.Log("No More Wave");
        }
    }

    public void SkipWave() 
    {
        if (allowSkip)
        {
            Debug.Log("Skip Wave");
            NextWave();
        }
    }

    public System.Action OnAllEnemiesClear;//所有波次結束、場上的敵人死光
    public void OnSpawnerFinish(SoldierSpawner spawner) 
    {
        spawners.Remove(spawner);
        //Destroy(spawner.gameObject);

        if (spawners.Count == 0)
        {
            isSpawnFinished = true;
        }
    }
    public void OnSpawnEnemy(Unit enemy) 
    {
        enemy.OnDead += OnEnemyDead;
        enemy.OnDestroy += OnEnemyDead;
        enemyExistCounter++;
    }
    public void OnEnemyDead(Unit enemy) 
    {
        enemyExistCounter--;

        enemy.OnDead -= OnEnemyDead;
        enemy.OnDestroy -= OnEnemyDead;

        if (waveIndex >= waves.Count && isSpawnFinished && enemyExistCounter == 0)
        {
            OnAllEnemiesClear.Invoke();
        }
    }
}