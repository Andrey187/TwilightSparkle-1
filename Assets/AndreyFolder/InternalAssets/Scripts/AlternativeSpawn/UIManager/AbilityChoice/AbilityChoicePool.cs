using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AbilityChoicePool : MonoBehaviour
{
    public event Action<Button> ButtonObtainedFromPool;
    [SerializeField] private AbilityAddWindow _abilityAddWindow;
    [SerializeField] private Transform buttonContainer;

    private List<Button> _shiffleButtons = new List<Button>();
    private List<Button> _instantiatedButtons = new List<Button>();

    private PoolObject<Button> _buttonPool;
    private IObjectFactory _objectFactory;
    private Dictionary<Button, UpgradeAbilitiesOnButtonClick> _buttonInPool;

    [Inject] private DiContainer _diContainer;
    private void Start()
    {
        _buttonInPool = new Dictionary<Button, UpgradeAbilitiesOnButtonClick>();

        Invoke("InintPool", 5f);
        _abilityAddWindow.GetObjectFromPool += GetObjectFromPool;
        _abilityAddWindow.ReturnObjectInPool += ReturnObjectInPool;
    }

    private void InintPool()
    {
        foreach (var button in _abilityAddWindow.AbilityButtons)
        {
            _objectFactory = new ObjectsFactory(button.GetComponent<Button>().transform);
            Button buttons = _objectFactory.CreateObject(button.transform.position).GetComponent<Button>();
            PoolObject<Button>.CreateInstance(buttons, buttonContainer, "Buttons_Container", _diContainer);

            _buttonPool = PoolObject<Button>.Instance;

            Button buttonInstance = _buttonPool.GetObjects(buttons.transform.position, buttons);
            buttonInstance.transform.SetParent(buttonContainer);
            _shiffleButtons.Add(buttonInstance);
            if (buttonInstance.GetComponent<UpgradeAbilitiesOnButtonClick>())
            {
                _buttonInPool.Add(buttonInstance, buttonInstance.GetComponent<UpgradeAbilitiesOnButtonClick>());
            }
            _abilityAddWindow.ShuffleButtons = _shiffleButtons;
        }
    }

    private void GetObjectFromPool(Button button)
    {
        button.gameObject.SetActive(true);
        button.transform.SetParent(buttonContainer);
        button.transform.position = buttonContainer.transform.position;
        _instantiatedButtons.Add(button);
        _abilityAddWindow.InstantiatedButtons = _instantiatedButtons;

        // Trigger the ButtonObtainedFromPool event
        ButtonObtainedFromPool?.Invoke(button);
    }

    private void ReturnObjectInPool(Button button)
    {
        _buttonPool.ReturnObject(button);
    }

    public void RemoveButtonFromPool(Button prefabButton)
    {
        // Iterate over the dictionary entries
        foreach (var entry in _buttonInPool)
        {
            Button button = entry.Key;
            UpgradeAbilitiesOnButtonClick upgrade = entry.Value;

            //// Check if the prefab of the button matches the provided prefab
            if (upgrade.Index == prefabButton.GetComponent<UpgradeAbilitiesOnButtonClick>().Index)
            {
                // Remove the entry from the dictionary
                _buttonInPool.Remove(button);
                _shiffleButtons.Remove(button);
                break; // Exit the loop since the button was found and removed
            }
        }
    }
}
