using UnityEngine;

public class Wobble : MonoBehaviour
{
    public float MaxWobble = 0.03f;
    public float WobbleSpeed = 1f;
    public float Recovery = 1f;

    private Vector3 angularVelocity;

    //Renderer rend;
    private Vector3 lastPos;
    private Vector3 lastRot;
    private float pulse;
    private float time = 0.5f;
    private Vector3 velocity;
    private float wobbleAmountToAddX;
    private float wobbleAmountToAddZ;
    private float wobbleAmountX;
    private float wobbleAmountZ;

    private void Start()
    {
        //rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        time += Time.deltaTime;
        wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, Time.deltaTime * Recovery);
        wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, Time.deltaTime * Recovery);
        pulse = 2 * Mathf.PI * WobbleSpeed;
        wobbleAmountX = wobbleAmountToAddX * Mathf.Sin(pulse * time);
        wobbleAmountZ = wobbleAmountToAddZ * Mathf.Sin(pulse * time);
        //rend.material.SetFloat("_WobbleX", wobbleAmountX);
        //rend.material.SetFloat("_WobbleZ", wobbleAmountZ);
        velocity = (lastPos - transform.position) / Time.deltaTime;
        angularVelocity = transform.rotation.eulerAngles - lastRot;
        wobbleAmountToAddX += Mathf.Clamp((velocity.x + angularVelocity.z * 0.2f) * MaxWobble, -MaxWobble, MaxWobble);
        wobbleAmountToAddZ += Mathf.Clamp((velocity.z + angularVelocity.x * 0.2f) * MaxWobble, -MaxWobble, MaxWobble);
        lastPos = transform.position;
        lastRot = transform.rotation.eulerAngles;
    }
}