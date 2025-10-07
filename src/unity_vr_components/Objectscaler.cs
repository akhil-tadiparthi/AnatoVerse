using UnityEngine;

public class ObjectScaler : MonoBehaviour
{
    [SerializeField] private OVRHand leftHand;
    [SerializeField] private OVRHand rightHand;
    [SerializeField] private HandPointer rightHandPointer;
    
    private float initialDistance;
    private Vector3 initialScale;
    private bool wasBothPinching;

    void Update()
    {
        bool leftPinching = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        bool rightPinching = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        GameObject currentTarget = rightHandPointer.CurrentTarget;

        if (currentTarget != null)
        {
            if (leftPinching && rightPinching && !wasBothPinching)
            {
                // Scaling started
                initialDistance = Vector3.Distance(leftHand.PointerPose.position, rightHand.PointerPose.position);
                initialScale = currentTarget.transform.localScale;
                wasBothPinching = true;
            }
            else if (leftPinching && rightPinching && wasBothPinching)
            {
                // Calculate current scale
                float currentDistance = Vector3.Distance(leftHand.PointerPose.position, rightHand.PointerPose.position);
                float scaleFactor = currentDistance / initialDistance;
                currentTarget.transform.localScale = initialScale * scaleFactor;
            }
            else
            {
                wasBothPinching = false;
            }
        }
    }
}