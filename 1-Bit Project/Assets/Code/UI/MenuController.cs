using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayGame()
    {
        StartCoroutine(WaitOneSecond());       
    }

    IEnumerator WaitOneSecond()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(.5f);

        // Code to execute after the 1 second delay
        Debug.Log(".5 seconds has passed");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
