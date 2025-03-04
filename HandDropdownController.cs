// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;
// using System.Collections.Generic;

// public class HandDropdownController : MonoBehaviour
// {
//     [Header("Hand Settings")]
//     public OVRHand leftHand;
//     public OVRHand rightHand;
//     public OVRSkeleton skeleton;
//     public float maxDistance = 2f;

//     [Header("Dropdown Settings")]
//     public TMP_Dropdown dropdown;
//     public List<GameObject> targetObjects;

//     private bool isPinching;
//     private int lastSelectedIndex = -1;

//     void Update()
//     {
//         if (!rightHand.IsTracked || skeleton == null) return;

//         // Get hand components
//         bool isPinchingNow = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
//         Vector3 handPos = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position;
//         Vector3 handForward = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.forward;

//         // Raycast for UI interaction
//         RaycastHit hit;
//         if (Physics.Raycast(handPos, handForward, out hit, maxDistance, LayerMask.GetMask("UI")))
//         {
//             HandleDropdownInteraction(hit.collider.gameObject, isPinchingNow);
//         }

//         // Handle pinch release
//         if (!isPinchingNow && isPinching)
//         {
//             ResetSelection();
//         }

//         isPinching = isPinchingNow;
//     }

//     void HandleDropdownInteraction(GameObject uiElement, bool isPinching)
//     {
//         // Get parent dropdown item
//         DropdownItem item = uiElement.GetComponentInParent<DropdownItem>();
//         if (item == null) return;

//         // Highlight on hover
//         int currentIndex = item.transform.GetSiblingIndex();
//         if (currentIndex != lastSelectedIndex)
//         {
//             ResetSelection();
//             item.image.color = Color.cyan;
//             lastSelectedIndex = currentIndex;
//         }

//         // Handle selection
//         if (isPinching)
//         {
//             // For main dropdown button
//             if (item.transform.parent == dropdown.transform)
//             {
//                 dropdown.Show();
//             }
//             // For dropdown options
//             else if (item.transform.parent.parent == dropdown.transform)
//             {
//                 dropdown.value = currentIndex;
//                 dropdown.Hide();
//                 UpdateObjectVisibility(currentIndex);
//             }
//         }
//     }

//     void UpdateObjectVisibility(int index)
//     {
//         for (int i = 0; i < targetObjects.Count; i++)
//         {
//             if (targetObjects[i] != null)
//             {
//                 targetObjects[i].SetActive(i == index);
//             }
//         }
//     }

//     void ResetSelection()
//     {
//         if (lastSelectedIndex != -1)
//         {
//             foreach (DropdownItem item in dropdown.GetComponentsInChildren<DropdownItem>())
//             {
//                 item.image.color = Color.white;
//             }
//             lastSelectedIndex = -1;
//         }
//     }
// }