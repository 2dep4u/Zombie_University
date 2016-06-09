using UnityEngine;
using System.Collections;

public class VoiceMailTrigger : MonoBehaviour {

    [SerializeField] private GameObject dialogueBox;
    [Multiline]
    [SerializeField] private string dialogueText = "default text";
    private AudioSource audioSource;
    private Collider trigger;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        trigger = GetComponent<Collider>();
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            audioSource.Play();
            trigger.enabled = false;
            dialogueBox.SendMessage("displayDialogue", dialogueText);
        }
    }

    public void stopAudioDialogue()
    {
        audioSource.Stop();
    }
}
