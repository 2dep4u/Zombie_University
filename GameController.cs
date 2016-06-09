using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.ImageEffects;
using System.Collections;

public class GameController : MonoBehaviour {

    public Vector3 position = new Vector3(1000f, 1000f, 1000f);
    public Vector3 resetPosition = new Vector3(1000f, 1000f, 1000f);

    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject Resume;

    [SerializeField] private Text gameOverText;
    [SerializeField] private float gameoverFadeTime = 5f;
    [SerializeField] [Multiline] private string dialogue = "default text";
    [SerializeField] private GameObject floorNeedle;
    [SerializeField] private GameObject zombieStart;
    [SerializeField] private GameObject zombieEnd;
    [SerializeField] private GameObject generators;
    [SerializeField] private GameObject electricSwitch;
    [SerializeField] private GameObject gasContainer;
    [SerializeField] private GameObject lighting;
    [SerializeField] private GameObject elevator;
    [SerializeField] private GameObject dialogueBox;

    [Multiline]
    [SerializeField] private string dialogue1;

    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip chaseMusic;
    [SerializeField] private AudioClip endGameMusic;


    private GameObject player;
    private PlayerController playerController;
    private Grayscale grayscale;
    private FirstPersonController fpsController;
    private AudioSource audioSource;
    private Collider col;

    void Awake()
    {
        gameOverText.enabled = false;
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        fpsController = player.GetComponent<FirstPersonController>();
        grayscale = player.GetComponentInChildren<Grayscale>();
        Menu.SetActive(false);
        playerController = player.GetComponentInChildren<PlayerController>();
        col = GetComponent<Collider>();
        col.enabled = false;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerDead();
            gameOverText.text = "You Survived!";
            playEndMusic();

        }

    }

    public void startPhase1()
    {
        StartCoroutine("phase1");
    }

    public void startPhase2()
    {
        StartCoroutine("phase2");
    }

    public void enableWinning()
    {
        col.enabled = true;
    }

    public void PlayerDead()
    {
        playEndMusic();
        zombieEnd.SetActive(false);
        Resume.SetActive(false);
        gameOverText.enabled = true;
        Menu.SetActive(true);
        fpsController.enabled = false;
        position = resetPosition;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StopAllCoroutines();
        playerController.scaredBreathing(false);
        StartCoroutine("GameOverFade");
    }

    public void LevelReset()
    {
        SceneManager.LoadScene("Main");
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void playChaseMusc()
    {
        StartCoroutine("chaseMusicProcess");
    }

    public void playEndMusic()
    {
       
        audioSource.clip = endGameMusic;
        audioSource.Stop();
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

    IEnumerator phase1()
    {
        yield return new WaitForSeconds(4f);
        generators.SendMessage("enableInteraction");
        gasContainer.SendMessage("enableInteraction");
        electricSwitch.SendMessage("playSwitchSound");
        dialogueBox.SendMessage("displayDialogue", dialogue);
        lighting.SetActive(false);
    }

    IEnumerator phase2()
    {
        yield return new WaitForSeconds(3f);
        floorNeedle.SetActive(true);
        zombieStart.SetActive(false);
        zombieEnd.SetActive(true);
        dialogueBox.SendMessage("displayDialogue", dialogue1);
        playerController.scaredBreathing(true);
    }

    IEnumerator GameOverFade()
    {
        float time = Time.deltaTime;
        while (time < gameoverFadeTime)
        {
            time += Time.deltaTime;
            grayscale.rampOffset = -time / gameoverFadeTime;
            yield return null;
        }
    }

    IEnumerator chaseMusicProcess()
    {
        audioSource.Stop();
        audioSource.volume = 0.4f;
        audioSource.clip = chaseMusic;
        audioSource.Play();
        yield return new WaitForSeconds(chaseMusic.length);
        audioSource.Stop();
        audioSource.volume = 0.2f;
        audioSource.clip = backgroundMusic;
        audioSource.Play();
        zombieEnd.SendMessage("freeFlag");
    }


}
