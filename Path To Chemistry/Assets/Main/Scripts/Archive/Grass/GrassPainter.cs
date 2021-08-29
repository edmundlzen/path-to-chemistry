using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class GrassPainter : MonoBehaviour
{
    public Mesh mesh;

    public Color AdjustedColor;

    [Range(1, 600000)] public int grassLimit = 50000;

    public int toolbarInt;

    public int toolbarIntEdit;

    [SerializeField] private List<Vector3> positions = new List<Vector3>();

    [SerializeField] private List<Color> colors = new List<Color>();

    [SerializeField] private List<int> indicies = new List<int>();

    [SerializeField] private List<Vector3> normals = new List<Vector3>();

    [SerializeField] private List<Vector2> length = new List<Vector2>();

    public int i;

    public float sizeWidth = 1f;
    public float sizeLength = 1f;
    public float density = 1f;


    public float normalLimit = 1;

    public float rangeR, rangeG, rangeB;
    public LayerMask hitMask = 1;
    public LayerMask paintMask = 1;
    public float brushSize;
    public float brushFalloffSize;


    public float Flow;

    [HideInInspector] public Vector3 hitPosGizmo;

    [HideInInspector] public Vector3 hitNormal;

    private MeshFilter filter;

    private int flowTimer;

    private Vector3 hitPos;


    private int[] indi;

    private Vector3 lastPosition = Vector3.zero;

    private Vector3 mousePos;

    private void Start()
    {
        filter = GetComponent<MeshFilter>();
    }

#if UNITY_EDITOR
    private void OnFocus()
    {
        // Remove delegate listener if it has previously
        // been assigned.
        SceneView.duringSceneGui -= OnScene;
        // Add (or re-add) the delegate.
        SceneView.duringSceneGui += OnScene;
    }

    private void OnDestroy()
    {
        // When the window is destroyed, remove the delegate
        // so that it will no longer do any drawing.
        SceneView.duringSceneGui -= OnScene;
    }

    private void OnEnable()
    {
        filter = GetComponent<MeshFilter>();
        SceneView.duringSceneGui += OnScene;
    }

    public void ClearMesh()
    {
        i = 0;
        positions = new List<Vector3>();
        indicies = new List<int>();
        colors = new List<Color>();
        normals = new List<Vector3>();
        length = new List<Vector2>();
    }

    public void RemovePoint(Vector3 grassPosition, Vector3 grassNormal, int grassDensity)
    {
        var step = 1f / grassDensity;
        for (var j = 0; j < grassDensity; j++)
        {
            positions.Remove(transform.InverseTransformPoint(new Vector3(grassPosition.x + step * j, grassPosition.y,
                grassPosition.z + step * j)));
            indicies.Remove(i);
            length.Remove(new Vector2(sizeWidth, sizeLength));
            colors.Remove(new Color(AdjustedColor.r + Random.Range(0, 1.0f) * rangeR,
                AdjustedColor.g + Random.Range(0, 1.0f) * rangeG, AdjustedColor.b + Random.Range(0, 1.0f) * rangeB, 1));
            normals.Remove(grassNormal);
            i--;
        }

        mesh.SetVertices(positions);
        indi = indicies.ToArray();
        mesh.SetIndices(indi, MeshTopology.Points, 0);
        mesh.SetUVs(0, length);
        mesh.SetColors(colors);
        mesh.SetNormals(normals);
        filter.mesh = mesh;
    }

    public void AddPoint(Vector3 grassPosition, Vector3 grassNormal, int grassDensity)
    {
        var step = 1f / grassDensity;
        for (var j = 0; j < grassDensity; j++)
        {
            positions.Add(transform.InverseTransformPoint(new Vector3(grassPosition.x + step * j, grassPosition.y,
                grassPosition.z + step * j)));
            indicies.Add(i);
            length.Add(new Vector2(sizeWidth, sizeLength));
            colors.Add(new Color(AdjustedColor.r + Random.Range(0, 1.0f) * rangeR,
                AdjustedColor.g + Random.Range(0, 1.0f) * rangeG, AdjustedColor.b + Random.Range(0, 1.0f) * rangeB, 1));
            normals.Add(grassNormal);
            i++;
        }

        mesh.SetVertices(positions);
        indi = indicies.ToArray();
        mesh.SetIndices(indi, MeshTopology.Points, 0);
        mesh.SetUVs(0, length);
        mesh.SetColors(colors);
        mesh.SetNormals(normals);
        filter.mesh = mesh;
    }

    private void OnScene(SceneView scene)
    {
        // only allow painting while this object is selected
        if (Selection.Contains(gameObject))
        {
            var e = Event.current;
            RaycastHit terrainHit;
            mousePos = e.mousePosition;
            var ppp = EditorGUIUtility.pixelsPerPoint;
            mousePos.y = scene.camera.pixelHeight - mousePos.y * ppp;
            mousePos.x *= ppp;

            // ray for gizmo(disc)
            var rayGizmo = scene.camera.ScreenPointToRay(mousePos);
            RaycastHit hitGizmo;

            if (Physics.Raycast(rayGizmo, out hitGizmo, 200f, hitMask.value)) hitPosGizmo = hitGizmo.point;

            if (e.type == EventType.MouseDrag && e.button == 1 && toolbarInt == 0)
            {
                // place based on density
                for (var k = 0; k < density; k++)
                {
                    // brushrange
                    var t = 2f * Mathf.PI * Random.Range(0f, brushSize);
                    var u = Random.Range(0f, brushSize) + Random.Range(0f, brushSize);
                    var r = u > 1 ? 2 - u : u;
                    var origin = Vector3.zero;

                    // place random in radius, except for first one
                    if (k != 0)
                    {
                        origin.x += r * Mathf.Cos(t);
                        origin.y += r * Mathf.Sin(t);
                    }
                    else
                    {
                        origin = Vector3.zero;
                    }

                    // add random range to ray
                    var ray = scene.camera.ScreenPointToRay(mousePos);
                    ray.origin += origin;

                    // if the ray hits something thats on the layer mask,  within the grass limit and within the y normal limit
                    if (Physics.Raycast(ray, out terrainHit, 200f, hitMask.value) && i < grassLimit &&
                        terrainHit.normal.y <= 1 + normalLimit && terrainHit.normal.y >= 1 - normalLimit)
                        if ((paintMask.value & (1 << terrainHit.transform.gameObject.layer)) > 0)
                        {
                            hitPos = terrainHit.point;
                            hitNormal = terrainHit.normal;
                            if (k != 0)
                            {
                                var grassPosition = hitPos; // + Vector3.Cross(origin, hitNormal);
                                grassPosition -= transform.position;

                                positions.Add(grassPosition);
                                indicies.Add(i);
                                length.Add(new Vector2(sizeWidth, sizeLength));
                                // add random color variations                          
                                colors.Add(new Color(AdjustedColor.r + Random.Range(0, 1.0f) * rangeR,
                                    AdjustedColor.g + Random.Range(0, 1.0f) * rangeG,
                                    AdjustedColor.b + Random.Range(0, 1.0f) * rangeB, 1));

                                //colors.Add(temp);
                                normals.Add(terrainHit.normal);
                                i++;
                            }
                            else
                            {
                                // to not place everything at once, check if the first placed point far enough away from the last placed first one
                                if (Vector3.Distance(terrainHit.point, lastPosition) > brushSize)
                                {
                                    var grassPosition = hitPos;
                                    grassPosition -= transform.position;
                                    positions.Add(grassPosition);
                                    indicies.Add(i);
                                    length.Add(new Vector2(sizeWidth, sizeLength));
                                    colors.Add(new Color(AdjustedColor.r + Random.Range(0, 1.0f) * rangeR,
                                        AdjustedColor.g + Random.Range(0, 1.0f) * rangeG,
                                        AdjustedColor.b + Random.Range(0, 1.0f) * rangeB, 1));
                                    normals.Add(terrainHit.normal);
                                    i++;

                                    if (origin == Vector3.zero) lastPosition = hitPos;
                                }
                            }
                        }
                }

                e.Use();
            }

            // removing mesh points
            if (e.type == EventType.MouseDrag && e.button == 1 && toolbarInt == 1)
            {
                var ray = scene.camera.ScreenPointToRay(mousePos);

                if (Physics.Raycast(ray, out terrainHit, 200f, hitMask.value))
                {
                    hitPos = terrainHit.point;
                    hitPosGizmo = hitPos;
                    hitNormal = terrainHit.normal;
                    for (var j = 0; j < positions.Count; j++)
                    {
                        var pos = positions[j];

                        pos += transform.position;
                        var dist = Vector3.Distance(terrainHit.point, pos);

                        // if its within the radius of the brush, remove all info
                        if (dist <= brushSize)
                        {
                            positions.RemoveAt(j);
                            colors.RemoveAt(j);
                            normals.RemoveAt(j);
                            length.RemoveAt(j);
                            indicies.RemoveAt(j);
                            i--;
                            for (var i = 0; i < indicies.Count; i++) indicies[i] = i;
                        }
                    }
                }

                e.Use();
            }

            //edit
            if (e.type == EventType.MouseDrag && e.button == 1 && toolbarInt == 2)
            {
                var ray = scene.camera.ScreenPointToRay(mousePos);

                if (Physics.Raycast(ray, out terrainHit, 200f, hitMask.value))
                {
                    hitPos = terrainHit.point;
                    hitPosGizmo = hitPos;
                    hitNormal = terrainHit.normal;
                    for (var j = 0; j < positions.Count; j++)
                    {
                        var pos = positions[j];

                        pos += transform.position;
                        var dist = Vector3.Distance(terrainHit.point, pos);

                        // if its within the radius of the brush, remove all info
                        if (dist <= brushSize)
                        {
                            brushFalloffSize = Mathf.Clamp(brushFalloffSize, 0, brushSize);
                            var falloff = Mathf.Clamp01((dist - brushFalloffSize) / (brushSize - brushFalloffSize));

                            var OrigColor = colors[j];

                            var newCol = new Color(AdjustedColor.r + Random.Range(0, 1.0f) * rangeR,
                                AdjustedColor.g + Random.Range(0, 1.0f) * rangeG,
                                AdjustedColor.b + Random.Range(0, 1.0f) * rangeB, 1);

                            var origLength = length[j];
                            var newLength = new Vector2(sizeWidth, sizeLength);
                            ;

                            flowTimer++;
                            if (flowTimer > Flow)
                            {
                                if (toolbarIntEdit == 0 || toolbarIntEdit == 2)
                                    colors[j] = Color.Lerp(newCol, OrigColor, falloff);
                                if (toolbarIntEdit == 1 || toolbarIntEdit == 2)
                                    length[j] = Vector2.Lerp(newLength, origLength, falloff);

                                flowTimer = 0;
                            }
                        }
                    }
                }

                e.Use();
            }

            // set all info to mesh
            mesh = new Mesh();
            mesh.SetVertices(positions);
            indi = indicies.ToArray();
            mesh.SetIndices(indi, MeshTopology.Points, 0);
            mesh.SetUVs(0, length);
            mesh.SetColors(colors);
            mesh.SetNormals(normals);
            filter.mesh = mesh;
        }
    }
#endif
}