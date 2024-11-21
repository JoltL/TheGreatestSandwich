using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MParticle : MonoBehaviour
{
    [SerializeField] bool m_destroyAtEnd;
    
    void Start()
    {
        var main = GetComponent<ParticleSystem>().main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }
    private void OnParticleSystemStopped()
    {
        if (!m_destroyAtEnd) return;
        Destroy(gameObject);
    }
}
