using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;



enum Sounds
{
    WALK,
    ATK
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // Singleton

    [Header("Audio Sources")]
    public AudioSource m_musicSource; // Source pour la musique
    public AudioSource m_sfxSource; // Source pour les effets sonores

    [Header("Audio Clips")]
    public List<AudioClip> m_musicClips; // Liste de musiques
    [Header("Pitch range")]
    public float m_minPitch;  // pith
    public float m_maxPitch;  // pith
    public List<AudioClip> m_sfxClips = new List<AudioClip>(); // Liste d'effets sonores


    [Header("Volume Settings")]
    [Range(0, 1)] public float m_musicVolume = 0.5f;
    [Range(0, 1)] public float m_sfxVolume = 0.5f;


    public bool m_musicEnabled = true;
    public bool m_sfxEnabled = true;  


    

    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("ALREAYD A SOUND MANAGER INSTANCE");
            Destroy(gameObject);
        }

        // Initialiser les volumes
        m_musicSource.volume = m_musicVolume;
        m_sfxSource.volume = m_sfxVolume;
    }



    public void PlayMusic(AudioClip musicClip)
    {
        if (musicClip == null) return;

        m_musicSource.clip = musicClip;
        m_musicSource.loop = true;
        m_musicSource.Play();

    }

    public void PlayMusic(string musicName)
    {
        AudioClip clip = m_musicClips.Find(sfx => sfx.name == musicName);
       
        if (clip != null)
        {
            if (m_sfxSource.clip == clip && m_sfxSource.isPlaying) return;
            m_musicSource.clip = clip;
            m_musicSource.loop = true;
            m_musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Effet sonore '{musicName}' non trouv� !");
        }

    }


    public void StopMusic()
    {
        m_musicSource.Stop();
    }

    public void PlaySFX(string sfxName)
    {
        AudioClip clip = m_sfxClips.Find(sfx => sfx.name == sfxName);
        float pitch = Random.Range(m_minPitch, m_maxPitch);
        if (clip != null)
        {
            if (m_sfxSource.clip == clip && m_sfxSource.isPlaying) return;
            m_sfxSource.pitch = pitch;
            m_sfxSource.PlayOneShot(clip, m_sfxVolume);
        }
        else
        {
            Debug.LogWarning($"Effet sonore '{sfxName}' non trouv� !");
        }
    }

    public void PlaySFXArray(string[] sfxName)
    {
        var randSfx = UnityEngine.Random.Range(0, sfxName.Length);
        AudioClip clip = m_sfxClips.Find(sfx => sfx.name == sfxName[randSfx]);
        float pitch = Random.Range(m_minPitch, m_maxPitch);
        if (clip != null)
        {
            if (m_sfxSource.clip == clip && m_sfxSource.isPlaying) return;
            m_sfxSource.pitch = pitch;
            m_sfxSource.PlayOneShot(clip, m_sfxVolume);
        }
        else
        {
            Debug.LogWarning($"Effet sonore '{sfxName}' non trouv� !");
        }
    }


    public void CutMusic()
    {
        m_musicEnabled = !m_musicEnabled;
        m_musicSource.volume = m_musicEnabled ? m_musicVolume : 0;
    }

    public void CutSfx()
    {
        m_sfxEnabled = !m_sfxEnabled;
        m_sfxSource.volume = m_sfxEnabled ? m_sfxVolume : 0;
    }

    public void SetMusicVolume(float volume)
    {
        m_musicVolume = Mathf.Clamp01(volume);
        m_musicSource.volume = m_musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        m_sfxVolume = Mathf.Clamp01(volume);
        m_sfxSource.volume = m_sfxVolume;
    }
}
