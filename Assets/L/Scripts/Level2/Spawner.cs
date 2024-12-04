using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
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

    [SerializeField] private GameObject _bread;

    [Header("Score")]
    public int _ingredientCount;

    public int _maxIngredients;

    [Header("Camera")]

    private CameraController _camera;

    [SerializeField] private GameObject _movementPos;

    public bool _hasSpawned = false;

    public int _distanceCount;

    private int _numberlimit =0;

    private float _posY;

    public bool _isTheEnd = false;

    public bool _canClick = false;

    [SerializeField] private bool _useLevel1;

    private bool _canSpawn = true;

    private bool _once = false;

    public bool _takeBread = false;

    [Header("EndIcon")]

    [SerializeField] private GameObject _whiskerIcon;
    [SerializeField] private Sprite _whiskerSpriteIcon;

    private void Start()
    {
        _posY = -2;

        _canClick = false;
        _camera = FindObjectOfType<CameraController>();

        _AllIngredient = GenerateWeightedList();

        if (_useLevel1)
        {

        Dictionary<string,int> dictIngredient = GameManager.Instance.GetCutIngredients();
        _cheese = dictIngredient["Cheese"];
        _ham = dictIngredient["Ham"];
        _salad = dictIngredient["Salad"];
        _tomato = dictIngredient["Tomato"];
        }
        _maxIngredients = _ham + _tomato + _cheese + _salad;

      
    }

    private void Update()
    {
        if (_canClick && !_once)
        {
            Spawn();
            _once = true;
        }
        //if (_canClick)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Drop();
        //    }

        //}

    }

    public void CanDrop()
    {
        if (_canClick)
        {
            Drop();
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

                if (!_stackedIngredient.Contains(_stackedIngredient[i]))
                {
                _camera.AddTarget(_stackedIngredient[i].transform);
                //print("tagfinish");

                }
                _stackedIngredient[i - 1].tag = "Untagged";

               _camera.RemoveTarget(_stackedIngredient[i - 1].transform);

                //print("taguntagged");
            }

        }

    }

    public void Spawn()
    {
        if(_canSpawn)
        {

            if (_ingredientCount < _maxIngredients && !_takeBread)
            {
                //print(_ingredientCount + "/" + _maxIngredients);

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

                GameObject thisRandomIngredient = Instantiate(selectedIngredient, position, transform.rotation, _parent);

                if (_stackedIngredient.Count > 0)
                {
                    int number = _stackedIngredient.Count;
                    GameObject item = _stackedIngredient[number - 1];

                    thisRandomIngredient.GetComponent<SpriteRenderer>().sortingOrder = item.GetComponent<SpriteRenderer>().sortingOrder + 1;
                }

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
            else if (_ingredientCount >= _maxIngredients && !_takeBread)
            {
                _AllIngredient.Clear();

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

                GameObject thisRandomIngredient = Instantiate(_bread, position, transform.rotation, _parent);
                thisRandomIngredient.GetComponentInChildren<Moving>()._pointA = _pointA;
                thisRandomIngredient.GetComponentInChildren<Moving>()._pointB = _pointB;
                _camera.AddTarget(thisRandomIngredient.transform);

                _thisIngredient = thisRandomIngredient;
                if (_stackedIngredient.Count > 0)
                {
                    int number = _stackedIngredient.Count;
                    GameObject item = _stackedIngredient[number - 1];

                    thisRandomIngredient.GetComponent<SpriteRenderer>().sortingOrder = item.GetComponent<SpriteRenderer>().sortingOrder + 1;
                }

                _hasSpawned = true;
                _takeBread = true;

            }
            else if (_ingredientCount >= _maxIngredients && _takeBread)
            { 
                //ENDING
                TheEnd();
               
                //2 photos? avant après?
                
               
                
            }
        
        }
    }

    public void TheEnd()
    {
        _takeBread = false;


        if (_AllIngredient.Count <= 0)
        {
        _whiskerIcon.GetComponent<Image>().sprite = _whiskerSpriteIcon;
        }

        _isTheEnd = true;
        _canSpawn = false;
        _canClick = false;


        _camera._targets.Clear();

        for (int i = 0; i < _stackedIngredient.Count; i++)
        {
            _camera.AddTarget(_stackedIngredient[i].transform);

        }
        _camera.EndZoom();

        //foreach (var item in _stackedIngredient)
        //{
        //    item.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        //}
        Debug.Log("Let's eat!");

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
            _posY += 0.5f;

            _movementPos.transform.position = new Vector3(0f,_posY,0f);

            _distanceCount = 0;

            //print("up" + _posY);

        }
      
    }

    //If use physics
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

    //

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
