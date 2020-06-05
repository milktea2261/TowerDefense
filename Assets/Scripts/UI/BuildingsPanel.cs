using UnityEngine;
using UnityEngine.UI;

public class BuildingsPanel : MonoBehaviour
{
    [SerializeField] private Button[] btns = null;
    private void Start() {
        for(int i = 0; i < btns.Length; i++) {
            Text txt = btns[i].GetComponentInChildren<Text>();
            Building building = LevelManager.Instance.buildingSystem.buildings[i];
            
            txt.text = string.Format("{0}\n${1}", building.name, building.cost);
        }
    }

    public void Show() {
        gameObject.SetActive(true);
    }
    public void Hide() {
        gameObject.SetActive(false);
    }
}
