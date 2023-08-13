using UnityEngine;
using UnityEngine.UI;

public class AutoClickButton : MonoBehaviour
{
    [SerializeField] private Button _autoButton;

    public bool AutoEnable = false;

    private void Start()
    {
        _autoButton.interactable = false;
        _autoButton.onClick.AddListener(() => AutoClickEnable());

        PlayerEventManager.Instance.PlayerLevelUp += ButtonInteractable;
        SceneReloadEvent.Instance.UnsubscribeEvents.AddListener(UnsubscribeEvents);
    }

    protected void UnsubscribeEvents()
    {
        PlayerEventManager.Instance.PlayerLevelUp -= ButtonInteractable;
    }

    private void ButtonInteractable(int value)
    {
        if(value == 15)
        {
            _autoButton.interactable = true;
            PlayerEventManager.Instance.PlayerLevelUp -= ButtonInteractable;
        }
    }

    public void AutoClickEnable()
    {
        AutoEnable = true;
        _autoButton.interactable = false;

        // Change the disabled color of the button
        ColorBlock colors = _autoButton.colors;
        colors.disabledColor = Color.green;
        _autoButton.colors = colors;
    }
}
