using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class DroneAgentNEW : Agent
{
    [Header("References")]
    public Transform spawnPoint;
    public Transform goalPoint;

    [Header("Physics")]
    public float thrustPower = 25f;
    public float torquePower = 10f;

    [Header("Settings")]
    public float goalRadius = 5f;
    public float maxDistance = 200f; // usato per normalizzare la distanza
    public bool visualMode = false; // ON per inference/heuristic, OFF per training

    [Header("Episode Stats")]
    public int collisionCount = 0;
    public bool hasCollided = false;
    public float collisionFlashTime = 0f;
    public float episodeTime = 0f;
    public int CurrentStep => StepCount;
    public int MaxStepValue => MaxStep;

    private Rigidbody rb;
    private float previousDistance;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // Reset posizione e fisica
        transform.position = spawnPoint.position;
        transform.rotation = Quaternion.Euler(-90, 0, 180); //.identity; // per heuristic only ma utile anche per policy
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        collisionCount = 0;
        episodeTime = 0f;
        hasCollided = false;
        collisionFlashTime = 0f;

        previousDistance = Vector3.Distance(transform.position, goalPoint.position);

        Debug.Log($"Spawn: {spawnPoint.position}, Goal: {goalPoint.position}, Distanza: {Vector3.Distance(spawnPoint.position, goalPoint.position)}");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 toGoal = goalPoint.position - transform.position;
        //Debug.DrawRay(transform.position, toGoal.normalized * 10f, Color.red);
        float currentDistance = toGoal.magnitude;

        //sensor.AddObservation(toGoal.normalized); // 3 - direzione verso goal
        sensor.AddObservation(transform.InverseTransformDirection(toGoal.normalized));
        sensor.AddObservation(transform.InverseTransformDirection(rb.linearVelocity)); // 3 - velocità lineare
        sensor.AddObservation(transform.InverseTransformDirection(rb.angularVelocity));  // 3 - velocità angolare
        sensor.AddObservation(transform.up); // 3 - orientamento drone (cruciale)
        sensor.AddObservation(currentDistance / maxDistance); // 1 - distanza normalizzata
        sensor.AddObservation(Vector3.Dot(transform.up, Vector3.up));  

        /*Debug.Log($"Drone pos: {transform.position}");
        Debug.Log($"Goal pos: {goalPoint.position}");
        Debug.Log($"ToGoal: {toGoal}");
        */
          // 1 - quanto è "dritto"
        // Totale: 14 
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float thrustAction = actions.ContinuousActions[0]; // [-1, 1]
        float pitch        = actions.ContinuousActions[1]; // [-1, 1]
        float roll         = actions.ContinuousActions[2]; // [-1, 1]
        float yaw          = actions.ContinuousActions[3]; // [-1, 1]

        // Thrust: mappa [-1,1] → [0, thrustPower] (drone non può spingere verso il basso)
        float hoverForce = rb.mass * Physics.gravity.magnitude;
        float thrust = hoverForce + thrustAction * thrustPower; //(thrustAction + 1f) / 2f * thrustPower;
        rb.AddForce(transform.up * thrust, ForceMode.Force);

        // Torques
        rb.AddTorque(transform.right   *  pitch * torquePower, ForceMode.Force);
        rb.AddTorque(transform.forward * -roll  * torquePower, ForceMode.Force);
        rb.AddTorque(transform.up      *  yaw   * torquePower, ForceMode.Force);

        // --- Reward shaping ---
        float currentDistance = Vector3.Distance(transform.position, goalPoint.position);

        // 1. Reward per avvicinarsi al goal (segnale denso, fondamentale)
        float delta = previousDistance - currentDistance;
        AddReward(delta * 0.5f);
        previousDistance = currentDistance;

        // 2. Penalità temporale (incentiva la velocità) - step penalty
        AddReward(-0.001f);

        // 3. UPRIGHT — solo penalità proporzionale, nessun bonus
        // Non termino l'episodio: lascia che il drone si corregga
        float uprightness = Vector3.Dot(transform.up, Vector3.up);
        if (uprightness < 0.7f)
            AddReward((uprightness - 0.7f) * 0.02f); // max -0.014/step quando capovolto
        // 4. ALTITUDINE — penalizzo lo scendere troppo sotto il goal
        float heightError = transform.position.y - goalPoint.position.y;
        if (heightError < -20f)
            AddReward(-0.003f);

        // 4. Goal raggiunto
        if (currentDistance < goalRadius)
        {
            AddReward(15f);
            hasCollided = false;
            if (visualMode)
                StartCoroutine(DelayedEndEpisode(2f));
            else
                EndEpisode();
        }

        // 6. FUORI BOUNDS — unica terminazione forzata
        if (Vector3.Distance(transform.position, spawnPoint.position) > maxDistance
            || transform.position.y < 0f)
        {
            AddReward(-5f);
            EndEpisode();
        }

        if (currentDistance < 20f)
            Debug.Log($"Vicino al goal! Distanza: {currentDistance}, GoalRadius: {goalRadius}");

        if (Time.frameCount % 50 == 0)
        Debug.Log($"toGoal_local: {transform.InverseTransformDirection((goalPoint.position - transform.position).normalized)}, uprightness: {Vector3.Dot(transform.up, Vector3.up):F2}");

        episodeTime += Time.fixedDeltaTime;
    }


    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == null) return;

        string tag = collision.gameObject.tag;
        if (tag == "Untagged" && collision.transform.parent != null)
            tag = collision.transform.parent.tag;
        if (tag == "Ground" || tag == "Boundary" || tag == "Obstacle")
        {
            hasCollided = true;
            //collisionCount++;
            collisionFlashTime = 2f;
            AddReward(-3f);
            StartCoroutine(DelayedEndEpisode(2.0f)); // pausa 1.5s prima del respawn
            //EndEpisode();
        }
    }

    private System.Collections.IEnumerator DelayedEndEpisode(float delay)
    {
        rb.linearVelocity = Vector3.zero;  // ferma il drone
        rb.angularVelocity = Vector3.zero;
        yield return new WaitForSeconds(delay);
        EndEpisode();
    }

    void Update()
    {
        if (collisionFlashTime > 0f)
        {
            collisionFlashTime -= Time.deltaTime;
            if (collisionFlashTime <= 0f)
                hasCollided = false;
        }
    }
    

    // Heuristic per test manuale
    /*
        Provides manual control of the agent for debugging and heuristic testing.
        This bypasses the neural network policy, allowing a human player to pilot the drone.
        Input:
        - [0] Thrust: Space key (+1f) to ascend, default (-0.5f) to descend/idle
        - [1] Pitch: Vertical axis (W/S or Up/Down arrows) for forward/backward tilt.
        - [2] Roll: Horizontal axis (A/D or Left/Right arrows) for side tilt.
        - [3] Yaw: Q key (-1f) to rotate left, E key (+1f) to rotate right.
    */
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var c = actionsOut.ContinuousActions;
        c[0] = Input.GetKey(KeyCode.Space) ? 1f : -0.5f; // thrust
        c[1] = Input.GetAxis("Vertical");                 // pitch
        c[2] = Input.GetAxis("Horizontal");               // roll
        c[3] = Input.GetKey(KeyCode.Q) ? -1f :
               Input.GetKey(KeyCode.E) ?  1f : 0f;       // yaw
    }

}