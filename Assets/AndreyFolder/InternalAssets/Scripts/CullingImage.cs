using UnityEngine;

public class CullingImage : ParamsForCalculateSpawnPositions
{
    [SerializeField] private GameObject _healthbarPrefab;
    [SerializeField] private HealthBar _healthBar;

    protected override void Start()
    {
        base.Start();
        _healthbarPrefab = gameObject;
        _healthBar = GetComponent<HealthBar>();
    }
    private float _lastExecutionTime;

    protected override void Run()
    {
        UnityEngine.Profiling.Profiler.BeginSample("CullingImage");
        float currentTime = Time.time;
        if (currentTime - _lastExecutionTime >= 1f)
        {
            _lastExecutionTime = currentTime;
            // �������� ��� ��������� ���������� Image � ����������� �� ���������
            _healthBar.Fill.enabled = IsVisible();
            _healthBar.Border.enabled = IsVisible();
        }
        UnityEngine.Profiling.Profiler.EndSample();
    }

    private bool IsVisible()
    {
        // �������� ������� _player � ������� image
        Vector3 playerPosition = _player.position;
        Vector3 imagePosition = _healthbarPrefab.transform.position;

        // ��������� ���������� ����� _player � image
        float distance = Vector3.Distance(playerPosition, imagePosition);
        // ���� ���������� ������ ��� ����� _circleOutsideTheCameraField, �� image �����
        return distance <= _circleOutsideTheCameraField;
    }
}
