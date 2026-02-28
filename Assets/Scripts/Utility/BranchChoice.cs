using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;
using UnityEngine.UI;
using TMPro;

public class BranchChoice : MonoBehaviour
{
    private Branch branch;
    private ArticyFlowPlayer flowPlayer;
    [SerializeField]
    TMP_Text buttonText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
    }

    public void OnBranchSelected()
    {
        flowPlayer.Play(branch);
    }
}
