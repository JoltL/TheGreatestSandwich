using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientEntity : MonoBehaviour
{
    Vector3 m_sign;
    int m_id;
    [SerializeField] IngredientData m_ingredientData;


    void Start()
    {
       
    }
  


    public void LoadIngredient()
    {
        Instantiate(m_ingredientData.m_sprite,transform);
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
    
}
