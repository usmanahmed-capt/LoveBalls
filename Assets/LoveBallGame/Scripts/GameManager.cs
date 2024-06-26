using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;
public class GameManager : MonoBehaviour
{

    [Tooltip("The color of the drawn lines")]
    public static GameManager Instance;
    public Color lineColor;
    public Material lineMaterial;
    public Transform Pencil;

    public SpriteRenderer penRendere;
    public SpriteRenderer penChildRendere;
    public List<GameObject> listLine = new List<GameObject>();
    public List<Vector2> listPoint = new List<Vector2>();
    private GameObject currentLine;
    public GameObject currentColliderObject;
    private GameObject hintTemp;
    public GameObject[] Balls;
    public TrailRenderer[] BallsLineRendere;
    public PhysicsMaterial2D customMaterial; // Reference to your custom material
    private Vector3 LastMosPos;
    private BoxCollider2D currentBoxCollider2D;
    private LineRenderer lines;
    private LineRenderer currentLineRenderer;
    private bool stopHolding;
    private bool allowDrawing = true;
    [HideInInspector]
    public bool completed;
    int clickCont;
    private List<Rigidbody2D> listObstacleNonKinematic = new List<Rigidbody2D>();
    private GameObject[] obstacles;
    float mosDis;
    bool canCreate;
    RaycastHit2D hit_1;
    RaycastHit2D hit_2;
    RaycastHit2D hit_3;
    GameObject TemLine;
    public float SfxSoundLevel;
    public GameObject PenSound;

    public GameObject[] Background;

    public GameObject[] Hint;
    public bool CanDrawPen;
    private List<Vector3>ballInitialPosition = new List<Vector3>();
    private Vector3 penInitialPosition;
    internal List<Ball> ballsScripts = new List<Ball>();
    public SpriteRenderer PaperBg;
    public Rigidbody2D[] MoveAbleObject;
    public Transform[] MoveAbleObjecttrns;
    public HingeJoint2D[] MoveAbleObjectHinge;
    private List<Vector3> MoveAbleInitialPosition = new List<Vector3>();
    private List<Quaternion> MoveAbleInitialRotation = new List<Quaternion>();
    public bool IsLineMaterialsNotAdded;

    public bool IsBannerOnThislvl;
    private TrignometricRotation PencileTringo;
    private void OnEnable()
    {
        
        
    }
    private void Awake()
    {
        Instance = this;

        if (PlayerPrefs.GetInt("FirstTime") == 0)
        {
            PlayerPrefs.SetString("FileName", "Lovebirds" + Random.Range(100f, 500f));
            PlayerPrefs.SetInt("FirstTime", 1);
            Debug.Log(PlayerPrefs.GetString("FileName"));
        }
    }
    void Start()
    {
      
        Pencil.gameObject.SetActive(false);
        for (int i = 0; i < Balls.Length; i++)
        {
            Balls[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            ballInitialPosition.Add(Balls[i].transform.localPosition);
            ballsScripts.Add(Balls[i].GetComponent<Ball>());
        }
        if (MoveAbleObject.Length > 0)
        {
            for (int i = 0; i < MoveAbleObject.Length; i++)
            {
                MoveAbleObject[i].bodyType = RigidbodyType2D.Static;
                MoveAbleInitialPosition.Add(MoveAbleObjecttrns[i].localPosition);
                MoveAbleInitialRotation.Add(MoveAbleObjecttrns[i].localRotation);
            }
        }
       
        penInitialPosition = Pencil.transform.position;
        lineMaterial.SetColor("_Color", lineColor);
        PencileTringo = Pencil.GetComponent<TrignometricRotation>();
        //Obs = GameObject.FindGameObjectsWithTag("Obstacle");
        //for (int i = 0; i < Obs.Length; i++)
        //{
        //    Obs[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        //}
    }



    void Update()
    {
        //if (PenCapacity.value <= 0.01f || !Input.GetMouseButton(0))
        //{
        //    Pencil.gameObject.SetActive(false);
        //}
        if (!GameController.Instance.CanPlayOn)
        {
            allowDrawing = false;
            PenSound.SetActive(false);
            Pencil.gameObject.SetActive(false);
            if (TemLine)
                Destroy(TemLine);

            return;
        }
        if (GamePlayController.Instance.IsLose || GamePlayController.Instance.IsWin || !CanDrawPen)
        {
            allowDrawing = false;
            canCreate = false;
            PenSound.SetActive(false);
            Pencil.gameObject.SetActive(false);
            if (TemLine)
                Destroy(TemLine);

            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            GameObject thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;     //Get the button on click
            if (thisButton != null)                                                                             //Is click on button
            {
                allowDrawing = false;
                // print("cant drwa");
            }
            else                                                                                                //Not click on button
            {
                DisableHind();
                allowDrawing = true;
                stopHolding = false;
                listPoint.Clear();
                CreateLine(Input.mousePosition);
                // print("draw");
            }
        }
        else if (Input.GetMouseButton(0) && !stopHolding && allowDrawing /*&& PenCapacity.value > 0*/)
        {
            
            RaycastHit2D rayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (LastMosPos != Camera.main.ScreenToWorldPoint(Input.mousePosition))
            {
                Pencil.gameObject.SetActive(true);
                Pencil.position = new Vector3(LastMosPos.x, LastMosPos.y, 0);
                if (rayHit.collider == null)
                {
                    //Pencil.gameObject.SetActive(true);
                    //Pencil.position = new Vector3(LastMosPos.x, LastMosPos.y, 0);
                    if (canCreate == false /*&& listPoint.Count >= 2*/)
                    {
                        PenSound.SetActive(true);
                        float dist = Vector3.Distance(LastMosPos, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                        PencileTringo.enabled = true;
                        GamePlayController.Instance.PenCapacity.value = GamePlayController.Instance.PenCapacity.value - dist / 50;
                        GamePlayController.Instance.FillBarOfCapacity.fillAmount = GamePlayController.Instance.PenCapacity.value;
                        GamePlayController.Instance.PenPercent.text = Mathf.FloorToInt(GamePlayController.Instance.PenCapacity.value * 100).ToString() + " %";
                        if (Mathf.FloorToInt(GamePlayController.Instance.PenCapacity.value * 100) < 71)
                        {
                            GamePlayController.Instance.StarCount = 2;
                            GamePlayController.Instance.DisableStarOfBar(3);
                        }
                        if (Mathf.FloorToInt(GamePlayController.Instance.PenCapacity.value * 100) < 41)
                        {
                            GamePlayController.Instance.StarCount = 1;
                            GamePlayController.Instance.DisableStarOfBar(2);
                        }
                        //if (Mathf.FloorToInt(PenCapacity.value * 100) < 25)
                        //{
                        //    Star1.gameObject.SetActive(false);
                        //}
                    }
                    else 
                    {
                        PenSound.SetActive(false);
                    }
                }
                else 
                {
                    Pencil.position = new Vector3(LastMosPos.x, LastMosPos.y, 0);
                    PenSound.SetActive(false);
                }
            }
            else
            {
                PencileTringo.enabled = false;
            }

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float ab = Vector2.Distance(LastMosPos, mousePos);
                mosDis = mosDis + ab;

            if (!listPoint.Contains(mousePos) && mosDis > .002f)
            {
                mosDis = 0;
                if (canCreate == false)
                {
                    if (rayHit.collider == null)
                    {
                        listPoint.Add(mousePos);             
                                                                               //currentLineRenderer.positionCount = listPoint.Count;
                                                                               //currentLineRenderer.SetPosition(listPoint.Count - 1, listPoint[listPoint.Count - 1]);
                    }
                }
                //Create collider
                if (listPoint.Count >= 2)
                {
                    if (canCreate == false)
                    {
                        if (rayHit.collider == null)
                        {
                            Vector2 point_1 = listPoint[listPoint.Count - 2];
                            Vector2 point_2 = listPoint[listPoint.Count - 1];

                            currentColliderObject = new GameObject("Collider");
                            currentColliderObject.transform.position = (point_1 + point_2) / 2;
                            currentColliderObject.transform.right = (point_2 - point_1).normalized;
                            currentColliderObject.transform.SetParent(currentLine.transform);

                            currentBoxCollider2D = currentColliderObject.AddComponent<BoxCollider2D>();
                            currentBoxCollider2D.enabled = false;
                            currentBoxCollider2D.size = new Vector2((point_2 - point_1).magnitude, 0.05f);
                            
                            if (currentLine.transform.childCount > 1)
                            {
                                lines = currentColliderObject.AddComponent<LineRenderer>(); //Comment Below Lines If you want the line render to be in parent 
                                lines.enabled = false;
                                lines.sharedMaterial = lineMaterial;
                                lines.positionCount = 2;
                                lines.startWidth = 0.09f;
                                lines.endWidth = 0.09f;
                                lines.startColor = lineColor;
                                lines.endColor = lineColor;
                                lines.useWorldSpace = true;
                                lines.numCapVertices = 90;
                                lines.sortingOrder = 167;
                                currentColliderObject.AddComponent<LineRendParent>();
                            }
                        }
                    }
                    Vector2 rayDirection;
                    if (canCreate == false)
                    {
                        rayDirection = currentColliderObject.transform.TransformDirection(Vector2.right);
                    }
                    else
                    {
                        rayDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - currentColliderObject.transform.position;
                    }

                    Vector2 pointDir = currentColliderObject.transform.TransformDirection(Vector2.up);
                    Vector2 rayPoint_1 = ((Vector2)currentColliderObject.transform.position);
                    Vector2 rayPoint_2 = ((Vector2)currentColliderObject.transform.position + pointDir * (currentBoxCollider2D.size.y / 2f));
                    Vector2 rayPoint_3 = ((Vector2)currentColliderObject.transform.position + (-pointDir) * (currentBoxCollider2D.size.y / 2f));

                    float rayLength = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - rayPoint_1).magnitude;

                    hit_1 = Physics2D.Raycast(rayPoint_1, rayDirection, rayLength);
                    hit_2 = Physics2D.Raycast(rayPoint_2, rayDirection, rayLength);
                    hit_3 = Physics2D.Raycast(rayPoint_3, rayDirection, rayLength);

                    Debug.DrawRay(rayPoint_1, rayDirection, Color.red, rayLength);
                    Debug.DrawRay(rayPoint_2, rayDirection, Color.green, rayLength);
                    Debug.DrawRay(rayPoint_3, rayDirection, Color.green, rayLength);

                    if (hit_1.collider != null || hit_2.collider != null || hit_3.collider != null)
                    {
                        GameObject hit = (hit_1.collider != null) ? (hit_1.collider.gameObject) : ((hit_2.collider != null) ? (hit_2.collider.gameObject) : (hit_3.collider.gameObject));
                        if (currentColliderObject.transform.parent != hit.transform.parent)
                        {
                            if (canCreate == false)
                            {
                                canCreate = true;
                                int siblingIndex = currentColliderObject.transform.GetSiblingIndex();
                                int childCount = currentColliderObject.transform.parent.childCount;
                                if (siblingIndex > 0 && siblingIndex - 1 < childCount)
                                {
                                    GameObject tempOBJ = currentColliderObject.transform.parent.GetChild(currentColliderObject.transform.GetSiblingIndex() - 1).gameObject;
                                    Destroy(currentBoxCollider2D.gameObject);
                                    listPoint.Remove(listPoint[listPoint.Count - 1]);
                                    currentColliderObject = tempOBJ;
                                    currentBoxCollider2D = currentColliderObject.GetComponent<BoxCollider2D>();
                                    currentColliderObject.AddComponent<LookAtScript>();
                                    TemLine = new GameObject("PreDictLine");
                                    TemLine.transform.position = currentColliderObject.transform.position;
                                    TemLine.transform.right = currentColliderObject.transform.right;
                                    lines = TemLine.AddComponent<LineRenderer>();
                                    lines.sharedMaterial = lineMaterial;
                                    lines.startWidth = 0.09f;
                                    lines.endWidth = 0.09f;
                                    lines.startColor = new Color(lineColor.r, lineColor.g, lineColor.b, .5f);
                                    lines.endColor = new Color(lineColor.r, lineColor.g, lineColor.b, .5f);
                                    lines.useWorldSpace = true;
                                    lines.numCapVertices = 90;
                                    lines.sortingOrder = 168;
                                    TemLine.AddComponent<LineRendParent>();
                                }

                            }
                        }
                    }
                    else
                    {
                        Destroy(currentColliderObject.GetComponent<LookAtScript>());
                        Destroy(TemLine);
                        canCreate = false;
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) && !stopHolding && allowDrawing)
        {           
            if (currentLine.transform.childCount > 0)
            {
                if (listPoint.Count >= 2)
                {
                    for (int i = 0; i < currentLine.transform.childCount; i++)
                    {
                        currentLine.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = true;
                    }
                    BoxCollider2D FirstBoxCollider = currentLine.transform.GetChild(0).GetComponent<BoxCollider2D>();
                    FirstBoxCollider.size= new Vector2(FirstBoxCollider.size.x/2f, FirstBoxCollider.size.y);
                    FirstBoxCollider.offset = new Vector2(FirstBoxCollider.size.x/3f, 0f);

                    BoxCollider2D FirstBoxCollider2 = currentLine.transform.GetChild(currentLine.transform.childCount-1).GetComponent<BoxCollider2D>();
                    FirstBoxCollider2.size = new Vector2(FirstBoxCollider2.size.x / 2f, FirstBoxCollider2.size.y);
                    FirstBoxCollider2.offset = new Vector2(-FirstBoxCollider2.size.x / 3f, 0f);
                    listLine.Add(currentLine);
                    currentLine.AddComponent<Rigidbody2D>().useAutoMass = true;
                    Rigidbody2D rb2d = currentLine.GetComponent<Rigidbody2D>();
                    float m = rb2d.mass * 100;
                    rb2d.useAutoMass = false;
                    rb2d.mass = m;
                    if(!IsLineMaterialsNotAdded)
                        rb2d.sharedMaterial = customMaterial;

                }
                else 
                {
                    Destroy(currentLine);
                }
               

            }
            else
            {
                Destroy(currentLine);
            }
            foreach (Rigidbody2D rigid in listObstacleNonKinematic)
            {
                rigid.isKinematic = false;
            }
            if (clickCont == 0 && listLine.Count > 0)
            {
                if (TemLine != null)
                {
                    Destroy(TemLine);
                }
                for (int i = 0; i < Balls.Length; i++)
                {
                    Balls[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    ballsScripts[i].charSorting.WingInterOter(true);
                    ballsScripts[i].DisableBirdsAnimAndFlyingEffects();
                }
                if (GamePlayController.Instance.CanTotatorActive)
                {
                    if (MoveAbleObjectHinge.Length > 0)
                    {
                        for (int i = 0; i < MoveAbleObjectHinge.Length; i++)
                        {
                            MoveAbleObjectHinge[i].enabled = true;
                        }
                    }
                    GamePlayController.Instance.CanTotatorActive = false;
                }
                if (MoveAbleObject.Length > 0)
                {
                    for (int i = 0; i < MoveAbleObject.Length; i++)
                    {
                        MoveAbleObject[i].bodyType = RigidbodyType2D.Dynamic;
                    }
                }
                //for (int i = 0; i < Obs.Length; i++)
                //{
                //    Obs[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                //}

                clickCont++;
            }
            if(currentColliderObject)
                Destroy(currentColliderObject.GetComponent<LookAtScript>());
            if(TemLine)
                Destroy(TemLine);


            canCreate = false;
            Pencil.gameObject.SetActive(false);
            PenSound.SetActive(false);
        }
        LastMosPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    void CreateLine(Vector2 mousePosition)
    {
        currentLine = new GameObject("Line");                                               
        currentLine.transform.SetParent(transform.parent, true);
        //currentLineRenderer = currentLine.AddComponent<LineRenderer>();
        //currentLineRenderer.sharedMaterial = lineMaterial;
        //currentLineRenderer.positionCount = 0;
        //currentLineRenderer.startWidth = 0.05f;
        //currentLineRenderer.endWidth = 0.05f;
        //currentLineRenderer.startColor = lineColor;
        //currentLineRenderer.endColor = lineColor;
        //currentLineRenderer.useWorldSpace = false;
    }
    int WatrCont;

  

    public void WaterCheck()
    {
        WatrCont++;
        if (WatrCont == 70)
        {
            Invoke("RelScene", 2);
        }
    }
    public void StopAllPhysics()
    {
        for (int i = 0; i < listLine.Count; i++)
        {
            Rigidbody2D rigid = listLine[i].GetComponent<Rigidbody2D>();
            rigid.bodyType = RigidbodyType2D.Kinematic;
            rigid.simulated = false;
        }       
    }

    internal void ResetAll()
    {
        for (int i = 0; i < Balls.Length; i++)
        {
            Balls[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            Balls[i].transform.position = ballInitialPosition[i];
            Balls[i].transform.localRotation =Quaternion.identity;
            BallsLineRendere[i].enabled = false;
            ballsScripts[i].charSorting.ResetWing();
        }
        if (MoveAbleObject.Length > 0)
        {
            for (int i = 0; i < MoveAbleObject.Length; i++)
            {
                MoveAbleObject[i].bodyType = RigidbodyType2D.Static;
                MoveAbleObjecttrns[i].localPosition = MoveAbleInitialPosition[i];
                MoveAbleObjecttrns[i].rotation = MoveAbleInitialRotation[i];
            }
        }
        if (MoveAbleObjectHinge.Length > 0)
        {
            for (int i = 0; i < MoveAbleObjectHinge.Length; i++)
            {
                MoveAbleObjectHinge[i].enabled = false;
            }
        }
        Pencil.transform.localPosition = penInitialPosition;
        Pencil.gameObject.SetActive(false);

        if (currentLine)
        {
            listLine.Add(currentLine);
        }
        for (int i = 0; i < listLine.Count; i++)
        {
            Destroy(listLine[i]);
        }
        clickCont = 0;
        listLine.Clear();
        for (int i = 0; i < Balls.Length; i++)
        {
            ballsScripts[i].PlayAnimOfBirds();
        }
        CanDrawPen = true;

    }
    public void ActiveAll()
    {
        for (int i = 0; i < Balls.Length; i++)
        {
            Balls[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
    }
    void RelScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ShowBackgroundImage(bool CanShowBackground) 
    {
        if (CanShowBackground)
        {
            for (int i = 0; i < Background.Length; i++)
            {
                Background[i].SetActive(true);
            }
        }
        else 
        {
            for (int i = 0; i < Background.Length; i++)
            {
                Background[i].SetActive(false);
            }
        }
       
    }

    internal void DisableHind() 
    {
        for (int i = 0; i <Hint.Length; i++)
        {
            Hint[i].SetActive(false);
        }
    }

}

