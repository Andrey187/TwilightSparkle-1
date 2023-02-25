using UnityEngine;

public class BotsLifeTime : MonoCache, IPooledObjects
{
    public StageEvent.ObjectType Type => type;

    [SerializeField]
    private StageEvent.ObjectType type;

    //временные парамы для теста
    [SerializeField] private float _lifeTime = 3;
    [SerializeField] private float _currentLifeTime;
    //[SerializeField] private float _speed = 1f;
    //[SerializeField] private Transform _targetDestination;
    //[SerializeField] private Rigidbody _rb;

    //private void Awake()
    //{
    //    _rb = ChildrenGet<Rigidbody>();
    //    _targetDestination = GameObject.FindGameObjectWithTag("Player").transform;
    //}

    public void OnCreate(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        _currentLifeTime = _lifeTime;
    }

    protected override void Run()
    {
        if ((_currentLifeTime -= Time.deltaTime) < 0)
        {
            ObjectPooler.Instance.DestroyObject(gameObject);
        }
    }

    //protected override void FixedRun()
    //{
    //    MoveToPlayer();
    //}


    //private void MoveToPlayer()
    //{
    //    //Vector3 parentTransform = new Vector3(transform.position.x, 0f, transform.position.z);
    //    //transform.position = Vector3.MoveTowards(parentTransform,
    //    //    ObjectPooler.Instance._player.transform.position, _speed * Time.deltaTime);

    //    Vector3 direction = (_targetDestination.position - transform.position).normalized;
    //    _rb.velocity = direction * _speed * Time.deltaTime;
    //}
}
