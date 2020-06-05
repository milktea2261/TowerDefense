using UnityEngine;
using UnityEngine.UI;

public class CommandsPanel : MonoBehaviour
{
    [SerializeField, ReadOnly]private Unit target;

    [SerializeField] private Button upgradeBtn = null;
    [SerializeField] private Button soldBtn = null;
    [SerializeField] private Text upgradeText = null;
    [SerializeField] private Text soldText = null;

    private void Init() {
        upgradeBtn.onClick.RemoveAllListeners();
        soldBtn.onClick.RemoveAllListeners();
    }

    private void Update() {
        if(target == null) { return; }

        if(target is Building) {
            Building building = (Building)target;
            upgradeText.text = "升級\n$" + building.cost;
            soldText.text = "販賣\n$" + building.TotalCost * GobalSetting.returnRate;
            upgradeBtn.interactable = building.CanUpgrade && LevelManager.Instance.player.money >= building.cost;
        }
    }

        public void Show(Unit unit) {
        Init();
        gameObject.SetActive(true);

        target = unit;
        if(unit is Building) {
            Building building = (Building)unit;
            upgradeBtn.onClick.AddListener(building.Upgrade);
            soldBtn.onClick.AddListener(building.Sold);
        }
        upgradeBtn.gameObject.SetActive(unit is Building);
        soldBtn.gameObject.SetActive(unit is Building);
    }
    public void Hide() {
        gameObject.SetActive(false);
        upgradeBtn.onClick.RemoveAllListeners();
        soldBtn.onClick.RemoveAllListeners();
        target = null;
    }
}
