using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Building : Unit
{
    public virtual bool CanUpgrade => level < maxLevel;
    public float TotalCost {
        get {
            float total = cost;
            for(int i = 0; i < level; i++) {
                total += data.GetCost(i);
            }
            return total;
        }
    }
    public virtual void Upgrade()
    {
        Debug.Log("Upgrade " + name);
        LevelManager.Instance.player.money -= cost;
        level++;

        //TODO: update values
        maxHealth.baseValue = data.GetMaxHealth(level);
        defense.baseValue = data.GetDefense(level);
        moveSpeed.baseValue = data.GetMoveSpeed(level);
        attack.baseValue = data.GetAttack(level);
        attackTimeInterval.baseValue = data.GetAttackTimeInterval(level);
        attackRange.baseValue = data.GetAttackRange(level);

        Health = maxHealth.FinalValue;
        name = data.name + "[lv." + level +"]";
    }
    public virtual void Sold()
    {
        
        Debug.Log("Sold " + name + " $: " + TotalCost * GobalSetting.returnRate);
        LevelManager.Instance.player.money += TotalCost * GobalSetting.returnRate;

        Destroy();
    }
}
