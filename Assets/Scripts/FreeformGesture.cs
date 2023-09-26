using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEditor;

//[Serializable]
public enum Shape { Null, Up, Down, Suspended, Square, Triangle, Circle };

public class FreeformGestures : MonoBehaviour
{
    private bool gestureAllowed = false;

    public float minimumDistance = 1.0f;
    private Vector3 lastPosition;
    public SpriteShapeController sc;
    private List<Vector2> points = new List<Vector2> { };


    //public GameObject Player;
    //public ObstacleSpawner ObstacleSpawner;

    // Enable this to show the normalized points (for creating new shapes)
    bool debug = false;
    public GameObject dot;
    private List<GameObject> dots = new List<GameObject>();

    [SerializeField]
    public List<Gesture> gestures = new List<Gesture>();

    public Gesture currentGesture;

    // Use this for initialization
    void Start()
    {
        if (gestures.Count > 0)
        {
            currentGesture = gestures[0];
        }
        else throw new Exception("There are no gestures, please add at least one.");
    }

    private static int NextIndex(int index, int pointCount)
    {
        return Mod(index + 1, pointCount);
    }

    private static int PreviousIndex(int index, int pointCount)
    {
        return Mod(index - 1, pointCount);
    }

    private static int Mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }

    private void Smoothen(SpriteShapeController sc, int pointIndex)
    {
        if (pointIndex > 0)
        {
            Vector3 position = sc.spline.GetPosition(pointIndex);
            Vector3 positionNext = sc.spline.GetPosition(NextIndex(pointIndex, sc.spline.GetPointCount()));
            Vector3 positionPrev = sc.spline.GetPosition(PreviousIndex(pointIndex, sc.spline.GetPointCount()));
            Vector3 forward = gameObject.transform.forward;

            float scale = Mathf.Min((positionNext - position).magnitude, (positionPrev - position).magnitude) * 0.33f;

            Vector3 leftTangent = (positionPrev - position).normalized * scale;
            Vector3 rightTangent = (positionNext - position).normalized * scale;

            sc.spline.SetTangentMode(pointIndex, ShapeTangentMode.Continuous);
            SplineUtility.CalculateTangents(position, positionPrev, positionNext, forward, scale, out rightTangent, out leftTangent);

            sc.spline.SetLeftTangent(pointIndex, leftTangent);
            sc.spline.SetRightTangent(pointIndex, rightTangent);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0 && GameManager.Instance.gameState == GameState.Playing)
        {
            Touch touch = Input.GetTouch(0);

            Vector3 tp = Camera.main.ScreenToWorldPoint(touch.position);
            tp.x -= Camera.main.transform.position.x;
            tp.z = 10.0f;

            var dt = Mathf.Abs((tp - lastPosition).magnitude);
            var md = (minimumDistance > 1.0f) ? minimumDistance : 1.0f;

            var spline = sc.spline;


            switch (touch.phase)
            {
                case TouchPhase.Began:
                    spline.isOpenEnded = true;
                    spline.Clear();

                    if (debug)
                    {
                        foreach (GameObject dot in dots)
                        {
                            Destroy(dot);
                        }

                        dots = new List<GameObject>();
                    }

                    points = new List<Vector2>();

                    break;

                case TouchPhase.Moved:

                    if (dt > md)
                    {
                        spline.InsertPointAt(spline.GetPointCount(), tp);
                        var newPointIndex = spline.GetPointCount() - 1;
                        Smoothen(sc, newPointIndex - 1);

                        //spline.SetHeight(newPointIndex, UnityEngine.Random.Range(0.1f, 2.0f));
                        lastPosition = tp;

                        points.Add(tp);
                    }

                    break;

                case TouchPhase.Ended:


                    if (points.Count > 1)
                    {
                        var shapeArea = GetArea(points);
                        var squaredArea = Mathf.Sqrt(shapeArea);

                        spline.isOpenEnded = false;
                        // Generating collider and get center from it
                        sc.BakeCollider();
                        Vector3 objectCenter = sc.polygonCollider.bounds.center;


                        List<Vector2> normalizedPoints = new List<Vector2> { };

                        //Spline slimeSpline = Player.GetComponent<SpriteShapeController>().spline;

                        //Slime slime = Player.GetComponent<Slime>();
                        //Spline spline = new Spline();
                        //slime.SplineClear();

                        var smallestPoint = points.OrderBy(v => v.sqrMagnitude).First();

                        foreach (Vector2 p in points)
                        {
                            //Vector2 normalizedPoint = new Vector2((p.x - objectCenter.x) / shapeArea, (p.y - objectCenter.y) / shapeArea);
                            //Vector2 normalizedPoint = new Vector2((p.x - objectCenter.x + Camera.main.transform.position.x - smallestPoint.x) / squaredArea, (p.y - objectCenter.y + Camera.main.transform.position.y - smallestPoint.y) / squaredArea);
                            Vector2 normalizedPoint = new Vector2((p.x - objectCenter.x + Camera.main.transform.position.x), (p.y - objectCenter.y + Camera.main.transform.position.y)) / squaredArea;
                            normalizedPoints.Add(normalizedPoint);
                            //Vector2 playerPosition = new Vector2((p.x - objectCenter.x + Camera.main.transform.position.x) / squaredArea, (p.y - objectCenter.y + Camera.main.transform.position.y) / squaredArea) * 3f;

                            //Debug.Log(slimeSpline.GetPointCount());

                            //if (slimeSpline.GetPointCount() == 0)
                            //{
                            // add first point to spline
                            //slime.SplineInsertPointAt(slimeSpline.GetPointCount() == 0 ? 0 : slimeSpline.GetPointCount() - 1, playerPosition);
                            //}
                            //else
                            //{
                            //    SafeSequentialInsertPointIntoSpline(slimeSpline.GetPointCount() - 1, playerPosition, slimeSpline);

                            //}
                            if (debug)
                            {
                                GameObject newDot = Instantiate(dot, normalizedPoint, Quaternion.identity);
                                dots.Add(newDot);

                            }
                            //newDot.transform.parent = Camera.main.transform;

                        }

                        List<float> results = new List<float>();

                        float amountPointsInBounds = 0f;

                        // Check if points are in collider
                        
                        foreach (Gesture g in gestures)
                        {
                            foreach (Vector2 p in normalizedPoints)
                            {
                                if (g.collider.GetComponent<PolygonCollider2D>().OverlapPoint(p))
                                {
                                    amountPointsInBounds++;
                                }
                            }

                            //Debug.Log(amountPointsInBounds + " > " + g.acceptableGesturePercent);

                            

                            float correctPrcntPoints = (amountPointsInBounds / normalizedPoints.Count) * 100f;
                            //float correctPrcntArea = (squaredArea / GetArea(normalizedPoints)) * 100f;

                            if (correctPrcntPoints > g.acceptableGesturePercent)
                            {
                                results.Add(correctPrcntPoints);
                            }

                            //Debug.Log(g.shape + " " + amountPointsInBounds);

                            Debug.Log(g.shape + " area: " + correctPrcntPoints);
                            Debug.Log(g.shape + " points: " + correctPrcntPoints);
                            amountPointsInBounds = 0f;
                        }

                        Shape bestScoredShape = Shape.Null;
                        Debug.Log(results.Count);
                        if(results.Count > 0)
                        {
                            bestScoredShape = gestures[results.IndexOf(results.Max())].shape;
                        }

                        
                        //int correctPrcntArea = (shapeArea / ()) / 100;

                        //m_Collider

                        //Debug.Log("area: " + correctPrcntPoints);
                        //Debug.Log("points: " + correctPrcntPoints);
                        Debug.Log(bestScoredShape);

                        //if (correctPrcntPoints > acceptableGesturePercent)
                        //{
                        //    Debug.Log(correctPrcntPoints);
                        //}

                        //TODO

                        // get gesture from obstacle
                        //Debug.Log("obstacle:");
                        //Debug.Log(ObstacleSpawner.GetNextObstacle());

                        // instantiate bounds

                        // check if performed gesture has enough score

                        // return true/false state back to slime
                    }
                    else
                    {
                        Debug.Log("No points recorded");
                    }

                    break;


            }

        }
    }

    private float GetArea(List<Vector2> points)
    {
        points.Add(points[0]);
        return Math.Abs(points.Take(points.Count - 1)
           .Select((p, i) => (points[i + 1].x - p.x) * (points[i + 1].y + p.y))
           .Sum() / 2
           );
    }

    private void SafeSequentialInsertPointIntoSpline(int index, Vector3 position, Spline spline)
    {
        float MinimalSafeDistanceBetweenPoints = 0.05f;

        Debug.Log("Inserting point " + index + " to length " + spline.GetPointCount());

        if (index < spline.GetPointCount() - 1)
        {
            throw new InvalidOperationException("Point is not being inserted sequentially.");
        }

        if (index > 0)
        {
            Vector3 previousPoint = spline.GetPosition(index - 1);
            // Check distance with the start of the spline, due to the position validating logic of Spline.
            Vector3 originPoint = spline.GetPosition(0);

            float squaredDistanceFromPreviousPoint = Vector3.SqrMagnitude(previousPoint - position);
            //float squaredDistanceFromOriginPoint = Vector3.SqrMagnitude(originPoint - position);

            Vector3 safeDirection = (position - originPoint).normalized + (position - previousPoint).normalized;

            if (squaredDistanceFromPreviousPoint <
                MinimalSafeDistanceBetweenPoints * MinimalSafeDistanceBetweenPoints
                //|| squaredDistanceFromOriginPoint <
                //MinimalSafeDistanceBetweenPoints * MinimalSafeDistanceBetweenPoints
                )
            {
                position += MinimalSafeDistanceBetweenPoints * safeDirection.normalized;
            }
        }

        //spline.InsertPointAt(index, position);
        //Player.GetComponent<Slime>().SplineInsertPointAt(index, position);
    }

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnOnGameStateChanged;
    }

    private void GameManagerOnOnGameStateChanged(GameState state)
    {
        //gestureAllowed = state == GameState.Playing;
    }

    public void setCurrentGesture(Gesture gesture)
    {
        currentGesture = gesture;
    }

}
