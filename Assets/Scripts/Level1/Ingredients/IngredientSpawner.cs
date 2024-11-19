using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] GameObject m_ingredientPref;
    IngredientEntity m_currentIngredient;
    [SerializeField] List<IngredientData> m_allIngredient;


    List<IngredientEntity> m_ingredients;

    public event Action OnIngredientListModified;

    void Start()
    {
        
    }

  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { 
        
            CreateIngredient();
        
        }
    }

    public void AddIngredientToList(IngredientEntity ingredient)
    {
        m_ingredients.Add(ingredient);
    }

    public void CreateIngredient()
    {
        IngredientEntity newIngredient = Instantiate(m_ingredientPref, transform).gameObject.GetComponent<IngredientEntity>();
        newIngredient.SetIngredientData(m_allIngredient[UnityEngine.Random.Range(0,m_allIngredient.Count)]);   
        OnIngredientListModified?.Invoke();
        m_ingredients.Add(newIngredient.gameObject.GetComponent<IngredientEntity>());
        //AddIngredientToList(newIngredient);
    }

    public void SetCurrentIngredient(IngredientEntity newCurrentIngredient)
    {
        m_currentIngredient = newCurrentIngredient;
    }

    public void CutIngredient()
    {
        m_ingredients.RemoveAt(0);
        SetCurrentIngredient(m_ingredients[0]);
        OnIngredientListModified?.Invoke();
    }
}
