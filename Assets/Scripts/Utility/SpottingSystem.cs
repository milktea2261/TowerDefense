using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 偵查系統，輕鬆找到範圍內的對象
/// </summary>
public static class SpottingSystem
{
    /// <summary>
    /// 球體碰撞檢測，過濾掉非目標類別的項目
    /// </summary>
    /// <typeparam name="T">對象類別</typeparam>
    /// <param name="position">中心點</param>
    /// <param name="radius">半徑</param>
    /// <param name="layerMask">圖層</param>
    /// <returns></returns>
    public static List<T> SphereCast<T>(Vector3 position, float radius, int layerMask = 0, string tag = "Untagged")
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius, layerMask);
        List<T> targets = new List<T>();
        if (colliders.Length > 0)
        {
            foreach (Collider c in colliders)
            {
                if(tag != "Untagged") {
                    if(c.CompareTag(tag)) {
                        T target = c.GetComponent<T>();
                        if(target != null) {
                            targets.Add(target);
                        }
                    }
                }
                else {
                    T target = c.GetComponent<T>();
                    if(target != null) {
                        targets.Add(target);
                    }
                }
            }
        }
        return targets;
    }

    /// <summary>
    /// 圓柱體碰撞檢測，過濾掉非目標類別的項目
    /// </summary>
    /// <typeparam name="T">對象類別</typeparam>
    /// <param name="upPos">上頂點</param>
    /// <param name="downPos">下頂點</param>
    /// <param name="radius">半徑</param>
    /// <param name="layerMask">圖層</param>
    /// <returns></returns>
    public static List<T> CylinderCast<T>(Vector3 upPos, Vector3 downPos, float radius, int layerMask = 0, string tag = "Untagged") 
    {
        Collider[] colliders = Physics.OverlapCapsule(upPos, downPos, radius, layerMask);
        List<T> targets = new List<T>();
        if (colliders.Length > 0)
        {
            foreach (Collider c in colliders)
            {
                if(tag != "Untagged") {
                    if(c.CompareTag(tag)) {
                        T target = c.GetComponent<T>();
                        if(target != null) {
                            targets.Add(target);
                        }
                    }
                }
                else {
                    T target = c.GetComponent<T>();
                    if(target != null) {
                        targets.Add(target);
                    }
                }
            }
        }
        return targets;
    }


    /// <summary>
    /// 未實作該功能
    /// 圓錐體碰撞檢測，過濾掉非目標類別的項目
    /// </summary>
    /// <typeparam name="T">對象類別</typeparam>
    /// <param name="upPos">上頂點</param>
    /// <param name="downPos">下頂點</param>
    /// <param name="upRadius">上半徑</param>
    /// <param name="donwRadius">下半徑</param>
    /// <param name="layerMask">圖層</param>
    /// <returns></returns>
    public static List<T> ConeCast<T>(Vector3 upPos, Vector3 downPos, float donwRadius, float upRadius = 0, int layerMask = 0, string tag = "Untagged")
    {
        return null;
    }

}
