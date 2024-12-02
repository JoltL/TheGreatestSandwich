using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientVisual : MonoBehaviour
{
    IngredientEntity m_ingredient;
    OscillatorScale m_oscillator;
    [SerializeField] CutFeedBack m_cutFeedBack;

    private void Start()
    {
        m_ingredient = GetComponentInParent<IngredientEntity>();
        m_oscillator = GetComponent<OscillatorScale>();
    }

    public void Strech(float force)
    {
        m_oscillator.StartOscillator(force);
    }


    public void CutFeedBack()
    {
        IngredientData data = m_ingredient.GetIngredientData();
        Quaternion rotation = Quaternion.identity;
        if (data.m_cutDirection.x !=0)
        {
            rotation = Quaternion.Euler(0,0,0);
            FeedBackManager.Instance.FreezeFrame(0.1f, 0.15f, 0.05f);
        }
        if (data.m_cutDirection.y != 0)
        {
            rotation = Quaternion.Euler(0, 0, 90);
            FeedBackManager.Instance.FreezeFrame(0.2f, 0.15f, 0.05f);
        }
        FeedBackManager.Instance.InstantiateParticle("Stain", data.m_particleColor, transform.position, transform.rotation);
        FeedBackManager.Instance.InstantiateParticle("Slash", transform.position, rotation);
      
        GameManager.Instance.CameraOne.OscillateShake(5, false, true);
        SoundManager.Instance.PlaySFX("Slash");
        SoundManager.Instance.PlaySFX("Splat");
        InstantiateCutFeedBack();
    }

    public void InstantiateCutFeedBack()
    {
        CutFeedBack inst = Instantiate(m_cutFeedBack,transform.position,transform.rotation);
        inst.SetCutVisual(m_ingredient.GetIngredientData().m_cutSprite);

    }

}
