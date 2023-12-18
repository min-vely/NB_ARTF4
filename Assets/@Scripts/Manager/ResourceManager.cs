using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager : MonoBehaviour
{
    #region Fields
    private Dictionary<string, UnityEngine.Object> _resources = new();

    #endregion

    #region Properties

    public bool LoadBase { get; set; }
    public bool LoadIntro { get; set; }
    public bool LoadGame { get; set; }
    public bool LoadLoading { get; set; }

    #endregion
    // ���ҽ� �񵿱� �ε� �޼���
    #region Asynchronous Loading

    /// <summary>
    /// �ݹ��� ó���ϴ� ���׸� �ڵ鷯�Դϴ�.
    /// </summary>
    private void AsyncHandlerCallback<T>(string key, AsyncOperationHandle<T> handle, Action<T> callback) where T : UnityEngine.Object
    {
        handle.Completed += operationHandle =>
        {
            _resources.Add(key, operationHandle.Result);
            callback?.Invoke(operationHandle.Result);
        };
    }

    /// <summary>
    /// �񵿱� ������� ���ҽ��� �ε��ϰ� �ݹ��� ȣ���մϴ�.
    /// </summary>
    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
    {
        string loadKey = key;
        if (_resources.TryGetValue(loadKey, out UnityEngine.Object resource))
        {
            callback?.Invoke(resource as T);
            return;
        }

        if (loadKey.Contains(".sprite"))
        {
            loadKey = $"{key}[{key.Replace(".sprite", "")}]";
            AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(loadKey);
            AsyncHandlerCallback(loadKey, handle, callback as Action<Sprite>);
        }
        else
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(loadKey);
            AsyncHandlerCallback(loadKey, handle, callback);
        }
    }

    /// <summary>
    /// Ư�� �󺧿� ���� ��� ���ҽ��� �񵿱� ������� �ε��ϰ� �ݹ��� ȣ���մϴ�.
    /// </summary>
    public void AllLoadAsync<T>(string label, Action<string, int, int> callback) where T : UnityEngine.Object
    {
        var operation = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        operation.Completed += operationHandle =>
        {
            int loadCount = 0;
            int totalCount = operationHandle.Result.Count;
            foreach (var result in operationHandle.Result)
            {
                LoadAsync<T>(result.PrimaryKey, obj =>
                {
                    loadCount++;
                    callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                });
            }
        };
    }
    /// <summary>
    /// Ư�� �󺧿� ���� ��� ���ҽ��� �񵿱� ������� ��ε��մϴ�.
    /// </summary>
    public void UnloadAllAsync<T>(string label) where T : UnityEngine.Object
    {
        var operation = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        operation.Completed += operationHandle =>
        {
            foreach (var result in operationHandle.Result)
            {
                if (_resources.TryGetValue(result.PrimaryKey, out UnityEngine.Object resource))
                {
                    Addressables.Release(resource);
                    _resources.Remove(result.PrimaryKey);
                    Debug.Log($"{resource} ��ε�");
                }
            }
        };
    }
    #endregion
    // ���ҽ� ���� �ε�&��ε� �޼���
    #region Synchronous Loading

    /// <summary>
    /// ���ҽ��� ���������� �ε��մϴ�.
    /// </summary>
    public T Load<T>(string key) where T : UnityEngine.Object
    {
        if (!_resources.TryGetValue(key, out UnityEngine.Object resource))
        {
            Debug.LogError($"Ű�� ã�� �� �����ϴ�. : {key}");
            return null;
        }
        return resource as T;
    }

    #endregion
    // ������ �ν��Ͻ�ȭ �޼���
    #region Instantiation

    /// <summary>
    /// �������� �ν��Ͻ�ȭ�ϰ� ������ �ν��Ͻ��� ��ȯ�մϴ�.
    /// </summary>
    public GameObject InstantiatePrefab(string key, Transform transform = null)
    {
        GameObject resource = Load<GameObject>(key);

        GameObject instance = Instantiate(resource, transform);

        if (instance == null)
        {
            Debug.LogError($"���ҽ��� �ν��Ͻ�ȭ���� ���߽��ϴ�.: { key}");
            return null;
        }
        return instance;
    }

    #endregion
}

