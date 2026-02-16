using System.Collections.Generic;
using UnityEngine;

public class PoolController : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform parent;
    [SerializeField] int startCount;

    Queue<GameObject> _pool = new();

    void Start()
    {
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < startCount; i++) 
        {
            AddToPool();
        }
    }

    public GameObject SpawnByPool()
    {
        if (_pool.Count == 0)
        {
            AddToPool();
        }

        GameObject _object = _pool.Dequeue();
        _object.SetActive(true);

        return _object; 
    }

    public void ReturnToPool(GameObject _gameObject)
    {
        _gameObject.SetActive(false);
        _pool.Enqueue(_gameObject);
    }

    void AddToPool()
    {
        GameObject _object = Instantiate(prefab, parent);
        _object.SetActive(false);

        _pool.Enqueue(_object);
    }
}
