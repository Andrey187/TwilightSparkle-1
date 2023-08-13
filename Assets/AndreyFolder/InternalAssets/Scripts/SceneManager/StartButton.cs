using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button _button;
    private float _lastExecutionTime;

    private void Start()
    {
        _button = gameObject.GetComponent<Button>();
        _button.interactable = false;
    }

    private void Update()
    {
        float currentTime = Time.time;
        if (currentTime - _lastExecutionTime >= 0.5f)
        {
            _lastExecutionTime = currentTime;
            if (CacheCamera.Instance._virtualCamera != null)
            {
                _button.interactable = true;
            }
        }
    }

    public void OnStartButtonClicked()
    {
        LoadScene.LoadSceneStart("LoadScene",
            "AndreyTestScene", Sound.SoundEnum.BackgroundMusic, 0.01f);
    }
}
