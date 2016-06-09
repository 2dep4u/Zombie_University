using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenu : MonoBehaviour {

    private AudioSource audioSource;
    [SerializeField] private AudioClip startButtonSound;
    [SerializeField] private GameObject StartButton;
    [SerializeField] private GameObject Loading;
    [SerializeField] private GameObject Quit;


    void Awake()
    {
        Time.timeScale = 1;
        Loading.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }
    public void startGame()
    {
        StartButton.SetActive(false);
        Loading.SetActive(true);
        audioSource.PlayOneShot(startButtonSound);
        StartCoroutine("loadGame");
    }

    public void quitGame()
    {
        Application.Quit();
    }

    IEnumerator loadGame()
    {
        Quit.SetActive(false);
        yield return new WaitForSeconds(startButtonSound.length);
        SceneManager.LoadScene("Main");
    }
}
