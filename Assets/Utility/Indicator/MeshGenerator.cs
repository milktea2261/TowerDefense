using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    /// <summary>
    /// 矩形
    /// 官方文本: https://docs.unity3d.com/Manual/Example-CreatingaBillboardPlane.html
    /// </summary>
    /// <param name="width">寬</param>
    /// <param name="height">高</param>
    /// <returns></returns>
    public static Mesh CreateRect(float width, float height) {
        Vector3[] vertices = new Vector3[4]{
            new Vector3(0, 0, 0),
            new Vector3(width, 0, 0),
            new Vector3(0, 0, height),
            new Vector3(width, 0, height)
        };

        //頂點座標
        vertices = new Vector3[4] {
            new Vector3(0, 0, 0),
            new Vector3(width, 0, 0),
            new Vector3(0, 0, height),
            new Vector3(width, 0, height)
        };
        //紋理坐標在0到1之間。網格中的每個頂點都有一個紋理坐標，該坐標指定材質的紋理上要採樣的位置。
        Vector2[] uvs = new Vector2[4]
        {
              new Vector2(0, 0),
              new Vector2(1, 0),
              new Vector2(0, 1),
              new Vector2(1, 1)
        };

        //排列頂點的順序，必須順時針排序。
        int[] triangles = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };

        Mesh mesh = new Mesh();
        mesh.name = "Rect(" + width + "*" + height + ")";
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();//自動計算法線

        return mesh;
    }

    /// <summary>
    /// 扇形
    /// </summary>
    /// <param name="radius">半徑</param>
    /// <param name="angle">角度，0-360度</param>
    /// <param name="segments">分段數，最低為1</param>
    /// <returns></returns>
    public static Mesh CreateSector(float radius, float angle, int segments) {
        if(segments < 1 || angle <= 0 || angle > 360) {
            Debug.LogError("Angle out of range OR Segments < 1");
            return null;
        }

        float a = -angle / 2f;//起點，頂點做對稱分佈
        Vector3[] vertices = new Vector3[3 + segments];
        Vector2[] uvs = new Vector2[vertices.Length];

        vertices[0] = Vector3.zero;
        uvs[0] = new Vector2(.5f, .5f);

        for(int i = 1; i < vertices.Length; i++) {
            vertices[i] = Vector.AngleToDirection(a + (i-1) * angle/segments) * radius;
            //將Vertex的區間轉換成[0-1]
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z)/2/radius + new Vector2(.5f, .5f);
        }

        int[] triangles = new int[segments * 3];
        int segIndex = 0;//目前的分段區間
        for(int i = 0; i < triangles.Length; i += 3) {
            triangles[i] = 0;
            triangles[i + 1] = segIndex + 1;
            triangles[i + 2] = segIndex + 2;
            segIndex++;
        }

        Mesh mesh = new Mesh();
        mesh.name = "Sector(" + radius + " rad, "  + angle + "deg)";
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();//自動計算法線

        return mesh;
    }
    public static Mesh CreateSector(float radius, float angle) {
        int segments = angle > 5 ? Mathf.FloorToInt(angle / 4f) : 1;
        return CreateSector(radius, angle, segments);
    }
}
