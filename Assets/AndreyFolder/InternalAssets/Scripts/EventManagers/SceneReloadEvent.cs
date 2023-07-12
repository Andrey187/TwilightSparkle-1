using UnityEngine;
using UnityEngine.Events;

public class SceneReloadEvent : MonoBehaviour
{
    public UnityEvent UnsubscribeEvents = new UnityEvent();

    private static SceneReloadEvent _instance;

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject("SceneReloadEvent");
            _instance = obj.AddComponent<SceneReloadEvent>();
            DontDestroyOnLoad(obj);
        }
    }

    public static SceneReloadEvent Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SceneReloadEvent>();
            }

            return _instance;
        }
    }

    public void OnUnsubscribeEvents()
    {
        UnsubscribeEvents?.Invoke();
    }
}
