using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Oscillator : MonoBehaviour
{

    [SerializeField] protected GameObject m_target;
    

    [SerializeField] protected float m_spring;
    [SerializeField] protected float m_damp;
    protected float m_displacement;
    protected float m_velocity;

    public event Action<float> OnStartOscillator;

    private void Awake()
    {
        OnStartOscillator += SetVelocity;
    }

    public virtual void Update()
    {
        float force = -m_spring * m_displacement - m_damp * m_velocity;
        m_velocity += force * Time.deltaTime;
        m_displacement += m_velocity * Time.deltaTime;

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            SetVelocity(10);
        }
    }


    void SetVelocity(float value)
    {
        m_velocity = value;
    }


    public void StartOscillator(float velocity)
    {
        OnStartOscillator?.Invoke(velocity);
    }

}
