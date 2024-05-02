using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerController : MonoBehaviour
{

    [SerializeField] float _burnRatePerSec = .9f;
    [SerializeField] float _intensivityLimit = .05f;
    private float _lifeCount = 0;
    private SpriteRenderer _spriteRenderer;


    private float _intensivity = 1;
    public float Intensivity { get { return _intensivity; } set { _intensivity = value; } }

    // Start is called before the first frame update
    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _lifeCount += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        

        if (_lifeCount > 1)
        {
            _lifeCount = 0;
            Refresh();
        }

        if (_intensivity < _intensivityLimit) Destroy(this.gameObject);

    }

    public void Refresh()
    {
        _intensivity *= _burnRatePerSec;
        Color oldColor = _spriteRenderer.color;
        _spriteRenderer.color = new Color(oldColor.r, oldColor.g, oldColor.b, _intensivity);
        Debug.Log(_spriteRenderer.color.a);
    }
}
