using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HandPointer : MonoBehaviour
{
    public OVRHand rightHand;
    public GameObject CurrentTarget { get; private set; }

    [SerializeField] private bool showRaycast = true;
    [SerializeField] private Color highlightColor = Color.red;

    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LineRenderer LineRenderer;

    private Color originalColor;
    private Renderer currentRenderer;

    void Update() => CheckHandPointer(rightHand);

    void CheckHandPointer(OVRHand hand)
    {
        if (Physics.Raycast(hand.PointerPose.position, hand.PointerPose.forward, out RaycastHit hit, Mathf.Infinity, targetLayer))
        {
            if (CurrentTarget != hit.transform.gameObject)
            {
                CurrentTarget = hit.transform.gameObject;
                currentRenderer = CurrentTarget.GetComponent<Renderer>();
                originalColor = currentRenderer.material.color;
                currentRenderer.material.color = highlightColor;
            }

            UpdateRayVisualization(hand.PointerPose.position, hit.point, true);
        }
        else if (CurrentTarget != null)
        {
            currentRenderer.material.color = originalColor;
            CurrentTarget = null;
            UpdateRayVisualization(hand.PointerPose.position, hand.PointerPose.position + hand.PointerPose.forward * 1000, false);
        }
        else
        {
            UpdateRayVisualization(hand.PointerPose.position, hand.PointerPose.position + hand.PointerPose.forward * 1000, false);
        }
    }

    void UpdateRayVisualization(Vector3 startPosition, Vector3 endPosition, bool hitSomething)
    {
        if (showRaycast && LineRenderer != null)
        {
            LineRenderer.enabled = true;
            LineRenderer.SetPosition(0, startPosition);
            LineRenderer.SetPosition(1, endPosition);
            LineRenderer.material.color = hitSomething ? Color.green : Color.white;
        }
        else if (LineRenderer != null)
        {
            LineRenderer.enabled = false;
        }
    }
}