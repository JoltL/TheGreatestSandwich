using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Spawner : MonoBehaviour
{

    [Header("Spawn")]
    [SerializeField] private GameObject _ingredient;

    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;

    [SerializeField] private Transform _parent;

    private GameObject _thisIngredient;

    public List<GameObject> _stackedIngredient;

    public List<GameObject> _AllIngredient;

    [Header("Score")]
    [SerializeField] private int _ingredientCount;

    [SerializeField] private int _maxIngredients;



    private void Start()
    {

        Spawn();

       
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Drop();
        }

        for (int i = 1; i < _stackedIngredient.Count; i++)
        {

            if (_stackedIngredient[i].transform.position.y > _stackedIngredient[i - 1].transform.position.y)
            {
                _stackedIngredient[i].tag = "Finish";
                _stackedIngredient[i - 1].tag = "Untagged";
            }

        }

    }

    public void Spawn()
    {
        if (_ingredientCount < _maxIngredients)
        {
            //Random position
            Vector2 position = new Vector2(0f, 0f);

            int randomPosition = Random.Range(0, 2);


            //Random.Range(min, max-1)
            if (randomPosition == 0)
            {
                position = _pointA.position;
            }
            else
            {
                position = _pointB.position;
            }

            //Instantiate
            GameObject thisRandomIngredient = Instantiate(_ingredient, position, transform.rotation, _parent);

            thisRandomIngredient.GetComponentInChildren<Moving>()._pointA = _pointA;
            thisRandomIngredient.GetComponentInChildren<Moving>()._pointB = _pointB;


            _thisIngredient = thisRandomIngredient;

            _AllIngredient.Add(_thisIngredient);


        }
        else
        {
            //ENDING
        }
    }


    public void Drop()
    {

        if (_thisIngredient != null)
        {

            _thisIngredient.GetComponentInChildren<Moving>()._isMoving = false;

            _thisIngredient.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            _ingredientCount++;
        }
    }

}
