using TMPro;
using UnityEngine;

public class Counters : MonoCache
{
    [SerializeField] private float _fps;
    [SerializeField] private TextMeshProUGUI _fpsText;
    [SerializeField] private TextMeshProUGUI _enemyText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private RenderingManager _renderingManager;

    private float elapsedTime;
    private bool isTimerRunning;

    private void Start()
    {
        Application.targetFrameRate = 60;
        ResetTimer();
    }

    protected override void OnEnabled()
    {
        StartTimer();
    }

    protected override void OnDisabled()
    {
        StopTimer();
    }

    protected override void Run()
    {
        _fps = 1.0f / Time.deltaTime;
        _fpsText.text = "FPS: " + (int)_fps;

        _enemyText.text = "Enemy Count: " + _renderingManager.enemyObjectsRenderer.Count;

        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
