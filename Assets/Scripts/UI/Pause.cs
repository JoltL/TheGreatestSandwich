using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] Image m_cutMusicButton;
    [SerializeField] Image m_cutSfxButton;

    public void Start()
    {
        UpdateMusicIcon();
        UpdateSfxIcon();
    }

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


    public void CutMusic()
    {
        SoundManager.Instance.CutMusic();
        UpdateMusicIcon();
    }

    public void CutSfx()
    {
        SoundManager.Instance.CutSfx();
        UpdateSfxIcon();
    }

    void UpdateMusicIcon()
    {
        Sprite enabledIcon = Resources.LoadAll<Sprite>("Sprites/Sound_Icons")[0];
        Sprite disabledIcon = Resources.LoadAll<Sprite>("Sprites/Sound_Icons")[1];
        m_cutMusicButton.sprite = SoundManager.Instance.m_musicEnabled ? enabledIcon : disabledIcon;
    }

    public void UpdateSfxIcon()
    {
        Sprite enabledIcon = Resources.LoadAll<Sprite>("Sprites/Sound_Icons")[2];
        Sprite disabledIcon = Resources.LoadAll<Sprite>("Sprites/Sound_Icons")[3];
        m_cutSfxButton.sprite = SoundManager.Instance.m_sfxEnabled ?  enabledIcon : disabledIcon;

    }
}
