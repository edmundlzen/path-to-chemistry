using UnityEngine;

public class ShaderInteractor : MonoBehaviour
{
    // Update is called once per frame
    private void Update()
    {
        Shader.SetGlobalVector("_PositionMoving", transform.position);
    }
}