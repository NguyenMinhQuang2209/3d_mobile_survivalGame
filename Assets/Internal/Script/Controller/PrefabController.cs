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
    public GameObject GetGameObject(ItemType itemType, string itemName)
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
    public List<PrefabObject> GetGameObject(ItemType itemType)
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
    public GameObject GetGameObject(string itemName)
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
}
[System.Serializable]
public class PrefabTypeObject
{
    public ItemType itemType;
    public List<PrefabObject> prefabObjects = new();
}


[System.Serializable]
public class PrefabObject
{
    public GameObject prefab;
    public string itemName;
}
