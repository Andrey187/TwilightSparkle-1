using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticleSystemPool : MonoBehaviour
{
    [SerializeField] private List<BaseParcticle> _baseParcticle;
    private PoolObject<BaseParcticle> _particlePool;
    private IObjectFactory _objectFactory;
    private Dictionary<ParticleData.ParticleType, List<BaseParcticle>> _particleDictionary = new Dictionary<ParticleData.ParticleType, List<BaseParcticle>>();

    private void Start()
    {
        ParticleEventManager.Instance.OnDeathParticleSetActive += DeathParticleInvoke;
        ParticleEventManager.Instance.OnHealParticleSetActive += HealParticleInvoke;

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
        PoolObject<BaseParcticle>.CreateInstance(allObjects, 0, gameObject.transform, "Particle");
        _particlePool = PoolObject<BaseParcticle>.Instance;

    }

    private void OnDisable()
    {
        _particleDictionary.Clear();
    }

    public void DeathParticleInvoke(GameObject obj)
    {
        Vector3 position = new Vector3(obj.transform.position.x, 0f, obj.transform.position.z);
        if (_particleDictionary.TryGetValue(ParticleData.ParticleType.Death, out List<BaseParcticle> particleList))
        {
            ParticleDeath deathParticle = particleList.FirstOrDefault(p => p is ParticleDeath) as ParticleDeath;

            if (deathParticle != null)
            {
                BaseParcticle particleInstance = _particlePool.GetObjects(position, deathParticle);
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
                BaseParcticle particleInstance = _particlePool.GetObjects(position, healParticle);
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
