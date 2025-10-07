using UnityEngine;

public class HandPinchDetector : MonoBehaviour
{
    [SerializeField] private HandPointer handPointer; 
    [SerializeField] private AudioClip pinchSound;
    [SerializeField] private AudioClip releaseSound;

    private bool hasPinched;
    private bool isIndexFingerPinching;
    private float pinchStrength;
    private OVRHand.TrackingConfidence _confidence;

    private Vector3 objectCenterOffset;
    private GameObject grabbedObject; // Track the grabbed object

    void Update()
    {
        CheckPinch(handPointer.rightHand);
    }

    void CheckPinch(OVRHand hand)
    {
        pinchStrength = hand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
        isIndexFingerPinching = hand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        _confidence = hand.GetFingerConfidence(OVRHand.HandFinger.Index);

        // Check for pinch start
        if (!hasPinched && isIndexFingerPinching && _confidence == OVRHand.TrackingConfidence.High)
        {
            if (handPointer.CurrentTarget != null)
            {
                grabbedObject = handPointer.CurrentTarget;
                hasPinched = true;
                objectCenterOffset = grabbedObject.transform.position - hand.transform.position;
                grabbedObject.GetComponent<AudioSource>().PlayOneShot(pinchSound);
            }
        }

        // If pinching and has grabbed an object
        if (hasPinched)
        {
            // Update the object's position based on hand movement
            grabbedObject.transform.position =  hand.transform.position + objectCenterOffset;
            // Update material properties
            Material currentMaterial = grabbedObject.GetComponent<Renderer>().material;
            currentMaterial.SetFloat("_Metallic", pinchStrength);

            // Check for release
            if (!isIndexFingerPinching || _confidence != OVRHand.TrackingConfidence.High)
            {
                grabbedObject.GetComponent<AudioSource>().PlayOneShot(releaseSound);
                hasPinched = false;
                grabbedObject = null;
            }
        }
        else
        {
            // Update material for current target if not pinching
            if (handPointer.CurrentTarget != null)
            {
                Material currentMaterial = handPointer.CurrentTarget.GetComponent<Renderer>().material;
                currentMaterial.SetFloat("_Metallic", pinchStrength);
            }
        }
    }
}