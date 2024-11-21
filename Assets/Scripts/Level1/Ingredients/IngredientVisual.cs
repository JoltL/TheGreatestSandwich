using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientVisual : MonoBehaviour
{
    IngredientEntity m_ingredient;
    OscillatorScale m_oscillator;

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
        }
        if (data.m_cutDirection.y != 0)
        {
            rotation = Quaternion.Euler(0, 0, 90);
        }
        FeedBackManager.Instance.InstantiateParticle("Explosion", data.m_particleColor, transform.position, transform.rotation);
        FeedBackManager.Instance.InstantiateParticle("Slash", transform.position, rotation);
        FeedBackManager.Instance.FreezeFrame(0.1f,0.11f, 0.05f);
        GameManager.Instance.CameraOne.OscillateShake(5, false, true);
        SoundManager.Instance.PlaySFX("Slash");
    }

}
