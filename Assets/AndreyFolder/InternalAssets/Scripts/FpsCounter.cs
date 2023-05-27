using TMPro;
using UnityEngine;

public class FpsCounter : MonoCache
{
    [SerializeField] private float _fps;
    [SerializeField] private TextMeshProUGUI _fpsText;

    protected override void Run()
    {
        _fps = 1.0f / Time.deltaTime;
        _fpsText.text = "FPS: " + (int)_fps;
    }
}
