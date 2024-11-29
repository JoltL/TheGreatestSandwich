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

    [SerializeField] private TMP_Text _maxnbIngredientText;

    [SerializeField] private GameObject _photoPanel;

    [SerializeField] private Slider _slider;

    [SerializeField] private GameObject _dissapearBeforePhoto;

    private float _sliderScore;

    [Header("Best Score")]

    [SerializeField] private int _bestScore;

    [SerializeField] private TMP_Text _bestScoreText;

    [Header("References")]

    [SerializeField] private Spawner _spawner;

    [SerializeField] private TakeScreenshot _takeScreenshot;
    private bool _once = false;

    [Header("Bonus")]

    [SerializeField] private GameObject _help;

    private bool _hardMode;

    int _nbOfMaxSlider;


    private void Start()
    {
        _bestScore = PlayerPrefs.GetInt("Best Score", 0);

        _spawner = GetComponent<Spawner>();

        _once = false;

        _slider.maxValue = 10f;

    }
    private void Update()
    {


        //+1 for bread

        int maxing = _spawner._maxIngredients + 1;
        _maxnbIngredientText.text = _spawner._ingredientCount.ToString() + "/" + maxing.ToString();
        _slider.value = Mathf.Lerp(_slider.value, _sliderScore, 5 * Time.deltaTime);
        _sliderScore = Mathf.Clamp(_sliderScore, 0f, _slider.maxValue);

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

        TotalScore();

        ShowUI();

        StartCoroutine(waitScreenshot());
        _spawner._isTheEnd = false;
    }

    IEnumerator waitScreenshot()
    {
       
        //_slider.gameObject.SetActive(false);  
        _dissapearBeforePhoto.SetActive(false);

        yield return new WaitForSeconds(1f);

        if (!_once)
        {
            _takeScreenshot.Screenshot();
            _once = true;
        }

    }

    void ShowUI()
    {
        _bestScoreText.text = _bestScore.ToString();

        //_spawner.TheEnd();

        _nbIngredientText[1].text = _nbIngredientText[0].text;

        _scoreText[1].text = _scoreText[0].text;

        _photoPanel.SetActive(true);
    }

    public void TotalScore()
    {
        if (_spawner._stackedIngredient.Count > _bestScore)
        {
            _bestScore = _spawner._stackedIngredient.Count;

            PlayerPrefs.SetInt("Best Score", _bestScore);

        }
    }

    public void AddScore(int points)
    {
        _score += points;

        _sliderScore += points;

        _help.SetActive(false);

        MinScore();

        //Difficulty : be more precise
        if (_hardMode)
        {
            if (_nbOfMaxSlider > 5)
            {
                _sliderScore -= 2;
            }
            else if (_nbOfMaxSlider > 2)
            {
                _sliderScore -= 1;
            }
            else
            {
                _sliderScore -= 0;
            }
        }


    }

    void MinScore()
    {
        if (_sliderScore <= 0f)
        {
            //IstheEnd();

            _spawner._isTheEnd = true;
        }

        if (_sliderScore >= _slider.maxValue)
        {
            _help.SetActive(true);
            _nbOfMaxSlider++;

            _sliderScore = 8f;

           
            _hardMode = true;

        }
    }

    //Button

    public void LoadScene(int scene)
    {
        GameManager.Instance.LoadScene(scene);
    }

}
