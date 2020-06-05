using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//貫通屬性，射線上的敵人都會受到傷害
[RequireComponent(typeof(LineRenderer))]
public class Projectile_Laser : Projectile
{
    [SerializeField] LineRenderer lineRend = null;


    public float rayLength = 10f;

    protected override void Start() {
        DetectCollision();
        Destroy(gameObject, 0.5f);
    }

    protected override void Update() {
    }

    protected override void DetectCollision() {
        Debug.DrawRay(transform.position, transform.forward * rayLength * Time.deltaTime, Color.red);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, rayLength, hitMask);
        if(hits.Length > 0) {
            foreach(RaycastHit hit in hits) {
                Hit(hit.collider, hit.point);
            }
        }

        lineRend.SetPosition(0, transform.position);
        lineRend.SetPosition(1, transform.position + transform.forward * rayLength);
    }

    protected override void Hit(Collider hit, Vector3 hitPoint) {
        Unit damagable = hit.GetComponent<Unit>();
        if(damagable != null) {
            Damage(damagable);
        }
    }

}
