using UnityEngine;

public class StartButton : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        LoadScene.LoadSceneStart(2, Sound.SoundEnum.BackgroundMusic);
    }
}
