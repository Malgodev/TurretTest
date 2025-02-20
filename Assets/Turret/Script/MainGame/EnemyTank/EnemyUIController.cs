using UnityEngine;
using UnityEngine.UI;

public class EnemyUIController : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Slider healthBar;

    void LateUpdate()
    {
        canvas.transform.LookAt(canvas.transform.position + Camera.main.transform.forward);
    }

    public void SetHealthBar(int value)
    {
        healthBar.value = value;
    }
}
