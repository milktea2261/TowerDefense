using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public System.Action OnBuffChanged;

    private Unit target;
    [SerializeField] private List<Buff> buffs = new List<Buff>();
    private bool isUpdate = false;

    private void Awake() {
        target = GetComponent<Unit>();
    }

    private void OnDisable() {
        StopAllCoroutines();
    }

    private IEnumerator UpdateStatus() {
        isUpdate = true;
        //Debug.Log("UpdateStatus");
        while(buffs.Count > 0) {
            for(int i = buffs.Count - 1; i >= 0; i--) {
                
                //觸發Buff效果(EX:持續性扣血)
                if(buffs[i].damageDot != 0) {
                    //Debug.Log("damageDot " + buffs[i].damageDot);
                    target.GetDamage(buffs[i].damageDot, buffs[i].source);
                }
                if(buffs[i].healDot != 0) {
                    target.GetHeal(buffs[i].healDot, buffs[i].source);
                }

                if(buffs[i].type == BuffType.Time) {
                    Buff newBuff = buffs[i];
                    newBuff.duration--;
                    buffs[i] = newBuff;
                    //移除Buff
                    if(buffs[i].duration <= 0) {
                        RemoveBuff(buffs[i]);
                    }
                }
            }
            yield return new WaitForSeconds(1f);
        }

        isUpdate = false;
        yield break;
    }

    public void AddBuff(Buff buff) {
        foreach(Buff b in buffs) {
            if(b.source == buff.source) {
                Debug.Log("Find Same Buff");
                return;
            }
        }

        //Debug.Log("AddBuff from " + buff.source);
        buffs.Add(buff);
        //add mods
        if(buff.moveSpeedFlat != 0) {
            target.moveSpeed.AddModifier(new AttributeModifier(buff.moveSpeedFlat, AttributeModifierType.Flat, buff.source));
        }
        if(buff.healthFlat != 0) {
            target.maxHealth.AddModifier(new AttributeModifier(buff.healthFlat, AttributeModifierType.Flat, buff.source));
        }
        if(buff.defenseFlat != 0) {
            target.defense.AddModifier(new AttributeModifier(buff.defenseFlat, AttributeModifierType.Flat, buff.source));
        }
        if(buff.attackFlat != 0) {
            target.attack.AddModifier(new AttributeModifier(buff.attackFlat, AttributeModifierType.Flat, buff.source));
        }
        if(buff.attackTimeIntervalFlat != 0) {
            target.attackTimeInterval.AddModifier(new AttributeModifier(buff.attackTimeIntervalFlat, AttributeModifierType.Flat, buff.source));
        }
        if(buff.attackRangeFlat != 0) {
            target.attackRange.AddModifier(new AttributeModifier(buff.attackRangeFlat, AttributeModifierType.Flat, buff.source));
        }


        //刷新時間
        if(gameObject.activeInHierarchy) {
            if(!isUpdate && buffs.Count > 0) {
                StartCoroutine(UpdateStatus());
            }
        }
        OnBuffChanged.Invoke();
    }
    public void RemoveBuff(Buff buff) {
        //Debug.Log("RemoveBUff " + buff);
        buffs.Remove(buff);

        //rmove mods
        if(buff.moveSpeedFlat != 0) {
            target.moveSpeed.RemoveModifier(new AttributeModifier(buff.moveSpeedFlat, AttributeModifierType.Flat, buff.source));
        }
        if(buff.healthFlat != 0) {
            target.maxHealth.RemoveModifier(new AttributeModifier(buff.healthFlat, AttributeModifierType.Flat, buff.source));
        }
        if(buff.defenseFlat != 0) {
            target.defense.RemoveModifier(new AttributeModifier(buff.defenseFlat, AttributeModifierType.Flat, buff.source));
        }
        if(buff.attackFlat != 0) {
            target.attack.RemoveModifier(new AttributeModifier(buff.attackFlat, AttributeModifierType.Flat, buff.source));
        }
        if(buff.attackTimeIntervalFlat != 0) {
            target.attackTimeInterval.RemoveModifier(new AttributeModifier(buff.attackTimeIntervalFlat, AttributeModifierType.Flat, buff.source));
        }
        if(buff.attackRangeFlat != 0) {
            target.attackRange.RemoveModifier(new AttributeModifier(buff.attackRangeFlat, AttributeModifierType.Flat, buff.source));
        }

        OnBuffChanged.Invoke();
    }
    public void RemoveBuffFromSource(object source) {
        //Debug.Log("RemoveBUff from " + source);
        buffs.RemoveAll(x => x.source == source);

        target.moveSpeed.RemoveAllModifiersFromSource(source);
        target.maxHealth.RemoveAllModifiersFromSource(source);
        target.defense.RemoveAllModifiersFromSource(source);
        target.attack.RemoveAllModifiersFromSource(source);
        target.attackTimeInterval.RemoveAllModifiersFromSource(source);
        target.attackRange.RemoveAllModifiersFromSource(source);
        OnBuffChanged.Invoke();
    }
    
}
