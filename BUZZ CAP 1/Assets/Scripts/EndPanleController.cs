using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPanelController : MonoBehaviour
{
    public GameObject endPanel;

    void Start()
    {
        endPanel.SetActive(false);
    }

    public void ShowEndPanel()
    {
        endPanel.SetActive(true);
    }

    public void OnNextButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnHomeButton()
    {
        SceneManager.LoadScene("Home"); // Replace "Home" with your home scene name
    }
}

