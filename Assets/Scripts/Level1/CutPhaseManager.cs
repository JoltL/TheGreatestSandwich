using System;
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

    [SerializeField] GameObject m_finishedMessage;

    bool m_timerIsActive = false;
    [SerializeField] float m_time;
    [SerializeField] float m_timeToFinish = 2;
    float m_timer;

    public event Action<int> OnStartTimerFeedback;

    private void Start()
    {
        GameManager.Instance.SetCutPhaseManager(this);
        m_ingredientSpawner = FindObjectOfType<IngredientSpawner>();
        m_fingerInputs = GetComponent<FingerInputsManager>();
        m_cutPhaseScore = GetComponent<CutPhaseScore>();

        m_fingerInputs.enabled = false;
        m_fingerInputs.OnSwipe += m_ingredientSpawner.TryToCut;
        m_ingredientSpawner.OnIngredientCut += IncreaseScore;
        m_timer = m_time;
        StartCoroutine(StartCoolDown());
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
            GameManager.Instance.SetIngredientDict(GetCutIngredients());
            //transform.SetParent(GameManager.Instance.transform);
            m_cuttedIngredients.Clear();
            StartCoroutine(FinishCoolDown());
           
        }
    }

    public void IncreaseScore(IngredientEntity ingredient)
    {
        m_cuttedIngredients.Add(ingredient);
        m_cutPhaseScore.IncreaseScore(1);
    }

    public void SortIngredients()
    {
        foreach (IngredientEntity ingredient in m_cuttedIngredients)
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

    IEnumerator StartCoolDown()
    {
        int i = 3;     
        while (i != -1)
        {
            OnStartTimerFeedback?.Invoke(i);
            yield return new WaitForSeconds(1);
            i--;     
        }
        m_fingerInputs.enabled = true;
        m_timerIsActive = true;
    }
    IEnumerator FinishCoolDown()
    {
        m_finishedMessage.SetActive(true);
        m_finishedMessage.GetComponent<OscillatorScale>().StartOscillator(5);
        m_finishedMessage.GetComponent<OscillatorRotation>().StartOscillator(150);
        yield return new WaitForSeconds(m_timeToFinish);
        GameManager.Instance.LoadScene(2);
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



    public CutPhaseScore GetCutPhaseScore() => m_cutPhaseScore;

    public Dictionary<string, int> GetCutIngredients()
    {
        Dictionary<string, int> dictIngredients = new Dictionary<string, int>();
        dictIngredients.Add("Cheese",m_cheese);
        dictIngredients.Add("Salad", m_salad);
        dictIngredients.Add("Tomato", m_tomato);
        dictIngredients.Add("Ham", m_ham);
        return dictIngredients;
    }


    #endregion


}
