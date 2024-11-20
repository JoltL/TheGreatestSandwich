using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutPhaseScore : MonoBehaviour
{
    int m_score;

    public event Action OnScoreChanged;

    private void Start()
    {
        m_score = 0;
    }

    public void IncreaseScore(int value)
    {
        if (value < 0) return;
        m_score += value;
        OnScoreChanged?.Invoke();
    }

    public int Score
    {
        get { return m_score; }
    }
}
