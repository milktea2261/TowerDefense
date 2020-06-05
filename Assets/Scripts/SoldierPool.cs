using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierPool : MonoBehaviour
{
    [SerializeField, ReadOnly]private List<Unit> soldiers = new List<Unit>();

    //創造
    public Soldier CreateUnit(Soldier obj) {
        //Debug.Log("Create");
        Soldier newUnit = Instantiate<Soldier>(obj);
        newUnit.gameObject.SetActive(false);

        newUnit.OnDead += FreezeUnit;
        newUnit.OnDestroy += FreezeUnit;

        return newUnit;
    }

    //派遣
    public Soldier SpawnUnit(Soldier obj, Vector3 position, Quaternion rotation) {
        //Debug.Log("SPAWN");
        Soldier spawn = (Soldier)soldiers.Find(x => x.GetType() == obj.GetType());
        soldiers.Remove(spawn);
        if(spawn == null) {
            spawn = CreateUnit(obj);
        }

        spawn.transform.position = position;
        spawn.transform.rotation = rotation;
        spawn.gameObject.SetActive(true);
        return spawn;
    }
    //回收
    public void FreezeUnit(Unit unit) {
        //Debug.Log("FREEZE");
        unit.gameObject.SetActive(false);
        soldiers.Add(unit);
    }
}
