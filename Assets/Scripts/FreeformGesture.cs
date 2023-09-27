using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEditor;
using System.Collections;

//[Serializable]
public enum Shape { Null, Up, Down, Suspended, Square, Triangle, Circle };

public class FreeformGestures : MonoBehaviour
{
    private bool gestureAllowed = false;

    public float distanceBetweenPoints = 1.0f;
    private Vector3 lastPosition;
    public SpriteShapeController spriteShapeController;
    private List<Vector2> points = new List<Vector2> { };

    public Slime slime;

    // Enable this to show the normalized points (for creating new shapes)
    bool debug = true;
    public GameObject dot;
    private List<GameObject> dots = new List<GameObject>();

    [SerializeField]
    public List<Gesture> gestures = new List<Gesture>(new Gesture());

    void Start()
    {
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
            var md = (distanceBetweenPoints > 1.0f) ? distanceBetweenPoints : 1.0f;

            var spline = spriteShapeController.spline;


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

                    //points = new List<Vector2>();

                    break;

                case TouchPhase.Moved:

                    if (dt > md)
                    {
                        spline.InsertPointAt(spline.GetPointCount(), tp);
                        var newPointIndex = spline.GetPointCount() - 1;
                        Smoothen(spriteShapeController, newPointIndex - 1);

                        lastPosition = tp;

                        points.Add(tp);
                    }

                    break;

                case TouchPhase.Ended:
                    spline.isOpenEnded = false;

                    if (points.Count > 1)
                    {
                        var shapeArea = GetArea(points);
                        var squaredArea = Mathf.Sqrt(shapeArea);

                        spriteShapeController.BakeCollider();
                        Vector3 objectCenter = spriteShapeController.polygonCollider.bounds.center;


                        List<Vector2> normalizedPoints = new List<Vector2> { };


                        var smallestPoint = points.OrderBy(v => v.sqrMagnitude).First();

                        foreach (Vector2 p in points)
                        {
                            Vector2 normalizedPoint = new Vector2((p.x - objectCenter.x + Camera.main.transform.position.x), (p.y - objectCenter.y + Camera.main.transform.position.y)) / squaredArea;
                            normalizedPoints.Add(normalizedPoint);
                            
                            if (debug)
                            {
                                GameObject newDot = Instantiate(dot, normalizedPoint, Quaternion.identity);
                                dots.Add(newDot);

                            }

                        }

                        List<float> results = new List<float>();

                        float amountPointsInBounds = 0f;

                        //gestures[0].collider.GetComponent<PolygonCollider2D>().
                        //foreach(gestures[0].collider.GetComponent<PolygonCollider2D>().)
                        //{

                        //}


                        // Check if points are in collider

                        foreach (Gesture g in gestures)
                        {
                            Vector2[] gesturePoints = g.collider.GetComponent<PolygonCollider2D>().points;
                            float gestureArea = GetAreaArray(gesturePoints);

                            float shapeNormalizedArea = GetArea(normalizedPoints);

                            //foreach (Vector3 gp in g.collider.GetComponent<PolygonCollider2D>().points)
                            //{

                            //}

                            foreach (Vector2 p in normalizedPoints)
                            {
                                if (g.collider.GetComponent<PolygonCollider2D>().OverlapPoint(p))
                                {
                                    amountPointsInBounds++;
                                }
                            }

                            float correctPrcntPoints = (amountPointsInBounds / normalizedPoints.Count) * 100f;

                            if (correctPrcntPoints > g.acceptableGesturePercent)
                            {
                                results.Add(correctPrcntPoints);
                            } else
                            {
                                results.Add(-1f);
                            }

                            //Debug.Log(g.shape + " score: " + correctPrcntPoints);
                            Debug.Log(g.shape + " area: " + gestureArea);
                            Debug.Log(g.shape + " shape area: " + shapeNormalizedArea);

                            amountPointsInBounds = 0f;
                        }

                        Gesture bestScoredGesture = new Gesture(Shape.Null);

                        float bestScore = Mathf.Max(results.ToArray());

                        if (results.Count > 0 && bestScore != -1)
                        {
                            bestScoredGesture = gestures[results.IndexOf(bestScore)];
                        }
                        else Debug.Log("Couldn't classify your gesture with any shape.");

                        Debug.Log("Gesture: " + bestScoredGesture.shape);
                        slime.UpdateGesture(bestScoredGesture);

                        //spline.InsertPointAt(0, new Vector3(100, 100, 100));
                        //spline.


                        spline.Clear();

                        points = new List<Vector2>();


                    }
                    else
                    {
                        Debug.Log("No points recorded");
                    }

                    break;

                //spline.Clear();
            }

            //spline.Clear();

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

    private float GetAreaArray(Vector2[] points)
    {
        points[points.Length - 1] = points[0];
        return Math.Abs(points.Take(points.Length - 1)
           .Select((p, i) => (points[i + 1].x - p.x) * (points[i + 1].y + p.y))
           .Sum() / 2
           );
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

    //public void setCurrentGesture(Gesture gesture)
    //{
    //    currentGesture = gesture;
    //}

}
