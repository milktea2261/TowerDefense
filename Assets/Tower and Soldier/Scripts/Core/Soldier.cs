using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(SoldierAgent))]
[RequireComponent(typeof(CapsuleCollider))]
public class Soldier : Unit
{
    [SerializeField]private SoldierAgent agent = null;
    public float DstToEnd => agent.dstToEndPoint;

    public override void Init() {
        base.Init();

        agent.SetMoveSpeed(moveSpeed.FinalValue);
    }
    public void SetPathANDTarget(Vector3[] points, Unit target) {
        m_target = target;

        agent.SetPathANDTarget(points, m_target.transform);
    }

    protected override void Dead() {
        base.Dead();
        LevelManager.Instance.player.money += cost * GobalSetting.returnRate;
    }

    public override void BuffChanged() {
        base.BuffChanged();
        agent.SetMoveSpeed(moveSpeed.FinalValue);
    }

    protected override void Update() {
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

        if(Vector3.Distance(transform.position, m_target.transform.position) <= 1) {
            Debug.Log("Reach Castle");
            m_target.GetDirectDamage(1, this);
            Destroy();
        }
    }


    protected override Unit GetTarget(List<Unit> units) {
        return null;
        //return base.GetTarget(units);
    }
    protected override void Aim() {
        Vector3 pos = new Vector3(m_target.transform.position.x, transform.position.y, m_target.transform.position.z);
        transform.LookAt(pos);
    }
}
