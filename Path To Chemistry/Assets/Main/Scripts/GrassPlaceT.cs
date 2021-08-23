using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassPlaceT : MonoBehaviour
{
    public GrassPainter grassPainter;

    private Renderer r;

    private bool oneShot = true;
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (oneShot)
        {
            for (int x = 0; x < 100; x++)
            {
                for (int z = 0; z < 100; z++)
                {
                    Debug.DrawLine(new Vector3(0,0,0), new Vector3(x, -4.93f, z), Color.red, 30);
                    grassPainter.AddPoint(new Vector3(x, -4.93f, z),
                        new Vector3(0f, 1f, 2.220446e-17f));
                }
            }

            oneShot = false;
        }
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
