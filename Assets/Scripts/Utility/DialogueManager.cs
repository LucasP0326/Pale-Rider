using System.Collections;
using System.Collections.Generic;
using Articy.Unity;
using Articy.Unity.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Articy.Pale_Rider;
using TMPro;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour, IArticyFlowPlayerCallbacks
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("UI")]
    //Reference to Dialogue UI
    [SerializeField]
    GameObject dialogueWidget;
    //Character Portrait
    [SerializeField]
    public Image speakerPortrait;
    //Reference to dialogue text
    [SerializeField]
    TMP_Text dialogueText;
    //Reference to speaker
    [SerializeField]
    TMP_Text dialogueSpeaker;
    [SerializeField]
    RectTransform branchLayoutPanel;
    [SerializeField]
    GameObject branchPrefab;
    [SerializeField]
    GameObject closePrefab;
    [SerializeField]
    RectTransform scrollContent;
    [SerializeField]
    public ScrollRect scrollRect;
    [SerializeField]
    TMP_Text speakerPrefab;
    [SerializeField]
    TMP_Text dialoguePrefab;

    [Header("Audio")]
    [SerializeField]
    AudioClip voiceOver;
    [SerializeField]
    AudioClip[] skillSFX;
    
    [SerializeField]
    AudioSource aSource;

    [Header("Events")]
    [SerializeField]
    private UnityEvent onDialogueClosed;

    public bool DialogueActive { get; set; }

    private ArticyFlowPlayer flowPlayer;

    void Start()
    {
        flowPlayer = GetComponent<ArticyFlowPlayer>();
        aSource = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue(IArticyObject aObject)
    {
        foreach(Transform child in scrollContent)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("I got to Dialogue Manager");
        DialogueActive = true;
        dialogueWidget.SetActive(DialogueActive);
        flowPlayer.StartOn = aObject;
    }

    public void CloseDialogueBox()
    {
        DialogueActive = false;
        dialogueWidget.SetActive(DialogueActive);
        flowPlayer.FinishCurrentPausedObject();
        foreach(Transform child in scrollContent)
        {
            Destroy(child.gameObject);
        }

        aSource.Stop();

        // Trigger the custom event
        onDialogueClosed?.Invoke();
    }

    public void OnFlowPlayerPaused(IFlowObject aObject)
    {
        //throw new System.NotImplementedException();
        //Remove existing Text
        //dialogueText.text = string.Empty;
        //dialogueSpeaker.text = string.Empty;

        //Stop Current Audio
        aSource.Stop();
        
        //Add Dialogue Text
        var objectWithText = aObject as IObjectWithLocalizableText;
        if (objectWithText != null)
        {
            dialogueText.text = objectWithText.Text;
        }

        //Add Speaker Text
        var objectWithSpeaker = aObject as IObjectWithSpeaker;
        if (objectWithSpeaker != null)
        {
            var speakerEntity = objectWithSpeaker.Speaker as Entity;
            if (speakerEntity != null)
            {
                dialogueSpeaker.text = speakerEntity.DisplayName;
            }
            //Add Character Portrait
            var speaker = objectWithSpeaker.Speaker;
            var speakerAsset = ((speaker as IObjectWithPreviewImage).PreviewImage.Asset as Asset);
            if (speakerAsset != null)
            {
                //No portrait for player character
                if (speakerEntity.DisplayName == "You")
                {
                    speakerPortrait.gameObject.SetActive(false);
                }
                else
                {
                    speakerPortrait.gameObject.SetActive(true);
                    speakerPortrait.sprite = speakerAsset.LoadAssetAsSprite();
                }
                speakerPortrait.sprite = speakerAsset.LoadAssetAsSprite();
            }
            
            //Play Audio
            var modelWithText = aObject as IObjectWithLocalizableText;
            if (modelWithText.Text.VOAssetRef != null)
            {
                aSource.clip = modelWithText.Text.LoadVOAssetAsAudioClip();
                aSource.Play();
            }   
        }
    }

    public void OnBranchesUpdated(IList<Branch> aBranches)
    {
        TMP_Text chara = Instantiate(speakerPrefab, scrollContent);
        TMP_Text dial = Instantiate(dialoguePrefab, scrollContent);
        chara.text = dialogueSpeaker.text;
        //Set character colors
        if (dialogueSpeaker.text == "Reptilian Complex" || dialogueSpeaker.text == "Endurance" || dialogueSpeaker.text == "Physicality" || dialogueSpeaker.text == "Volition" || dialogueSpeaker.text == "Reflexivity")
        {
            chara.color = new Color(0.8f, 0.2f, 0.2f); // Red color for these attributes
        }
        else if (dialogueSpeaker.text == "Paleomammalian Complex" || dialogueSpeaker.text == "Empathy" || dialogueSpeaker.text == "Suggestion" || dialogueSpeaker.text == "Authority" || dialogueSpeaker.text == "Rhetoric")
        {
            chara.color = new Color(0.2f, 0.8f, 0.2f); // Green color for these attributes
        }
        else if (dialogueSpeaker.text == "Neomammalian Complex" || dialogueSpeaker.text == "Encyclopedia" || dialogueSpeaker.text == "Logic" || dialogueSpeaker.text == "Perception" || dialogueSpeaker.text == "Conceptualization")
        {
            chara.color = new Color(0.2f, 0.2f, 0.8f); // Blue color for these attributes
        }
        else if (dialogueSpeaker.text == "The Pale" || dialogueSpeaker.text == "Self-Actualization" || dialogueSpeaker.text == "Perspicacity" || dialogueSpeaker.text == "Savor Faire" || dialogueSpeaker.text == "Tenebrality")
        {
            chara.color = new Color(0.5f, 0.2f, 0.5f); // Purple color
        }
        dial.text = dialogueText.text;
        // Auto-scroll to the bottom
        StartCoroutine(ScrollToBottom());

        ClearAllBranches();

        bool dialogueIsFinished = true;
        foreach (var branch in aBranches)
        {
            if (branch.Target is IDialogueFragment)
            {
                dialogueIsFinished = false;
            }
        }

        // Check if the current speaker is the player
        if (dialogueSpeaker.text == "You")
        {
            // Automatically proceed to the next dialogue fragment
            foreach (var branch in aBranches)
            {
                if (branch.Target is IDialogueFragment)
                {
                    flowPlayer.Play(branch);
                    return;
                }
            }
        }

        if (!dialogueIsFinished)
        {
            foreach (var branch in aBranches)
            {
                GameObject btn = Instantiate(branchPrefab, branchLayoutPanel);
                btn.GetComponent<BranchChoice>().AssignBranch(flowPlayer, branch);
            }
        }
        else
        {
            GameObject btn = Instantiate(closePrefab, branchLayoutPanel);
            var btnComp = btn.GetComponent<Button>();
            btnComp.onClick.AddListener(CloseDialogueBox);
        }
    }

    void ClearAllBranches()
    {
        foreach (Transform child in branchLayoutPanel)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerator ScrollToBottom()
    {
        yield return null; // Wait one frame
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
