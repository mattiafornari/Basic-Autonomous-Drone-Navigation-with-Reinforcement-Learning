using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionGUI : MonoBehaviour
{
    [Header("References")]
    public DroneAgentObstacles drone;
    public Transform target;
    
    [Header("UI Elements")]
    public TMP_Text distanceText;
    public Image progressFill;
    public TMP_Text progressPercentText;
    public TMP_Text collisionsText;
    public TMP_Text timeText;
    public TMP_Text statusText;
    
    private float initialDistance;
    private int lastEpisode = -1;  
    private bool isSuccess = false;  
    
    void Start()
    {
        if (drone != null && target != null)
            initialDistance = Vector3.Distance(drone.transform.position, target.position);
    }
    
    void Update()
    {
        if (drone == null || target == null) return;

        if (drone.CompletedEpisodes != lastEpisode)
        {
            initialDistance = Vector3.Distance(drone.transform.position, target.position);
            lastEpisode = drone.CompletedEpisodes;
            isSuccess = false;  // Reset success flag
        }

        // Distance
        float dist = Vector3.Distance(drone.transform.position, target.position);
        distanceText.text = $"Distance: {dist:F1}m";
        
        // Progress %
        float progress = Mathf.Clamp01(1 - (dist / initialDistance));
        progressFill.fillAmount = progress;
        progressPercentText.text = $"{progress * 100:F0}%";
        
        // Collisions
        //collisionsText.text = $"Collisions: {drone.collisionCount}";
        // Sostituisci la riga collisionsText
        collisionsText.text = drone.hasCollided ? "HIT!" : "OK";
        collisionsText.color = drone.hasCollided ? Color.red : Color.green;
        
        // Time
        timeText.text = $"Time: {drone.episodeTime:F1}s";
        
        // Status
        if (dist < drone.goalRadius)
        {
            isSuccess = true;
            
            // Lampeggio verde durante i 2 sec pausa
            float blink = Mathf.PingPong(Time.time * 2f, 1f);
            statusText.text = "SUCCESS!!!";
            statusText.color = Color.Lerp(Color.green, Color.white, blink);
            statusText.fontSize = 32 + (int)(blink * 8);  // bottone 32-40
            statusText.text = "SUCCESS!!!";
            statusText.color = Color.green;
        }
        else if (drone.hasCollided)
        {
            statusText.text = "HIT!";
            statusText.color = Color.red;
            //statusText.fontSize = 28;
        }
        else if (drone.StepCount >= drone.MaxStep)
        {
            statusText.text = "TIMEOUT";
            statusText.color = Color.red;
            //statusText.fontSize = 28;
        }
        else
        {
            statusText.text = "IN PROGRESS";
            statusText.color = Color.yellow;
        }
    }
}