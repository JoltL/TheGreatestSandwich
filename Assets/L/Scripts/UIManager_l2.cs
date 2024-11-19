using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager_l2 : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    [SerializeField] private Spawner _spawner;


    private void Start()
    {
        _spawner = GetComponent<Spawner>();
    }
    private void Update()
    {
        _scoreText.text = _spawner._stackedIngredient.Count.ToString();
    }
}
