using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//原地範圍攻擊，附加火傷。EX:地震塔
public class Tower_Rook : Tower
{
    [SerializeField]ParticleSystem particle = null;

    protected override void Attack() {
        particle.Play();
        foreach(Unit target in unitsInRange) {
            if(target.tag == EnemyTag) {

                target.GetDamage(attack.FinalValue, this);
                UseSpeicalAblity(target);
            }
        }
    }
}
