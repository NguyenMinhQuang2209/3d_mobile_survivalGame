using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    private Material greenMaterial;
    public Material redMaterial;
    private List<Collider> colliders = new();
    [SerializeField] private List<int> colliderMasks = new();

    [SerializeField] private bool useCollider = false;
    [SerializeField] private bool needCollider = false;

    private Renderer renderering;
    private bool canBuilding = false;
    private void Start()
    {
        greenMaterial = ColorController.instance.GetMaterial(ColorName.Green);
        redMaterial = ColorController.instance.GetMaterial(ColorName.Red);
        renderering = GetComponent<Renderer>();
    }
    private void Update()
    {
        if (useCollider)
        {
            canBuilding = needCollider ? colliders.Count > 0 : colliders.Count == 0;
            renderering.material = canBuilding ? greenMaterial : redMaterial;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        int mask = other.gameObject.layer;
        if (colliderMasks.Contains(mask))
        {
            colliders.Add(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        int mask = other.gameObject.layer;
        if (colliderMasks.Contains(mask))
        {
            colliders.Remove(other);
        }
    }
}
