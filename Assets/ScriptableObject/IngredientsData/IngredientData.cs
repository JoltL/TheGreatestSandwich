using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientData", menuName = "The Greatest Sandwich/Ingredient Data")]
public class IngredientData : ScriptableObject
{
    public Vector2 m_cutDirection;
    public string m_name;
    public GameObject m_sprite;

}
