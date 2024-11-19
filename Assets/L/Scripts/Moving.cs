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

    [Header("Spawner")]

    private Spawner _spawner;

    [Header("Stack")]

    private bool _alreadyFinish = false;


    private void Start()
    {

        _spawner = FindObjectOfType<Spawner>();

        if (_pointA != null && _pointB != null)
        {
            _target = _pointA;

            _isMoving = true;

        }
    }
    private void Update()
    {

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

    private void OnTriggerEnter2D(Collider2D other)
    {


        if (!_alreadyFinish)
        {

            StartCoroutine(StayStable());

            //Spawn with plate
            if (other.gameObject.CompareTag("Player"))
            {

                this.gameObject.tag = "Finish";

                _spawner._stackedIngredient.Add(this.gameObject);

                _spawner.Spawn();

                print("Spawn by touching the plate");

                _alreadyFinish = true;

               

                return;
            }

            //Spawn with tag Finish
            if (other.gameObject.CompareTag("Finish"))
            {
                //other.gameObject.tag = "Untagged";
                //this.gameObject.tag = "Finish";

                _spawner._stackedIngredient.Add(this.gameObject);

                _spawner.Spawn();
                print("Spawn by touching the top ingredient");

                _alreadyFinish = true;

            }

            //Spawn with ground
            
            if (other.gameObject.CompareTag("Respawn"))
            {

                _alreadyFinish = true;
                _spawner._stackedIngredient.Remove(this.gameObject);
                _spawner.Spawn();

                this.gameObject.tag = "Respawn";
                print("Spawn by touching the ground");

            }
        }
     
    }

    IEnumerator StayStable()
    {


        yield return new WaitForSeconds(1f);

        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;


            print(gameObject.transform.rotation.z +"+" + this.gameObject);
        if (this.gameObject.transform.rotation.z > 2 || this.gameObject.transform.rotation.z < -2)
        {

            _spawner._stackedIngredient.Remove(gameObject);
            Destroy(gameObject);

        }


        for (int i = 1; i < _spawner._stackedIngredient.Count; i++)
        {
            print("Tag");
            if (_spawner._stackedIngredient[i].transform.position.y > _spawner._stackedIngredient[i - 1].transform.position.y)
            {
                _spawner._stackedIngredient[i].tag = "Finish";
                _spawner._stackedIngredient[i - 1].tag = "Untagged";
            }

        }



    }

    /// <summary>
    /// Add if rotation > so remove in the list or destroy
    /// </summary>
  

    //When touching but falling after
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            _spawner._stackedIngredient.Remove(this.gameObject);
            this.gameObject.tag = "Respawn";

        }
    }
}
