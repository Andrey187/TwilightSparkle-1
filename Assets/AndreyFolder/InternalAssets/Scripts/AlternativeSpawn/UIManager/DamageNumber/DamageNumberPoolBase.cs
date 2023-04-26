using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace DamageNumber
{
    public abstract class DamageNumberPoolBase<T, U> : MonoCache where T : Component, IDamageNumber where U : IList<T>, new()
    {
        [SerializeField] public Transform CanvasTransform;
        [SerializeField] public TextMeshPro TextMeshPrefab;
        [SerializeField] private string _containerName;
        protected Dictionary<object, U> _damageNumbers;
        protected int _poolObjectCount = 1;
        protected List<T> _damageNumberList;
        protected PoolObject<T> _textPool;
        protected TextMeshPro _textPrefab;
        protected T _damageNumber;
        protected IObjectFactory objectFactory;

        protected virtual void Start()
        {
            _textPrefab = TextMeshPrefab.GetComponent<TextMeshPro>();
            _damageNumber = TextMeshPrefab.GetComponent<T>();
            objectFactory = new ObjectsFactory(_textPrefab.transform);

            _damageNumbers = new Dictionary<object, U>();

            InitPool();
        }
        protected virtual void InitPool()
        {
            U _damageNumberList = new U();

            PoolObject<T>.CreateInstance(_damageNumberList.ToArray(), _poolObjectCount,
               CanvasTransform, _containerName);
            _textPool = PoolObject<T>.Instance;
        }

        protected abstract Color Color(object ability);

        public void Initialize(int damageAmount, Transform target, object ability)
        {
            TextMeshPro text = GetTextForAbility(target, ability);
            text.SetText(damageAmount.ToString());

            if (damageAmount != 0)
            {
                text.color = Color(ability);
                _damageNumber.NumberReset();
                _damageNumber.Direction = (target.position - target.position).normalized;
                _damageNumber.SetNumberDirection();
            }
            else
            {
                _damageNumber.NumberReset();
            }
            _damageNumber.Direction.Normalize();
            StartCoroutine(Duration(text));
        }

        protected TextMeshPro GetTextForAbility(Transform target, object ability)
        {
            if (!_damageNumbers.TryGetValue(ability, out U _damageNumberList))
            {
                _damageNumberList = new U();
                for (int i = 0; i < _poolObjectCount; i++)
                {
                    T textPro = objectFactory.CreateObject(Vector3.zero).GetComponent<T>();
                    _damageNumberList.Add(textPro);
                }
                _damageNumbers.Add(ability, _damageNumberList);
            }

            TextMeshPro text = _textPool.GetObjects(target.position, _damageNumberList.ToArray()).GetComponent<TextMeshPro>();

            return text;
        }

        protected IEnumerator Duration(TextMeshPro text)
        {
            yield return new WaitForSeconds(_damageNumber.LifeTime);
            _textPool.ReturnObject(text.GetComponent<T>());
            _damageNumber.NumberReset();
        }
    }
}
