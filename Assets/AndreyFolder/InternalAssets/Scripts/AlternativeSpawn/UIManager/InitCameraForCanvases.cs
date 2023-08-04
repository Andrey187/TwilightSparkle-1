using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitCameraForCanvases : MonoBehaviour
{
    [SerializeField] private List<Canvas> _setCamera;
    [SerializeField] private List<Canvas> _activateCanvas;
    private Camera _camera;

    private void Awake()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Invoke("ActivateUI",0.5f);
    }

    private void ActivateUI()
    {
        foreach (var canvas in _setCamera)
        {
            canvas.worldCamera = _camera;
        }

        foreach(var canvas in _activateCanvas)
        {
            canvas.gameObject.SetActive(true);
        }
    }
}
