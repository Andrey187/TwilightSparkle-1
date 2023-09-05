using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ParticleSystemPool : MonoBehaviour
{
    [SerializeField] private List<BaseParcticle> _baseParcticle;
    [SerializeField] private bool _autoExpand;
    private PoolObject<BaseParcticle> _particlePool;
    private IObjectFactory _objectFactory;
    private Dictionary<ParticleData.ParticleType, List<BaseParcticle>> _particleDictionary = new Dictionary<ParticleData.ParticleType, List<BaseParcticle>>();

    [Inject] private DiContainer _diContainer;
    private void Start()
    {
        ParticleEventManager.Instance.OnDeathParticleSetActive += DeathParticleInvoke;
        ParticleEventManager.Instance.OnHealParticleSetActive += HealParticleInvoke;


        Invoke("InitPool", 3f);
    }

    private void OnDisable()
    {
        _particleDictionary.Clear();
    }

    private void InitPool()
    {
        for (int i = 0; i < _baseParcticle.Count; i++)
        {
            _objectFactory = new ObjectsFactory(_baseParcticle[i].gameObject.transform);

            ParticleData.ParticleType type = _baseParcticle[i].ParticleType;

            // Check if the key already exists in the dictionary
            if (!_particleDictionary.ContainsKey(type))
            {
                _particleDictionary[type] = new List<BaseParcticle>();
            }

            for (int j = 0; j < 50; j++)
            {
                BaseParcticle particle = _objectFactory.CreateObject(Vector3.zero).GetComponent<BaseParcticle>();
                _particleDictionary[type].Add(particle);
            }
        }
        List<BaseParcticle> allObjects = _particleDictionary.SelectMany(pair => pair.Value).ToList();
        PoolObject<BaseParcticle>.CreateInstance(allObjects, gameObject.transform, "Particle", _diContainer);
        _particlePool = PoolObject<BaseParcticle>.Instance;
    }

    public void DeathParticleInvoke(GameObject obj)
    {
        Vector3 position = new Vector3(obj.transform.position.x, 0f, obj.transform.position.z);
        if (_particleDictionary.TryGetValue(ParticleData.ParticleType.Death, out List<BaseParcticle> particleList))
        {
            ParticleDeath deathParticle = particleList.FirstOrDefault(p => p is ParticleDeath) as ParticleDeath;

            if (deathParticle != null)
            {
                BaseParcticle particleInstance = _particlePool.GetObjects(position, deathParticle, _autoExpand);
                StartCoroutine(ReturnToPoolAfterDelay(particleInstance, 1.5f));
            }
        }
    }

    public void HealParticleInvoke(GameObject obj)
    {
        Vector3 position = new Vector3(obj.transform.position.x, 0f, obj.transform.position.z);
        if (_particleDictionary.TryGetValue(ParticleData.ParticleType.Heal, out List<BaseParcticle> particleList))
        {
            ParticleHeal healParticle = particleList.FirstOrDefault(p => p is ParticleHeal) as ParticleHeal;

            if (healParticle != null)
            {
                BaseParcticle particleInstance = _particlePool.GetObjects(position, healParticle, _autoExpand);
                StartCoroutine(ReturnToPoolAfterDelay(particleInstance, 1.5f));
            }
        }
    }


    private IEnumerator ReturnToPoolAfterDelay(BaseParcticle obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        ReturnToPool(obj);
    }

    private void ReturnToPool(BaseParcticle obj)
    {
        _particlePool.ReturnObject(obj);
    }
}
