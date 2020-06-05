using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//增幅周圍傷害、防禦、速度
public class Soldier_King : Soldier
{
    protected override void OnUnitEnter(Unit target) {
        base.OnUnitEnter(target);

        if(target.tag == tag) {
            UseSpeicalAblity(target);
        }
    }

    protected override void OnUnitExit(Unit target) {
        base.OnUnitExit(target);

        if(target.tag == tag) {
            RemoveSpeicalAblity(target);
        }
    }
}
