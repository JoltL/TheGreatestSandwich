using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutPhaseManager : MonoBehaviour
{
    IngredientSpawner m_ingredientSpawner;
    FingerInputs m_fingerInputs;

    private void Start()
    {
        m_ingredientSpawner = FindObjectOfType<IngredientSpawner>();
        m_fingerInputs = GetComponent<FingerInputs>();

        m_fingerInputs.OnSwipe += m_ingredientSpawner.TryToCut;
    }

  
}
