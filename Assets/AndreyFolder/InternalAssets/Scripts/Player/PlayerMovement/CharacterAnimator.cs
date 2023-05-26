using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimation(string parameterName, bool value)
    {
        animator.SetBool(parameterName, value);
    }
}
