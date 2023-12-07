using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BasePointerClick : MonoBehaviour, IPointerClickHandler
{
	public event UnityAction BaseClicked;

	public void OnPointerClick(PointerEventData eventData)
	{
		BaseClicked?.Invoke();
	}
}
