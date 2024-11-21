using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class OscillatorRotation : Oscillator
{
    Vector3 m_baseTargetScale;

    void Start()
    {
        m_baseTargetScale = m_target.transform.localScale;
    }
    public override void Update()
    {
        base.Update();
       
    }
}
