using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutPhaseTimerDisplay : MonoBehaviour
{
    Color RED = new Color(336,53,100,1);
    CutPhaseManager m_cutPhaseManager;
    Color m_baseColor;
    [SerializeField] TextMeshProUGUI m_timerText;
    [SerializeField] GameObject m_missedPanel;


    private void Start()
    {
        m_baseColor = m_timerText.color;
        m_cutPhaseManager = FindObjectOfType<CutPhaseManager>();
        m_cutPhaseManager.OnCutMissed += Malus;
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

    void Malus()
    {
        FeedBack(Color.red);
        m_missedPanel.SetActive(true);
    }

    void FeedBack(Color color)
    {
        GetComponent<Oscillator>().StartOscillator(800);
        StartCoroutine(FeedBackTimer(color));
    }

    IEnumerator FeedBackTimer(Color color)
    {
        m_timerText.color = color;
        yield return new WaitForSeconds(0.5f);
        m_timerText.color = m_baseColor;
        m_missedPanel.SetActive(false);
    }
}
