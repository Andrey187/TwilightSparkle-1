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

        for (int i = 0; i < _baseParcticle.Count; i++)
        {
            _objectFactory = new ObjectsFactory(_baseParcticle[i].gameObject.transform);
            BaseParcticle particle = _objectFactory.CreateObject(Vector3.zero).GetComponent<BaseParcticle>();

            ParticleData.ParticleType type = _baseParcticle[i].ParticleType;

            // Check if the key already exists in the dictionary
            if (!_particleDictionary.ContainsKey(type))
            {
                _particleDictionary[type] = new List<BaseParcticle>();
            }

            for (int j = 0; j < 100; j++)
            {
                _particleDictionary[type].Add(particle);
            }
        }
        BaseParcticle[] objects = _particleDictionary.SelectMany(pair => pair.Value).ToArray();
        PoolObject<BaseParcticle>.CreateInstance(objects, 0, gameObject.transform, "Particle");
        _particlePool = PoolObject<BaseParcticle>.Instance;
    }

    public void DeathParticleInvoke(GameObject obj)
    {
        if (_particleDictionary.TryGetValue(ParticleData.ParticleType.Death, out List<BaseParcticle> particleList))
        {
            BaseParcticle deathParticle = _particlePool.GetObjects(obj.transform.position, particleList.FirstOrDefault());

            StartCoroutine(ReturnToPoolAfterDelay(deathParticle.gameObject, 1.5f));
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        ReturnToPool(obj);
    }

    private void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
