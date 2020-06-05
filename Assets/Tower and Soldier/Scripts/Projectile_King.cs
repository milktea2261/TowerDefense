using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//鎖鏈攻擊，事先決定好攻擊對象，由近到遠，子彈沒有飛行時間
public class Projectile_King : Projectile
{
    public Unit[] linkedTarget;

    protected override void Start() {
        for(int i = 0; i < linkedTarget.Length; i++) {
            Damage(linkedTarget[i]);
        }
        Destroy(gameObject);
    }

}
