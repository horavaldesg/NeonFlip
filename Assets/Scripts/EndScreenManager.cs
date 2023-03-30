using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bestTime;
    [SerializeField] private TextMeshProUGUI bestScoreCoin;
    [SerializeField] private TextMeshProUGUI currentScoreCoin;
    [SerializeField] private TextMeshProUGUI currentTime;
    private StringVal m_PickUpCoinsPath;
    private StringVal m_TimePath;
    private StringVal m_BestPickUpCoinsPath;
    private StringVal m_BestTimePath;

    private void OnEnable()
    {
        m_PickUpCoinsPath = Resources.Load<StringVal>("ScriptableObjects/CoinsCollectedPath");
        m_TimePath = Resources.Load<StringVal>("ScriptableObjects/CurrentTime");
        m_BestPickUpCoinsPath = Resources.Load<StringVal>("ScriptableObjects/CoinsCollectedBest");
        m_BestTimePath = Resources.Load<StringVal>("ScriptableObjects/BestTime");
        
        bestTime.SetText(m_BestTimePath.val);
        bestScoreCoin.SetText(m_BestPickUpCoinsPath.val);
        currentScoreCoin.SetText(m_PickUpCoinsPath.val);
        currentTime.SetText(m_TimePath.val);
    }
}
