using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    public static ColorController instance;

    [SerializeField] private List<ColorItem> colorItems = new();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public Material GetMaterial(ColorName colorName)
    {
        foreach (ColorItem item in colorItems)
        {
            if (item.colorName == colorName)
            {
                return item.material;
            }
        }
        return null;
    }

}
[System.Serializable]
public class ColorItem
{
    public Material material;
    public ColorName colorName;
}