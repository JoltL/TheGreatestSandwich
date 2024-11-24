using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager_l2 : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _scoreText;
    private float _score;

    [SerializeField] private TMP_Text[] _nbIngredientText;

    [SerializeField] private Spawner _spawner;

    [SerializeField] private GameObject _photoPanel;


    private void Start()
    {
        _spawner = GetComponent<Spawner>();
    }
    private void Update()
    {
        _nbIngredientText[0].text = _spawner._stackedIngredient.Count.ToString();

        _scoreText[0].text = _score.ToString();

        if (_spawner._isTheEnd)
        {
            _nbIngredientText[1].text = _nbIngredientText[0].text;

            _scoreText[1].text = _scoreText[0].text;

            _photoPanel.SetActive(true);
        }
    }

    public void AddScore(int points)
    {
        _score += points; 
    }
}
