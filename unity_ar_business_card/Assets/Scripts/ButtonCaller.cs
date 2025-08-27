using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonCaller : MonoBehaviour, IPointerClickHandler
{
    public int linkIndex;
    [SerializeField] private ButtonsLinks linkHandler;


    public void OnPointerClick(PointerEventData eventData)
    {
        linkHandler?.LinkToPage(linkIndex);
    }
}
