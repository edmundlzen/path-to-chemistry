using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrass : MonoBehaviour
{
    public GrassGenerationSettings grassGenerationSettings;
    public GrassComputeScript grassComputeScript;
    public GrassPainter grassPainter;
    private bool grassGenerated = false;
    private Renderer r;

    // Update is called once per frame
    void Update()
    {
        if (grassGenerated)
        {
            return;
        }
        if (r == null || grassPainter == null || grassComputeScript == null || grassGenerationSettings == null)
        {
            r = GetComponent<Renderer>();
        } else
        {
            RaycastHit hit;
            for (float x = r.bounds.min.x; x < r.bounds.max.x; x++)
            {
                for (float z = r.bounds.min.z; z < r.bounds.max.y; z++)
                {
                    if (Physics.Raycast(new Vector3(x, r.bounds.max.y + 5f, z), Vector3.down, out hit))
                    {
                        grassPainter.AddPoint(new Vector3(x, 0.9f, z), new Vector3(0, 1, 0), grassGenerationSettings.grassDensity);
                    }
                }
            }
            grassComputeScript.UpdateGrass();
            grassGenerated = true;
        }
    }
}
