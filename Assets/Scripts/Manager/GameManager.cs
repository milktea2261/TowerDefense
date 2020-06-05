using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遊戲管理員，最上層的管理員，與下層管理員作互動
/// 
/// 處理跨場景等各種全域問題
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Singleton 
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public SceneController sceneController;
}
