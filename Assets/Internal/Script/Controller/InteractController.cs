using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    public static InteractController instance;

    public TextMeshProUGUI interactText;
    public GameObject interactContainer;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        ClearInteractText();
    }
    public void ChangeInteractText(string prompt, Vector3 pos)
    {
        interactContainer.SetActive(true);
        interactContainer.transform.position = pos;
        interactText.text = prompt;
        if (prompt == string.Empty)
        {
            ClearInteractText();
        }
    }
    public void ClearInteractText()
    {
        interactText.text = string.Empty;
        interactContainer.SetActive(false);
    }
}
