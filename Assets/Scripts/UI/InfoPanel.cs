using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField]private Text content = null;
    [SerializeField, ReadOnly]private Unit target;

    private void Update() {
        if(target == null) { return; }

        content.text = string.Empty;
        content.text += target.name + "\n";
        content.text += string.Format("HP [{0}/{1}]\n", target.Health, target.maxHealth.FinalValue);
        if(target is Building) {
            Building building = (Building)target;
            content.text += string.Format("ATK[{0}]\tRANGE[{1}]\n", building.attack.FinalValue, building.attackRange.FinalValue);
        }
        else if(target is Soldier) {
            Soldier soldier = (Soldier)target;
            content.text += string.Format("DEF[{0}]\tSPD[{1}]\n", soldier.defense.FinalValue, soldier.moveSpeed.FinalValue);
        }
    }

    public void ShowInfo(Unit unit) {
        target = unit;
    }

    public void HideInfo() {
        content.text = string.Empty;
    }
}
