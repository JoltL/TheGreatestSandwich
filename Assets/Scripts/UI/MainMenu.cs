using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.LoadScene(1);
        Debug.Log("zizi");
    }
}