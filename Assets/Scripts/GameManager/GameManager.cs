using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    [SerializeField] Animator m_sceneTransition;
    bool m_inTransition = false;

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

    public void LoadScene(int scene)
    {
        if (m_inTransition) return;
        StartCoroutine(LoadSceneCoroutine(scene));
    }


    IEnumerator LoadSceneCoroutine(int scene)
    {
        m_sceneTransition.SetTrigger("End");
        m_inTransition = true;
        yield return new WaitForSeconds(0.5f);
        m_sceneTransition.SetTrigger("Start");
        m_inTransition = false;
        SceneManager.LoadScene(scene);
        if (scene == 0)
        {       
            foreach (Component component in GetComponents<MonoBehaviour>())
            {
                Destroy(component);
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            }
            Destroy(SoundManager.Instance.gameObject);
        };
    }



    #region ACCESSORS

    public CutPhaseManager GetCutPhaseManager() => m_cutPhaseManager;

    public void SetCutPhaseManager(CutPhaseManager cutPhaseManager)
    {
        m_cutPhaseManager = cutPhaseManager;
    }

    public Dictionary<string,int> GetCutIngredients()
    {
        return m_cutPhaseManager.GetCutIngredients();
    }

    public void SetCameraOne(CutPhaseCameraManager cutPhaseCameraManager)
    {
        m_cameraOne = cutPhaseCameraManager;
    }

    #endregion
}
