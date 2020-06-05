using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    //public Level[] levels;
    //public Button btnPrefab;

    public void SelectLevel(string sceneName) 
    {
        Debug.Log("Select " + sceneName);
        GameManager.Instance.sceneController.LoadLevel(sceneName, true);
    }
}

[System.Serializable]
public class Level 
{
    public string levelName;
    public string levelInfo;
    public string sceneName;
}
