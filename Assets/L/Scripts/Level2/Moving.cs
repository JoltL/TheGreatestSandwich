using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Moving : MonoBehaviour
{
    [Header("Move")]
    public float _moveSpeed = 3f;

    public Transform _pointA;
    public Transform _pointB;

    private Transform _target;
    private bool _movingToB = true;

    public bool _isMoving = false;

    [SerializeField] private float _limit;

    [Header("Reference")]

    private Spawner _spawner;

    private UIManager_l2 _uiManager;

    CameraController _camera;

    [Header("Stack")]

    public bool _isStacked = false;

    private bool _isRotten;

    bool _once = false;

    [SerializeField] private bool _isSliding;


    [Header("Animation")]
    private Animator _animator;

    [SerializeField] private TMP_Text _text; //Bonus text (+1)

    private int _bonus;

    [SerializeField] private GameObject _fx;




    private void Start()
    {

        _uiManager = FindObjectOfType<UIManager_l2>();
        _animator = GetComponent<Animator>();
        _camera = FindObjectOfType<CameraController>();
        _spawner = FindObjectOfType<Spawner>();

        MoveDifficulty();

        if (_pointA != null && _pointB != null)
        {
            _target = _pointA;

            _isMoving = true;

        }
    }
    private void Update()
    {
        //Move limits
        float limitsx = Mathf.Clamp(transform.position.x, -_limit, _limit);
        transform.position = new Vector3(limitsx, transform.position.y, transform.position.z);


        if (_isMoving == true)
        {
            MoveTo();
        }

    }

    void MoveTo()
    {

        float step = _moveSpeed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, _target.position, step);

        if (Vector2.Distance(transform.position, _target.position) < 0.001f)
        {

            if (_movingToB)
            {
                _target = _pointA;
                //transform.eulerAngles = new Vector2(0f, 180f);

            }
            else
            {
                _target = _pointB;
                //transform.eulerAngles = new Vector2(0f, 0f);
            }

            _movingToB = !_movingToB;

        }

    }

    //VOIR LA DIFFICULTE
    void MoveDifficulty()
    {
        int randomSpeed;

        if (_spawner._ingredientCount >= _spawner._maxIngredients * 0.75f)
        {
            randomSpeed = Random.Range(8, 11);
            print("4 :" + randomSpeed);
        }
        else if (_spawner._ingredientCount >= _spawner._maxIngredients * 0.5f)
        {
            randomSpeed = Random.Range(7, 10);
            print("3 :" + randomSpeed);
        }
        else if (_spawner._ingredientCount >= _spawner._maxIngredients * 0.25f)
        {
            randomSpeed = Random.Range(6, 9);
            print("2 :" + randomSpeed);
        }
        else
        {
            randomSpeed = Random.Range(5, 8);
            print("1 :" + randomSpeed);
        }
        _moveSpeed = randomSpeed;
    }


    void CheckRotation()
    {
        float rotationZ = transform.eulerAngles.z;

        // Convertir en une plage de -180 à 180 degrés
        if (rotationZ > 180f) rotationZ -= 360f;

        // Vérifier si l'objet est incliné au-delà d'un seuil (par ex. 15 degrés)
        if (Mathf.Abs(rotationZ) > 15f)
        {
            HandleTiltedObject();
        }
    }

    void HandleTiltedObject()
    {
        Debug.Log("Ingrédient incliné");

        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        gameObject.tag = "Untagged";
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        _spawner._stackedIngredient.Remove(this.gameObject);
    }

    private int CalculateScore(float distance)
    {
        if (distance >= 1.25)
            return -2;
        else if (distance >= 1f)
            return -1;
        else if (distance >= 0.5f)
            return 0;
        else if (distance >= 0.25f)
            return 1;
        else if (distance >= 0.05f)
            return 2;
        else
            return 3;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!_isStacked)
        {

            if(GameManager.Instance)
            GameManager.Instance.CameraOne.OscillateShake(5, false, true);

            _animator.SetTrigger("Squish");

            if (SoundManager.Instance)
            {
                SoundManager.Instance.PlaySFX(gameObject.name);
            }

            if (_fx != null)
            {
                _fx.SetActive(true);

            }

            //Spawn with tag Finish
            if (other.gameObject.CompareTag("Finish"))
            {

                _isStacked = true;

                //print("Trigger Finish with" + other.gameObject.name);


                if (!_isSliding)
                { StartCoroutine(StayStable()); }
                else
                {
                    StartCoroutine(StackUpdate());
                }
            }

            //Spawn with ground

            else if (other.gameObject.CompareTag("Respawn"))
            {
                if (GameManager.Instance)
                    GameManager.Instance.CameraOne.OscillateShake(10, true, false);
                _isRotten = true;
                _spawner._stackedIngredient.Remove(this.gameObject);

                _isStacked = true;

                _camera.RemoveTarget(gameObject.transform);

                //this.gameObject.GetComponent<Moving>().enabled = false;
                //print("Spawn by touching the ground" + other.gameObject.name);

                if (!_isSliding)
                { StartCoroutine(StayStable()); }
                else
                {
                    StartCoroutine(StackUpdate());
                }

            }

            else
            {
                if (GameManager.Instance)
                    GameManager.Instance.CameraOne.OscillateShake(10, true, false);

                if (SoundManager.Instance)
                {
                    SoundManager.Instance.PlaySFX("Wrong");
                }

                _isRotten = true;
                _spawner._stackedIngredient.Remove(this.gameObject);


                _isStacked = true;

                _camera.RemoveTarget(gameObject.transform);

                print("Spawn by touching other things" + other.gameObject.name);


                if (!_isSliding)
                {
                    StartCoroutine(StayStable());
                }
                else
                {
                    StartCoroutine(StackUpdate());
                }
            }

        }


    }

    IEnumerator StayStable()
    {

        yield return new WaitForSeconds(0.5f);

        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;


        if (!_isRotten)
        {
            _spawner._stackedIngredient.Add(this.gameObject);

            _spawner.Stacked(1);



            float distanceFromCenter = Mathf.Abs(transform.position.x);

            _bonus = CalculateScore(distanceFromCenter);

            _uiManager.AddScore(_bonus);


            if (SoundManager.Instance)
            {
                if (_bonus < 0)
                {

                    SoundManager.Instance.PlaySFX("Wrong");

                }
                else if (_bonus > 0)
                {

                    SoundManager.Instance.PlaySFX("Good");

                }
                else
                {
                    SoundManager.Instance.PlaySFX("Click");
                }

            }



        }
        else
        {
            _bonus = -1;
            _uiManager.AddScore(_bonus);

            if (SoundManager.Instance)
            {
                SoundManager.Instance.PlaySFX("Wrong");
            }
        }


        _text.text = _bonus.ToString();
        _text.gameObject.SetActive(true);
        StartCoroutine(SetactiveFalse());

        for (int i = 1; i < _spawner._stackedIngredient.Count; i++)
        {
            if (_spawner._stackedIngredient.Count > 1)
            {
                _camera.RemoveTarget(_spawner._stackedIngredient[i - 1].transform);

            }

        }

        CheckRotation();

        _spawner.Spawn();
        //print("isSpawning");

    }

    IEnumerator SetactiveFalse()
    {
        yield return new WaitForSeconds(0.5f);

        _text.gameObject.SetActive(false);
    }

    //No freeze
    IEnumerator StackUpdate()
    {
        yield return new WaitForSeconds(0.5f);


        if (!_isRotten)
        {
            _spawner._stackedIngredient.Add(this.gameObject);

            _spawner.Stacked(1);

            // Calculer la distance par rapport au centre (x = 0)
            float distanceFromCenter = Mathf.Abs(transform.position.x);

            // Calculer le score en fonction de la distance
            int score = CalculateScore(distanceFromCenter);

            // Afficher ou utiliser le score (par exemple, l'ajouter à un score global)
            print("Score: " + score);


            if (score < 0)
            {
                if (SoundManager.Instance)
                {
                    SoundManager.Instance.PlaySFX("Wrong");
                }
            }
            else if (score > 0)
            {
                if (SoundManager.Instance)
                {
                    SoundManager.Instance.PlaySFX("Good");
                }
            }

            _uiManager.AddScore(score);


        }

        for (int i = 1; i < _spawner._stackedIngredient.Count; i++)
        {
            if (_spawner._stackedIngredient.Count > 1)
            {
                _camera.RemoveTarget(_spawner._stackedIngredient[i - 1].transform);

            }

        }
        CheckRotation();

        _spawner.Spawn();


    }

    //When touching but falling after
    private void OnTriggerExit2D(Collider2D other)
    {

        if (_isStacked)
        {
            if (!_once)
            {

                if (GameManager.Instance)
                    GameManager.Instance.CameraOne.OscillateShake(10, true, false);

                _camera.RemoveTarget(gameObject.transform);
                gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
                //_spawner.Stacked(-1);
                _isRotten = true;

                if (SoundManager.Instance)
                {
                    SoundManager.Instance.PlaySFX("Wrong");
                }

                _spawner.TopIngredient();

                _once = true;
            }

        }
    }
}
