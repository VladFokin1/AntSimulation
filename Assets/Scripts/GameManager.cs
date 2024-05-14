using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _antPrefab;
    [SerializeField] int _antsNumOnStart;
    [SerializeField] Slider _antsSlider;
    [SerializeField] TMPro.TMP_InputField _antsInputField;
    [SerializeField] Button _startButton;
    [SerializeField] GameObject _homeObj;
    [SerializeField] GameObject _foodPrefab;


    private bool _isRunning = false;
    private ObjectPool pool;

    private int _currentNumOfAnts;

    private void Awake()
    {
        
        _antsInputField.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(); });
        _antsSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
        _startButton.onClick.AddListener(delegate { OnButtonClick(); });

        _antsSlider.value = _antsNumOnStart;
        _antsInputField.text = _antsNumOnStart.ToString();
        pool = new ObjectPool(_antPrefab, _antsNumOnStart);

    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !IsMouseOverUI() && !_isRunning)
            OnMouseClick();
    }

    private void OnSliderValueChanged()
    {
        _antsInputField.text = _antsSlider.value.ToString();
        _currentNumOfAnts = (int)_antsSlider.value;
    }

    private void OnInputFieldValueChanged()
    {
        _antsSlider.value = Int32.Parse(_antsInputField.text);
        _currentNumOfAnts = Int32.Parse(_antsInputField.text);
    }

    private void OnButtonClick()
    {
        if (!_isRunning)
        {
            _startButton.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Stop";
            for (int i = 0; i < _currentNumOfAnts; i++)
            {
                GameObject ant = pool.Get();
                ant.transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
            }
                
            _isRunning = true;
        }
        else
        {
            _startButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Start";
            pool.ReleaseAll();
            _isRunning = false;
        }
        
    }

    private void OnMouseClick()
    {
        Vector3 mouse = Input.mousePosition;
        Vector3 mouseInGame = Camera.main.ScreenToWorldPoint(mouse);
        mouseInGame.z = -0.5f;
        Instantiate(_foodPrefab, mouseInGame, Quaternion.identity);
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
