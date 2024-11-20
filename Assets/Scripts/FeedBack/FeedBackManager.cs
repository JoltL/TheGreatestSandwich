using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBackManager : MonoBehaviour
{
    public static FeedBackManager Instance;

    [SerializeField] List<ParticleSystem> m_particles = new List<ParticleSystem>();

    Coroutine m_FreezeFrameCoroutine;

    void Awake()
    {
        
        Debug.Log(Time.timeScale);
        if (Instance != null)
        {
            Debug.LogError("Plus d'une instance feedback manager dans la scene");
            Destroy(gameObject);
            return;
        }
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void InstantiateParticle(String particleName,Vector3 position, Quaternion rotation)
    {
        ParticleSystem particle = m_particles.Find(p => p.name == particleName);
        Instantiate(particle.gameObject,position,rotation);
    }


    public void FreezeFrame(float duration, float timeScale)
    {
        m_FreezeFrameCoroutine = StartCoroutine(FreezeFrameCoroutine(duration,timeScale));
    }

    IEnumerator FreezeFrameCoroutine(float duration, float timeScale)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1;
    }
}
