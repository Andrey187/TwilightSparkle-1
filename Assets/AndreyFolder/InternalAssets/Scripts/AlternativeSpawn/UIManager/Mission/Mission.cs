using System.Collections;
using UnityEngine;

public class Mission : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    // Start is called before the first frame update
    private void Start()
    {
        _animator.enabled = false;
        StartCoroutine(ActivateAnimation());
    }

    private IEnumerator ActivateAnimation()
    {
        yield return new WaitForSeconds(2f);
        _animator.enabled = true;

        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
