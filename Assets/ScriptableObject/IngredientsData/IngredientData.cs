using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientData", menuName = "The Greatest Sandwich/Ingredient Data")]
public class IngredientData : ScriptableObject
{
    public int m_ID;
    public Vector2 m_cutDirection;
    public string m_name;
    public GameObject m_sprite;

}
