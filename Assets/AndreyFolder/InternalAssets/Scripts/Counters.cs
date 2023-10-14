using System.ComponentModel;
using TMPro;
using UnityEngine;
using Zenject;

public class Counters : MonoCache, ICounters
{
    [SerializeField] private float _fps;
    [SerializeField] private TextMeshProUGUI _fpsText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private CameraArea _cameraArea;

    public int _totalKilledEnemy = 0;
    private int _killedEnemy = 0;
    private int _minutes;
    private int _seconds;
    private float elapsedTime;
    private bool isTimerRunning;
    [Inject] private IGamePause _gamePause;

    public event PropertyChangedEventHandler PropertyChanged;

    public int KilledEnemy 
    {   get => _killedEnemy; 
        set
        {
            if (_killedEnemy != value)
            {
                _killedEnemy = value;
            }
        }
    }

    public float Timer => _minutes;

    public float SecTimer => _seconds;

    private void Awake()
    {
        EnemyEventManager.Instance.ObjectDie += EnemyDie;
        Application.targetFrameRate = 90;
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
            if (_gamePause.IsPaused)
                return;

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
        _minutes = Mathf.FloorToInt(elapsedTime / 60f);
        _seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", _minutes, _seconds);
    }

    private void EnemyDie(GameObject obj)
    {
        if (!obj.activeSelf) 
        {
            _killedEnemy++; _totalKilledEnemy++;
            OnPropertyChanged(nameof(KilledEnemy));
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
