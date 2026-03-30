using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionGUI_empty : MonoBehaviour
{
    [Header("References")]
    public DroneAgentNEW drone;
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
            statusText.text = "SUCCESS!!!";
            statusText.color = Color.green;
        }
        else if (drone.hasCollided)
        {
            statusText.text = "HIT!";
            statusText.color = Color.red;
        }
        else if (drone.StepCount >= drone.MaxStep)
        {
            statusText.text = "TIMEOUT";
            statusText.color = Color.red;
        }
        else
        {
            statusText.text = "IN PROGRESS";
            statusText.color = Color.yellow;
        }
    }
}