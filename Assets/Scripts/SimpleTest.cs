// Crea: Assets/Scripts/SimpleTest.cs

using UnityEngine;

public class SimpleTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("SPACE PRESSED!");
            GetComponent<Rigidbody>().AddForce(Vector3.up * 100f);
        }
    }
}
