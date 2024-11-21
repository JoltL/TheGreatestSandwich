using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager_l2 : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _scoreText;

    [SerializeField] private Spawner _spawner;

    [SerializeField] private GameObject _photoPanel;


    private void Start()
    {
        _spawner = GetComponent<Spawner>();
    }
    private void Update()
    {
        _scoreText[0].text = _spawner._stackedIngredient.Count.ToString();

        if(_spawner._isTheEnd)
        {
            _scoreText[1].text = _scoreText[0].text;
            _photoPanel.SetActive(true);
        }
    }
}
