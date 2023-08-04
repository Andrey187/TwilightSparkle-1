using Cinemachine;
using UnityEngine;

public class CacheCamera : MonoBehaviour
{
    public CinemachineVirtualCamera _virtualCamera;
    public float _cameraWidth;
    public float _cameraHeight;

    private static CacheCamera _instance;
    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject("CacheCamera");
            _instance = obj.AddComponent<CacheCamera>();
            DontDestroyOnLoad(obj);
        }
    }

    public static CacheCamera Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CacheCamera>();
            }

            return _instance;
        }
    }

    private void Start()
    {
        Invoke("CacheCameraParams", 3f);
    }

    public void CacheCameraParams()
    {
        _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        CameraState state = _virtualCamera.State;

        // Calculate the camera's display size based on the orthographic size and aspect ratio
        float orthographicSize = state.Lens.OrthographicSize;
        float aspectRatio = state.Lens.Aspect;
        _cameraHeight = 2f * orthographicSize;
        _cameraWidth = _cameraHeight * aspectRatio;
        
    }
}
