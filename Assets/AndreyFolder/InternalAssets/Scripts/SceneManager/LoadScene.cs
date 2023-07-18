using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public static class LoadScene
{
    private static Image LoadBar;
    private static LoadingProgressBar _loadingProgressBar;
    private static AsyncOperation asyncOperation;

    public static async void LoadSceneStart(int sceneID, Sound.SoundEnum sound)
    {
        SceneManager.LoadScene("LoadScene");
        
        await Task.Delay(100);
        
        SceneVariable(sceneID, sound);
    }

    private static async void SceneVariable(int sceneID, Sound.SoundEnum sound)
    {
        _loadingProgressBar = Object.FindObjectOfType<LoadingProgressBar>();
        LoadBar = _loadingProgressBar.ImageBar;
        
        OnSceneLoadCompleted(sceneID);
        await LoadSceneAsync();
        await Task.Delay(500); // Delay for a short period after the loading is complete
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        if(AudioManager.Instance != null)
        {
            AudioSettingsData.Instanñe.LoadSettings();
            AudioManager.Instance.PlayMusic(sound);
        }
    }

    private static async Task LoadSceneAsync()
    {
        
        float targetProgress = 0.9f; // The target progress value before the loading is complete
        float currentProgress = 0f; // The current progress value
        
        while (currentProgress < targetProgress)
        {
            currentProgress += 0.05f; // Increase the progress by a small increment
            LoadBar.fillAmount = currentProgress; // Update the loading bar

            if (AudioManager.Instance != null)
            {
                float fadeProgress = 1f - (currentProgress / targetProgress);
                AudioManager.Instance.FadeOutMusic(fadeProgress);
            }

            await Task.Delay(100); // Delay for a short period before the next progress update
        }
        LoadBar.fillAmount = 1f; // Set the loading bar to full at the end
    }

    private static void OnSceneLoadCompleted(int sceneID)
    {
        Time.timeScale = 0f;
        asyncOperation = SceneManager.LoadSceneAsync(sceneID, LoadSceneMode.Additive);
    }
}
