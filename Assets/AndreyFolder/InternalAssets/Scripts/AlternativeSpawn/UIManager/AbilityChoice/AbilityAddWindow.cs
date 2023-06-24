using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AbilityAddWindow : MonoBehaviour
{
    public List<Button> AbilityButtons { get => abilityButtons; set => abilityButtons = value; }
    public List<Button> InstantiatedButtons { get; set; }
    public List<Button> ShuffleButtons { get; set; }

    public event Action<Button> GetObjectFromPool;
    public event Action<Button> ReturnObjectInPool;

    [SerializeField] private List<Button> abilityButtons;
    private List<Button> shuffledButtons;
    private List<Button> selectedButtons;

    private void OnEnable()
    {
        // Shuffle the buttons
        shuffledButtons = ShuffleList(ShuffleButtons);
        foreach (Button a in shuffledButtons)
        {
            a.gameObject.SetActive(false);
        }
        // Select the first 3 buttons from the shuffled list
        selectedButtons = shuffledButtons.Take(3).ToList();
        //Instantiate and position the selected buttons in the buttonContainer
        foreach (Button buttonPrefab in selectedButtons)
        {
            GetObjectFromPool?.Invoke(buttonPrefab);
        }

        // Reposition the shuffled buttons within the buttonContainer
        for (int i = 0; i < selectedButtons.Count; i++)
        {
            Button buttonInstance = selectedButtons[i];
            buttonInstance.transform.SetSiblingIndex(i);
        }
    }

    private void OnDisable()
    {
        // Deactivate the button container and remove the instantiated buttons when the script is disabled
        //gameObject.SetActive(false);

        foreach (Button buttonInstance in InstantiatedButtons)
        {
            ReturnObjectInPool?.Invoke(buttonInstance);
        }
        InstantiatedButtons.Clear();
        shuffledButtons.Clear();
        selectedButtons.Clear();
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
