using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientEntity : MonoBehaviour
{
    Vector3 m_sign;
    int m_id;
    [SerializeField] IngredientData m_ingredientData;
    [SerializeField] GameObject m_visual;


    void Start()
    {
       
    }
  


    public void LoadIngredient()
    {
        m_id = m_ingredientData.m_ID;
        Instantiate(m_ingredientData.m_sprite,m_visual.transform);
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
    }

    #region ACCESSORS
    public int ID
    {
        get { return m_id; }
    }
    public Vector3 Sign
    {
        get { return m_sign; }
    }
    #endregion

}
