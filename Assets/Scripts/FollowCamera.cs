using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 15, -25);

    void Start()
    {
        Debug.Log("=== FOLLOWCAMERA START ===");
        Debug.Log($"Target assigned: {target != null}");
        if (target != null)
            Debug.Log($"Target name: {target.name}");
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is NULL!");
            return;
        }
        
        //Force the camera position to match the target plus the offset
        Vector3 newPos = target.position + offset;
        transform.position = newPos;
        transform.LookAt(target.position);
        
        // Debug ogni 60 frame
        if (Time.frameCount % 60 == 0)
        {
            Debug.Log($"Camera: {transform.position} | Drone: {target.position}");
        }
    }
}