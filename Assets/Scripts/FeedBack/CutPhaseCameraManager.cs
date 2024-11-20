using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutPhaseCameraManager : MonoBehaviour
{
    OscillatorPosition m_oscillator;

    public void Start()
    {
        m_oscillator = GetComponent<OscillatorPosition>();
    }

    public void OscillateShake(float force,bool x , bool y)
    {
        m_oscillator.ResetOscillatePos2D();
        m_oscillator.OscillateX = x;
        m_oscillator.OscillateY = y;
        m_oscillator.StartOscillator(force); 
    }

}
