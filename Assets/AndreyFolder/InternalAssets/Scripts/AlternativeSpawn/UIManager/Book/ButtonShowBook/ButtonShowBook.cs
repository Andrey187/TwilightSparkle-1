using UnityEngine;
using UnityEngine.UI;

public class ButtonShowBook : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _defaultImage;
    [SerializeField] private Image _alternativeImage;
    [SerializeField] private Animator _animator;

    private bool _isBookShown = false;

    private void Start()
    {
        _button.onClick.AddListener(ToggleBook);
    }

    private void ToggleBook()
    {
        if (_isBookShown)
        {
            HideBook();
        }
        else
        {
            ShowBook();
        }
    }

    private void ShowBook()
    {
        _animator.Play("ShowBook");

        _defaultImage.enabled = false;
        _alternativeImage.enabled = true;
        _button.image = _alternativeImage;
        _isBookShown = true;
    }

    private void HideBook()
    {
        _animator.Play("HideBook");

        _alternativeImage.enabled = false;
        _defaultImage.enabled = true;
        _button.image = _defaultImage;
        _isBookShown = false;
    }
}
