using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip titleMusic;

    void Start()
    {
        // Set the audio source to loop the title music
        audioSource.volume = 0.6f;
        audioSource.clip = titleMusic; // Assign the title music to the audio source
        audioSource.loop = true;       // Enable looping
        audioSource.Play();            // Play the music
    }

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
