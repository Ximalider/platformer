using UnityEngine;

public class CollisionDataRetriever : MonoBehaviour
{
    [SerializeField, Range(0f, 90f)] private float walkableAngle = 40f;
    [SerializeField, Range(0f, 90f)] private float wallAngle = 90f;
    public bool OnGround { get; private set; }
    public bool OnWall { get; private set; }

    public float Friction { get; private set; }

    public Vector2 ContactNormal { get; private set; }

    private PhysicsMaterial2D _material;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        OnGround = false;
        Friction = 0;
        OnWall = false;
    }
    public void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactNormal = collision.GetContact(i).normal;
            OnGround |= ContactNormal.y > (90 - walkableAngle) * Mathf.Deg2Rad;
            OnWall = !OnGround && ContactNormal.y > (90 - wallAngle) * Mathf.Deg2Rad;
        }
    }
    private void RetrieveFriction(Collision2D collision)
    {
        _material = collision.collider.sharedMaterial;
        Friction = 0;

        if (_material)
        {
            Friction = _material.friction;
        }
    }
}
