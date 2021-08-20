using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassPlaceT : MonoBehaviour
{
    public GrassPainter grassPainter;

    private Renderer r;
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Renderer>();
        
        grassPainter.AddPoint(new Vector3(-0.5307579f, -4.059805f, -0.8507843f ), new Vector3(0.1731887f, 0.9565451f, 0.2345785f));
    }

    // Update is called once per frame
    void Update()
    {
        if (grassPainter == null)
        {
            RaycastHit hit;
            var rayOrigin = new Vector3(transform.position.x, transform.position.y - 0.51f, transform.position.z);
            var lineEnd = new Vector3(transform.position.x, -1, transform.position.z);
            Debug.DrawLine(rayOrigin, lineEnd, Color.red);
            if (Physics.Raycast(rayOrigin, Vector3.down, out hit))
            {
                print("WORKING");
                grassPainter.AddPoint(hit.point, hit.normal);
            }
        }
    }
}
