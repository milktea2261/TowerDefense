using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Knight : Projectile
{
    public float explodeRadius = 1f;

    protected override void Hit(Collider hit, Vector3 hitPoint) {
        Collider[] colliders = Physics.OverlapSphere(hitPoint, explodeRadius, hitMask);
        foreach(Collider c in colliders) {
            Unit damagable = c.GetComponent<Unit>();
            if(damagable != null) {
                Damage(damagable);
            }
        }

    }
}
