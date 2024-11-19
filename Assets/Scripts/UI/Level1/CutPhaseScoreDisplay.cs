using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutPhaseScoreDisplay : MonoBehaviour
{
    CutPhaseManager m_cutPhaseManager;
    CutPhaseScore m_cutPhaseScore;
    [SerializeField] TextMeshProUGUI m_scoreText;

    private void Start()
    {
        m_cutPhaseManager = FindObjectOfType<CutPhaseManager>();
        //TODO FINIR
    }

    void DisplayScore()
    {

    }


}
