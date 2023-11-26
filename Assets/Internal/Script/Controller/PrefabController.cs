using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabController : MonoBehaviour
{
    public static PrefabController instance;

    public List<PrefabTypeObject> prefabTypeObjects = new();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public GameObject GetGameObject(PreItemType itemType, PreItemName itemName)
    {
        foreach (var item in prefabTypeObjects)
        {
            if (item.itemType == itemType)
            {
                foreach (var child in item.prefabObjects)
                {
                    if (child.itemName == itemName)
                        return child.prefab;
                }
            }
        }
        return null;
    }
    public List<PrefabObject> GetGameObject(PreItemType itemType)
    {
        foreach (var item in prefabTypeObjects)
        {
            if (item.itemType == itemType)
            {
                return item.prefabObjects;
            }
        }
        return null;
    }
    public GameObject GetGameObject(PreItemName itemName)
    {
        foreach (var item in prefabTypeObjects)
        {
            foreach (var child in item.prefabObjects)
            {
                if (child.itemName == itemName)
                    return child.prefab;
            }
        }
        return null;
    }

    public static implicit operator PrefabController(MessageController v)
    {
        throw new NotImplementedException();
    }
}
[System.Serializable]
public class PrefabTypeObject
{
    public PreItemType itemType;
    public List<PrefabObject> prefabObjects = new();
}


[System.Serializable]
public class PrefabObject
{
    public GameObject prefab;
    public PreItemName itemName;
}
