using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    public Transform targetPoint;
    public bool changeScene = false;
    public string sceneName;
    public string spawnPointID; // Unique ID for the spawn location in the new scene
    private bool isTeleporting = false;
    public float teleportCooldown = 1f; // Delay before teleporting again

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            StartCoroutine(Teleport(other));
        }
    }

    private IEnumerator Teleport(Collider player)
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
                player.transform.position = targetPoint.position;
            }
        }

        yield return new WaitForSeconds(teleportCooldown);
        isTeleporting = false;
    }
}