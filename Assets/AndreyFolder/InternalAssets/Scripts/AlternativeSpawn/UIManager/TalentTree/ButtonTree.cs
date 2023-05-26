using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTree : MonoCache
{
    [SerializeField] private GameObject _canvasTree;
    private Button _button;

    private void Start()
    {
        _button = Get<Button>();
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        // Trigger the AddAttackScript method of the AttackScript class
        if (_canvasTree.activeSelf != true) { _canvasTree.SetActive(true); }
        else if(_canvasTree.activeSelf == true) { _canvasTree.SetActive(false); }
    }
}
