using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;
    [Serializable] public struct ObjectInfo //структура или класс не имеет значения, но структура хранится в стеке, по идее должно быстрее работать
    {
        public enum ObjectType
        {
            ENEMY_1,
            ENEMY_2,
            ENEMY_3,
            ENEMY_4,
        }

        public ObjectType Type;
        public GameObject Prefab;
        public int StartCount; //начальное кол-во объектов в пуле
    }

    [SerializeField] private List<ObjectInfo> _objectsInfo;

    private Dictionary<ObjectInfo.ObjectType, Pool> _pools; //Доступ к словарю по ключу всегда будет быстрее, 
                                                            // чем перебирать список и запрашивать у каждого элемента его идентификатор.

    public GameObject _player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        InitPool();

        //ToDo кэш в другом месте
        _player = GameObject.FindGameObjectWithTag("Player");
    }


    private void InitPool()
    {
        _pools = new Dictionary<ObjectInfo.ObjectType, Pool>();

        var emtyGO = new GameObject();

        foreach(var obj in _objectsInfo)
        {
            var container = Instantiate(emtyGO, transform, false);
            container.name = obj.Type.ToString();

            _pools[obj.Type] = new Pool(container.transform);

            for(int i =0; i< obj.StartCount; i++)
            {
                var go = InstantiateObject(obj.Type, container.transform);
                _pools[obj.Type].Objects.Enqueue(go); //Enqueue поставить в очередь
            }
        }

        Destroy(emtyGO);
    }

    private GameObject InstantiateObject(ObjectInfo.ObjectType type, Transform parent)
    {
        var go = Instantiate(_objectsInfo.Find(x => x.Type == type).Prefab, parent);
        go.SetActive(false);
        return go;
    }
    
    /// <summary>
    /// Метод для получения объекта. Проверяем, если ли объект в очереди пула и берем его, если нет, то создаем новый
    /// </summary>
    public GameObject GetObject(ObjectInfo.ObjectType type)
    {
        var obj = _pools[type].Objects.Count > 0 ?
            _pools[type].Objects.Dequeue() : InstantiateObject(type, _pools[type].Container); //Dequeue Удалить из очереди

        obj.SetActive(true);
        return obj;
    }

    /// <summary>
    /// Метод уничтожения объекта
    /// </summary>
    public void DestroyObject(GameObject obj)
    {
        _pools[obj.GetComponent<IPooledObjects>().Type].Objects.Enqueue(obj);
        obj.SetActive(false);
    }
}
