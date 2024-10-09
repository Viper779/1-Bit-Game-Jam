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
        TurretHealth.isDestroyed = false;

        SimplePauseManager.Instance.TogglePause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1, LoadSceneMode.Single);

        GameObject persistentObject = GameObject.FindWithTag("Destroy");  // Or by name: GameObject.Find("ObjectName")

        if (persistentObject != null)
        {
           Destroy(persistentObject);
        }
    }
}
