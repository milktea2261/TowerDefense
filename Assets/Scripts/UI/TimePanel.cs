using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimePanel : MonoBehaviour
{
    [SerializeField]private Text timeStateText = null;
    [SerializeField]private Text timeSpeedText = null;

    public void TogglePause() {
        if(LevelManager.Instance.IsPause()) {
            LevelManager.Instance.Resume();
            timeStateText.text = "Pause";
        }
        else {
            LevelManager.Instance.Pause();
            timeStateText.text = "Play";
        }
    }
    public void ToggleTimeSpeed() {
        float timeSpeed = LevelManager.Instance.ToogleGameSpeed();

        timeSpeedText.text = "X " + timeSpeed.ToString("0.0");
    }
}
