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

    public void InstantiateParticle(String particleName,Color color, Vector3 position, Quaternion rotation)
    {
      
        ParticleSystem particle = m_particles.Find(p => p.name == particleName);
        GameObject particleInstance = Instantiate(particle.gameObject, position, rotation);
        var main = particleInstance.GetComponent<ParticleSystem>().main;
        main.startColor = color;
        main.stopAction = ParticleSystemStopAction.Callback;
    }


    public void FreezeFrame(float delay,float duration, float timeScale)
    {
        m_FreezeFrameCoroutine = StartCoroutine(FreezeFrameCoroutine(delay, duration,timeScale));
    }



    IEnumerator FreezeFrameCoroutine(float delay,float duration, float timeScale)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }
}
