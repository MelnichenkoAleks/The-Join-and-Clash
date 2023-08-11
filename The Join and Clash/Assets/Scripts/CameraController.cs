using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform _player; 
    public Vector3 offset; 
    public float smoothSpeed = 5f; 

    void FixedUpdate()
    {
        FollowPlayer(); 
    }

    void FollowPlayer()
    {
        Vector3 targetPosition = new Vector3(_player.position.x, _player.position.y, _player.position.z) + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);

        transform.position = smoothedPosition;

        transform.rotation = Quaternion.Euler(15f, 0, 0);
    }
}