using System.Collections;
using UnityEngine;

public class KnockbackMechanic : MonoBehaviour, IKnockback
{
    [SerializeField] private float _pushbackDistance = 0.1f;
    [SerializeField] private float _pushbackDuration = 0.3f;
    private bool _isBeingPushed = false;
    private Vector3 _pushbackDirection;
    
    public void KnockBack(GameObject gameObject, Transform transform)
    {
        // Start a coroutine to move the character towards the destination
        if (gameObject.activeSelf)
        {
            if (!_isBeingPushed)
            {
                _pushbackDirection = -transform.forward;
                StartCoroutine(MoveToDestination(transform));
            }
        }
    }

    private IEnumerator MoveToDestination(Transform transform)
    {
        _isBeingPushed = true;
        float timer = 0f;

        while (timer < _pushbackDuration)
        {
            if (Time.timeScale > 0f)
            {
                float distance = Mathf.Lerp(0f, _pushbackDistance, timer / _pushbackDuration);
                transform.position += _pushbackDirection * distance;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        _isBeingPushed = false;
    }
}
