using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SensText : MonoBehaviour
{
    TextMeshProUGUI text;
    public FloatVariable value;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.SetText(value.Value.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        text.SetText(value.Value.ToString());
    }
}
