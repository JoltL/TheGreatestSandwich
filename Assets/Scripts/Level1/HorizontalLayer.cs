using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways] // Permet de voir les changements en temps réel dans l'éditeur
public class HorizontalLayer : MonoBehaviour
{

    IngredientSpawner m_ingredientSpawner;
    [Header("Layer Group Settings")]
    public float spacing = 1.5f; // Espacement horizontal entre les objets
    public float layerOffsetY = 1.0f; // Décalage vertical entre les couches
    public int objectsPerLayer = 150; // Nombre d'objets par couche

    [Header("Dynamic Update")]
    public bool autoUpdate = true; // Mettre à jour automatiquement dans l'éditeur


    public void Start()
    {
        m_ingredientSpawner = GetComponent<IngredientSpawner>();
        m_ingredientSpawner.OnIngredientListModified += ReorganizeGroup;
    }

    void OnValidate()
    {
        // Réorganiser automatiquement les objets si autoUpdate est activé
        if (autoUpdate)
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

        // Commencer à index 1 pour ignorer l'objet parent
        int index = 0;
        for (int i = 1; i < children.Length; i++)
        {
            // Calculer la couche et la position horizontale
            int layer = index / objectsPerLayer; // Numéro de couche
            int positionInLayer = index % objectsPerLayer; // Position dans la couche

            // Calculer la position finale
            Vector3 newPosition = new Vector3(
                positionInLayer * spacing,   // Position en X
                layer * layerOffsetY,       // Position en Y
                0                           // Position en Z
            );

            // Appliquer la position à l'enfant
            children[i].localPosition = Vector3.Lerp(children[i].localPosition,newPosition,Time.deltaTime*3);

            // Incrémenter l'index
            index++;
        }
    }
}
