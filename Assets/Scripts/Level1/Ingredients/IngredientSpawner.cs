using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{

    [SerializeField] int m_baseIngredientNum = 10;
    [SerializeField] GameObject m_ingredientPref;
    IngredientEntity m_currentIngredient;
    [SerializeField] List<IngredientData> m_allIngredient;


    List<IngredientEntity> m_ingredients;

    public event Action OnIngredientListModified;

    void Start()
    {
        Init();
    }

  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { 
        
            CreateIngredient(false);
        
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {

            CutIngredient();

        }
    }



    public void Init()
    {
        for (int i = 0;i< m_baseIngredientNum;i++)
        {
            CreateIngredient(true);
        }
    }

    public void CreateIngredient(bool setCurrentIngredient)
    {
        IngredientEntity newIngredient = Instantiate(m_ingredientPref, transform.position+new Vector3(100,0,0),transform.rotation).gameObject.GetComponent<IngredientEntity>();
        newIngredient.transform.parent = transform;
        newIngredient.SetIngredientData(m_allIngredient[UnityEngine.Random.Range(0,m_allIngredient.Count)]);   
        OnIngredientListModified?.Invoke();
        m_ingredients = GetComponentsInChildren<IngredientEntity>().ToList();
        if (setCurrentIngredient)
        {
            SetCurrentIngredient(m_ingredients[0]);
        }
    }

    public void SetCurrentIngredient(IngredientEntity newCurrentIngredient)
    {
        m_currentIngredient = newCurrentIngredient;
    }

    public void CutIngredient()
    {
        //Destroy(m_currentIngredient.gameObject);
        m_currentIngredient.gameObject.SetActive(false);
        Destroy(m_currentIngredient.gameObject,3);
        m_ingredients.RemoveAt(0);
        OnIngredientListModified?.Invoke();
        SetCurrentIngredient(m_ingredients[0]);
        if (m_ingredients.Count < m_baseIngredientNum)
        {
            CreateIngredient(false);
        }
      
        
    }
}
