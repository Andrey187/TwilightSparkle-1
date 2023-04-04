using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPooler : MonoCache
{
    public static ObjectPooler Instance;
    [SerializeField] private StageData _stageData;
    private Dictionary<StageEvent.ObjectType, Pool> _pools; //������ � ������� �� ����� ������ ����� �������, 
                                                            // ��� ���������� ������ � ����������� � ������� �������� ��� �������������.

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        InitPool();
    }

    private void InitPool()
    {
        _pools = new Dictionary<StageEvent.ObjectType, Pool>();

        var emtyGO = new GameObject();

        foreach (var obj in _stageData.ListStageEvent)
        {
            var container = Instantiate(emtyGO, transform, false);
            container.name = obj.Type.ToString();

            _pools[obj.Type] = new Pool(container.transform);

            for(int i =0; i< obj.SpawnCount; i++)
            {
                var go = InstantiateObject(obj.Type, container.transform);
                _pools[obj.Type].Objects.Enqueue(go); //Enqueue ��������� � �������
            }
        }

        Destroy(emtyGO);
    }

    int a;
    private GameObject InstantiateObject(StageEvent.ObjectType type, Transform parent)
    {
        var go = Instantiate(_stageData.ListStageEvent.Find(x => x.Type == type).Prefab, parent);
        go.name = go.name + a; a++;
        go.SetActive(false);
        return go;
    }
    
    /// <summary>
    /// ����� ��� ��������� �������. ���������, ���� �� ������ � ������� ���� � ����� ���, ���� ���, �� ������� �����
    /// </summary>
    public GameObject GetObject(StageEvent.ObjectType type)
    {
        var obj = _pools[type].Objects.Count > 0 ?
            _pools[type].Objects.Dequeue() : InstantiateObject(type, _pools[type].Container); //Dequeue ������� �� �������
        obj.SetActive(true);
        return obj;
    }

    /// <summary>
    /// ����� ����������� �������
    /// </summary>
    public void DestroyObject(GameObject obj)
    {
        foreach (var go in _stageData.ListStageEvent)
        {
            _pools[go.Type].Objects.Enqueue(obj);
            obj.SetActive(false);
        }
    }
}
