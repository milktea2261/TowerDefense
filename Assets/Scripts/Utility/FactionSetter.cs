using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class FactionSetter : MonoBehaviour
{
    public Material darkMat;
    public Material lightMat;

    MeshRenderer rend;

    private void Awake() {
        rend = GetComponent<MeshRenderer>();
    }

    public void SetFaction(bool isDark) {
        if(isDark) {
            //TODO
            tag = "Dark";
            rend.material = darkMat;
        }
        else {
            //TODO
            tag = "Light";
            rend.material = lightMat;
        }
    }
}
