using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonSound : MonoBehaviour
{
   public void ButtonOnClick()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(Sound);
    }

    void Sound()
    {
        if (SoundManager.Instance)
            SoundManager.Instance.PlaySFX("Bloop");

    }
}
