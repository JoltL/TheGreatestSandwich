using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_l2 : MonoBehaviour
{
    [Header("Score")]

    [SerializeField] private TMP_Text[] _scoreText;

    private float _score;

    [SerializeField] private TMP_Text[] _nbIngredientText;

    [SerializeField] private GameObject _photoPanel;

    [SerializeField] private Slider _slider;

    private int _multiplier = 1;

    [Header("References")]

    [SerializeField] private Spawner _spawner;

    [SerializeField] private TakeScreenshot _takeScreenshot;

    private bool _once = false;

    [SerializeField] private GameObject _help;


    private void Start()
    {
        _spawner = GetComponent<Spawner>();

        _once = false;

        _slider.maxValue = 10f;

    }
    private void Update()
    {
        _slider.value = Mathf.Clamp(_slider.value, 0f, 10f);

        _nbIngredientText[0].text = _spawner._stackedIngredient.Count.ToString();

        _scoreText[0].text = _score.ToString();


        if (_spawner._isTheEnd)
        {
            IstheEnd();
        }
    }

   
    void IstheEnd()
    {

        _spawner.TheEnd();

        _nbIngredientText[1].text = _nbIngredientText[0].text;

        _scoreText[1].text = _scoreText[0].text;

        _photoPanel.SetActive(true);

        StartCoroutine(waitScreenshot());
    }

    IEnumerator waitScreenshot()
    {
        _slider.gameObject.SetActive(false);    

        yield return new WaitForSeconds(1f);

        if (!_once)
        {
            _takeScreenshot.Screenshot();
            _once = true;
        }

    }

    public void AddScore(int points)
    {
        _score += points;


        _slider.value += points;

        _help.SetActive(false);

        MinScore();


    }

    void MinScore()
    {
        if (_slider.value <= 0f)
        {
            IstheEnd();
        }

        if (_slider.value == _slider.maxValue)
        {
            _help.SetActive(true);
        }
    }

}
