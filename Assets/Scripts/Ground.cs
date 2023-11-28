using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Ground : MonoBehaviour, IPointerClickHandler
{
	public event UnityAction Clicked;

	public Vector3 HitPosition { get; private set; }

	public void OnPointerClick(PointerEventData eventData)
	{
		Ray ray = Camera.main.ScreenPointToRay(eventData.position);

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			HitPosition = hit.point;

			Clicked.Invoke();
		}
	}
}
