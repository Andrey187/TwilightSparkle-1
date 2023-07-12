using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public void ResumeInMainScene()
    {
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("MenuScene");
    }

    public void LoadMenuScene()
    {
        // Pause the game
        Time.timeScale = 0f;

        // Load the MenuScene asynchronously
        SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Additive);
    }

    //For Test Load Scene
    public void FirstSceneLoad()
    {
        LoadScene.LoadSceneStart(1, Sound.SoundEnum.BackgroundMusicFirstScene);
    }

    public void RestartGame()
    {
        // Reset any necessary game state, variables, or managers
        // Load the initial scene or restart the current scene
        Time.timeScale = 1f;
        SceneReloadEvent.Instance.OnUnsubscribeEvents();
        ObjectPoolManager.Instance.ClearAllPools();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
