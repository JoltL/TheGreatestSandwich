using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientEntity : MonoBehaviour
{
    Vector3 m_sign;
    int m_id;
    [SerializeField] IngredientData m_ingredientData;
    [SerializeField] IngredientVisual m_visual;


    void Start()
    {
       
    }
  


    public void LoadIngredient()
    {
        m_id = m_ingredientData.m_ID;
        Instantiate(m_ingredientData.m_sprite,m_visual.gameObject.transform);
        gameObject.name = m_ingredientData.name;
        m_sign = m_ingredientData.m_cutDirection;

    }

    public void SetIngredientData(IngredientData newData)
    {
        m_ingredientData = newData;
        LoadIngredient();
    }

    public void OnCut()
    {
        Debug.Log("Cut");
        GameManager.Instance.CameraOne.OscillateShake(5,false, true);
        // Disable the ingredient 
        gameObject.SetActive(false);
    }


    #region ACCESSORS

    public IngredientVisual IngredientVisual { get { return m_visual; } }
    public int ID { get { return m_id; } }
    public Vector3 Sign { get { return m_sign; } }

    #endregion

}
