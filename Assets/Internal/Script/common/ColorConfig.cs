using UnityEngine;

public class ColorConfig : MonoBehaviour
{
    [SerializeField] private int changeMaterialIn = 0;
    public void ChangeMaterial(Material newMaterials)
    {
        if (changeMaterialIn < 0)
        {
            return;
        }
        if (gameObject.TryGetComponent<MeshRenderer>(out var meshRenderItem))
        {
            Material[] tempList = new Material[meshRenderItem.materials.Length];
            for (int i = 0; i < meshRenderItem.materials.Length; i++)
            {
                if (i != changeMaterialIn)
                {
                    tempList[i] = meshRenderItem.materials[i];
                }
                else
                {
                    tempList[i] = newMaterials;
                }
            }
            meshRenderItem.materials = tempList;
        }
        else
        {
            Debug.Log("Can not change color");
        }
    }
}
