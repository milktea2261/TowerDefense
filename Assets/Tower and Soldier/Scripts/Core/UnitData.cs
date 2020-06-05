using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 對應Unit的參數
/// </summary>
[CreateAssetMenu(menuName = "Data/Unit")]
public class UnitData : ScriptableObject
{
    [Multiline(3)]
    public string description;
    [Space()]
    [SerializeField] private float cost = 0;

    [Header("Health")]
    [SerializeField]private float maxHealth = 0;
    [SerializeField] private float defense = 0;
    [Header("Speed")]
    [SerializeField] private float moveSpeed = 0;
    [Header("Damage")]
    [SerializeField] private float attack = 0;
    [SerializeField] private float attackTimeInterval = 0;
    [SerializeField] private float attackRange = 0;

    [Header("Upgrade Setting")]//加法獎勵
    [SerializeField] private float maxHealthAdder = 0;
    [SerializeField] private float defenseAdder = 0;
    [SerializeField] private float moveSpeedAdder = 0;
    [SerializeField] private float damageAdder = 0;
    [SerializeField] private float attackTimeIntervalAdder = 0;
    [SerializeField] private float attackRangeAdder = 0;
    [SerializeField] private float costAdder = 0;
    //[Space()]//乘法獎勵
    //[SerializeField] private float maxHealthMultiplier = 1;
    //[SerializeField] private float defenseMultiplier = 1;
    //[SerializeField] private float moveSpeedMultiplier = 1;
    //[SerializeField] private float damageMultiplier = 1;
    //[SerializeField] private float attackTimeIntervalMultiplier = 1;
    //[SerializeField] private float attackRangeMultiplier = 1;
    //[SerializeField] private float costMultiplier = 1;

    public Buff specialAblity;

    //通用公式: [基礎值 + (等級 * 加法獎勵)] * 乘法獎勵^等級
    public float GetMaxHealth(int level) { return maxHealth + level * maxHealthAdder; }
    public float GetDefense(int level) { return defense + level * defenseAdder; }
    public float GetMoveSpeed(int level) { return moveSpeed + level * moveSpeedAdder; }
    public float GetAttack(int level) { return attack + level * damageAdder; }
    public float GetAttackTimeInterval(int level) { return attackTimeInterval + level * attackTimeIntervalAdder; }
    public float GetAttackRange(int level) { return attackRange + level * attackRangeAdder; }
    public float GetCost(int level) { return cost + level * costAdder; }
}