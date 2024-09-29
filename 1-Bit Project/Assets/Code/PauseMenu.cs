using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public void ResumeGame()
    {
        SimplePauseManager.Instance.TogglePause();
    }    

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
