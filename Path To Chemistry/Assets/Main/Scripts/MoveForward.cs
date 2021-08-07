using UnityEngine;

    public class MoveForward : MonoBehaviour
    {
        [SerializeField]
        private float speed = 5f;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            GetComponent<Transform>().position = GetComponent<Transform>().position + new Vector3(0, 0, speed * Time.deltaTime);
        }
    }
