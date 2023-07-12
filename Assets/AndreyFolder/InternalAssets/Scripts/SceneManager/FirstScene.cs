using UnityEngine;

public class FirstScene : MonoBehaviour
{
    private void Awake()
    {
        LoadScene.LoadSceneStart(1, Sound.SoundEnum.BackgroundMusicFirstScene);
    }
}
