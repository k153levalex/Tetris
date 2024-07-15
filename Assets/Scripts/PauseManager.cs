using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject panelPause;

    void Start()
    {
        panelPause.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            if (panelPause.activeSelf)
                HidePanel();
            else
                ShowPanel();
        }
    }

    public void ShowPanel()
    {
        Time.timeScale = 0f;
        panelPause.SetActive(true);
    }

    public void HidePanel()
    {
        panelPause.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnClickButtonContinue()
    {
        HidePanel();
    }

    public void OnClickButtonMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }    
}
