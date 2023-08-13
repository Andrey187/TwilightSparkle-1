using UnityEngine;

public class AttentionController : MonoBehaviour
{
    [SerializeField] private GameObject _attentionObject;

    public void ObjectActivate()
    {
        _attentionObject.SetActive(true);
    }

    public void ObjectDeactivate()
    {
        _attentionObject.SetActive(false);
    }
}
