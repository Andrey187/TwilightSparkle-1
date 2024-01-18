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
    private Dictionary<ParticleData.ParticleType, List<BaseParcticle>> _particleDictionary = new Dictionary<ParticleData.ParticleType, List<BaseParcticle>>();

    [Inject] private DiContainer _diContainer;
    private void Start()
    {
        ParticleEventManager.Instance.OnDeathParticleSetActive += ParticleInvoke;
        ParticleEventManager.Instance.OnHealParticleSetActive += ParticleInvoke;
        ParticleEventManager.Instance.OnParticlePortalSetActive += ParticleInvoke;

        SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
        Invoke("InitPool", 3f);
    }

    private void UnsubscribeEvents()
    {
        _particleDictionary.Clear();
        ParticleEventManager.Instance.OnDeathParticleSetActive -= ParticleInvoke;
        ParticleEventManager.Instance.OnHealParticleSetActive -= ParticleInvoke;
        ParticleEventManager.Instance.OnParticlePortalSetActive -= ParticleInvoke;
    }

    private void InitPool()
    {
        for (int i = 0; i < _baseParcticle.Count; i++)
        {
            ParticleData.ParticleType type = _baseParcticle[i].ParticleType;

            // Check if the key already exists in the dictionary
            if (!_particleDictionary.ContainsKey(type))
            {
                _particleDictionary[type] = new List<BaseParcticle>();
            }

            for (int j = 0; j < 50; j++)
            {
                _particleDictionary[type].Add(_baseParcticle[i]);
            }
        }
        List<BaseParcticle> allObjects = _particleDictionary.SelectMany(pair => pair.Value).ToList();
        PoolObject<BaseParcticle>.CreateInstance(allObjects, gameObject.transform, "Particle", _diContainer);
        _particlePool = PoolObject<BaseParcticle>.Instance;
    }
    private void ParticleInvoke(GameObject obj, ParticleData.ParticleType particleType)
    {
        Vector3 position = new Vector3(obj.transform.position.x, 0f, obj.transform.position.z);
        if (_particleDictionary.TryGetValue(particleType, out List<BaseParcticle> particleList))
        {
            BaseParcticle particle = particleList.FirstOrDefault(p => p.GetType() == p.GetParticleType(particleType));

            if (particle != null)
            {
                BaseParcticle particleInstance = _particlePool.GetObjects(position, particle, _autoExpand);
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
