using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] CutPhaseManager m_cutPhaseManager;
    [SerializeField] CutPhaseCameraManager m_cameraOne;


    public CutPhaseCameraManager CameraOne
    {
        get {return m_cameraOne;}
    }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Plus d'une instance game manager dans la scene");
            Destroy(gameObject);
            return;
        }
        if (Instance == null)
        {
            Instance = this;
          
        }    
        m_cutPhaseManager = GetComponent<CutPhaseManager>();
    }

    private void Start()
    {
        m_cameraOne = Camera.main.gameObject.GetComponent<CutPhaseCameraManager>();
    }


    #region ACCESSORS

    public CutPhaseManager GetCutPhaseManager() => m_cutPhaseManager;

    #endregion
}
