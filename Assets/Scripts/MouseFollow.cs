using System.Collections;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    public IEnumerator FollowMouse(Transform targetObject)
    {
        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetObject.transform.position = hit.point;
            }

            yield return null;
        }
    }
}
