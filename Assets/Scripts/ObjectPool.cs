using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool
{
    private GameObject _prefab;
    private List<GameObject> _objects;

    public ObjectPool(GameObject prefab, int prewarmObjects)
    {
        _prefab = prefab;
        _objects = new List<GameObject>();

        for (int i = 0; i < prewarmObjects; i++)
        {
            var obj = GameObject.Instantiate(_prefab);
            obj.SetActive(false);
            _objects.Add(obj);
        }
    }

    public GameObject Get()
    {
        var obj = _objects.FirstOrDefault(x => !x.active);

        if (obj == null)
        {
            obj = Create();
        }

        obj.SetActive(true);
        return obj;
    }

    public void Release(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void ReleaseAll()
    {
        foreach(GameObject obj in _objects)
            obj.SetActive(false);
    }

    private GameObject Create()
    {
        var obj = GameObject.Instantiate(_prefab);
        _objects.Add(obj);
        return obj;
    }
}


