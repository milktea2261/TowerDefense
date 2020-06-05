using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public GameObject menu;
    public Text title;

    public void GameOver(bool isWin) 
    {
        title.text = isWin ? "WIN" : "LOSE";
        menu.SetActive(true);
    }

}
