using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class OptionsManager : MonoBehaviour
{
    Slider slider;
    public FloatVariable currentFloat;
    public BoolVariable currentBool;
    public bool isFloat;
    // Start is called before the first frame update
    void Start()
    {
        slider = this.GetComponent<Slider>();
        if (!isFloat)
        {
            if(currentBool.Condition == true)
            {
                slider.value = 1;
            }
            else if(currentBool.Condition == false)
            {
                slider.value = 0;
            }


        }
        else if(isFloat)
        {
            slider.value = currentFloat.Value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeHorizontalSens(FloatVariable floatValue)
    {
        floatValue.Value = slider.value;
    }
    public void ChangeVerticalSens(FloatVariable floatValue)
    {

        floatValue.Value = slider.value;
    }
    public void ChangeControllerHorizontalSens(FloatVariable floatValue)
    {

        floatValue.Value = slider.value;
    }
    
    public void ChangeControllerVerticalSens(FloatVariable floatValue)
    {

        floatValue.Value = slider.value;
    }
    public void Vibration(BoolVariable vibration)
    {

        if (slider.value == 0)
        {
            vibration.Condition = false;
        }
        else if(slider.value == 1)
        {
            vibration.Condition = true;
        }
    }
    
}
