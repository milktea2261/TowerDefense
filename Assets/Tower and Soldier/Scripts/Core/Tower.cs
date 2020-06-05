using UnityEngine;

public class Tower : Building
{
    public RangeIndicator rangeIndicator;

    public Transform muzzle;
    public Transform turret;
    public Projectile bullet;

    protected override void Awake() {
        base.Awake();
        rangeIndicator.radius = attackRange.FinalValue;
    }

    protected virtual void Start() {
        rangeIndicator.gameObject.SetActive(false);
    }

    public override void Init() {
        base.Init();
        rangeIndicator.gameObject.SetActive(false);
    }

    protected override void Aim() {
        if(turret == null) {
            Debug.Log("No turret");
            return;
        }
        float heightOffset = 0.5f;
        turret.LookAt(m_target.transform.position + Vector3.up * heightOffset);
        Debug.DrawLine(turret.position, m_target.transform.position + Vector3.up * heightOffset, Color.white);
    }
    protected override void Attack() {
        if(muzzle == null) {
            Debug.LogError(name + "No Muzzle");
        }

        if(bullet == null) {
            Debug.LogWarning(name + " No Bullet");
            m_target.GetDamage(attack.FinalValue, this);
            return;
        }
        Shot(muzzle.rotation);
    }
    protected virtual Projectile Shot(Quaternion rotation) {
        Projectile b = Instantiate(bullet, muzzle.position, rotation);
        b.OnDamage += OnBulletHit;
        return b;
    }
    public virtual void OnBulletHit(Unit target) {
        target.GetDamage(attack.FinalValue, this);
    }

    public override void Upgrade() {
        base.Upgrade();
        rangeIndicator.radius = attackRange.FinalValue;
    }

    public override void OnSelected() {
        base.OnSelected();
        rangeIndicator.gameObject.SetActive(true);
    }
    public override void OnDeselected() {
        base.OnDeselected();
        rangeIndicator.gameObject.SetActive(false);
    }
}
