using UnityEngine;
using UnityEngine.Events;
public class VRButton : MonoBehaviour, IClickable
{
    public UnityEvent onClickEvent;

    public void OnClick()
    {
        Debug.Log($"Clicked {name}");
        onClickEvent?.Invoke();
    }
}