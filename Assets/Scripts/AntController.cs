using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntController : MonoBehaviour
{
    [SerializeField] GameObject _toHomeMarker;
    [SerializeField] GameObject _toFoodMarker;
    [SerializeField] float _timeToMarker = .2f;
    [SerializeField] float _markersIntensivityBurnRate = 0.9f;
    [SerializeField] Transform target;
    [SerializeField] float _speed = 10f;

    //privates
    private Rigidbody2D _rb;
    private float _timeFromLastMarker = 0;
    private float _markersIntensivity = 1f;



    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.up = Vector3.Lerp(transform.up, (target.position - transform.position), 0.25f);

        
        _timeFromLastMarker += Time.deltaTime;

        if (_timeFromLastMarker > _timeToMarker)
        {
            _timeFromLastMarker = 0;
            MarkerController marker = Instantiate(_toHomeMarker, transform.position, Quaternion.identity).GetComponent<MarkerController>();
            marker.Intensivity = _markersIntensivity;
            marker.Refresh();
            _markersIntensivity *= _markersIntensivityBurnRate;
        }
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(transform.position + transform.up * Time.deltaTime * _speed);
    }
}
