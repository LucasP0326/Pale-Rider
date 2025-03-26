using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Articy.Unity;
using StarterAssets;
using DoorScript;

public class Interactable : MonoBehaviour
{
    private Transform player; // Reference to the player
    public GameObject playerController;
    public DialogueManager dialogueManager;
    public ArticyObject availableDialogue;
    public AudioSource aSource;
    
    [Header("Event Manager")]
    public UnityEvent onInteract; // Assignable event in the Inspector

    [Header("Teleporting")]
    public Transform targetPoint;
    public bool changeScene = false;
    public string sceneName;
    public string spawnPointID;
    private bool isTeleporting = false;
    public float teleportCooldown = 1f; // Delay before teleporting again
    public float interactionRange = 2f; // Maximum distance for interaction

    [Header("Door")]
    public GameObject door;
    public AudioClip soundEffect;

    [Header("Dialogue")]
    public bool hasDialogue;

    [Header("Object Outline")]
    private Outline _outline;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        aSource = GetComponent<AudioSource> ();

        // Add or get the Outline component
        _outline = GetComponent<Outline>();
        if (_outline == null)
        {
            _outline = gameObject.AddComponent<Outline>();
        }
        _outline.enabled = false; // Disable by default

        if (hasDialogue)
            availableDialogue = gameObject.GetComponent<ArticyReference>().reference.GetObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        _outline.enabled = true; // Enable outline when hovered over
    }

    private void OnMouseExit()
    {
        _outline.enabled = false; // Disable outline when not hovered
    }

    private void OnMouseDown()
    {
        //onClick?.Invoke(); // Call the assigned function(s)
        ThirdPersonController playerController = FindFirstObjectByType<ThirdPersonController>();
        if (playerController != null)
        {
            playerController.tempInteractableObject = gameObject;
            //playerController.MoveToTarget(transform.position); // Make the player run to the object
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactionRange && !isTeleporting)
        {
            OnInteract();
        }
        else
        {
            Debug.Log("Player is too far to interact.");
        }
    }

    public void OnInteract()
    {
        if (soundEffect != null)
        {
            aSource.clip = soundEffect;
            aSource.Play();
        }
        onInteract?.Invoke(); // Calls the function(s) assigned in the Inspector
    }

    //Possible Interaction Functions
    public void TryTeleport()
    {
        if (player == null) return;

        StartCoroutine(TeleportRoutine(player));
    }

    public void Door()
    {
        if (door != null)
        {
            door.GetComponent<Door>().OpenDoor();
        }
    }

    public void BeginDialogue()
    {
        Debug.Log("I am on Interactable");
        if (availableDialogue)
        {
            dialogueManager.StartDialogue(availableDialogue);
            //availableDialogue = null;
        }
    }

    private IEnumerator TeleportRoutine(Transform player)
    {
        isTeleporting = true;

        if (changeScene)
        {
            // Store the spawn point ID before switching scenes
            PlayerPrefs.SetString("SpawnPoint", spawnPointID);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            // In-scene teleportation (if not changing scene)
            Transform targetPoint = GameObject.Find(spawnPointID)?.transform;
            if (targetPoint != null)
            {
                player.position = targetPoint.position;
            }
        }

        yield return new WaitForSeconds(teleportCooldown);
        isTeleporting = false;
    }
}
