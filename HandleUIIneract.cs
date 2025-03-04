using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class HandUIInteractor : MonoBehaviour
{
    [Header("Hand Settings")]
    public OVRHand hand;
    public LayerMask uiLayer;
    public float maxDistance = 2f;

    [Header("Dropdown Reference")]
    public TMP_Dropdown targetDropdown;
    
    private GameObject currentHoveredElement;
    private bool wasPinching;

    void Update()
    {
        if (!hand.IsTracked) return;

        bool isPinching = hand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        CheckUIDropdownInteraction(isPinching);
    }

    void CheckUIDropdownInteraction(bool isPinching)
    {
        RaycastHit hit;
        bool hitUI = Physics.Raycast(hand.PointerPose.position, 
                                    hand.PointerPose.forward, 
                                    out hit, 
                                    maxDistance, 
                                    uiLayer);

        // Hover state
        if (hitUI && hit.collider.CompareTag("UIDropdown"))
        {
            HandleDropdownHover(hit.collider.gameObject);
        }
        else
        {
            ClearHoverState();
        }

        // Pinch interaction
        if (isPinching && !wasPinching && hitUI)
        {
            HandleDropdownClick(hit.collider.gameObject);
        }

        wasPinching = isPinching;
    }

    void HandleDropdownHover(GameObject uiElement)
    {
        if (currentHoveredElement != uiElement)
        {
            ClearHoverState();
            currentHoveredElement = uiElement;
            // Add hover effect (e.g., scale up)
            uiElement.transform.localScale *= 1.1f;
        }
    }

    void ClearHoverState()
    {
        if (currentHoveredElement != null)
        {
            currentHoveredElement.transform.localScale /= 1.1f;
            currentHoveredElement = null;
        }
    }

    void HandleDropdownClick(GameObject uiElement)
    {
        if (uiElement.name == "Dropdown_Header")
        {
            // Toggle dropdown open/close
            targetDropdown.Show();
        }
        else if (uiElement.name.Contains("Item"))
        {
            // Select dropdown option
            int selectedIndex = targetDropdown.options.FindIndex(
                option => option.text == uiElement.GetComponentInChildren<TMP_Text>().text
            );
            targetDropdown.value = selectedIndex;
            targetDropdown.Hide();
        }
    }
}