using UnityEngine;
using UnityEngine.EventSystems;

public class Ground : MonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		Ray ray = Camera.main.ScreenPointToRay(eventData.position);

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			Debug.Log(hit.point);
		}
	}
}
