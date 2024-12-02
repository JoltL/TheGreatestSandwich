using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutPhaseCameraManager : MonoBehaviour
{
    OscillatorPosition m_oscillatorPosition;
    OscillatorRotation m_oscillatorRotation;

    public void Start()
    {
        GameManager.Instance.SetCameraOne(this);
        m_oscillatorPosition = GetComponent<OscillatorPosition>();
        m_oscillatorRotation = GetComponent<OscillatorRotation>();
        
    }

    public void OscillateShake(float force,bool x , bool y)
    {
        m_oscillatorPosition.ResetOscillatePos2D();
        m_oscillatorPosition.OscillateX = x;
        m_oscillatorPosition.OscillateY = y;
        m_oscillatorPosition.StartOscillator(force); 
    }
    public void OscillateShakeRotate(float force)
    {
        m_oscillatorRotation.StartOscillator(force);
    }

}
