using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {


    [SerializeField] public AudioClip dingSound;
    [SerializeField] public AudioClip motorSound;
    [SerializeField] public GameObject dialogueBox;
    [SerializeField] private GameObject elevatorPoint;

    [Multiline]
    [SerializeField] public string dialogueText = "default Text";

    private GameController lastPlayerSighting;
    private AudioSource elevatorSound;
    private Animator anim;
    private Collider col;
    private Material mat;
    private bool havePower = false;

    void Start()
    {
        elevatorSound = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();
        mat = GetComponent<Renderer>().material;
        mat.DisableKeyword("_EMISSION");
        lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.SetTrigger("Start");
            elevatorSound.Play();
            col.enabled = false;
            tag = "Untagged";
        }
    }

    void Interact()
    {
        if (havePower)
        {
            dialogueBox.SendMessage("displayDialogue", dialogueText);
            lastPlayerSighting.enableWinning();
            StopCoroutine("pulseElevator");
            mat.DisableKeyword("_EMISSION");
            StartCoroutine("delayElevator");
            tag = "Untagged";
        }

    }

    public void setPower()
    {
        havePower = true;
        tag = "Elevator";
        StartCoroutine("pulseElevator");
    }

    IEnumerator pulseElevator()
    {
        bool flag = false;
        while (true)
        {
            flag = !flag;
            if (flag)
                mat.EnableKeyword("_EMISSION");
            else
                mat.DisableKeyword("_EMISSION");
            yield return new WaitForSeconds(1f);
            yield return null;
        }
    }

    IEnumerator delayElevator()
    {
        elevatorSound.PlayOneShot(dingSound);
        yield return new WaitForSeconds(dingSound.length);
        lastPlayerSighting.position = elevatorPoint.transform.position;
        elevatorSound.PlayOneShot(motorSound);
        yield return new WaitForSeconds(motorSound.length);
        elevatorSound.Play();
        anim.SetBool("isOpen", true);
    }
}
