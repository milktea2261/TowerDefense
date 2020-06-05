using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Unity——来个可变角度的扇形技能指示器
//网上有不少这种生成面片的文章
//http://baizihan.me/2016/10/draw-sector/

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class RangeIndicator : MonoBehaviour
{
    public float radius = 2;//半徑
    [Range(1,360)]
    public float angle = 100;//角度

    private MeshFilter meshFilter;

    private void Awake() {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void Update() {
        meshFilter.mesh = MeshGenerator.CreateSector(radius, angle);
    }

}