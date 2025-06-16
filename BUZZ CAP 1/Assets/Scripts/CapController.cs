using UnityEngine;

public class CapController : MonoBehaviour
{
    public EndPanelController panelController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("End"))
        {
            panelController.ShowEndPanel();
        }
    }
}

