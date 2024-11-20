using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutPhaseScoreDisplay : MonoBehaviour
{
    CutPhaseManager m_cutPhaseManager;
    CutPhaseScore m_cutPhaseScore;
    [SerializeField] TextMeshProUGUI m_scoreText;

    private void Awake()
    {
        m_cutPhaseManager = GameManager.Instance.GetCutPhaseManager();   
       
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        m_cutPhaseScore = m_cutPhaseManager.GetCutPhaseScore();
        m_cutPhaseScore.OnScoreChanged += DisplayScore;        
        //TODO FINIR
    }

    private void Update()
    {
        if (Input.GetButton("Jump"))
        {
            Debug.Log(m_cutPhaseManager.GetCutPhaseScore().ToString());
        }
    }

    void DisplayScore()
    {
        m_scoreText.text = m_cutPhaseScore.Score.ToString();
    }


}
