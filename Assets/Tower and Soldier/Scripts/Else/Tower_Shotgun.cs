using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Shotgun : Tower_Pawn
{
    public int amountPerShot = 10;//每次射出的子彈數量，傷害會平均分散
    public float angle = 10f;//水平散射角度

    protected override void Attack()
    {
        if (muzzle == null || bullet == null)
        {
            Debug.LogError(name + "No Muzzle or Bullet");
        }

        //由左至右，每隔一個角度間隔，發射子彈
        Quaternion newRotation = muzzle.rotation;//目前的角度
        newRotation *= Quaternion.Euler(Vector3.up * -angle / 2);//轉換成向左的起始角度
        float angleInterval = angle / amountPerShot;//角度間隔
        for (int i = 0; i < amountPerShot; i++)
        {
            Shot(newRotation);
            newRotation *= Quaternion.Euler(Vector3.up * angleInterval);//每次向右N度的角度間隔
        }
        
    }
}
