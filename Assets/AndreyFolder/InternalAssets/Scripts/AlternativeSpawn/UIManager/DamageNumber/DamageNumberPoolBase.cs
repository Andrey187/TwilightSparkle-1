using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace DamageNumber
{
    public abstract class DamageNumberPoolBase<T, U> : MonoCache where T : Component, IDamageNumber where U : IList<T>, new()
    {
        [SerializeField] protected Transform CanvasTransform;
        [SerializeField] protected TextMeshPro TextMeshPrefab;
        [SerializeField] protected string _containerName;

        protected Dictionary<object, U> _damageNumbersDictionary;
        protected int _poolObjectCount = 1;
        protected PoolObject<T> _textPool;
        protected TextMeshPro _textPrefab;
        protected IObjectFactory objectFactory;
        

        protected virtual void Start()
        {
            _textPrefab = TextMeshPrefab.GetComponent<TextMeshPro>();
            objectFactory = new ObjectsFactory(_textPrefab.transform);

            _damageNumbersDictionary = new Dictionary<object, U>();

            InitPool();
        }
        protected virtual void InitPool()
        {
            U _damageNumberList = new U();

            PoolObject<T>.CreateInstance(_damageNumberList.ToList(), _poolObjectCount,
               CanvasTransform, _containerName);
            _textPool = PoolObject<T>.Instance;
        }

        protected abstract Color Color(object ability);

        protected internal virtual void Initialize(int damageAmount, Transform target, object ability)
        {
            TextMeshPro text = GetTextForAbility(target, ability);
            text.SetText(damageAmount.ToString());

            T damageNumberType = text.GetComponent<T>();

            if(damageNumberType != null)
            {
                if (damageAmount != 0)
                {
                    text.color = Color(ability);
                }

                StartCoroutine(Duration(text, damageNumberType));
            }
            else
            {
                Debug.LogError($"{damageNumberType} : component not found on the TextMeshPro prefab.");
            }
            
        }

        protected TextMeshPro GetTextForAbility(Transform target, object ability)
        {
            if (!_damageNumbersDictionary.TryGetValue(ability, out U _damageNumberList))
            {
                _damageNumberList = new U();
                for (int i = 0; i < _poolObjectCount; i++)
                {
                    T textPro = objectFactory.CreateObject(Vector3.zero).GetComponent<T>();
                    _damageNumberList.Add(textPro);
                }
                _damageNumbersDictionary.Add(ability, _damageNumberList);
            }
            return GetCachedComponent<TextMeshPro>(_textPool.GetObjects(target.position, _damageNumberList.ToArray()));
        }

        protected IEnumerator Duration(TextMeshPro text, T prefab)
        {
            yield return new WaitForSeconds(prefab.LifeTime);
            _textPool.ReturnObject(GetCachedComponent<T>(text));
        }

        protected TComponent GetCachedComponent<TComponent>(Component component) where TComponent : Component
        {
            if (component == null)
            {
                Debug.LogError("Component is null.");
                return null;
            }

            TComponent cachedComponent = component as TComponent;
            if (cachedComponent == null)
            {
                cachedComponent = component.GetComponent<TComponent>();
            }

            if (cachedComponent == null)
            {
                Debug.LogError($"Component of type {typeof(TComponent)} not found on the object: {component.name}");
            }

            return cachedComponent;
        }
    }
}
