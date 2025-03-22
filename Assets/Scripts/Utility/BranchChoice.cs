using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Articy.Unity;
using Articy.Unity.Interfaces;
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

        var objectWithMenuText = target as IObjectWithMenuText;
        if (objectWithMenuText != null)
        {
            buttonText.text = objectWithMenuText.MenuText;
        }

        if (string.IsNullOrEmpty(buttonText.text))
        {
            /*var objectWithText = target as IObjectWithText;
            if (objectWithText != null)
            {
                buttonText.text = objectWithText.Text;
            }*/
            buttonText.text = ">>>";
        }
    }

    public void OnBranchSelected()
    {
        flowPlayer.Play(branch);
    }
}
