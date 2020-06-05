using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderText : MonoBehaviour
{
    public UnityEngine.UI.Text slidertxt;

    public void SetSliderTxt(float sliderValue)
    {
        slidertxt.text = string.Format("{0:00.0}%", sliderValue*100);
    }
}
