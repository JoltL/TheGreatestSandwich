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

    private bool _isStacked = false;

    private bool _isRotten;

    CameraController _camera;


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

        // Convertir en une plage de -180 à 180 degrés
        if (rotationZ > 180f) rotationZ -= 360f;

        // Vérifier si l'objet est incliné au-delà d'un seuil (par ex. 15 degrés)
        if (Mathf.Abs(rotationZ) > 15f)
        {
            // Action si l'objet est incliné
            HandleTiltedObject();
        }
    }

    void HandleTiltedObject()
    {
        Debug.Log("Ingrédient incliné");
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        _spawner._stackedIngredient.Remove(this.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!_isStacked)
        {

            StartCoroutine(StayStable());

            //Spawn with tag Finish
            if (other.gameObject.CompareTag("Finish"))
            {

                //print("Spawn by touching the top ingredient");
               
                _isStacked = true;

            }

            //Spawn with ground

            if (other.gameObject.CompareTag("Respawn"))
            {
                _isRotten = true;

                _isStacked = true;
                _spawner._stackedIngredient.Remove(this.gameObject);

                _camera.RemoveTarget(gameObject.transform);

                this.gameObject.tag = "Respawn";
                print("Spawn by touching the ground");
            }
        }

    }

    IEnumerator StayStable()
    {


        yield return new WaitForSeconds(1f);

        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        CheckRotation();

        if (!_isRotten)
        {
            _spawner._stackedIngredient.Add(this.gameObject);

        }

        for (int i = 1; i < _spawner._stackedIngredient.Count; i++)
        {
            if(_spawner._stackedIngredient.Count > 1)
            {
        _camera.RemoveTarget(_spawner._stackedIngredient[i-1].transform);

            }

        }

        _spawner.Spawn();

    }

    //When touching but falling after
    private void OnTriggerExit2D(Collider2D other)
    {

        if (_isStacked)
        {

            _camera.RemoveTarget(gameObject.transform);

            if (other.gameObject.CompareTag("Finish"))
            {

                _isRotten = true;

                if (_spawner._stackedIngredient.Contains(gameObject))
                {
                    gameObject.tag = "Finish";
                    other.gameObject.tag = "Respawn";
                    _spawner._stackedIngredient.Remove(this.gameObject);

                    print("Other tag respawn" + this.gameObject);
                }

                else
                {
                    _spawner._stackedIngredient.Remove(this.gameObject);
                    this.gameObject.tag = "Respawn";
                    other.gameObject.tag = "Finish";

                    print("Me tag respawn" + this.gameObject);
                }
            }
        }
    }
}
