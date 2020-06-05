using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Text ResourceText;
    public InfoPanel info;

    public BuildingsPanel buildingsPanel;
    public CommandsPanel commandsPanel;

    public GameOverMenu gameOverMenu;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        ResourceTextUpdate();
    }

    private void ResourceTextUpdate() {
        ResourceText.text = string.Empty;
        ResourceText.text += string.Format("[Wave:{0}/{1}]    ", LevelManager.Instance.waveManager.CurrentWave, LevelManager.Instance.waveManager.waves.Count);
        ResourceText.text += string.Format("[$:{0}]    ", LevelManager.Instance.player.money);
        ResourceText.text += string.Format("[Hp:{0}]    ", LevelManager.Instance.player.castle.Health);
    }

    public void SelectUnit(Unit unit) {
        buildingsPanel.Hide();
        commandsPanel.Show(unit);
        info.ShowInfo(unit);
    }
    public void DeselectUnit() {
        buildingsPanel.Show();
        commandsPanel.Hide();
        info.HideInfo();
    }

    #region SceneMethod
    public void MainMenu()
    {
        GameManager.Instance.sceneController.LoadMainMenu();
    }
    public void Restart()
    {
        GameManager.Instance.sceneController.ReloadLevel();
    }

    #endregion
}
