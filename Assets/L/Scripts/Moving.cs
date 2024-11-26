using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float _moveSpeed = 3f;

    public Transform _pointA;
    public Transform _pointB;

    private Transform _target;
    private bool _movingToB = true;

    public bool _isMoving = false;

    [SerializeField] private float _limit;

    [Header("Spawner")]

    private Spawner _spawner;

    [Header("Stack")]

    public bool _isStacked = false;

    private bool _isRotten;

    CameraController _camera;

    [SerializeField] private bool _isSliding;


    private void Start()
    {
        _camera = FindObjectOfType<CameraController>();

        _spawner = FindObjectOfType<Spawner>();

        _moveSpeed = Random.Range(4f, 10f);

        if (_pointA != null && _pointB != null)
        {
            _target = _pointA;

            _isMoving = true;

        }
    }
    private void Update()
    {
        float limitsx = Mathf.Clamp(transform.position.x, -_limit, _limit);

        transform.position = new Vector3(limitsx, transform.position.y, transform.position.z);

        if (_isMoving == true)
        {
            MoveTo();

        }

    }
    void MoveTo()
    {

        //Speed
        float step = _moveSpeed * Time.deltaTime;

        //Move toward with speed
        transform.position = Vector2.MoveTowards(transform.position, _target.position, step);

        //If distance is close
        if (Vector2.Distance(transform.position, _target.position) < 0.001f)
        {
            // If point B achieved, go point A and switch
            if (_movingToB)
            {
                _target = _pointA;
                transform.eulerAngles = new Vector2(0f, 180f);

            }
            else
            {
                _target = _pointB;
                transform.eulerAngles = new Vector2(0f, 0f);
            }
            //if MovingtoB, don't go to B
            _movingToB = !_movingToB;

        }

    }


    void CheckRotation()
    {
        // Obtenir la rotation autour de l'axe Z
        float rotationZ = transform.eulerAngles.z;

        // Convertir en une plage de -180 � 180 degr�s
        if (rotationZ > 180f) rotationZ -= 360f;

        // V�rifier si l'objet est inclin� au-del� d'un seuil (par ex. 15 degr�s)
        if (Mathf.Abs(rotationZ) > 15f)
        {
            // Action si l'objet est inclin�
            HandleTiltedObject();
        }
    }

    void HandleTiltedObject()
    {
        Debug.Log("Ingr�dient inclin�");
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        gameObject.tag = "Untagged";
        gameObject.GetComponent<Moving>().enabled = false;
        _spawner._stackedIngredient.Remove(this.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!_isStacked)
        {

            //Spawn with tag Finish
            if (other.gameObject.CompareTag("Finish"))
            {

                _isStacked = true;

                print("Trigger Finish with" + other.gameObject.name);

                
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
                _isRotten = true;
                _spawner._stackedIngredient.Remove(this.gameObject);

                _isStacked = true;

                _camera.RemoveTarget(gameObject.transform);

                //this.gameObject.GetComponent<Moving>().enabled = false;
                print("Spawn by touching the ground" + other.gameObject.name);

                if (!_isSliding)
                { StartCoroutine(StayStable()); }
                else
                {
                    StartCoroutine(StackUpdate());
                }
               
            }

            else
            {

                _isRotten = true;

                _spawner._stackedIngredient.Remove(this.gameObject);


                _isStacked = true;

                _camera.RemoveTarget(gameObject.transform);

                print("Spawn by touching other things" + other.gameObject.name);


                if (!_isSliding)
                { StartCoroutine(StayStable()); }
                else
                {
                    StartCoroutine(StackUpdate());
                }
            }

        }

      
    }

    IEnumerator StayStable()
    {


        yield return new WaitForSeconds(1f);

        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;


        if (!_isRotten)
        {
            _spawner._stackedIngredient.Add(this.gameObject);

            _spawner.Stacked(1);

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
        print("isSpawning");

    }

    //No freeze
    IEnumerator StackUpdate()
    {
        yield return new WaitForSeconds(1f);


        if (!_isRotten)
        {
            _spawner._stackedIngredient.Add(this.gameObject);

            _spawner.Stacked(1);

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
            _camera.RemoveTarget(gameObject.transform);
            //_spawner.Stacked(-1);
            _isRotten = true;

        }
    }
}
