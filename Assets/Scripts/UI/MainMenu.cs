using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator _catAnimator;
    [SerializeField] private Animator _playAnimator;

    [SerializeField] private GameObject _pause;
    [SerializeField] private TMP_Text _text;
    private int _bestScore;

    private void Start()
    {
        if(_text != null)
        {
       _bestScore = PlayerPrefs.GetInt("Best Score", _bestScore);

        _text.text = _bestScore.ToString();

        }
    }
    public void Slash()
    {
        _catAnimator.SetTrigger("VSliced");
        _playAnimator.SetTrigger("Cut");
        StartGame();

    }

    public void StartGame()
    {
        FeedBackManager.Instance.FreezeFrame(0.1f,0.1f,0.05f);
        SoundManager.Instance.PlaySFX("Slash");
        FeedBackManager.Instance.InstantiateParticle("Slash",new Vector3(-1.29f,-1.55f,0.03f),Quaternion.Euler(0,0,90));
        FeedBackManager.Instance.InstantiateParticle("Stain",new Color32(99,255,186,255), new Vector3(-1.29f, -1.55f, 0.03f), Quaternion.Euler(0, 0, 90));
        GameManager.Instance.LoadScene(1);
    }

    public void PauseGame()
    {

        if (Time.timeScale > 0f)
        {
            Time.timeScale = 0f;
            if (_pause != null)
            {
                _pause.SetActive(true);
            }
        }
        else
        {
            Time.timeScale = 1f;

            if (_pause != null)
            {
                _pause.SetActive(false);
            }
        }
    }
    public void OnApplicationQuit()
    {
        Application.Quit();
        print("Quit");
    }
}
