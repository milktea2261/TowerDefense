using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//沒有傷害力、會治癒同伴
public class Soldier_Bishop : Soldier
{
    public LayerMask soldierMask;

    private float healTimer = 0;
    protected override void Update() {
        base.Update();

        if(healTimer <= 0) {
            RangeHeal();
            healTimer = attackTimeInterval.FinalValue;
        }
        healTimer -= Time.deltaTime;
    }

    //範圍治療
    private void RangeHeal() {
        foreach(Unit unit in unitsInRange) {
            if(unit.tag == tag) {
                unit.GetHeal(attack.FinalValue, this);
            }
        }
    }
}
