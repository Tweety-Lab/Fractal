using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindow : MonoBehaviour
{
    [SerializeField] private RectTransform dragRectTransform;
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        if (dragRectTransform == null)
        {
            dragRectTransform = transform.parent.GetComponent<RectTransform>();
        }
    }
    public void OnDrag(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        //Vector2 pos;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(
        //    (RectTransform)canvas.transform,
        //    pointerData.position,
        //    canvas.worldCamera,
        //    out pos);
        //transform.position = canvas.transform.TransformPoint(pos);
        dragRectTransform.anchoredPosition += pointerData.delta / canvas.scaleFactor;
    }
    public void MoveAbove()
    {
        dragRectTransform.SetAsLastSibling();
    }
}
