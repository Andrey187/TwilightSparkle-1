using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstScene : MonoBehaviour
{
    private async void Awake()
    {
        LoadScene.LoadSceneStart("LoadScene",
            "MainScene", Sound.SoundEnum.BackgroundMusicFirstScene, 0.05f);

        await Task.Delay(500);

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}

