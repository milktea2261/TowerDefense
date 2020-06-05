using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(BuffManager))]
public class Unit : MonoBehaviour, ISelectable
{
    public Action<Unit> OnDead;//正常死亡
    public Action<Unit> OnDestroy;//強制死亡

    [Header("Debug")]
    [SerializeField, ReadOnly] protected string debugStatus = string.Empty;
    [Space(16)]

    [SerializeField] public BuffManager buffManager = null;
    [SerializeField] protected UnitData data = null;
    [SerializeField] protected FactionSetter faction = null;

    public bool isDarkFaction = false;

    protected int maxLevel = 5;
    [Range(0, 5)] public int level = 0;
    public float cost { get { return level < maxLevel ? data.GetCost(level) : 0; } }//花費

    [Header("Health")]
    [SerializeField] protected bool isDead = false;
    [SerializeField, ReadOnly] private float _health;
    public float Health { get { return _health; } protected set { _health = value; } }
    public Attribute maxHealth;//血量上限
    public Attribute defense;//防禦值
    [Header("Speed")]
    public Attribute moveSpeed;//移動速度
    [Header("Damage")]
    public Attribute attack;//傷害值
    public Attribute attackTimeInterval;//攻擊間隔
    public Attribute attackRange;//攻擊範圍
    [Space()]
    [Header("Targeting")]
    public LayerMask targetMask;
    public TargetingSystem.TargetMode targetMode = TargetingSystem.TargetMode.Closet;
    [SerializeField, ReadOnly] protected Unit m_target = null;
    [SerializeField, ReadOnly] protected List<Unit> unitsInRange;
    protected float attackTimer = 0;

    #region 初始化
    protected virtual void Awake() {
        if(data == null) {
            Debug.LogError(name + "Not Found Data");
        }

        buffManager.OnBuffChanged += BuffChanged;
        data.specialAblity.source = this;

        Init();
    }
    public virtual void Init() {
        isDead = false;

        SetHealth(data.GetMaxHealth(level), data.GetDefense(level));
        SetSpeed(data.GetMoveSpeed(level));
        SetAttack(data.GetAttack(level), data.GetAttackTimeInterval(level), data.GetAttackRange(level));

        Health = maxHealth.FinalValue;
        name = data.name + "[lv." + level + "]";
    }
    public virtual void SetHealth(float maxHealth, float defense) {
        this.maxHealth.baseValue = maxHealth;
        this.defense.baseValue = defense;
    }
    public virtual void SetSpeed(float moveSpeed) {
        this.moveSpeed.baseValue = moveSpeed;
    }
    public virtual void SetAttack(float attack, float attackTimeInterval, float attackRange) {
        this.attack.baseValue = attack;
        this.attackTimeInterval.baseValue = attackTimeInterval;
        this.attackRange.baseValue = attackRange;
    }

    public virtual void BuffChanged() {
        //Debug.Log("buff Changed");
        SetHealth(data.GetMaxHealth(level), data.GetDefense(level));
        SetSpeed(data.GetMoveSpeed(level));
        SetAttack(data.GetAttack(level), data.GetAttackTimeInterval(level), data.GetAttackRange(level));

        _health = Mathf.Clamp(_health, 0, maxHealth.FinalValue);
    }
    #endregion

    #region 攻擊
    protected virtual string EnemyTag => this.tag == "Dark" ? "Light" : "Dark";
    protected virtual void Update() {
        if(isDead) {
            debugStatus = "Dead";
            return;
        }
        //索敵
        List<Unit> newUnitsInRange = SpotUnits();

        //enter range(舊的沒有，新的有)
        IEnumerable<Unit> exitUnits = newUnitsInRange.Except(unitsInRange);
        foreach(Unit t in exitUnits) {
            OnUnitEnter(t);
        }

        //exit range(舊的有，新的沒有)
        IEnumerable<Unit> enterUnits = unitsInRange.Except(newUnitsInRange);
        foreach(Unit t in enterUnits) {
            OnUnitExit(t);
        }
        unitsInRange = newUnitsInRange;


        if(m_target == null) {
            debugStatus = "Searching";
            m_target = GetTarget(unitsInRange);
        }
        else {
            //超出範圍
            if(CheckTargetExist()) {
                debugStatus = "Attacking";
                //Aim
                Aim();
                //攻擊
                if(attackTimer <= 0) {
                    Attack();
                    attackTimer = attackTimeInterval.FinalValue;
                }
            }
            else {
                m_target = null;
            }
        }

        if(attackTimer > 0) {
            attackTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 在範圍地的所有單位
    /// </summary>
    /// <returns></returns>
    protected virtual List<Unit> SpotUnits() {
        return SpottingSystem.SphereCast<Unit>(transform.position, attackRange.FinalValue, targetMask);
    }
    /// <summary>
    /// 進入範圍內的單位
    /// </summary>
    /// <param name="target"></param>
    protected virtual void OnUnitEnter(Unit target) {
        //Debug.Log(target.name + " Enter");
    }
    /// <summary>
    /// 離開範圍的單位
    /// </summary>
    /// <param name="target"></param>
    protected virtual void OnUnitExit(Unit target) {
        if(target == null) return;
        //Debug.Log(target.name + " Enter");
    }
    /// <summary>
    /// 篩選出攻擊目標
    /// </summary>
    /// <param name="units"></param>
    /// <returns></returns>
    protected virtual Unit GetTarget(List<Unit> units) {
        List<Unit> targets = units;
        targets.RemoveAll(x => x.tag == tag);
        return TargetingSystem.GetTarget(targets.ToArray(), transform.position, targetMode);
    }

    protected virtual bool CheckTargetExist() {
        //不存在敵人
        if(m_target == null) {
            return false;
        }

        //敵人死亡
        if(m_target.Health <= 0 || m_target.isDead) {
            return false;
        }

        //敵人不在範圍內
        float enemyRadius = 0.5f;//敵人的半徑
        if(Vector3.Distance(transform.position, m_target.transform.position) > (attackRange.FinalValue + enemyRadius)) {
            return false;
        }
        return true;
    }
    protected virtual void Aim() {
        transform.LookAt(m_target.transform.position);
    }
    protected virtual void Attack() {
        m_target.GetDamage(attack.FinalValue, this);
    }

    protected virtual void UseSpeicalAblity(Unit unit) {
        Buff copyBuff = data.specialAblity;
        Debug.Log(copyBuff);
        copyBuff.source = this;
        unit.buffManager.AddBuff(copyBuff);
    }
    protected virtual void RemoveSpeicalAblity(Unit unit) {
        Buff copyBuff = data.specialAblity;
        copyBuff.source = this;
        unit.buffManager.RemoveBuff(copyBuff);
    }

    #endregion

    //直接的傷害
    public virtual void GetDirectDamage(float damage, object damager) {
        if(isDead) return;
        if(damage <= 0) return;

        Health -= damage;
        if(Health <= 0) Dead();
    }
    //正常的傷害公式
    public virtual void GetDamage(float damage, object damager) {
        if(isDead) return;

        Debug.Log(damager + " hit " + name + " [" + damage + "]");
        float finalDamage = Mathf.Clamp(damage - defense.FinalValue, 0, float.MaxValue);//避免造成補血的奇怪事情
        Health -= finalDamage;
        if(Health <= 0) Dead();
    }

    //治療
    public virtual void GetHeal(float heal, object healer) {
        if(isDead) return;
        if(heal <= 0) return;
        //Debug.Log(healer + " heal " + name + " [" + heal + "]");
        Health = Mathf.Clamp(Health + heal, 0, maxHealth.FinalValue);
    }
    /// <summary>
    /// 正常死亡
    /// </summary>
    protected virtual void Dead() {
        //Debug.Log(name + " DEAD");
        OnDead.Invoke(this);
        isDead = true;

        Unit[] units = FindObjectsOfType<Unit>();
        foreach(Unit unit in units) {
            unit.buffManager.RemoveBuffFromSource(this);
        }
    }

    /// <summary>
    /// 非正常，強制性死亡
    /// </summary>
    public virtual void Destroy() {
        isDead = true;
        OnDestroy.Invoke(this);
    }

    public virtual void SetFaction(bool isDark) {
        isDarkFaction = isDark;
        faction.SetFaction(isDark);
    }

    public virtual void OnSelected() {
        //Debug.Log("Select " + name);
        UIManager.instance.SelectUnit(this);
    }
    public virtual void OnDeselected() {
        //Debug.Log("Deselect " + name);
        UIManager.instance.DeselectUnit();
    }

    protected virtual void OnDrawGizmosSelected() {
        //範圍顯示
        Gizmos.color = Color.blue;
        Vector3[] points = Draw.DrawArc(transform.position, transform.forward, 360, attackRange.FinalValue);
        for(int i = 1; i < points.Length - 1; i++) {
            Gizmos.DrawLine(points[i], points[i + 1]);
        }
    }
}

