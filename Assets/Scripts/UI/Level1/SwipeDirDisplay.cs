using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwipeDirDisplay : MonoBehaviour
{

    IngredientSpawner m_ingredientSpawner;
    [SerializeField]TextMeshProUGUI m_swipeDirText;
    [SerializeField] SpriteRenderer m_arrowSprite;

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
                m_arrowSprite.transform.rotation = Quaternion.Euler(0f,0f,0f);
                m_arrowSprite.flipX = false;
                m_arrowSprite.flipY = false;
                m_swipeDirText.text = "→";
                break;
            case (-1, 0):
                m_arrowSprite.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                m_arrowSprite.flipX = true;
                m_arrowSprite.flipY = false;
                m_swipeDirText.text = "←";
                break;
            case (0, 1):
                m_arrowSprite.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                m_arrowSprite.flipY = true;
                m_arrowSprite.flipX = false;
                m_swipeDirText.text = "↑";
                break;
            case (0, -1):
                m_arrowSprite.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                m_arrowSprite.flipY = false;
                m_arrowSprite.flipX = false;
                m_swipeDirText.text = "↓";
                break;
        }
    }
}
