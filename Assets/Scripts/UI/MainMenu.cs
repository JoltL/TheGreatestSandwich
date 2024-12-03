using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator _catAnimator;
    [SerializeField] private Animator _playAnimator;

    [SerializeField] private GameObject _pause;
    public void Slash()
    {
        _catAnimator.SetTrigger("VSliced");
        _playAnimator.SetTrigger("Cut");
        StartGame();

    }

    public void StartGame()
    {
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
