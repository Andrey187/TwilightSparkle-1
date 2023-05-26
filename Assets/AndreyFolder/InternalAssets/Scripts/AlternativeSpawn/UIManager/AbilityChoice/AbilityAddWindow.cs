using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityAddWindow : MonoBehaviour
{
    [SerializeField] private List<Button> abilityButtons;
    public List<Button> AbilityButtons { get => abilityButtons; set => abilityButtons = value; }
    public List<Button> InstantiatedButtons { get; set; }
    public List<Button> ShiffleButtons { get; set; }

    public event Action<Button> GetObjectFromPool;
    public event Action<Button> ReturnObjectInPool;

    private void OnEnable()
    {
        //Instantiate and position the selected buttons in the buttonContainer
        foreach (Button buttonPrefab in ShiffleButtons)
        {
            GetObjectFromPool?.Invoke(buttonPrefab);
        }

        List<Button> shuffledButtons = ShuffleList(InstantiatedButtons);

        // Reposition the shuffled buttons within the buttonContainer
        for (int i = 0; i < shuffledButtons.Count; i++)
        {
            Button buttonInstance = shuffledButtons[i];
            buttonInstance.transform.SetSiblingIndex(i);
        }
    }

    private void OnDisable()
    {
        // Deactivate the button container and remove the instantiated buttons when the script is disabled
        gameObject.SetActive(false);

        foreach (Button buttonInstance in InstantiatedButtons)
        {
            ReturnObjectInPool?.Invoke(buttonInstance);
        }

        InstantiatedButtons.Clear();
    }

    private List<T> ShuffleList<T>(List<T> list)
    {
        List<T> shuffledList = new List<T>(list);

        int n = shuffledList.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T temp = shuffledList[k];
            shuffledList[k] = shuffledList[n];
            shuffledList[n] = temp;
        }

        return shuffledList;
    }
}
