using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector
{
    /// <summary>
    /// 計算角度和方向，在右方為正[0-180度]，僅用於平面(Vector3.up)上
    /// </summary>
    /// <param name="v1">面向前方的方向</param>
    /// <param name="v2">面向對象的方向</param>
    /// <returns></returns>
    public static float SignAngle(Vector3 v1, Vector3 v2)
    {
        float angle = Vector3.Angle(v1, v2);//夾角
        float sign = Mathf.Sign(Vector3.Cross(v1, v2).y);//外積後取Y值，判斷方向
        return sign * angle;
    }

    /// <summary>
    /// 角度轉換成平面上的方向
    /// </summary>
    /// <param name="angle">角度</param>
    /// <returns></returns>
    public static Vector3 AngleToDirection(float angle) {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

}
