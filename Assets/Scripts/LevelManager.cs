using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 關卡管理:全局掌控(暫停、重新開始、離開)，劇情管理(播放劇情、劇情動畫、跳過劇情)
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public WaveManager waveManager;
    public BuildingPlacementSystem buildingSystem;
    public SoldierPool unitPool;

    public Player player;

    private void Start() {
        gameSpeed = speedArray[speedIndex];
        Pause();
    }

    public void GameOver(bool isWin) {
        Pause();
        UIManager.instance.gameOverMenu.GameOver(isWin);
    }

    #region Game_TimeControl

    private bool isPause = false;
    [SerializeField] private float gameSpeed = 1;
    private int speedIndex = 0;
    private float[] speedArray = new float[] { 1f, 2f, 4f, 8f, 0.5f };

    public bool IsPause() { return isPause; }
    [ContextMenu("Resume")]
    public void Resume() {
        Debug.Log("Resume");
        isPause = false;
        Time.timeScale = gameSpeed;
        waveManager.enabled = true;
    }

    [ContextMenu("Pause")]
    public void Pause() {
        Debug.Log("pause");
        isPause = true;
        Time.timeScale = 0;
        waveManager.enabled = false;
    }
    public float ToogleGameSpeed() {
        speedIndex = (speedIndex + 1) % speedArray.Length;
        gameSpeed = speedArray[speedIndex];
        if(!isPause) {
            Time.timeScale = gameSpeed;
        }
        return gameSpeed;
    }

    #endregion

}