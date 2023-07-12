using UnityEngine;

public class SettingsMenuManager : MonoBehaviour
{
    public GameObject mainSettingsMenu;
    public GameObject musicSettingsMenu;

    private void Start()
    {
        HideMusicSettingsMenu();
    }

    public void ShowMusicSettingsMenu()
    {
        mainSettingsMenu.SetActive(false);
        musicSettingsMenu.SetActive(true);
    }

    public void HideMusicSettingsMenu()
    {
        musicSettingsMenu.SetActive(false);
        mainSettingsMenu.SetActive(true);
    }
}
