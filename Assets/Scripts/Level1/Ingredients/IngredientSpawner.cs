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


    [SerializeField] List<IngredientData> m_allIngredients;
    List<float> m_weights = new List<float>();
    [SerializeField] float m_weightReductionFactor = 0.7f;
    int m_lastSpawnIndex;


    List<IngredientEntity> m_ingredients;

    public event Action OnCurrentIngredientChanged;
    public event Action<IngredientEntity> OnIngredientCut;
    public event Action OnIngredientMissingCut;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        for (int i = 0; i < m_allIngredients.Count; i++)
        {
            m_weights.Add(1f);
        }
        for (int i = 0;i< m_baseIngredientNum;i++)
        {
            CreateIngredient(true);
        } 
    }

    public void CreateIngredient(bool setCurrentIngredient)
    {
        int selectedIndex = GetWeightedRandomIndex();
        //Instantiate the ingredient
        IngredientEntity newIngredient = Instantiate(m_ingredientPref, transform.position+new Vector3(5,0,0),transform.rotation).gameObject.GetComponent<IngredientEntity>();       
        //Change the ingredient prent
        newIngredient.transform.parent = transform;
        //Set ingredient data
        //newIngredient.SetIngredientData(m_allIngredients[UnityEngine.Random.Range(0,m_allIngredients.Count)]);   
        newIngredient.SetIngredientData(m_allIngredients[selectedIndex]);
        //Add the ingredient in ingredients list
        m_ingredients = GetComponentsInChildren<IngredientEntity>().ToList();
        //Ajust the weight of selected ingredient type;
        AdjustWeights(selectedIndex);
        m_lastSpawnIndex = selectedIndex;

        //Check if we need to change the current ingredient at spawn
        if (setCurrentIngredient)
        {
            SetCurrentIngredient(m_ingredients[0]);
        }
    }


    public void TryToCut(Vector2 cutVector)
    {

        if ((Vector3)cutVector == m_currentIngredient.Sign)
        {
            CutIngredient();
            var rand = UnityEngine.Random.Range(1,10);
            if (rand < 4) {
                SoundManager.Instance.PlaySFXArray(new string[]{"Cat-001","Cat-002"});
            }
        }
        else { // If is not the correct direction
            SoundManager.Instance.PlaySFX("Cat-003");
            SoundManager.Instance.PlaySFX("WrongII");
            GameManager.Instance.GetCutPhaseManager().CutMissed();
            GameManager.Instance.CameraOne.OscillateShakeRotate(850);
            GameManager.Instance.CameraOne.OscillateShake(5, true,false);
            FeedBackManager.Instance.FreezeFrame(0,0.05f, 0.05f);
        }
       
    }

    public void CutIngredient()
    {
        // Call action ingrecient cut
        OnIngredientCut?.Invoke(m_currentIngredient);
        m_currentIngredient.OnCut();
        // Remove the current ingredient from the ingredients list
        m_ingredients.RemoveAt(0);
        // Set the new current ingredient
        SetCurrentIngredient(m_ingredients[0]);
        if (m_ingredients.Count < m_baseIngredientNum/2)
        {
            CreateIngredient(false);
        }
        m_currentIngredient.IngredientVisual.Strech(10);
    }

    #region RANDOM WEIGHT SYSTEM
    int GetWeightedRandomIndex()
    {
        // Calculer la somme totale des poids
        float totalWeight = 0f;
        foreach (float weight in m_weights)
        {
            totalWeight += weight;
        }

        // Générer une valeur aléatoire entre 0 et la somme des poids
        float randomValue = UnityEngine.Random.Range(0f, totalWeight);

        // Identifier l'objet correspondant à la valeur aléatoire
        float cumulativeWeight = 0f;
        for (int i = 0; i < m_weights.Count; i++)
        {
            cumulativeWeight += m_weights[i];
            if (randomValue <= cumulativeWeight)
            {
                return i;
            }
        }

        // Par sécurité, retourner le dernier index si aucune correspondance (devrait rarement arriver)
        return m_weights.Count - 1;
    }

    void AdjustWeights(int selectedIndex)
    {
        // Réduire le poids de l'objet choisi
        m_weights[selectedIndex] *= m_weightReductionFactor;

        // Normaliser les poids pour éviter qu'ils deviennent trop petits
        NormalizeWeights();
    }

    void NormalizeWeights()
    {
        // Calculer la somme totale des poids
        float totalWeight = 0f;
        foreach (float weight in m_weights)
        {
            totalWeight += weight;
        }

        // Réajuster les poids pour que la somme soit égale à 1
        for (int i = 0; i < m_weights.Count; i++)
        {
            m_weights[i] /= totalWeight;
        }
    }
    #endregion


    #region ACCESSORS
    public void SetCurrentIngredient(IngredientEntity newCurrentIngredient)
    {
        m_currentIngredient = newCurrentIngredient;
        OnCurrentIngredientChanged?.Invoke();
    }

    public IngredientEntity GetCurrentIngredient()
    {
        return m_currentIngredient;
    }
    #endregion

}
