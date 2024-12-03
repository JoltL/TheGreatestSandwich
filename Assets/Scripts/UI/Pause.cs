using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public void Restart()
    {
        Time.timeScale = 1f;
        GameManager.Instance.LoadScene(1);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        GameManager.Instance.LoadScene(0);
    }
}
