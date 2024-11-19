using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways] // Permet de voir les changements en temps r�el dans l'�diteur
public class HorizontalLayer : MonoBehaviour
{

    IngredientSpawner m_ingredientSpawner;
    [Header("Layer Group Settings")]
    [SerializeField] float m_lerpSpeed = 4;
    [SerializeField] float m_spacing = 1.5f; // Espacement horizontal entre les objets
    [SerializeField] float m_layerOffsetY = 1.0f; // D�calage vertical entre les couches
    [SerializeField] int m_objectsPerLayer = 150; // Nombre d'objets par couche

    [Header("Dynamic Update")]
    [SerializeField] bool m_autoUpdate = true; // Mettre � jour automatiquement dans l'�diteur


    public void Start()
    {
        m_ingredientSpawner = GetComponent<IngredientSpawner>();
        m_ingredientSpawner.OnIngredientListModified += ReorganizeGroup;
    }

    void OnValidate()
    {
        // R�organiser automatiquement les objets si autoUpdate est activ�
        if (m_autoUpdate)
        {
            ReorganizeGroup();
        }
    }

    private void Update()
    {
        ReorganizeGroup();
    }

    public void ReorganizeGroup()
    {
        // Obtenir tous les enfants
        Transform[] children = GetComponentsInChildren<Transform>();

        // Commencer � index 1 pour ignorer l'objet parent
        int index = 0;
        for (int i = 1; i < children.Length; i++)
        {
            // Calculer la couche et la position horizontale
            int layer = index / m_objectsPerLayer; // Num�ro de couche
            int positionInLayer = index % m_objectsPerLayer; // Position dans la couche

            // Calculer la position finale
            Vector3 newPosition = new Vector3(
                positionInLayer * m_spacing,   // Position en X
                layer * m_layerOffsetY,       // Position en Y
                0                           // Position en Z
            );

            // Appliquer la position � l'enfant
            children[i].localPosition = Vector3.Lerp(children[i].localPosition,newPosition,Time.deltaTime*3);

            // Incr�menter l'index
            index++;
        }
    }
}
