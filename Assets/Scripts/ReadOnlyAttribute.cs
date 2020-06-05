using UnityEngine;
using System;

/// <summary>
/// 讓欄位只可以顯示，不能更改
/// 備註:使用陣列時，長度可以更改，元素不能
/// </summary>

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class ReadOnlyAttribute : PropertyAttribute { }

