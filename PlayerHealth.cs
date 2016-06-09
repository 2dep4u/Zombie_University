using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.ImageEffects;
using System.Collections;


public class PlayerHealth : MonoBehaviour {

    public float health = 150f;
    public float restoreHealthTime = 4f;
    public float fadeTime = 1f;
    public Text gameOver;
    public GameObject retry;
    public AudioClip hurt;

    private AudioSource audioSource;
    private GameController gameController;
    private FirstPersonController firstPersonController;
    private Grayscale grayscale;
    private MotionBlur motionblur;
    private float timer;
    private bool playerDead;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        grayscale = GetComponentInChildren<Grayscale>(true);
        motionblur = GetComponentInChildren<MotionBlur>(true);
    }

    public void takeDamage(float amount)
    {
        health -= amount;
        audioSource.PlayOneShot(hurt);
        HurtEffects();
    }

    void HurtEffects()
    {
        Debug.Log(health);
        if (health > 100f && health <= 150f)
        {
            grayscale.enabled = false;
            motionblur.enabled = false;
        }
        else if (health > 50f && health <= 100f)
        {
            grayscale.enabled = false;
            motionblur.enabled = true;
        }
        else if (health > 0f && health <= 50f)
        {
            grayscale.enabled = true;
            motionblur.enabled = true;
        }
        else
        {
            StopCoroutine("RestoreHealth");
            gameController.PlayerDead();
        }

        if (health > 0 && health < 150f)
        {
            StopCoroutine("RestoreHealth");
            StartCoroutine("RestoreHealth");
        }
    }

    IEnumerator RestoreHealth()
    {
        yield return new WaitForSeconds(restoreHealthTime);
        health += 50f;
        HurtEffects();
    }
}
