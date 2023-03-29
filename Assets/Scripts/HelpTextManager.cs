using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpTextManager : MonoBehaviour
{
    GameObject helpText;
    public BoolVariable boolVariable;
    Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Toggle>() != null)
            toggle = GetComponent<Toggle>();
        if(GameObject.FindGameObjectWithTag("HelpText") != null)
            helpText = GameObject.FindGameObjectWithTag("HelpText");
        toggle.isOn = boolVariable.Condition;
        helpText.SetActive(boolVariable.Condition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ManageHelp()
    {
        boolVariable.Condition = helpText.activeSelf;
        
        helpText.SetActive(!boolVariable.Condition);
    }
}
