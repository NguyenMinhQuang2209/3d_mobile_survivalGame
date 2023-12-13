using TMPro;
using UnityEngine;

public class ShowTxtConfig : MonoBehaviour
{
    public TextMeshProUGUI txtView;
    private float offsetY = 0.1f;

    public void Config(string newV, float offsetY, float destroyTime)
    {
        this.offsetY = offsetY;
        txtView.text = newV;
        Destroy(gameObject, destroyTime);
    }
    private void Update()
    {
        transform.position += offsetY * Time.deltaTime * Vector3.up;
    }
}
