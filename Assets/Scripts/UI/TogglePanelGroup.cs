using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePanelGroup : MonoBehaviour
{
    public GameObject[] Panels;

    public void OpenPanel(GameObject target) {

        foreach(GameObject obj in Panels) {
            obj.SetActive(obj == target);
        }
    }

}
