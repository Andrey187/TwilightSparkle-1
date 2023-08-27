using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private AsyncOperationHandle<SceneInstance> _loadScene;

    public static SceneLoadManager Instanñe;

    private void Awake()
    {
        if (Instanñe == null)
        {
            Instanñe = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void ResumeInMainScene()
    {
        if (_loadScene.IsValid() && _loadScene.IsDone)
        {
            await _loadScene.Task;
            Addressables.UnloadSceneAsync(_loadScene).Completed += OnSceneUnloaded;
        }
        Time.timeScale = 1f;
    }

    private void OnSceneUnloaded(AsyncOperationHandle<SceneInstance> handle)
    {
        Addressables.Release(handle);
    }
    private void OnSceneLoadComplete(AsyncOperationHandle<SceneInstance> handle)
    {
        handle.Result.ActivateAsync(); // Activate the unloaded scene
        _loadScene = handle; // Update the _loadScene handle with the new scenes
    }

    public void LoadMenuScene(string addressableKey)
    {
        // Pause the game
        Time.timeScale = 0f;

        // Load the MenuScene asynchronously
        _loadScene = Addressables.LoadSceneAsync(addressableKey, LoadSceneMode.Additive);
        _loadScene.Completed += OnSceneLoadComplete; // Attach the completion callback
        Debug.Log(_loadScene);
    }

    public async Task RestartGameAddressablesMethod()
    {
        Time.timeScale = 1f;
        SceneReloadEvent.Instance.OnUnsubscribeEvents();
        ObjectPoolManager.Instance.ClearAllPools();

        await EnvironmentLoader.Instanñe.LoadReloadedPrefabs();
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