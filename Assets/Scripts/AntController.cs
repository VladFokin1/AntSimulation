using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntController : MonoBehaviour
{
    [SerializeField] GameObject _toHomeMarker;
    [SerializeField] GameObject _toFoodMarker;
    [SerializeField] float _timeToMarker = .2f;
    [SerializeField] float _markersIntensivityBurnRate = 0.9f;
    [SerializeField] int _startMarkersAmount;
    //[SerializeField] Transform target;
    [SerializeField] float _speed = 10f;

    //privates
    private Rigidbody2D _rb;
    private float _timeFromLastMarker = 0;
    private float _markersIntensivity = 1f;
    private bool _isBusy = false;
    private Vector3 _targetPos;
    private bool _goToTargetPos;
    private MarkerController _mostIntensiveToHome;
    private MarkerController _mostIntensiveToFood;
    private int _markersAmount;

    private List<MarkerController> _toHomeList;
    private List<MarkerController> _toFoodList;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _toHomeList = new List<MarkerController>();
        _toFoodList = new List<MarkerController>();
        _markersAmount = _startMarkersAmount;
    }


    // Update is called once per frame
    void Update()
    {
        
        _timeFromLastMarker += Time.deltaTime;

        if (_timeFromLastMarker > _timeToMarker && _markersAmount > 0)
        {
            _timeFromLastMarker = 0;
            MarkerController marker = Instantiate(_isBusy ? _toFoodMarker : _toHomeMarker, transform.position, Quaternion.identity).GetComponent<MarkerController>();
            marker.Intensivity = _markersIntensivity;
            marker.Refresh();
            _markersIntensivity *= _markersIntensivityBurnRate;

            _markersAmount -= 1;
        }

        //going to home or food
        if (_goToTargetPos)
        {
            transform.up = Vector3.Lerp(transform.up, (_targetPos - transform.position).normalized, 0.25f);
        }
        //search for home
        else if (_isBusy)
        {
            if (_mostIntensiveToHome != null)
                transform.up = Vector3.Lerp(transform.up, (_mostIntensiveToHome.transform.position - transform.position).normalized, 0.25f);
        }
        //search for food
        else if (!_isBusy)
        {
            if (_mostIntensiveToFood != null)
                transform.up = Vector3.Lerp(transform.up, (_mostIntensiveToFood.transform.position - transform.position).normalized, 0.25f);
        }

        if (Random.Range(0, 100) > 90) 
            transform.Rotate(transform.forward, Random.Range(-10f, 10f));
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(transform.position + transform.up * Time.deltaTime * _speed);

        if (_isBusy)
        {
            if (_toHomeList.Count > 0)
            {
                _mostIntensiveToHome = _toHomeList[0];
                foreach (MarkerController marker in _toHomeList)
                {
                    if (marker.Intensivity > _mostIntensiveToHome.Intensivity)
                        _mostIntensiveToHome = marker;
                }
            }
            else _mostIntensiveToHome = null;
        }
        else
        {
            if (_toFoodList.Count > 0)
            {
                _mostIntensiveToFood = _toFoodList[0];
                foreach (MarkerController marker in _toFoodList)
                {
                    if (marker.Intensivity > _mostIntensiveToFood.Intensivity)
                        _mostIntensiveToFood = marker;
                }
            }
            else _mostIntensiveToFood = null;
        }
    }


    public void OnFoodFound(Collider2D other)
    {
        if (!_isBusy)
        {

            _targetPos = other.transform.position;
            _goToTargetPos = true;
        }

    }


    public void OnFoodTouched(Collider2D other)
    {
        if (!_isBusy)
        {
            _goToTargetPos = false;
            _isBusy = true;
            _markersIntensivity = 1f;
            _mostIntensiveToHome = null;
            _markersAmount = _startMarkersAmount;
            _toHomeList.Clear();

            Destroy(other.gameObject);
        }

        transform.Rotate(transform.forward, 180f);
    }


    public void OnHomeFound(Collider2D other)
    {
        if (_isBusy)
        {
            _targetPos = other.transform.position;
            _goToTargetPos = true;
        }
    }


    public void OnHomeTouched(Collider2D other)
    {
        if (_isBusy)
        {
            _goToTargetPos = false;
            _isBusy = false;
            _markersIntensivity = 1f;
            _markersAmount = _startMarkersAmount;
            _mostIntensiveToFood = null;
            _toFoodList.Clear();
        }
        transform.Rotate(transform.forward, Random.Range(150f, 210f));
    }


    public void OnMarkerFound(Collider2D other)
    {
        MarkerController marker = other.GetComponent<MarkerController>();

        if (_isBusy && other.CompareTag("ToHome")) 
            _toHomeList.Add(marker);

        else if (!_isBusy && other.CompareTag("ToFood")) 
            _toFoodList.Add(marker);
    }


    public void OnMarkerLost(Collider2D other)
    {
        MarkerController marker = other.GetComponent<MarkerController>();

        if (_isBusy && other.CompareTag("ToHome")) 
            _toHomeList.Remove(marker);

        else if (!_isBusy && other.CompareTag("ToFood")) 
            _toFoodList.Remove(marker);
    }

    public void OnWallCollide()
    {
        //Debug.Log(true);
        _rb.MovePosition(transform.position * .99f);
        transform.Rotate(transform.forward, Random.Range(170f, 195f));
    }

}
