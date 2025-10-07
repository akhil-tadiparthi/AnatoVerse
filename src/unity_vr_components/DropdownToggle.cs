using UnityEngine;
using UnityEngine.UI;

public class ButtonToggleManager : MonoBehaviour
{
    [System.Serializable]
    public class ToggleEntry
    {
        public Button button; // Assign your UI button in the Inspector
        public GameObject targetObject; // Assign the object to toggle
    }

    public ToggleEntry[] toggleEntries; // List of button-object pairs

    void Start()
    {
        // Link each button to its object
        foreach (ToggleEntry entry in toggleEntries)
        {
            entry.button.onClick.AddListener(() => ToggleObject(entry.targetObject));
        }
    }

    private void ToggleObject(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
}