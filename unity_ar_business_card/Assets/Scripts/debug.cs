using UnityEngine;
using UnityEngine.EventSystems;


public class debug : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down fired on: " + gameObject.name);
    }
}

