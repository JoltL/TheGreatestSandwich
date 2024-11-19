using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutPhaseManager : MonoBehaviour
{
    IngredientSpawner m_ingredientSpawner;
    FingerInputsManager m_fingerInputs;
    CutPhaseScore m_cutPhaseScore;

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
        m_timer -= Time.deltaTime;
        m_timer = Mathf.Clamp(m_timer, 0, 100);
        if(m_timer <= 0)
        {
            m_fingerInputs.enabled = false;
        }
    }

    public void IncreaseScore()
    {
        m_cutPhaseScore.IncreaseScore(1);
    }


   
  
}
