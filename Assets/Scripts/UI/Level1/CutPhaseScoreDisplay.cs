using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutPhaseScoreDisplay : MonoBehaviour
{
    CutPhaseManager m_cutPhaseManager;
    CutPhaseScore m_cutPhaseScore;
    [SerializeField] TextMeshProUGUI m_scoreText;

 

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        m_cutPhaseManager = GameManager.Instance.GetCutPhaseManager();
        m_cutPhaseScore = m_cutPhaseManager.GetCutPhaseScore();
        m_cutPhaseScore.OnScoreChanged += DisplayScore;        
    } 

    void DisplayScore()
    {
        m_scoreText.text = m_cutPhaseScore.Score.ToString();
    }


}
