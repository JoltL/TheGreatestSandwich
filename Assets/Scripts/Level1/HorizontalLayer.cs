using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways] // Permet de voir les changements en temps réel dans l'éditeur
public class HorizontalLayer : MonoBehaviour
{

    [Header("Layer Group Settings")]
    [SerializeField] float m_lerpSpeed = 4;
    [SerializeField] float m_spacing = 1.5f; // Espacement horizontal entre les objets
    [SerializeField] float m_layerOffsetY = 1.0f; // Décalage vertical entre les couches
    [SerializeField] int m_objectsPerLayer = 150; // Nombre d'objets par couche

    [Header("Dynamic Update")]
    [SerializeField] bool m_autoUpdate = true; // Mettre à jour automatiquement dans l'éditeur

    [SerializeField] List<Transform> m_children;
  

    void OnValidate()
    {
        // Réorganiser automatiquement les objets si autoUpdate est activé
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
        IngredientEntity[] children = GetComponentsInChildren<IngredientEntity>();


        // Commencer à index 1 pour ignorer l'objet parent
        int index = 0;
        for (int i = 0; i < children.Length; i++)
        {
            // Calculer la couche et la position horizontale
            int layer = index / m_objectsPerLayer; // Numéro de couche
            int positionInLayer = index % m_objectsPerLayer; // Position dans la couche

            // Calculer la position finale
            Vector3 newPosition = new Vector3(
                positionInLayer * m_spacing,   // Position en X
                layer * m_layerOffsetY,       // Position en Y
                0                           // Position en Z
            );

            // Appliquer la position à l'enfant
            children[i].transform.localPosition = Vector3.Lerp(children[i].transform.localPosition,newPosition,Time.deltaTime*m_lerpSpeed);

            // Incrémenter l'index
            index++;
        }
    }
}
