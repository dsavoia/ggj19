using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour {

    protected struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public const float skinWidth = .005f;
    const float dstBetweenRays = .25f;

    protected RaycastOrigins raycastOrigins;

    protected float horizontalRaySpacing;
    protected float verticalRaySpacing;

    public new BoxCollider2D collider;

    public LayerMask collisionMask;

    [HideInInspector]
    public int horizontalRayCount;
    [HideInInspector]
    public int verticalRayCount;

    protected virtual void Awake()
    {
        collider = GetComponent<BoxCollider2D>();        
    }

    public virtual void Start()
    {
        CalculateRaySpacing();
    }

    protected void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    protected void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;


        horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }    
}
