using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayerMask;
    [SerializeField] private FlameThrowerTimer _flameThrowerTimer;
   
    private void OnTriggerEnter(Collider other)
    {
        ButtonActivate(other);
    }

    private void OnTriggerStay(Collider other)
    {
        ButtonActivate(other);
    }

    private void ButtonActivate(Collider other)
    {
        if (_targetLayerMask == (_targetLayerMask | (1 << other.gameObject.layer)))
        {
            if (_flameThrowerTimer.FillAmount >= 1)
            {
                _flameThrowerTimer._abilityButton.interactable = true;
            }
        }
    }
}
