using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitManager : MonoBehaviour
{
    public void OnClickButtonBack()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnClickButtonExit()
    {
        Application.Quit();
    }
}
