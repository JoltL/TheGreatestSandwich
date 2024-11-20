using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class OscillatorScale : Oscillator
{
    Vector3 m_baseTargetScale;

    void Start()
    {
        m_baseTargetScale = m_target.transform.localScale;
    }
    public override void Update()
    {
        base.Update();
        Vector3 newScale = m_baseTargetScale + new Vector3(m_displacement, -m_displacement) * 4;
        newScale.x = Mathf.Clamp(newScale.x, 0, 1000);
        newScale.y = Mathf.Clamp(newScale.y, 0, 1000);
        newScale.z = Mathf.Clamp(newScale.z, 0, 1000);
        m_target.transform.localScale = newScale;
    }
}
