using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] CutPhaseManager m_cutPhaseManager;

    void Awake()
    {
        //Debug.Log(PLAYER_LAYER);
      
        if (Instance != null)
        {
            Debug.LogError("Plus d'une instance game manager dans la scene");
            Destroy(gameObject);
            return;
        }
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }    
        m_cutPhaseManager = GetComponent<CutPhaseManager>();
    }

    public CutPhaseManager GetCutPhaseManager() => m_cutPhaseManager;



}
