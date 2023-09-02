using System;
using UnityEngine;
using System.Threading.Tasks;
public class FireBallAbility: BaseAbilities
{
    [SerializeField] private LayerMask _enemyLayer;
    private FireBall _fireBall;
    private FireDoTEffect _fireDotEffect;
    private Collider[] _hitColliders;
    private float _lastExecutionTime;
    private const int _maxCollidersToProcess = 30;
    protected internal override event Action<BaseAbilities> SetDie;

    protected internal event Action SetCreate;

    protected override event Action<BaseEnemy, int, IAbility, IDoTEffect> _setDamage;
    protected override float LastExecutionTime { get=> _lastExecutionTime; set=> _lastExecutionTime = value; }

    protected override void OnEnabled()
    {
        base.OnEnabled();

        //_isActive = true;
        //LifeAsync();
    }

    protected override void OnDisabled()
    {
        base.OnDisabled();
        //_isActive = false;
    }

    private void Start()
    {
        _fireBall = FireBall.Instance;
        _fireDotEffect = new FireDoTEffect();
        _setDamage = AbilityEventManager.Instance.AbillityDamage;
        _fireBall.CurrentAbility = _fireBall;
        _fireBall.DoTEffect = _fireDotEffect;
    }

    private bool _isActive = false;

    //private async void LifeAsync()
    //{
    //    await BeginTest();
    //}

    //private async Task BeginTest()
    //{
    //    while (_isActive)
    //    {
    //        UnityEngine.Profiling.Profiler.BeginSample("FireBall Loop");

    //        PerformDamageCheck();

    //        UnityEngine.Profiling.Profiler.EndSample();
    //        await Task.Delay(500); // await 0.5s
    //    }
    //}

    //protected override void PerformDamageCheck()
    //{
    //    if (LifeTime <= 0)
    //    {
    //        return; // Exit the method early without processing collisions
    //    }

    //    RaycastHit[] hits = Physics.SphereCastAll(transform.position, damageRadius, Vector3.forward, 0f, _enemyLayer);

    //    foreach (RaycastHit hit in hits)
    //    {
    //        Collider hitCollider = hit.collider;
    //        // Assuming enemies have a BaseEnemy component
    //        _baseEnemy = hitCollider.GetComponent<BaseEnemy>();

    //        if (_baseEnemy != null)
    //        {
    //            // Apply damage to the enemy
    //            _setDamage?.Invoke(_baseEnemy, _fireBall.Damage, _fireBall.CurrentAbility, _fireBall.DoTEffect);
    //        }

    //        if ((_layerMaskForDie.value & (1 << hitCollider.transform.gameObject.layer)) > 0)
    //        {
    //            // If the collided object has the specified layer, invoke SetDie
    //            SetDie?.Invoke(this);
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        UnityEngine.Profiling.Profiler.BeginSample("FireBall Loop");

        int otherLayer = other.gameObject.layer;

        if ((_enemyLayer.value & (1 << otherLayer)) > 0) // enemyLayer is the layer of your enemies
        {
            // Apply damage to the enemy
            _setDamage?.Invoke(other.GetComponent<BaseEnemy>(), _fireBall.Damage, _fireBall.CurrentAbility, _fireBall.DoTEffect);
        }

        if ((_layerMaskForDie.value & (1 << other.gameObject.layer)) > 0)
        {
            // If the collided object has the specified layer, invoke SetDie
            SetDie?.Invoke(this);
        }
        UnityEngine.Profiling.Profiler.EndSample();
    }

    protected internal override void RaiseSetCreateEvent()
    {
        SetCreate?.Invoke();
    }
}