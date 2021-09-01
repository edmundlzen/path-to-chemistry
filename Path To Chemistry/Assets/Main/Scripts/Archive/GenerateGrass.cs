using System.Collections;
using UnityEngine;

public class GenerateGrass : MonoBehaviour
{
    public GrassGenerationSettings grassGenerationSettings;
    public GrassComputeScript grassComputeScript;
    public GrassPainter grassPainter;
    public Transform playerTransform;
    public bool generateGrass;
    private Renderer r;

    // Update is called once per frame
    private void Update()
    {
        if (r == null || grassPainter == null || grassComputeScript == null || grassGenerationSettings == null)
        {
            r = GetComponent<Renderer>();
        }
        else if (generateGrass && Vector3.Distance(transform.position, playerTransform.position) < 61f)
        {
            StartCoroutine("AddGrass");
            generateGrass = false;
        }
    }

    private IEnumerator AddGrass()
    {
        // Generate grass
        RaycastHit hit;
        for (var x = r.bounds.min.x; x < r.bounds.max.x; x++)
        for (var z = r.bounds.min.z; z < r.bounds.max.z; z++)
            if (Physics.Raycast(new Vector3(x, r.bounds.max.y + 5f, z), -Vector3.up, out hit))
            {
                // grassPainter.AddPoint(hit.point, hit.normal, grassGenerationSettings.grassDensity);
                grassComputeScript.UpdateGrass();
                yield return null; // new WaitForSeconds(.1f);
            }
    }
}