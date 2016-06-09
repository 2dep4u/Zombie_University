using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlideDoor : MonoBehaviour {

    [SerializeField] public AudioClip lockSound;
    [SerializeField] public GameObject dialogueBox;
    [Multiline]
    [SerializeField] public string dialogueText = "default Text";
    [SerializeField] public int keyID = 0;


    public bool isLocked = false;
    private Material mat;
    private AudioSource slideSound;
    private Animator anim;
    private bool isOpen = false;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.DisableKeyword("_EMISSION");
        slideSound = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    public void DoorAction(List<int> keys)
    {
        if(!isLocked || keys.Contains(keyID))
        {
            isOpen = !isOpen;
            anim.SetBool("isOpen", isOpen);
            slideSound.Play();
        }
        else
        {
            dialogueBox.SendMessage("displayDialogue", dialogueText);
            slideSound.PlayOneShot(lockSound);
        }   
    }

    public void zombieOpen()
    {
        if (!isOpen)
        {
            isOpen = true;
            anim.SetBool("isOpen", isOpen);
            slideSound.Play();
        }
    }
}
