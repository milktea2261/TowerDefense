using UnityEngine;

public class Projectile : MonoBehaviour
{
    public event System.Action<Unit> OnDamage;

    public float moveSpeed = 10f;

    public float lifeTime = 1.5f;
    public LayerMask hitMask;

    protected virtual void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    protected virtual void Update()
    {
        Move();
        DetectCollision();
    }

    protected virtual void Move() {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
    protected virtual void DetectCollision() {
        Debug.DrawRay(transform.position, transform.forward * moveSpeed * Time.deltaTime, Color.red);
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, moveSpeed * Time.deltaTime, hitMask)) {
            Hit(hit.collider, hit.point);
        }
    }
    protected virtual void Hit(Collider hit, Vector3 hitPoint) {
        Unit damagable = hit.GetComponent<Unit>();
        if(damagable.isDarkFaction == LevelManager.Instance.player.isDarkFaction) {
            return;
        }
        if(damagable != null) {
            Damage(damagable);
            Destroy(gameObject);
        }
    }

    protected virtual void Damage(Unit damagable) 
    {
        OnDamage.Invoke(damagable);
    }
}
