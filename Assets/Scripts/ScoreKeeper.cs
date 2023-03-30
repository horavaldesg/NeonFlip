using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bestTime;
    [SerializeField] private TextMeshProUGUI bestScoreCoin;

    private StringVal m_BestTimePath;
    private StringVal m_BestScorePath;
    
    private void Start()
    {
        m_BestTimePath = Resources.Load<StringVal>("ScriptableObjects/BestTime");
        m_BestScorePath = Resources.Load<StringVal>("ScriptableObjects/CoinsCollectedBest");
    }

    private void Update()
    {
        bestTime.SetText(PlayerPrefs.GetFloat(m_BestTimePath.val).ToString());
        bestScoreCoin.SetText(PlayerPrefs.GetInt(m_BestScorePath.val).ToString());
    }
}
