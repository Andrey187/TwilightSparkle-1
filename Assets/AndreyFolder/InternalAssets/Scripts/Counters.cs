using TMPro;
using UnityEngine;

public class Counters : MonoCache
{
    [SerializeField] private float _fps;
    [SerializeField] private TextMeshProUGUI _fpsText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private CameraArea _cameraArea;

    public int _killedEnemy = 0;
    private float elapsedTime;
    private bool isTimerRunning;

    private void Start()
    {
        EnemyEventManager.Instance.ObjectDie += KillEnemy;
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
        CounterUpdate();
    }

    private void CounterUpdate()
    {
        _fps = 1.0f / Time.deltaTime;
        _fpsText.text = "FPS: " + (int)_fps;

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

    private void KillEnemy(GameObject obj)
    {
        if (!obj.activeSelf) 
        {
            _killedEnemy++;
        }
    }
}
