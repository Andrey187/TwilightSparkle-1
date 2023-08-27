using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

namespace DamageNumber
{
    public abstract class DamageNumberPoolBase<T, U> : MonoCache where T : Component, IDamageNumber where U : IList<T>, new()
    {
        [SerializeField] protected Transform CanvasTransform;
        [SerializeField] protected TextMeshProUGUI TextMeshPrefab;
        [SerializeField] protected string _containerName;

        protected Dictionary<object, U> _damageNumbersDictionary;
        protected int _poolObjectCount = 1;
        protected PoolObject<T> _textPool;
        protected TextMeshProUGUI _textPrefab;
        protected IObjectFactory objectFactory;

        protected Dictionary<TextMeshProUGUI, Coroutine> _coroutineDictionary = new Dictionary<TextMeshProUGUI, Coroutine>();
        [Inject] private DiContainer _diContainer;

        protected virtual void Start()
        {
            _textPrefab = TextMeshPrefab.GetComponent<TextMeshProUGUI>();
            objectFactory = new ObjectsFactory(_textPrefab.transform);

            _damageNumbersDictionary = new Dictionary<object, U>();

            InitPool();
        }
        protected virtual void InitPool()
        {
            U _damageNumberList = new U();

            PoolObject<T>.CreateInstance(_damageNumberList.ToList(), _poolObjectCount,
               CanvasTransform, _containerName, _diContainer);
            _textPool = PoolObject<T>.Instance;
        }

        protected abstract Color Color(object ability);

        protected internal virtual void Initialize(int damageAmount, Transform target, object ability)
        {
            TextMeshProUGUI text = GetTextForAbility(target, ability);
            text.SetText(damageAmount.ToString());

            T damageNumberType = text.GetComponent<T>();

            if(damageNumberType != null)
            {
                if (damageAmount != 0)
                {
                    text.color = Color(ability);
                }

                Coroutine coroutine = StartCoroutine(Duration(text, damageNumberType));
                _coroutineDictionary[text] = coroutine;
            }
            else
            {
                Debug.LogError($"{damageNumberType} : component not found on the TextMeshPro prefab.");
            }
            
        }

        protected TextMeshProUGUI GetTextForAbility(Transform target, object ability)
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
            return GetCachedComponent<TextMeshProUGUI>(_textPool.GetObjects(target.position, _damageNumberList.ToArray()));
        }

        protected IEnumerator Duration(TextMeshProUGUI text, T prefab)
        {
            yield return new WaitForSeconds(prefab.LifeTime);
            _textPool.ReturnObject(GetCachedComponent<T>(text));
            if (_coroutineDictionary.TryGetValue(text, out Coroutine coroutine))
            {
                StopCoroutine(coroutine);
                _coroutineDictionary.Remove(text);
            }
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
