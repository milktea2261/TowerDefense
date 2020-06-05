using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//被攻擊後 提高速度
public class Soldier_Knight : Soldier
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] private List<Soldier> oldList;
    public LayerMask soldierMask;
    

    bool isUseAblity = false;

    public override void Init() {
        base.Init();
        isUseAblity = false;
    }

    public override void GetDamage(float damage, object damager) {
        base.GetDamage(damage, damager);
        if(!isUseAblity) {
            Debug.Log("Run fast");
            UseSpeicalAblity(this);
            particle.Play();

            isUseAblity = true;
        }
    }
}
