using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityChoicePool : MonoBehaviour
{
    [SerializeField] private AbilityAddWindow _abilityAddWindow;
    [SerializeField] private Transform buttonContainer;

    private List<Button> _shiffleButtons = new List<Button>();
    private List<Button> _instantiatedButtons = new List<Button>();

    private PoolObject<Button> _buttonPool;
    private IObjectFactory _objectFactory;

    private void Start()
    {
        foreach (var button in _abilityAddWindow.AbilityButtons)
        {
            _objectFactory = new ObjectsFactory(button.GetComponent<Button>().transform);
            Button buttons = _objectFactory.CreateObject(button.transform.position).GetComponent<Button>();
            PoolObject<Button>.CreateInstance(buttons, 3, buttonContainer,  "Buttons_Container");

            _buttonPool = PoolObject<Button>.Instance;

            Button buttonInstance = _buttonPool.GetObjects(buttons.transform.position, buttons);
            buttonInstance.transform.SetParent(buttonContainer);
            _shiffleButtons.Add(buttonInstance);
            _abilityAddWindow.ShiffleButtons = _shiffleButtons;
        }

        _abilityAddWindow.GetObjectFromPool += GetObjectFromPool;
        _abilityAddWindow.ReturnObjectInPool += ReturnObjectInPool;
    }

    private void GetObjectFromPool(Button button)
    {
        button.gameObject.SetActive(true);
        button.transform.SetParent(buttonContainer);
        button.transform.position = buttonContainer.transform.position;


        _instantiatedButtons.Add(button);
        _abilityAddWindow.InstantiatedButtons = _instantiatedButtons;
    }

    private void ReturnObjectInPool(Button button)
    {
        _buttonPool.ReturnObject(button);
    }
}
