using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private AsyncOperationHandle<SceneInstance> _loadScene;

    public static SceneLoadManager Instan�e;

    private void Awake()
    {
        if (Instan�e == null)
        {
            Instan�e = this;
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

    public void RestartGame()
    {
        // Reset any necessary game state, variables, or managers
        // Load the initial scene or restart the current scene
        Time.timeScale = 1f;
        SceneReloadEvent.Instance.OnUnsubscribeEvents();
        ObjectPoolManager.Instance.ClearAllPools();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public async Task RestartGameAddressablesMethod()
    {
        Time.timeScale = 1f;
        SceneReloadEvent.Instance.OnUnsubscribeEvents();
        ObjectPoolManager.Instance.ClearAllPools();

        await EnvironmentLoader.Instan�e.LoadReloadedPrefabs();
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


//AssetReference prefabReference = EnvironmentLoader.Instan�e.EnvironmentPrefabReferences[0];

//// Check if the prefab is already loaded
//if (prefabReference.RuntimeKeyIsValid())
//{
//    Debug.Log("Loading prefab: " + prefabReference.RuntimeKey);
//    AsyncOperationHandle<GameObject> prefabHandle = Addressables.LoadAssetAsync<GameObject>(prefabReference);

//    await prefabHandle.Task;

//    if (prefabHandle.Status == AsyncOperationStatus.Succeeded)
//    {
//        Debug.Log("Prefab loaded successfully.");

//        GameObject prefabObject = prefabHandle.Result;
//        // Move the instantiated prefab to the active scene.
//        Instantiate(prefabObject);
//    }
//}