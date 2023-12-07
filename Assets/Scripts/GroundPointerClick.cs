using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GroundPointerClick : MonoBehaviour, IPointerClickHandler
{
	public event UnityAction GroundClicked;

	public Vector3 ClickPosition { get; private set; }

	public void OnPointerClick(PointerEventData eventData)
	{
		Ray ray = Camera.main.ScreenPointToRay(eventData.position);

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			ClickPosition = hit.point;

			GroundClicked.Invoke();
		}
	}
}
