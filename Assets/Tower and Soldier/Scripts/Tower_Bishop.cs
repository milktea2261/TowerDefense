using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//緩速敵人
public class Tower_Bishop : Tower
{
    protected override void OnUnitEnter(Unit target) {
        base.OnUnitEnter(target);

        if(target.tag == EnemyTag) {
            UseSpeicalAblity(target);
        }
    }

    protected override void OnUnitExit(Unit target) {
        base.OnUnitExit(target);

        if(target.tag == EnemyTag) {
            RemoveSpeicalAblity(target);
        }
    }

}
