using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressBar : MonoBehaviour
{
    public Image ImageBar;

    private void Start()
    {
        ImageBar = gameObject.GetComponent<Image>();
    }
}
