using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BranchChoice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Branch branch;
    private ArticyFlowPlayer flowPlayer;
    [SerializeField]
    TMP_Text buttonText;
    public Color defaultColor;
    public Color seenColor;
    public Color highlightColor = Color.yellow;
    public Color skillCheckPerformedColor = Color.black;

    [Header("UI Elements")]
    public GameObject skillCheckInfoPanek;
    public TMP_Text skillText;
    public TMP_Text difficultyText;
    public TMP_Text bonusText;
    public TMP_Text chanceText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (buttonText != null)
            defaultColor = buttonText.color;
        else
            defaultColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignBranch(ArticyFlowPlayer aFlowPlayer, Branch aBranch)
    {
        branch = aBranch;
        flowPlayer = aFlowPlayer;
        IFlowObject target = aBranch.Target;
        buttonText.text = string.Empty;

        if (target is IObjectWithMenuText objWithMenuText)
            buttonText.text = objWithMenuText.MenuText;
        else if (target is IObjectWithLocalizableMenuText objWithLocalizableMenuText)
            buttonText.text = objWithLocalizableMenuText.MenuText;
        else if (target is IObjectWithText objectWithText)
            buttonText.text = objectWithText.Text;
        else if (target is IObjectWithLocalizableText objWithLocalizableText)
            buttonText.text = objWithLocalizableText.Text;
        else if (target is IObjectWithDisplayName objWithDisplayName)
            buttonText.text = objWithDisplayName.DisplayName;
        else if (target is IObjectWithLocalizableDisplayName objWithLocalizableDisplayName)
            buttonText.text = objWithLocalizableDisplayName.DisplayName;
        else if (target is IArticyObject articyObject)
            buttonText.text = articyObject.TechnicalName;
        else
            buttonText.text = target == null ? "null" : target.GetType().Name;

        if (string.IsNullOrEmpty(buttonText.text))
            buttonText.text = ">>>";

        // If this entry is an OutputPin helper, remove the branch choice object.
        if (buttonText != null && buttonText.text == "OutputPin")
        {
            Destroy(this.gameObject);
            return;
        }

        // If the target has the DialogueSeen feature and it has been seen, gray out the text.
        if (target is Articy.Pale_Rider.IObjectWithFeatureDialogueSeen seenObj)
        {
            var feature = seenObj.GetFeatureDialogueSeen();
            if (feature != null && feature.BooleanValue)
                buttonText.color = seenColor;
            else
                buttonText.color = defaultColor;
        }
        if (target is Articy.Pale_Rider.IObjectWithFeatureSkillCheckDialogue skillCheckObj)
        {
            var feature = skillCheckObj.GetFeatureSkillCheckDialogue();
            if (feature != null)
            {
                if (feature.BooleanValue)
                {
                    if (target is Articy.Pale_Rider.IObjectWithFeatureBlackCheck blackCheckObj)
                    {
                        var blackCheckFeature = blackCheckObj.GetFeatureBlackCheck();
                        if (feature != null && feature.BooleanValue)
                        {
                            if (blackCheckFeature != null && blackCheckFeature.BooleanValue)
                                buttonText.color = skillCheckPerformedColor;
                        }
                        else
                            buttonText.color = defaultColor;
                    }
                }
                else
                    buttonText.color = defaultColor;
            }
        }
    }

    public void OnBranchSelected()
    {
        // If the branch target supports the DialogueSeen feature, mark it as seen.
        if (branch != null)
        {
            var target = branch.Target;
            if (target is Articy.Pale_Rider.IObjectWithFeatureDialogueSeen seenObj)
            {
                var feature = seenObj.GetFeatureDialogueSeen();
                if (feature != null && !feature.BooleanValue)
                    feature.BooleanValue = true;
            }

            if (target is Articy.Pale_Rider.IObjectWithFeatureSkillCheckDialogue skillCheckObj)
            {
                var feature = skillCheckObj.GetFeatureSkillCheckDialogue();
                if (feature != null && !feature.BooleanValue)
                    feature.BooleanValue = true;
                else if (feature != null && feature.BooleanValue)
                {
                    return;
                }
            }
        }

        flowPlayer.Play(branch);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = defaultColor;
    }
}
