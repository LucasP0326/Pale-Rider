using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public SpawnPoint[] spawnPoints; // Assign all possible spawn points in the Inspector

    void Start()
    {
        if (!PlayerPrefs.HasKey("SpawnPoint"))
        {
            PlayerPrefs.SetString("SpawnPoint", "DefaultSpawn"); // Ensure a fallback
        }

        string spawnPointID = PlayerPrefs.GetString("SpawnPoint", "DefaultSpawn");

        foreach (SpawnPoint sp in spawnPoints)
        {
            if (sp.spawnID == spawnPointID)
            {
                GameObject target = GameObject.FindGameObjectWithTag("Player");
                if (target != null)
                {
                    target.transform.position = sp.transform.position;

                    // Ensure the camera still follows the player
                    IsometricCameraFollow cameraFollow = Camera.main.GetComponent<IsometricCameraFollow>();
                    if (cameraFollow != null)
                    {
                        cameraFollow.target = target.transform;
                    }
                }
                break;
            }
        }

        // Clear spawn memory after use (only if coming from a teleport)
        PlayerPrefs.DeleteKey("SpawnPoint");
    }
}
