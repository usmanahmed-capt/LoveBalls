using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class LinesDrawer : MonoBehaviour {

	public GameObject linePrefab;
	public GameObject shadowLinePrefab;
	public LayerMask cantDrawOverLayer;
	// In LinesDrawer.cs
	public LayerMask obstacleLayerMask;
	int cantDrawOverLayerIndex;
	int obstacleLayerIndex;

	[Space ( 30f )]
	public Gradient lineColor;

	[Space(30f)]
	public Gradient shadowColor;

	
	float fillBarPercentage = 1f;
	int starsRemaining = 3;

	public float linePointsMinDistance;
	public float lineWidth;
	private Vector2 shadowLineStartPosition;
	public bool shadowLineCollides = false;
	Lines currentLine;
	Lines shadowLine;

	Camera cam;
	ContactFilter2D contactFilter = new ContactFilter2D();
    public	List<Collider2D> results = new List<Collider2D>(); // Adjust array size as needed
	public Animator PenAnim;
	bool isFingerMoving = false;
	Vector2 lastMousePosition;
	private float fillAmount = 1f;
	public float fillReductionPerDistance = 0.009f; // Adjust this for desired reduction per unit distance



	void Start ( ) {
		cam = Camera.main;
		cantDrawOverLayerIndex = LayerMask.NameToLayer ("Ball");
		obstacleLayerIndex = LayerMask.NameToLayer("Obstacle");
		fillBarPercentage = 1f;
		GamePlayController.Instance.fillBar.fillAmount = 1f;
		for (int i = 0; i < GamePlayController.Instance.stars.Length; i++) GamePlayController.Instance.stars[i].SetActive(true);
	}

	void Update ( ) {

		if (GamePlayController.Instance.IsWin || GamePlayController.Instance.IsLose)
			return;

		if ( Input.GetMouseButtonDown ( 0 ) )
			BeginDraw ( );
		if (currentLine != null)
		{
			Draw();
		

			// Check for finger/mouse movement
//#if UNITY_EDITOR
			Vector2 currentMousePosition;
			currentMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
//#else
//Vector2 currentMousePosition;
//        if (Input.touchCount > 0)
//        {
//            Touch touch = Input.GetTouch(0);
//            if (touch.phase == TouchPhase.Moved)
//            {
//                currentMousePosition = cam.ScreenToWorldPoint(touch.position);
//			}
//        }
//#endif
			isFingerMoving = (currentMousePosition != lastMousePosition);

			IsAnimActive(isFingerMoving, true, currentMousePosition);

			if (isFingerMoving && !GamePlayController.Instance.shadowLineCollides)
			{
				// Calculate distance moved since last frame
				Vector2 distanceMoved = currentMousePosition - lastMousePosition;
				float distanceMagnitude = distanceMoved.magnitude;

				// Reduce fill amount based on distance traveled
				fillAmount -= fillReductionPerDistance * distanceMagnitude;
				fillAmount = Mathf.Clamp(fillAmount, 0f, 1f);
				GamePlayController.Instance.fillBar.fillAmount = fillAmount;
				GamePlayController.Instance.ReducedStar(fillAmount);
			}
			lastMousePosition = currentMousePosition;
		}
		if ( Input.GetMouseButtonUp ( 0 ) )
			EndDraw ( );
		
	}




	// Begin Draw ----------------------------------------------
	void BeginDraw ( ) {

	Vector2	currentMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
		lastMousePosition = currentMousePosition;
		currentLine = Instantiate ( linePrefab, this.transform ).GetComponent <Lines> ( );
		results.Clear();
		//Set line properties
		currentLine.UsePhysics ( false );
		currentLine.SetLineColor ( lineColor );
		currentLine.SetPointsMinDistance ( linePointsMinDistance );
		currentLine.SetLineWidth ( lineWidth );
	}
	// Draw ----------------------------------------------------
	void Draw ( ) 
	{
		Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
		// Check if mousePos hits any collider with layers "CantDrawOver" or "Obstacle"
		RaycastHit2D hit = Physics2D.CircleCast(mousePosition, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer | obstacleLayerMask);
        if (hit)
		{
			if (hit.collider.gameObject.layer == cantDrawOverLayerIndex)
			{
                // End the draw if it hits the "CantDrawOver" layer
                if (!GamePlayController.Instance.shadowLineCollides)
                    EndDraw();

			}
			else if (hit.collider.gameObject.layer == obstacleLayerIndex)
			{
				// Create shadow line only if not already created
				if (shadowLine == null)
				{
					shadowLine = Instantiate(shadowLinePrefab, this.transform).GetComponent<Lines>();
					shadowLine.UsePhysics(true); // Disable physics for shadow lines
					shadowLine.SetLineColor(shadowColor); // Set desired shadow color
					shadowLine.SetLineWidth(lineWidth);
					shadowLineStartPosition = currentLine.GetLastPoint(); // Store starting position
					CheckShadowLineCollisions(shadowLine);
				}
				shadowLine.ClearPoints(); // Clear previous shadow line points
				shadowLine.AddPointToShow(shadowLineStartPosition); // Add starting position
				shadowLine.AddPointToShow(mousePosition); // Add current mouse positi

			}
		}
		else
		{
			// Check for collisions along the entire shadow line
			if (shadowLine != null)
			{
				CheckShadowLineCollisions(shadowLine);

				if (!GamePlayController.Instance.shadowLineCollides)
				{ // Only add if no collisions detected
                    shadowLine.AddPoint(shadowLine.GetLastPoint());
                    currentLine.AddPoints(shadowLine.points);
                    Destroy(shadowLine.gameObject);
                    shadowLine = null;
                }
				else
				{
					shadowLine.ClearPoints(); // Clear previous shadow line points
					shadowLine.AddPointToShow(shadowLineStartPosition); // Add starting position
					shadowLine.AddPointToShow(mousePosition); // Add current mouse position
				}
			}else
				currentLine.AddPoint(mousePosition);
		}
	}
	RaycastHit2D hit;
	Vector2 raycastPosition;
	void CheckShadowLineCollisions(Lines shadowLine)
    {
        GamePlayController.Instance.shadowLineCollides = false; // Reset flag for each check
        List<Vector2> shadowLinePoints = shadowLine.points;
		float lineWith = lineWidth / 3f;

		for (int i = 1; i < shadowLinePoints.Count; i++)
        {
            Vector2 startPoint = shadowLinePoints[i - 1];
            Vector2 endPoint = shadowLinePoints[i];

            // Perform raycasts along the segment with a certain step size
            float raycastDistance = Vector2.Distance(startPoint, endPoint);
           // int numRaycasts = Mathf.RoundToInt(raycastDistance / lineWidth); // Adjust as needed
            int numRaycasts = Mathf.Max(10, Mathf.RoundToInt(raycastDistance / lineWidth * 2f)); // More raycasts for longer segments
            float stepSize = raycastDistance / numRaycasts;
            //Debug.Log(numRaycasts);
            for (int j = 0; j < numRaycasts; j++)
            {
                 raycastPosition = startPoint + (stepSize * j) * (endPoint - startPoint).normalized;
				hit = Physics2D.CircleCast(raycastPosition, lineWith, Vector2.zero, 1f, cantDrawOverLayer | obstacleLayerMask);
                Debug.DrawRay(raycastPosition, (endPoint - startPoint).normalized * raycastDistance, Color.green);
                if (hit)
                {
                    GamePlayController.Instance.shadowLineCollides = true;
                    break; // Exit the inner loop if a collision is found
                }
            }

            if (GamePlayController.Instance.shadowLineCollides)
                break; // Exit the outer loop if a collision is found
        }
    }
	// End Draw ------------------------------------------------
	void EndDraw ( ) {
		IsAnimActive(false, false,Vector2.zero);
		if ( currentLine != null ) {

			if ( currentLine.pointsCount < 2 ) {
				//If line has one point
				//  Destroy(currentLine.gameObject);
				currentLine.AddPointLastOnlyDownAndUp(currentLine.GetLastPoint());
				//Add the line to "CantDrawOver" layer
				currentLine.gameObject.layer = cantDrawOverLayerIndex;
				//Activate Physics on the line
				currentLine.UsePhysics(true);

				currentLine = null;


				if (shadowLine != null)
				{
					Destroy(shadowLine.gameObject);
					shadowLine = null;
				}
			} else {
                //Add the line to "CantDrawOver" layer
                currentLine.gameObject.layer = cantDrawOverLayerIndex;
				//Activate Physics on the line
				currentLine.UsePhysics ( true );

				currentLine = null;
				if (shadowLine != null)
				{
					Destroy(shadowLine.gameObject);
					shadowLine = null;
				}
			}
		}
	}
	

	public void IsAnimActive(bool canAnimActive, bool IsObjectAtiveOnDownAndDisableOnUp,Vector2 positionPen)
	{
		if (canAnimActive && IsObjectAtiveOnDownAndDisableOnUp)
		{
			if (!PenAnim.gameObject.activeSelf)
				PenAnim.gameObject.SetActive(true);
			if (!PenAnim.enabled)
				PenAnim.enabled = true;

           // Calculate fill bar reduction based on movement


            PenAnim.transform.position = positionPen;
		}
		else if (!canAnimActive && IsObjectAtiveOnDownAndDisableOnUp)
		{
			if (PenAnim.enabled)
				PenAnim.enabled = false;

			PenAnim.transform.position = positionPen;
		}
		else if (!canAnimActive &&!IsObjectAtiveOnDownAndDisableOnUp)
		{
				PenAnim.gameObject.SetActive(false);
				PenAnim.enabled = false;
		}
	}
}
