using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InfoAtaque : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI   info;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Ratón encima del botón");
        // Acción al pasar el ratón
        info.gameObject.SetActive(true); //se muestra la informacion del ataque
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Ratón fuera del botón");
        // Acción al salir
        info.gameObject.SetActive(false); //se vuelve a ocultar la información del ataque 
    }
}

