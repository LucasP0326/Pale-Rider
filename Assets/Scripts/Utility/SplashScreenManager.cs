using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SplashScreenManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(loadingscene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator loadingscene ()
    {
        yield return new WaitForSeconds(4.5f);
            SceneManager.LoadScene("Main Menu");
    }
}
