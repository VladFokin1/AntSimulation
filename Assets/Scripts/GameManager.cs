using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _antPrefab;
    [SerializeField] int _antsNumOnStart;
    [SerializeField] Slider _antsSlider;
    [SerializeField] TMPro.TMP_InputField _antsInputField;
    [SerializeField] Button _startButton;
    [SerializeField] GameObject _home;

    private int _currentNumOfAnts;

    private void Awake()
    {
        _antsSlider.value = _antsNumOnStart;
        _antsInputField.text = _antsNumOnStart.ToString();
        _antsInputField.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(); });
        _antsSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
        _startButton.onClick.AddListener(delegate { OnStartButtonClick(); });
    }



    // Update is called once per frame
    void Update()
    {
        
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

    private void OnStartButtonClick()
    {
        for (int i = 0; i < _currentNumOfAnts; i++)
            Instantiate(_antPrefab, _home.transform.position, Quaternion.identity);
    }
}
