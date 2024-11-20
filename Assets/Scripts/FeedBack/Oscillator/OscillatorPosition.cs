using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class OscillatorPosition : Oscillator
{
    Vector3 m_baseTargetScale;
    [SerializeField] float m_mulitiplier = 4f;
    [SerializeField] bool m_OscillateX = true;
    [SerializeField] bool m_OscillateY = false;
    [SerializeField] bool m_OscillateZ = false;

    void Start()
    {
        m_baseTargetScale = m_target.transform.localScale;
    }
    public override void Update()
    {
        base.Update();
        Vector3 pos = new Vector3(m_target.transform.position.x, m_target.transform.position.y, m_target.transform.position.z);
        if (m_OscillateX) pos.x = m_displacement * m_mulitiplier;
        if (m_OscillateY) pos.y = m_displacement * m_mulitiplier;
        if (m_OscillateZ) pos.z = m_displacement * m_mulitiplier;
        m_target.transform.position = pos;
    }



    public void ResetOscillatePos2D()
    {
        m_OscillateX = false;
        m_OscillateY = false;
        m_OscillateZ = false;
    }

    public bool OscillateX
    {
        set { m_OscillateX = value;}
    }
    public bool OscillateY
    {
        set { m_OscillateY = value; }
    }
    public bool OscillateZ
    {
        set { m_OscillateZ = value; }
    }
}
