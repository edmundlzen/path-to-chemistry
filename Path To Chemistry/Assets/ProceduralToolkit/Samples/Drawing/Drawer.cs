using UnityEngine;

namespace ProceduralToolkit.Samples
{
    /// <summary>
    ///     An example showing usage of DebugE, GLE and GizmosE
    /// </summary>
    [ExecuteInEditMode]
    public class Drawer : MonoBehaviour
    {
        private const float radius = 1.5f;
        private const float coneAngle = 30;
        private const float coneLength = 1;

        private void Update()
        {
            var color = ColorE.aqua;
            var rotation = transform.rotation * Quaternion.Euler(0, 0, -120);
            var position = transform.position + rotation * Vector3.up;

            DebugE.DrawWireHemisphere(position, rotation, radius, color);
            DebugE.DrawWireCone(position, rotation, radius, coneAngle, coneLength, color);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = ColorE.lime;
            var rotation = transform.rotation * Quaternion.Euler(0, 0, 120);
            var position = transform.position + rotation * Vector3.up;

            GizmosE.DrawWireHemisphere(position, rotation, radius);
            GizmosE.DrawWireCone(position, rotation, radius, coneAngle, coneLength);
        }

        private void OnRenderObject()
        {
            GLE.BeginLines();
            {
                GL.Color(ColorE.fuchsia);
                var rotation = transform.rotation;
                var position = transform.position + Vector3.up;

                GLE.DrawWireHemisphere(position, rotation, radius);
                GLE.DrawWireCone(position, rotation, radius, coneAngle, coneLength);
            }
            GL.End();
        }
    }
}