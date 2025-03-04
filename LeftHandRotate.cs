using UnityEngine;

public class LeftHandRotationController : MonoBehaviour
{
    [SerializeField] private OVRHand leftHand;
    [SerializeField] private HandPointer rightHandPointer;
    [SerializeField] private float rotationSpeed = 1f;
    
    private Quaternion previousRotation;
    private bool wasPinching;

      private OVRHand.TrackingConfidence _confidence;

    void Update()
    {
        bool isPinching = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        

        if (_confidence == OVRHand.TrackingConfidence.High && rightHandPointer.CurrentTarget != null)
        {
            if (isPinching && !wasPinching)
            {
                // Rotation started
                previousRotation = leftHand.PointerPose.rotation;
            }
            else if (isPinching && wasPinching)
            {
                // Calculate rotation delta
                Quaternion deltaRotation = leftHand.PointerPose.rotation * Quaternion.Inverse(previousRotation);
                rightHandPointer.CurrentTarget.transform.rotation *= deltaRotation;
                previousRotation = leftHand.PointerPose.rotation;
            }
        }
        wasPinching = isPinching;
    }
}