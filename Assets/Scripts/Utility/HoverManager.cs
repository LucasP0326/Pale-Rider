using UnityEngine;

public class HoverManager : MonoBehaviour
{
    public LayerMask interactableLayer = ~0;
    public float maxDistance = 100f;
    Camera cam;
    Interactable current;

    void Awake() => cam = Camera.main;

    void Update()
    {
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableLayer))
        {
            Interactable it = hit.collider.GetComponentInParent<Interactable>();
            if (it != current)
            {
                if (current != null) current.SetHover(false);
                current = it;
                if (current != null) current.SetHover(true);
            }
        }
        else
        {
            if (current != null)
            {
                current.SetHover(false);
                current = null;
            }
        }
    }
}