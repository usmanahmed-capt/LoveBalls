using UnityEngine;
using System.Collections.Generic;

public class Lines : MonoBehaviour {

	public LineRenderer lineRenderer;
	public EdgeCollider2D edgeCollider;
	public Rigidbody2D rigidBody;

	/*[HideInInspector]*/ public List<Vector2> points = new List<Vector2> ( );
	[HideInInspector] public int pointsCount = 0;

	//The minimum distance between line's points.
	float pointsMinDistance = 0.1f;

	//Circle collider added to each line's point
	float circleColliderRadius;
	public bool isShadowlayer;

	public void AddPointLastOnlyDownAndUp(Vector2 newPoint)
	{
		points.Add(newPoint);
		pointsCount++;
		//Line Renderer
		lineRenderer.positionCount = pointsCount;
		lineRenderer.SetPosition(pointsCount - 1, newPoint);
		edgeCollider.points = points.ToArray();
	}

	public void AddPoint ( Vector2 newPoint ) {
		//If distance between last point and new point is less than pointsMinDistance do nothing (return)
		if ( pointsCount >= 1 && Vector2.Distance ( newPoint, GetLastPoint ( ) ) < pointsMinDistance )
			return;

		points.Add ( newPoint );
		pointsCount++;

		//Add Circle Collider to the Point
		CircleCollider2D circleCollider = this.gameObject.AddComponent <CircleCollider2D> ( );
		circleCollider.offset = newPoint;
		circleCollider.radius = circleColliderRadius;

		//Line Renderer
		lineRenderer.positionCount = pointsCount;
		lineRenderer.SetPosition ( pointsCount - 1, newPoint );

		//Edge Collider
		//Edge colliders accept only 2 points or more (we can't create an edge with one point :D )
		if ( pointsCount > 1 )
			edgeCollider.points = points.ToArray ( );
	}

	public void AddPointToShow(Vector2 newPoint)
	{
		//If distance between last point and new point is less than pointsMinDistance do nothing (return)
		if (pointsCount >= 1 && Vector2.Distance(newPoint, GetLastPoint()) < pointsMinDistance)
			return;

		points.Add(newPoint);
		pointsCount++;

		//Add Circle Collider to the Point
		//CircleCollider2D circleCollider = this.gameObject.AddComponent<CircleCollider2D>();
		//circleCollider.offset = newPoint;
		//circleCollider.radius = circleColliderRadius;

		//Line Renderer
		lineRenderer.positionCount = pointsCount;
		lineRenderer.SetPosition(pointsCount - 1, newPoint);

		//Edge Collider
		//Edge colliders accept only 2 points or more (we can't create an edge with one point :D )
		if (pointsCount > 1)
			edgeCollider.points = points.ToArray();
	}

	public Vector2 GetLastPoint ( ) {
		return ( Vector2 )lineRenderer.GetPosition ( pointsCount - 1 );
	}

	public void UsePhysics ( bool usePhysics ) {
		// isKinematic = true  means that this rigidbody is not affected by Unity's physics engine
		rigidBody.isKinematic = !usePhysics;
	}

	public void SetLineColor ( Gradient colorGradient ) {
		lineRenderer.colorGradient = colorGradient;
	}

	public void SetPointsMinDistance ( float distance ) {
		pointsMinDistance = distance;
	}

	public void SetLineWidth ( float width ) {
		lineRenderer.startWidth = width;
		lineRenderer.endWidth = width;

		circleColliderRadius = width / 2f;

		edgeCollider.edgeRadius = circleColliderRadius;
	}
	public void ClearPoints()
	{
		points.Clear(); // Clear the list of points
		pointsCount = 0; // Reset the point count

		lineRenderer.positionCount = 0; // Clear the LineRenderer's points

		if (edgeCollider != null)
		{
			edgeCollider.points = null; // Clear the EdgeCollider's points
		}
		if (edgeCollider != null)
		{
			edgeCollider.points = null; // Clear the EdgeCollider's points
		}

		// Optionally, clear any associated circle colliders here
	}

	public void AddPoints(List<Vector2> newPoints)
	{
		foreach (Vector2 point in newPoints)
		{
			AddPoint(point);
		}
	}

	public float GetLineLength()
	{
		Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);
        return lineRenderer.positionCount > 1 ? positions[lineRenderer.positionCount - 1].magnitude : 0f;
	}

	private void Update()
    {
		
	}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (isShadowlayer)
    //    {
    //        GamePlayController.Instance.shadowWithCollision = true;
    //    }
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (isShadowlayer)
    //    {
    //        GamePlayController.Instance.shadowWithCollision = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (isShadowlayer)
    //    {
    //        Debug.LogError("Exit");
    //        GamePlayController.Instance.shadowWithCollision = false;
    //    }
    //}


}
