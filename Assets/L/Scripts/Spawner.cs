using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Spawner : MonoBehaviour
{

    [Header("Spawn")]
    [SerializeField] private GameObject[] _ingredient;

    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;

    [SerializeField] private Transform _parent;

    private GameObject _thisIngredient;

    public List<GameObject> _stackedIngredient;

    public List<GameObject> _AllIngredient;

    //A modifier avec le premier niveau
    [SerializeField] private int _ham = 3;
    [SerializeField] private int _cheese = 3;
    [SerializeField] private int _tomato = 3;
    [SerializeField] private int _salad = 3;


    [Header("Score")]
    [SerializeField] private int _ingredientCount;

    [SerializeField] private int _maxIngredients;

    [Header("Camera")]

    private CameraController _camera;

    [SerializeField] private GameObject _movementPos;

    public bool _hasSpawned = false;

    private int _distanceCount;
    private int _numberlimit =0;

    private float _posY;

    public bool _isTheEnd = false;

    private bool _canClick = true;


    private void Start()
    {
        _camera = FindObjectOfType<CameraController>();

        _maxIngredients = _ham + _tomato + _cheese + _salad;

        Spawn();

       
    }

    private void Update()
    {

        if (_canClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Drop();
            }

        }

    }

    public void TopIngredient()
    {
        if (_stackedIngredient.Count == 1)
        {
            _stackedIngredient[0].tag = "Finish";
            return;
        }

        for (int i = 1; i < _stackedIngredient.Count; i++)
        {

            if (_stackedIngredient[i].transform.position.y > _stackedIngredient[i - 1].transform.position.y)
            {
                _stackedIngredient[i].tag = "Finish";
                _camera.AddTarget(_stackedIngredient[i].transform);
                print("tagfinish");

                _stackedIngredient[i - 1].tag = "Untagged";

               _camera.RemoveTarget(_stackedIngredient[i - 1].transform);

                print("taguntagged");
            }

        }

    }

    public void Spawn()
    {

        if (_ingredientCount < _maxIngredients)
        {
            //Random position
            Vector2 position = new Vector2(0f, 0f);
            
            int randomPosition = Random.Range(0, 2); //Random.Range(min, max-1)

            if (randomPosition == 0)
            {
                position = _pointA.position;
            }
            else
            {
                position = _pointB.position;
            }

            // Générer la liste des ingrédients et leurs quantités
            _AllIngredient = GenerateWeightedList();
            // Sélectionner un ingrédient aléatoire
            int randomIndex = Random.Range(0, _AllIngredient.Count);
            GameObject selectedIngredient = _AllIngredient[randomIndex];

            //Instantiate
            GameObject thisRandomIngredient = Instantiate(selectedIngredient, position, transform.rotation, _parent);

            thisRandomIngredient.GetComponentInChildren<Moving>()._pointA = _pointA;
            thisRandomIngredient.GetComponentInChildren<Moving>()._pointB = _pointB;

            _thisIngredient = thisRandomIngredient;

            _camera.AddTarget(thisRandomIngredient.transform);

            // Mettre à jour les quantités restantes
            if (selectedIngredient == _ingredient[0]) _ham--;
            else if (selectedIngredient == _ingredient[1]) _cheese--;
            else if (selectedIngredient == _ingredient[2]) _tomato--;
            else if (selectedIngredient == _ingredient[3]) _salad--;

            _hasSpawned = true;

        }
        else
        {
            //ENDING
            _isTheEnd = true;

            _canClick = false;

            Debug.Log("Let's eat!");

                _camera._targets.Clear();

            for (int i = 0; i < _stackedIngredient.Count; i++)
            {
                _camera.AddTarget(_stackedIngredient[i].transform);

            }
                _camera.EndZoom();
        }
    }

    private List<GameObject> GenerateWeightedList()
    {
        List<GameObject> weightedList = new List<GameObject>();

        for (int i = 0; i < _ham; i++) weightedList.Add(_ingredient[0]); 
        for (int i = 0; i < _cheese; i++) weightedList.Add(_ingredient[1]); 
        for (int i = 0; i < _tomato; i++) weightedList.Add(_ingredient[2]); 
        for (int i = 0; i < _salad; i++) weightedList.Add(_ingredient[3]); 

        return weightedList;
    }

    //Spawn position hauteur
    void DistancePosition()
    {

        if(_distanceCount >= 2)
        {
            _posY += 0.2f;

            _movementPos.transform.position = new Vector3(0f,_posY,0f);

            _distanceCount = 0;

            print("up" + _posY);

        }
      
    }
    void ReachedLevel()
    {
        if(_numberlimit >= 5)
        {

            FrozeIngredient();
            _numberlimit = 0;
        }
    }

    void FrozeIngredient()
    {
        foreach (GameObject all in _stackedIngredient)
        {
            all.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    public void Drop()
    {
        TopIngredient();

        ReachedLevel();

        if (_thisIngredient != null)
        {

            DistancePosition();

            _thisIngredient.GetComponentInChildren<Moving>()._isMoving = false;

            _thisIngredient.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            _ingredientCount++;

        }
    }

    public void Stacked(int number)
    {
        _numberlimit += number; 
        _distanceCount += number;

    }

}
