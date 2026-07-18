using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Articy.Unity; // Import Articy namespace
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;
using StarterAssets;
using DoorScript;
using TMPro; // Import TextMeshPro namespace
using System; // <-- added for Convert
using System.Reflection; // <-- added for reflection

public class Interactable : MonoBehaviour
{
    public bool eventOnStart = false;
    
    private Transform player; // Reference to the player
    private GameObject playerController;
    public DialogueManager dialogueManager;
    private ArticyObject availableDialogue;
    private AudioSource aSource;

    [Header("Horse")]
    public bool isHorse = false;
    public HorseManager horseManager;

    [Header("One-Time Interaction")]
    public bool oneTimeInteraction = false; // If true, the object can only be interacted with once
    public bool hasBeenInteracted = false; // Tracks if the object has been interacted with
    public GameObject oneTimeInteractionBubble; // The prefab bubble to show when object can be interacted with once.
    public float interactionBubbleRange = 5f;

    [Header("Interaction Bubble Text")]
    public GameObject interactionTextPrefab; // Prefab for the 3D TextMeshPro object
    public string interactionText = "Default Interaction Text"; // Text to display when the bubble is clicked
    public float interactionTextLength = 3f; // Duration to display the text

    
    [Header("Event Manager")]
    public UnityEvent onInteract; // Assignable event in the Inspector
    private GameObject interactionBubbleInstance;

    [Header("Teleporting")]
    public InventoryManager inventoryManager;
    public TimeManager timeManager;
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
    public bool oneTimeDialogue = false; // If true, the object can only be interacted with once for dialogue
    public bool hasDialogueOnce = false; // Tracks if the object has been interacted with for dialogue
    public bool hasDialogue;

    [Header("Articy Variable Checker")]
    public bool ArticyVariableConditionMet = true; // Default to true, will be set to false if condition is not met
    public string articyVariableToCheckPath;
    public VariableType articyVariableToCheckType = VariableType.Int;
    public int articyIntValueToCheck = 0;
    public bool articyBoolValueToCheck = false;
    public float articyFloatValueToCheck = 0f;
    public string articyStringValueToCheck = "";

    [Header("Articy Variable Setter")]
    [Tooltip("Path format: Section.VariableName e.g. Quests.LeaveThePale")]
    public string articyVariablePath;
    public VariableType articyVariableType = VariableType.Int;
    public int articyIntValue = 0;
    public bool articyBoolValue = false;
    public float articyFloatValue = 0f;
    public string articyStringValue = "";

    public enum VariableType { Int, Bool, Float, String }

    [Header("Object Outline")]
    private Outline _outline;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryManager = FindFirstObjectByType<InventoryManager>();
        timeManager = FindFirstObjectByType<TimeManager>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        playerController = GameObject.FindGameObjectWithTag("Player");
        dialogueManager = FindFirstObjectByType<DialogueManager>();
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

        if (oneTimeInteraction && !hasBeenInteracted)
        {
            ShowInteractionBubble();
        }
        OnLateStart();
    }

    void OnLateStart()
    {
        //Interact on Enter Scene
        if (eventOnStart)
        {
            OnInteract();
        }
    }

    void Awake()
    {
        if (hasDialogue)
            availableDialogue = gameObject.GetComponent<ArticyReference>().reference.GetObject();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (oneTimeInteraction && !hasBeenInteracted && distance <= interactionBubbleRange)
        {
            interactionBubbleInstance.SetActive(true); // Show the interaction bubble when in range
        }
        else if (oneTimeInteraction && hasBeenInteracted)
        {
            interactionBubbleInstance.SetActive(false); // Hide the interaction bubble when already interacted
        }
        else if (oneTimeInteraction && distance > interactionBubbleRange)
        {
            interactionBubbleInstance.SetActive(false); // Hide the interaction bubble when out of range
        }
        if (!isHorse)
        {   
            if (_outline.enabled)
            {
                Debug.Log("Hovering over " + gameObject.name);
                if (Input.GetMouseButtonDown(0))
                {
                    OnMouseDown();
                    Debug.Log("Mouse clicked on " + gameObject.name);
                }
            }
            else
            {
                OnMouseExit();
            }
        }
        CheckArticyVariable();
    }

    private void ShowInteractionBubble()
    {
        if (oneTimeInteractionBubble != null)
        {
            // Instantiate the interaction bubble slightly above the object
            interactionBubbleInstance = Instantiate(oneTimeInteractionBubble, transform.position + Vector3.up * 1.5f, Quaternion.identity);

            // Parent the bubble to this object for better organization
            interactionBubbleInstance.transform.SetParent(transform);

            // Ensure the bubble always faces the camera
            //interactionBubbleInstance.AddComponent<Billboard>();

            // Set up the interaction logic
            interactionBubbleInstance.GetComponent<InteractionBubble>().Setup(this);
        }
    }

    private void OnMouseEnter()
    {
        _outline.enabled = true; // Enable outline when hovered over
    }

    // add near top of Interactable class (public so HoverManager can call it)
    public void SetHover(bool hover)
    {
        if (_outline == null) _outline = GetComponent<Outline>() ?? gameObject.AddComponent<Outline>();
        _outline.enabled = hover;
    }

    private void OnMouseExit()
    {
        _outline.enabled = false; // Disable outline when not hovered
    }

    public void OnMouseDown()
    {
        //onClick?.Invoke(); // Call the assigned function(s)
        ThirdPersonController playerController = FindFirstObjectByType<ThirdPersonController>();
        if (playerController.inMenu == false)
        {
            if (playerController != null && playerController.paused == false)
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
    }

    public void OnInteract()
    {
        if (soundEffect != null)
        {
            aSource.clip = soundEffect;
            aSource.Play();
        }
        if (!player.GetComponent<ThirdPersonController>().inMenu && !player.GetComponent<ThirdPersonController>().paused && !isTeleporting && !player.GetComponent<ThirdPersonController>().inDialogue)
        {
            if (ArticyVariableConditionMet)
            {
                onInteract?.Invoke(); // Calls the function(s) assigned in the Inspector
            }
            else
            {
                Debug.Log("Articy variable condition not met. Interaction aborted.");
            }
        }
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
        if (availableDialogue != null && playerController.GetComponent<ThirdPersonController>().inDialogue == false)
        {
            if (oneTimeDialogue && !hasDialogueOnce)
            {
                hasDialogueOnce = true; // Set to true after the first interaction
                dialogueManager.StartDialogue(availableDialogue);
                //availableDialogue = null;
            }
            else if (!oneTimeDialogue)
            {
                hasDialogueOnce = false; // Reset for future interactions
                dialogueManager.StartDialogue(availableDialogue);
            }
            else
            {
                Debug.LogWarning("No available dialogue assigned.");
            }
        }
    }

    public void SpeechBubble()
    {
        if (interactionTextPrefab != null && !hasBeenInteracted)
        {
            // Instantiate the 3D TextMeshPro object slightly above the interaction bubble
            GameObject textInstance = Instantiate(interactionTextPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);

            // Set the text content and add <mark> tag for black highlight
            TMP_Text tmpText = textInstance.GetComponent<TMP_Text>();
            if (tmpText != null)
            {
                tmpText.text = $"<mark=#000000FF>{interactionText}</mark>"; // FF is alpha for fully opaque black
            }

            // Optionally destroy the text after a delay
            Destroy(textInstance, 3f); // Destroy after 3 seconds
        }
    }

    public void MountHorse()
    {
        Debug.Log("MountHorse called");
        if (playerController.GetComponent<ThirdPersonController>().isMounted)
        {
            Dismount();
        }
        else
        {
            horseManager.Mount();
        }
    }

    public void Dismount()
    {
        horseManager.Dismount();
    }

    public void SetArticyVariable()
    {
        if (string.IsNullOrWhiteSpace(articyVariablePath))
        {
            Debug.LogWarning("Articy variable path is empty.");
            return;
        }

        // Get top-level generated global variables instance
        var gv = Articy.Pale_Rider.GlobalVariables.ArticyGlobalVariables.Default;
        if (gv == null)
        {
            Debug.LogWarning("Articy GlobalVariables not loaded (ArticyGlobalVariables.Default is null).");
            return;
        }

        var parts = articyVariablePath.Split('.');
        object current = gv;
        Type currentType = current.GetType();

        for (int i = 0; i < parts.Length; i++)
        {
            string part = parts[i];

            // Try property first, then field
            PropertyInfo prop = currentType.GetProperty(part, BindingFlags.Public | BindingFlags.Instance);
            FieldInfo field = currentType.GetField(part, BindingFlags.Public | BindingFlags.Instance);

            if (i == parts.Length - 1)
            {
                // set value on last member
                Type memberType = prop != null ? prop.PropertyType : field != null ? field.FieldType : null;
                if (memberType == null)
                {
                    Debug.LogWarning($"Member '{part}' not found on type {currentType.Name}.");
                    return;
                }

                object valueToSet = null;
                try
                {
                    switch (articyVariableType)
                    {
                        case VariableType.Int:
                            valueToSet = Convert.ChangeType(articyIntValue, memberType);
                            break;
                        case VariableType.Bool:
                            valueToSet = Convert.ChangeType(articyBoolValue, memberType);
                            break;
                        case VariableType.Float:
                            valueToSet = Convert.ChangeType(articyFloatValue, memberType);
                            break;
                        case VariableType.String:
                            valueToSet = Convert.ChangeType(articyStringValue, memberType);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Failed to convert value to target type {memberType.Name}: {e.Message}");
                    return;
                }

                if (prop != null) prop.SetValue(current, valueToSet);
                else field.SetValue(current, valueToSet);

                Debug.Log($"Set Articy variable '{articyVariablePath}' to {valueToSet}");
                return;
            }
            else
            {
                // traverse to next object in path
                object next = null;
                if (prop != null) next = prop.GetValue(current);
                else if (field != null) next = field.GetValue(current);
                else
                {
                    Debug.LogWarning($"Member '{part}' not found on type {currentType.Name} while traversing path.");
                    return;
                }

                if (next == null)
                {
                    Debug.LogWarning($"Member '{part}' is null while traversing path.");
                    return;
                }

                current = next;
                currentType = current.GetType();
            }
        }
    }
    
    private bool IsVariableValueMatch(Type memberType, object valueToCheck)
    {
        if (valueToCheck == null)
        {
            return false;
        }

        switch (articyVariableToCheckType)
        {
            case VariableType.Int:
                if (!IsSupportedIntegerType(memberType))
                {
                    return false;
                }
                return Convert.ToInt32(valueToCheck) == articyIntValueToCheck;

            case VariableType.Bool:
                if (memberType != typeof(bool))
                {
                    return false;
                }
                return Convert.ToBoolean(valueToCheck) == articyBoolValueToCheck;

            case VariableType.Float:
                if (!IsSupportedFloatType(memberType))
                {
                    return false;
                }
                return Math.Abs(Convert.ToSingle(valueToCheck) - articyFloatValueToCheck) < 0.0001f;

            case VariableType.String:
                if (memberType != typeof(string))
                {
                    return false;
                }
                return Convert.ToString(valueToCheck) == articyStringValueToCheck;
        }

        return false;
    }

    private bool IsSupportedIntegerType(Type memberType)
    {
        return memberType == typeof(int) ||
               memberType == typeof(short) ||
               memberType == typeof(long) ||
               memberType == typeof(byte) ||
               memberType == typeof(uint) ||
               memberType == typeof(ushort) ||
               memberType == typeof(ulong);
    }

    private bool IsSupportedFloatType(Type memberType)
    {
        return memberType == typeof(float) ||
               memberType == typeof(double) ||
               memberType == typeof(decimal);
    }

    public void CheckArticyVariable()
    {
        if (string.IsNullOrWhiteSpace(articyVariableToCheckPath))
        {
            Debug.LogWarning("Articy variable path to check is empty.");
            return;
        }

        // Get top-level generated global variables instance
        var gv = Articy.Pale_Rider.GlobalVariables.ArticyGlobalVariables.Default;
        if (gv == null)
        {
            Debug.LogWarning("Articy GlobalVariables not loaded (ArticyGlobalVariables.Default is null).");
            return;
        }

        var parts = articyVariableToCheckPath.Split('.');
        object current = gv;
        Type currentType = current.GetType();

        for (int i = 0; i < parts.Length; i++)
        {
            string part = parts[i];

            // Try property first, then field
            PropertyInfo prop = currentType.GetProperty(part, BindingFlags.Public | BindingFlags.Instance);
            FieldInfo field = currentType.GetField(part, BindingFlags.Public | BindingFlags.Instance);

            if (prop == null && field == null)
            {
                Debug.LogWarning($"Member '{part}' not found on type {currentType.Name} while traversing path.");
                return;
            }

            if (i == parts.Length - 1)
            {
                object valueToCheck = prop != null ? prop.GetValue(current) : field.GetValue(current);
                Type memberType = prop != null ? prop.PropertyType : field.FieldType;
                bool conditionMet = IsVariableValueMatch(memberType, valueToCheck);

                ArticyVariableConditionMet = conditionMet;
                Debug.Log($"Checked Articy variable '{articyVariableToCheckPath}': condition met = {conditionMet}");
                return;
            }

            current = prop != null ? prop.GetValue(current) : field.GetValue(current);
            if (current == null)
            {
                Debug.LogWarning($"Member '{part}' is null while traversing path.");
                return;
            }

            currentType = current.GetType();
        }
    }

    private IEnumerator TeleportRoutine(Transform player)
    {
        isTeleporting = true;

        if (changeScene)
        {
            //Store Inventory and Time Data
            //inventoryManager.SaveInventory(); // Save inventory before changing scene
            timeManager.SaveTimeToArticy(); // Save time before changing scene
            
            // Store the spawn point ID before switching scenes
            //PlayerPrefs.SetString("SpawnPoint", spawnPointID); //Try saving to Articy perchance?  Hrmmmmmm?????
            ArticyGlobalVariables.Default.GlobalVariables.SpawnPoint = spawnPointID;
            PlayerPrefs.Save(); // Ensure the value is saved immediately
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
