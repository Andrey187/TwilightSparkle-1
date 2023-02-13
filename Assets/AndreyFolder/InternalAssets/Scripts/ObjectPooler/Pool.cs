using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    public Transform Container { get; private set; }

    public Queue<GameObject> Objects; // используем очередь, а не список по той причине,
                                      // что когда берем объект из очереди он автоматически из неё удаляется



    public Pool(Transform container)
    {
        Container = container;
        Objects = new Queue<GameObject>();
    }
}
