using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientVisual : MonoBehaviour
{
    OscillatorScale m_oscillator;

    private void Start()
    {
        m_oscillator = GetComponent<OscillatorScale>();
    }

    public void Strech(float force)
    {
        m_oscillator.StartOscillator(force);
    }
}
