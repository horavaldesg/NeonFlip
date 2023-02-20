using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BoolText : MonoBehaviour
{
    TextMeshProUGUI text;
    public BoolVariable value;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.SetText(value.Condition.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        text.SetText(value.Condition.ToString());
    }
}
