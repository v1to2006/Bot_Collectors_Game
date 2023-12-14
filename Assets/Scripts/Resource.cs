using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Resource : MonoBehaviour
{
    private BoxCollider _boxCollider;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public BoxCollider BoxCollider() => _boxCollider;
    public Rigidbody Rigidbody() => _rigidbody;
}
