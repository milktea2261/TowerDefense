using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field)]
public class ExtendEnumAttribute : PropertyAttribute
{
    public readonly bool display = true;
    public ExtendEnumAttribute(bool displayValues = true)
    {
        display = displayValues;
    }
}

/* 使用範例
 * public enum Status { Cost, Hp, asd, };
 * 
 * [ExtendEnum(false)]//T:顯示索引值
 * public Status status;
 */

/* 修改
 * 檢查輸入的字元是否符合enum的規則
 * 提供縮減、交換元素的方法
 */