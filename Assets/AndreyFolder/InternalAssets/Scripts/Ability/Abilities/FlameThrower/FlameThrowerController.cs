using UnityEngine;

public class FlameThrowerController : MonoCache
{
    [SerializeField] private GameObject _flameThrower;
    
    public void ActivateFlameThrower()
    {
        _flameThrower.SetActive(true);
    }
}
