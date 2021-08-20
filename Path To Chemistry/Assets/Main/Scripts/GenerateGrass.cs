using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrass : MonoBehaviour
{
    public GrassGenerationSettings grassGenerationSettings;
    public GrassPainter grassPainter;
    public bool oneShot = false;
    private Renderer r;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (oneShot)
        {
            r = GetComponent<Renderer>();
            if (r != null)
            {
                RaycastHit hit;
                for (float x = r.bounds.min.x; x < r.bounds.max.x; x++)
                {
                    for (float z = r.bounds.min.z; z < r.bounds.max.y; z++)
                    {
                        print(new Vector2(x, z));
                        if (Physics.Raycast(new Vector3(x, r.bounds.max.y + 5f, z), Vector3.down, out hit))
                        {
                            print("ME");
                            grassPainter.AddPoint(hit.point, hit.normal);
                        }
                    }
                }
            }
        }
    }
}
