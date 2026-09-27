using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResizeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Наведение курсора: показываем imgFone
        transform.localScale = new Vector3(1.1f, 1.07f, 1f);
        //print($"Enter to {gameObject.name}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Выход курсора: скрываем imgFone
        transform.localScale = new Vector3(1f, 1f, 1f);
        //print($"Exit from {gameObject.name}");
    }
}
