using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 圖形計算器
/// 回傳一連串的座標點來描述圖形
/// </summary>
public static class Draw
{
    /// <summary>
    /// 描繪一個扇形，該法適用法線向上的座標系統
    /// 改善:基於不同坐標系的繪製(增加法向量)EX:傾斜的坐標系、2D系統的座標(XYZ)
    /// </summary>
    /// <param name="center"></param>扇形的中心點
    /// <param name="startDir"></param>扇形的方向
    /// <param name="angle"></param>角度
    /// <param name="radius"></param>半徑
    /// <param name="angleInterval"></param>角度間隔，影響弧線的密度
    /// <returns>index 0為圓心點，其餘為弧線座標</returns>
    public static Vector3[] DrawArc(Vector3 center, Vector3 startDir, float angle, float radius, int angleInterval = 2) {
        List<Vector3> points = new List<Vector3>();
        points.Add(center);//圓心點
        float startAngle = Vector3.SignedAngle(Vector3.forward, startDir, Vector3.up);

        float tempAngle = (startAngle - angle / 2);//起點
        Vector3 newPoint;
        for(int i = 0; i < angle; i += angleInterval) {
            newPoint = new Vector3(Mathf.Sin(tempAngle * Mathf.Deg2Rad), 0, Mathf.Cos(tempAngle * Mathf.Deg2Rad));
            points.Add(newPoint * radius + center);
            tempAngle += angleInterval;
        }
        tempAngle = (startAngle + angle / 2);//終點
        newPoint = new Vector3(Mathf.Sin(tempAngle * Mathf.Deg2Rad), 0, Mathf.Cos(tempAngle * Mathf.Deg2Rad));
        points.Add(newPoint * radius + center);

        return points.ToArray();
    }

    /// <summary>
    /// 描繪貝茲曲線
    /// </summary>
    /// <param name="p1">起點</param>
    /// <param name="p2">終點</param>
    /// <param name="t1">控制點1</param>
    /// <param name="t2">控制點2</param>
    /// <param name="division">分割數</param>
    /// <returns></returns>
    public static Vector3[] DrawBezier(Vector3 p1, Vector3 p2, Vector3 t1, Vector3 t2, int division) {
        Vector3[] points = new Vector3[division + 1];
        for(int i = 0; i <= points.Length; i++) {
            points[i] = CubicBezierLerp(p1, p2, t1, t2, 1f / division * i);
        }
        return null;
    }

    /// <summary>
    /// 在貝茲線段上的插值
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="t1"></param>
    /// <param name="t2"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3 CubicBezierLerp(Vector3 p1, Vector3 p2, Vector3 t1, Vector3 t2, float t) {
        return Mathf.Pow((1 - t), 3) * p1 + 3 * Mathf.Pow((1 - t), 2) * t * p2 + 3 * (1 - t) * Mathf.Pow(t, 2) * t1 + Mathf.Pow(t, 3) * t2;
    }

}
