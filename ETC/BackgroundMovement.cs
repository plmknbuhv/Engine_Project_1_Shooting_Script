using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private void Update()
    {
        transform.position += Vector3.down * (speed * Time.deltaTime);
        
        if (transform.position.y <= -152)
        {
            transform.position += new Vector3(0, 424f);
        }
    }
}