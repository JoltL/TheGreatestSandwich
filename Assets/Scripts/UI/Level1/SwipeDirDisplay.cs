using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwipeDirDisplay : MonoBehaviour
{

    IngredientSpawner m_ingredientSpawner;
    [SerializeField]TextMeshProUGUI m_swipeDirText;

    private void Start()
    {
        m_ingredientSpawner = FindObjectOfType<IngredientSpawner>();
        if (!m_ingredientSpawner) { Debug.LogWarning("Ingredient spawner is null"); }
        if (!m_swipeDirText) { Debug.LogWarning("Swipe Dir Text is null"); }
        m_ingredientSpawner.OnCurrentIngredientChanged += DisplaySwipeDir;
    }
    private void DisplaySwipeDir()
    {
        IngredientEntity currentIngredient = m_ingredientSpawner.GetCurrentIngredient();
        Vector2 sign = currentIngredient.Sign;
        switch ((sign.x,sign.y))
        {
            case (1,0):
                m_swipeDirText.text = "→";
                break;
            case (-1, 0):
                m_swipeDirText.text = "←";
                break;
            case (0, 1):
                m_swipeDirText.text = "↑";
                break;
            case (0, -1):
                m_swipeDirText.text = "↓";
                break;
        }
    }
}
