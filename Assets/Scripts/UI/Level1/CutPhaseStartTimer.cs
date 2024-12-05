using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutPhaseStartTimer : MonoBehaviour
{
    [SerializeField] CutPhaseManager m_cutPhaseManager;
    [SerializeField] TextMeshProUGUI m_timerText;
    void Start()
    {
        //m_cutPhaseManager = GameManager.Instance.GetCutPhaseManager();
        m_cutPhaseManager.OnStartTimerFeedback += TimerFeedback;
    }

    
    void TimerFeedback(int time)
    {
        GetComponent<OscillatorScale>().StartOscillator(10);
        GetComponent<OscillatorRotation>().StartOscillator(150);
        SoundManager.Instance.PlaySFX("Woo");
        if (time == 0)
        {
            m_timerText.text = "GO !";
            SoundManager.Instance.PlaySFX("Bell");
            Destroy(gameObject, 1f);
        }
        else
        {
            m_timerText.text = time.ToString();
        }
    }
}
