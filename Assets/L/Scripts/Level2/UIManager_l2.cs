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

    [SerializeField] private GameObject _frontSlider;

    [SerializeField] private GameObject _dissapearBeforePhoto;

    private float _sliderScore;

    [Header("Best Score")]

    [SerializeField] private int _bestScore;

    [SerializeField] private TMP_Text _bestScoreText;

    [Header("References")]

    [SerializeField] private Spawner _spawner;

    [SerializeField] private TakeScreenshot _takeScreenshot;
    private bool _once = false;

    public GameObject _wrongPanel;

    [Header("Bonus")]

    [SerializeField] private GameObject _help;

    private bool _hardMode;

    int _nbOfMaxSlider;

    [SerializeField] private float _baseLossRate = 0.1f; // Perte initiale par seconde

    private bool _isPreventing;

    [SerializeField] private Color[] _colors;

    [Header("Countdown")]

    [SerializeField] private TMP_Text _countdownText;

    private bool _timerIsActive = true;

    private bool _sliderStop = true;


    private void Start()
    {
        _baseLossRate = 0.05f;

        _maxnbIngredientText.gameObject.SetActive(false);

        _bestScore = PlayerPrefs.GetInt("Best Score", 0);

        _spawner = GetComponent<Spawner>();

        _once = false;

        _slider.maxValue = 10f;
        _sliderScore = 5f;
        _slider.value = 5f;

        _maxnbIngredientText.text = _spawner._AllIngredient.Count.ToString();

    }
    private void Update()
    {
        if (_timerIsActive)
            StartCoroutine(Countdown());

        if (!_timerIsActive)
        {

            //+1 for bread // int maxing = _spawner._maxIngredients + 1;+ "/" + maxing.ToString()
            _maxnbIngredientText.text = _spawner._AllIngredient.Count.ToString();

            if (_sliderScore > 5)
            {
                _sliderStop = false;

            }
            if (!_sliderStop)
            {

                _slider.value = Mathf.Lerp(_slider.value, _sliderScore, 5 * Time.deltaTime);
                _sliderScore = Mathf.Clamp(_sliderScore, 0f, _slider.maxValue);

                _nbIngredientText[0].text = _spawner._stackedIngredient.Count.ToString();

                _scoreText[0].text = _score.ToString();

            }
        }


        if (_hardMode && !_spawner._isTheEnd)
        {
            float lossRate = _baseLossRate + (_nbOfMaxSlider * 0.1f); // Augmentation progressive
            _sliderScore -= lossRate * Time.deltaTime;

            if (_sliderScore <= 0f)
            {
                //IstheEnd();

                _spawner._isTheEnd = true;

            }
            else if (_sliderScore <= 5f && !_isPreventing)
            {
                StartCoroutine(PreventSliderDiminution());

            }
            else if (_sliderScore > 5 && _isPreventing)
            {

                _isPreventing = false;

            }

            print(_spawner._isTheEnd);
        }

        if (_spawner._isTheEnd)
        {
            IstheEnd();
        }
    }

    IEnumerator Countdown()
    {
        _timerIsActive = false;

        int i = 4;
        while (i != 0)
        {
            _countdownText.GetComponent<OscillatorScale>().StartOscillator(10);
            _countdownText.GetComponent<OscillatorRotation>().StartOscillator(150);

            yield return new WaitForSeconds(0.8f);
            i--;
            if (SoundManager.Instance)
                SoundManager.Instance.PlaySFX("Woo");


            _countdownText.text = i.ToString();
        }

        if (i == 0)
        {
            _countdownText.GetComponent<OscillatorScale>().StartOscillator(10);
            _countdownText.GetComponent<OscillatorRotation>().StartOscillator(150);

            _countdownText.text = "GO !";
            if (SoundManager.Instance)
                SoundManager.Instance.PlaySFX("Bell");


            yield return new WaitForSeconds(0.8f);
            _spawner._canClick = true;
            _countdownText.gameObject.SetActive(false);
            _maxnbIngredientText.gameObject.SetActive(true);
        }

    }

    void IstheEnd()
    {
        _spawner._takeBread = false;

        _spawner.TheEnd();

        StartCoroutine(waitScreenshot());

        TotalScore();

        ShowUI();

        _spawner._isTheEnd = false;
    }

    IEnumerator waitScreenshot()
    {
        //foreach (var item in _spawner._stackedIngredient)
        //{
        //    item.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        //}

        if (!_once)
        {
            if (SoundManager.Instance)
                SoundManager.Instance.PlaySFX("Bell");
            _slider.gameObject.GetComponent<Slider>().enabled = false;

            _dissapearBeforePhoto.SetActive(false);

            _once = true;
            yield return new WaitForSeconds(1f);

            _takeScreenshot.Screenshot();

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

        if (points > 0 && _sliderStop)
        { 
        
        _score += points;
        _nbIngredientText[0].gameObject.GetComponent<OscillatorScale>().StartOscillator(10);
        _nbIngredientText[0].gameObject.GetComponent<OscillatorRotation>().StartOscillator(150);

        _sliderScore += points;

        MinScore();

        DifficultySlider();
        }
        else if (!_sliderStop) 
        {

            _score += points;
            _nbIngredientText[0].gameObject.GetComponent<OscillatorScale>().StartOscillator(10);
            _nbIngredientText[0].gameObject.GetComponent<OscillatorRotation>().StartOscillator(150);

            _sliderScore += points;
            if (_nbOfMaxSlider>0)
            {

            _help.SetActive(false);
            }

            MinScore();

            DifficultySlider();

        }
    }

    void DifficultySlider()
    {

        //Difficulty : be more precise
        if (_hardMode)
        {
            if(_nbOfMaxSlider > _spawner._maxIngredients / 1.5f)
            {
                _baseLossRate = 0.2f;
            }
            else if (_nbOfMaxSlider > _spawner._maxIngredients / 3)
            {
                //_sliderScore -= 2;
                _baseLossRate = 0.15f;

            }
            else if (_nbOfMaxSlider > _spawner._maxIngredients / 6)
            {

                //_sliderScore -= 1;
                _baseLossRate = 0.1f;
            }
            else
            {
                _sliderScore -= 0;
            }
            print(_baseLossRate);
        }

    }

    IEnumerator PreventSliderDiminution()
    {
        _isPreventing = true;

        for (int i = 0; i < 3; i++)
        {

            _frontSlider.GetComponent<Image>().color = _colors[0];
            if (SoundManager.Instance)
            {
                SoundManager.Instance.PlaySFX("Bloop");
            }
            yield return new WaitForSeconds(0.25f);
            _frontSlider.GetComponent<Image>().color = _colors[1];
            if (SoundManager.Instance)
            {
                SoundManager.Instance.PlaySFX("Baad");
            }
            yield return new WaitForSeconds(0.25f);
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

            _hardMode = true;

        }
    }

    //Button

    public void LoadScene(int scene)
    {
        Time.timeScale = 1f;
        GameManager.Instance.LoadScene(scene);
    }

}
