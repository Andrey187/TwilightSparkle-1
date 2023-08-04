using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class EnvironmentLoader : MonoBehaviour
{
    [SerializeField] public List<AssetReference> EnvironmentPrefabReferences;
    [SerializeField] public List<GameObject> CreatedObjects;
    private int _currentPrefabIndex = 1;

    public static EnvironmentLoader Instanñe;

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

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "AndreyTestScene")
        {
            _ = LoadReloadedPrefabs(scene);
            _ = LoadPrefabsSequentially(scene); // Start the async loading task and forget about the result.
        }
    }

    public async Task LoadReloadedPrefabs(Scene activeScene)
    {
        AssetReference prefabReference = EnvironmentPrefabReferences[0];

        // Check if the prefab is already loaded
        if (prefabReference.RuntimeKeyIsValid())
        {
            AsyncOperationHandle<GameObject> prefabHandle = Addressables.InstantiateAsync(prefabReference);
            await prefabHandle.Task;

            if (prefabHandle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject prefabObject = prefabHandle.Result;

                // Move the instantiated prefab to the active scene.
                SceneManager.MoveGameObjectToScene(prefabObject, activeScene);
                CreatedObjects.Add(prefabObject);
            }
            else
            {
                Debug.LogError($"Failed to load prefab at index {_currentPrefabIndex}.");
            }
        }
        else
        {
            Debug.Log($"Prefab at index {_currentPrefabIndex} is not set in the Addressables.");
        }
        // Add a delay before loading the next prefab (adjust the delay duration as needed).
        await Task.Delay(TimeSpan.FromSeconds(0.3f));
    }

    public async Task LoadReloadedPrefabs()
    {
        foreach (GameObject oldPrefab in CreatedObjects)
        {
            EnvironmentPrefabReferences[0].ReleaseInstance(oldPrefab); // Destroy the previous prefab instances
        }
        CreatedObjects.Clear(); // Clear the list of old prefab instances

        AssetReference prefabReference = EnvironmentPrefabReferences[0];

        // Check if the prefab is already loaded
        if (prefabReference.RuntimeKeyIsValid())
        {
            AsyncOperationHandle<GameObject> prefabHandle = Addressables.InstantiateAsync(prefabReference);
            await prefabHandle.Task;

            if (prefabHandle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject prefabObject = prefabHandle.Result;
                // Move the instantiated prefab to the active scene.
                CreatedObjects.Add(prefabObject);
            }
            else
            {
                Debug.LogError($"Failed to load prefab at index {_currentPrefabIndex}.");
            }
        }
        else
        {
            Debug.Log($"Prefab at index {_currentPrefabIndex} is not set in the Addressables.");
        }
    }

    private async Task LoadPrefabsSequentially(Scene activeScene)
    {
        while (_currentPrefabIndex < EnvironmentPrefabReferences.Count)
        {
            AssetReference prefabReference = EnvironmentPrefabReferences[_currentPrefabIndex];

            // Check if the prefab is already loaded
            if (prefabReference.RuntimeKeyIsValid())
            {
                AsyncOperationHandle<GameObject> prefabHandle = Addressables.InstantiateAsync(prefabReference);
                await prefabHandle.Task;

                if (prefabHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject prefabObject = prefabHandle.Result;

                    // Move the instantiated prefab to the active scene.
                    SceneManager.MoveGameObjectToScene(prefabObject, activeScene);
                }
                else
                {
                    Debug.LogError($"Failed to load prefab at index {_currentPrefabIndex}.");
                }
            }
            else
            {
                Debug.Log($"Prefab at index {_currentPrefabIndex} is not set in the Addressables.");
            }

            // Move to the next prefab index for the next iteration.
            
            _currentPrefabIndex++;

            // Add a delay before loading the next prefab (adjust the delay duration as needed).
            await Task.Delay(TimeSpan.FromSeconds(0.3f));
        }
    }
}
