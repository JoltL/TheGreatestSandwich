using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutPhaseTimerDisplay : MonoBehaviour
{
    CutPhaseManager m_cutPhaseManager;
    [SerializeField] TextMeshProUGUI m_timerText;


    private void Start()
    {
        m_cutPhaseManager = GameManager.Instance.GetCutPhaseManager();
        if (!m_timerText)
        {
            Debug.LogError("timer text value is null !!",gameObject);
        }
    }

    private void Update()
    {
       UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        if (m_cutPhaseManager.TimerIsActive)
        {
            int intTimer = (int)m_cutPhaseManager.Timer;
            m_timerText.text = $"{intTimer.ToString()}s";
        }
    }
}
