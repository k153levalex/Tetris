using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OnClickButtonPlay()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnClickButtonRecords()
    {
        SceneManager.LoadScene("Records");
    }

    public void OnClickButtonExit()
    {
        SceneManager.LoadScene("Exit");
    }
}
