using UnityEngine;

public class UIFollowBot : MonoCache
{
    [SerializeField] private Transform _objectToFollow;
    private RectTransform _rectTransform;
    private Canvas _canvas;
    private Camera _camera;

    private void Awake()
    {
        _rectTransform = Get<RectTransform>();
        _canvas = Get<Canvas>();
        _camera = Camera.main;
        _canvas.worldCamera = _camera;
    }

    protected override void Run()
    {
        if (_objectToFollow != null)
            _rectTransform.anchoredPosition = _objectToFollow.localPosition;
    }
}
