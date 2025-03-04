using UnityEngine;
using UnityEngine.Events;

public class ButtonPointer  : MonoBehaviour
{
    public OVRHand rightHand;
    public GameObject CurrentTarget { get; private set; }

    [SerializeField] private bool showRaycast = true;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float pinchThreshold = 0.9f;

    private Color originalColor;
    private Renderer currentRenderer;
    private bool wasPinching;

    void Update()
    {
        CheckHandPointer(rightHand);
        CheckForClick();
    }

    void CheckHandPointer(OVRHand hand)
    {
        if (Physics.Raycast(hand.PointerPose.position, hand.PointerPose.forward, 
            out RaycastHit hit, Mathf.Infinity, targetLayer))
        {
            HandleTargetUpdate(hit.transform.gameObject);
            UpdateRayVisualization(hand.PointerPose.position, hit.point, true);
        }
        else
        {
            ClearTarget();
            UpdateRayVisualization(hand.PointerPose.position, 
                hand.PointerPose.position + hand.PointerPose.forward * 1000, false);
        }
    }

    void CheckForClick()
    {
        bool isPinching = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        
        if (isPinching && !wasPinching && CurrentTarget != null)
        {
            HandleClick();
        }
        
        wasPinching = isPinching;
    }

    void HandleTargetUpdate(GameObject newTarget)
    {
        if (CurrentTarget != newTarget)
        {
            ClearTarget();
            CurrentTarget = newTarget;
            currentRenderer = CurrentTarget.GetComponent<Renderer>();
            
            if (currentRenderer != null)
            {
                originalColor = currentRenderer.material.color;
                currentRenderer.material.color = Color.red;
            }
        }
    }

    void HandleClick()
    {
        var clickable = CurrentTarget.GetComponent<IClickable>();
        if (clickable != null)
        {
            clickable.OnClick();
        }
    }

    void ClearTarget()
    {
        if (currentRenderer != null)
        {
            currentRenderer.material.color = originalColor;
        }
        CurrentTarget = null;
        currentRenderer = null;
    }

    void UpdateRayVisualization(Vector3 start, Vector3 end, bool hit)
    {
        if (!showRaycast || lineRenderer == null) return;
        
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.material.color = hit ? Color.green : Color.white;
    }
}

// Interface for clickable objects
public interface IClickable
{
    void OnClick();
}

// Put this on any object you want to make clickable