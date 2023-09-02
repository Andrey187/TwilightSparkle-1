using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

namespace DamageNumber
{
    public abstract class DamageNumberPoolBase <T, U> : MonoCache where T : Component, IDamageNumber where U : IList<T>, new()
    {
        [SerializeField] protected Transform _canvasTransform;
        [SerializeField] protected TextDamage<T> _textMeshPrefab;
        [SerializeField] protected string _containerName;

        protected int _poolObjectCount = 1;
        protected PoolObject<T> _textPool;
        protected TextMeshProUGUI _textPrefab;
        protected IObjectFactory objectFactory;

        protected U _damageNumberList = new U();
        [Inject] private DiContainer _diContainer;

        protected virtual void Start()
        {
            _textPrefab = _textMeshPrefab.GetComponent<TextMeshProUGUI>();
            SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
            Invoke("InitPool", 1f);
        }
        protected abstract void UnsubscribeEvents();

        protected virtual void InitPool()
        {
            objectFactory = new ObjectsFactory(_textPrefab.transform);
                
            // Add the bots to the List
            for (int j = 0; j < 500; j++)
            {
                T text = objectFactory.CreateObject(Vector3.zero).GetComponent<T>();
                _damageNumberList.Add(text);
            }

            PoolObject<T>.CreateInstance(_damageNumberList.ToList(),
               _canvasTransform, _containerName, _diContainer);
            _textPool = PoolObject<T>.Instance;
        }

        protected abstract Color Color(object ability);

        protected internal abstract void InitializeGetObjectFromPool(int damageAmount, Transform target, object ability);
        
        protected T GetObjectFromPool(int damageAmount, Transform target, object ability)
        {
            TextMeshProUGUI text = GetTextForAbility(target);
            text.SetText(damageAmount.ToString());

            T damageNumberType = text.GetComponent<T>();

            if (damageNumberType != null)
            {
                if (damageAmount != 0)
                {
                    text.color = Color(ability);
                }
            }
            return damageNumberType;
        }


        protected TextMeshProUGUI GetTextForAbility(Transform target)
        {
            return GetCachedComponent<TextMeshProUGUI>(_textPool.GetObjects(target.position, _damageNumberList.ToArray()));
        }

        protected TComponent GetCachedComponent<TComponent>(Component component) where TComponent : Component
        {
            TComponent cachedComponent = component as TComponent;
            if (cachedComponent == null)
            {
                cachedComponent = component.GetComponent<TComponent>();
            }

            return cachedComponent;
        }

        protected abstract void InitializeReturnToPool(T component);

        protected void ReturnToPool(T component)
        {
            _textPool.ReturnObject(component);
        }
    }
}
