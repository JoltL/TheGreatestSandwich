using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutPhaseManager : MonoBehaviour
{
   
    IngredientSpawner m_ingredientSpawner;
    FingerInputsManager m_fingerInputs;
    CutPhaseScore m_cutPhaseScore;


    bool m_timerIsActive = true;
    [SerializeField] float m_time;
    float m_timer;

    private void Start()
    {
        m_ingredientSpawner = FindObjectOfType<IngredientSpawner>();
        m_fingerInputs = GetComponent<FingerInputsManager>();
        m_cutPhaseScore = GetComponent<CutPhaseScore>();    


        m_fingerInputs.OnSwipe += m_ingredientSpawner.TryToCut;
        m_ingredientSpawner.OnIngredientCut += IncreaseScore;
        m_timer = m_time;
    }


    public void Update()
    {
        TimerManager();
    }

    void TimerManager()
    {
        if (!m_timerIsActive) return;
        if (m_timer > 0)
        {
            m_timerIsActive = true;
            m_timer -= Time.deltaTime;
            m_timer = Mathf.Clamp(m_timer, 0, 100);
        }
        if (m_timer <= 0)
        {
            m_timerIsActive = false;
            m_fingerInputs.enabled = false;
        }
    }


    public bool TimerIsActive
    {
        get { return m_timerIsActive; }
    }

    public float Timer
    {
        get { return m_timer; }
    }

    public void IncreaseScore()
    {
        m_cutPhaseScore.IncreaseScore(1);
    }

    public CutPhaseScore GetCutPhaseScore() => m_cutPhaseScore;

   
  
}
