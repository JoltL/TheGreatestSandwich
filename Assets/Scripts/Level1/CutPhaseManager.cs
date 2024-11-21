using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutPhaseManager : MonoBehaviour
{
   
    IngredientSpawner m_ingredientSpawner;
    FingerInputsManager m_fingerInputs;
    CutPhaseScore m_cutPhaseScore;

    List<IngredientEntity> m_cuttedIngredients = new List<IngredientEntity>();

    int m_cheese;
    int m_salad;
    int m_ham;
    int m_tomato;


    bool m_timerIsActive = true;
    [SerializeField] float m_time;
    float m_timer;

    private void Start()
    {
        GameManager.Instance.SetCutPhaseManager(this);
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
            SortIngredients();
        }
    }

    #region ACCESORS

    public bool TimerIsActive
    {
        get { return m_timerIsActive; }
    }

    public float Timer
    {
        get { return m_timer; }
    }

    public void IncreaseScore(IngredientEntity ingredient)
    {
        m_cuttedIngredients.Add(ingredient);
        m_cutPhaseScore.IncreaseScore(1);
    }

    public void SortIngredients()
    {
        foreach(IngredientEntity ingredient in m_cuttedIngredients)
        {
            switch (ingredient.ID)
            {
                case 0:
                    m_salad++;
                break;
                case 1:
                    m_tomato++; 
                break;
                case 2:
                    m_cheese++;
                break;
                case 3:
                    m_ham++;
                break;
            }
        }
    }

    public CutPhaseScore GetCutPhaseScore() => m_cutPhaseScore;
    #endregion


}
