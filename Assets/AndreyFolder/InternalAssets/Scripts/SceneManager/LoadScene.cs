using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using System;

public static class LoadScene
{
    private static Image LoadBar;
    private static LoadingProgressBar _loadingProgressBar;
    private static AsyncOperationHandle<SceneInstance> _nextLoadedScene;
    public static AsyncOperationHandle<SceneInstance> _loadScene;

    public static async void LoadSceneStart(string loadScene, string sceneAddress, Sound.SoundEnum sound, float delay)
    {
        // Load the new scene additively
        _loadScene = Addressables.LoadSceneAsync(loadScene, LoadSceneMode.Additive);

        // Wait for the new scene to finish loading
        await _loadScene.Task;

        // Unload the previous scene before starting the new scene loading process
        if (_nextLoadedScene.IsValid() && _nextLoadedScene.IsDone)
        {
            await Task.Delay(1000);
            await _nextLoadedScene.Task;
            // Unload the previous scene
            Addressables.UnloadSceneAsync(_nextLoadedScene, true);
        }

        await Task.Delay(1000);

        // Call the SceneVariable method in the new scene
        await SceneVariable(sceneAddress, sound, delay);
    }

    private static async Task SceneVariable(string sceneAddress, Sound.SoundEnum sound, float delay)
    {
        try
        {
            _loadingProgressBar = UnityEngine.Object.FindObjectOfType<LoadingProgressBar>();
            LoadBar = _loadingProgressBar.ImageBar;

            Time.timeScale = 0f;
            _nextLoadedScene = Addressables.LoadSceneAsync(sceneAddress, LoadSceneMode.Additive);

            await LoadAnimation(delay);
            await Task.Delay(500); // Delay for a short period after the loading is complete

            UnloadSceneCompleted();

            if (AudioManager.Instance != null)
            {
                AudioSettingsData.Instanñe.LoadSettings();
                AudioManager.Instance.PlayMusic(sound);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred in SceneVariable: {ex}");
        }
        finally
        {
            Time.timeScale = 1f;
        }
    }

    private static async Task LoadAnimation(float delay)
    {
        float targetProgress = 0.9f; // The target progress value before the loading is complete
        float currentProgress = 0f; // The current progress value
        
        while (currentProgress < targetProgress)
        {
            currentProgress += delay; // Increase the progress by a small increment
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

    private static async void UnloadSceneCompleted()
    {
        if (_loadScene.IsValid() && _loadScene.IsDone)
        {
            await _loadScene.Task;
            Addressables.UnloadSceneAsync(_loadScene, true);
        }
    }

}

//private static AsyncOperation asyncOperation;
//SceneManager.LoadScene("LoadScene");

//SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());


//public static async void LoadSceneStart(int sceneID, Sound.SoundEnum sound)
//{
//    SceneManager.LoadScene("LoadScene");

//    await Task.Delay(100);

//    SceneVariable(sceneID, sound);
//}

//private static async void SceneVariable(int sceneID, Sound.SoundEnum sound)
//{
//    _loadingProgressBar = Object.FindObjectOfType<LoadingProgressBar>();
//    LoadBar = _loadingProgressBar.ImageBar;

//    OnSceneLoadCompleted(sceneID);
//    await LoadSceneAsync();
//    await Task.Delay(500); // Delay for a short period after the loading is complete
//    Time.timeScale = 1f;
//    SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

//    if(AudioManager.Instance != null)
//    {
//        AudioSettingsData.Instanñe.LoadSettings();
//        AudioManager.Instance.PlayMusic(sound);
//    }
//}

//asyncOperation = Addressables.LoadSceneAsync(sceneAddress, LoadSceneMode.Additive);
//private static void OnSceneLoadCompleted(int sceneID)
//{
//    Time.timeScale = 0f;
//    asyncOperation = SceneManager.LoadSceneAsync(sceneID, LoadSceneMode.Additive);
//}