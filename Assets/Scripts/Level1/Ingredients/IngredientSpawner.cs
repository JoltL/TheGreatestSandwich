using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if (Input.GetKeyDown(KeyCode.Space)) { 
        
            CreateIngredient();
        
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {

            CutIngredient();

        }
    }



    public void CreateIngredient()
    {
        IngredientEntity newIngredient = Instantiate(m_ingredientPref, transform).gameObject.GetComponent<IngredientEntity>();
        newIngredient.SetIngredientData(m_allIngredient[UnityEngine.Random.Range(0,m_allIngredient.Count)]);   
        OnIngredientListModified?.Invoke();
        m_ingredients = GetComponentsInChildren<IngredientEntity>().ToList();
        SetCurrentIngredient(m_ingredients[0]);
    }

    public void SetCurrentIngredient(IngredientEntity newCurrentIngredient)
    {
        m_currentIngredient = newCurrentIngredient;
    }

    public void CutIngredient()
    {
        m_ingredients.RemoveAt(0);
        Destroy(m_currentIngredient.gameObject);
        OnIngredientListModified?.Invoke();
        SetCurrentIngredient(m_ingredients[0]);
        
    }
}
