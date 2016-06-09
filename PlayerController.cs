using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour {

    private List<int> keys = new List<int>();

    [SerializeField] private Text actionDisplay;    
    [SerializeField] private AudioClip pickUpItem;
    [SerializeField] private GameObject telephone;
    [SerializeField] private GameObject flashLightObject;
    [SerializeField] private GameObject Menu;

    private FirstPersonController fpsController;
    private Light flashLight;
    private AudioSource audioSource;
    private AudioSource flashLightSound;
    private bool flashLightOnOff;
    private bool isPaused;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        flashLight = flashLightObject.GetComponent<Light>();
        flashLightSound = flashLightObject.GetComponent<AudioSource>();
        flashLightOnOff = false;
        flashLight.enabled = false;
        isPaused = false;
        fpsController = GetComponentInParent<FirstPersonController>();
        fpsController.enabled = true;
        Time.timeScale = 1;
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 2))
        {
            if (hit.collider != null)
            {
                bool goodHit = false;
                string tagName = hit.collider.tag;
                switch (tagName)
                {
                    case "Door":
                        actionDisplay.enabled = true;
                        actionDisplay.text = "Press E to open";
                        goodHit = true;
                        break;
                    case "Vaccine":
                    case "Key":
                    case "Gas":
                        actionDisplay.enabled = true;
                        actionDisplay.text = "Press E to pickup " + tagName;
                        goodHit = true;
                        break;
                    case "Patient":
                        actionDisplay.enabled = true;
                        actionDisplay.text = "Press E to use Vaccine";
                        goodHit = true;
                        break;
                    case "Switch":
                    case "Generator":
                    case "Elevator":
                        actionDisplay.enabled = true;
                        actionDisplay.text = "Press E to use " + tagName;
                        goodHit = true;
                        break;
                }

                if (Input.GetKeyDown(KeyCode.E) && goodHit)
                {
                    GameObject target = hit.collider.gameObject;
                    string tag = hit.collider.tag;
                    tagAction(target, tag);
                }
            }
        }
        else
        {
            actionDisplay.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            flashLightOnOff = !flashLightOnOff;
            flashLight.enabled = flashLightOnOff;
            flashLightSound.Play();
        }

        if (Input.GetKeyDown(KeyCode.Space))
            telephone.SendMessage("stopAudioDialogue");

        if (Input.GetKeyDown(KeyCode.Escape))
            pauseGame();
    }

    void tagAction(GameObject target, string tag)
    {
        switch (tag)
        {
            case "Door":
                target.transform.SendMessage("DoorAction", keys);
                break;
            case "Key":
                audioSource.PlayOneShot(pickUpItem);
                target.transform.SendMessage("getKeyID", gameObject.transform);
                target.transform.SendMessage("destroySelf");
                break;
            case "Gas":
            case "Vaccine":
                audioSource.PlayOneShot(pickUpItem);
                target.SendMessage("Interact");
                break;
            case "Patient":
            case "Generator":
            case "Switch":
            case "Elevator":    
                target.SendMessage("Interact");
                break;
        }
    }

    public void addKey(int keyID)
    {
        keys.Add(keyID);
    }

    public void scaredBreathing(bool play)
    {
        if (play)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
        else
            audioSource.Stop();

    }

    public void pauseGame()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            Menu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            fpsController.enabled = false;
        }
        else
        {
            Time.timeScale = 1;
            Menu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            fpsController.enabled = true;
        }

    }

}
