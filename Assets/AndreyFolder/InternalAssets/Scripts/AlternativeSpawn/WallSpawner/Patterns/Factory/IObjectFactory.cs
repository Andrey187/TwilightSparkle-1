using UnityEngine;

public interface IObjectFactory
{
    Transform[] CreateObjects(Vector3 pos, int count);

    Transform CreateObject(Vector3 pos);
}
