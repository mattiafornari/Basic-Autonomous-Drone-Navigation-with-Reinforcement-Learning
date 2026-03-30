using UnityEngine;

public class RotateTurbine : MonoBehaviour
{
    [Header("Impostazioni Turbina")]
    [Tooltip("Velocità di rotazione in gradi al secondo. Utilizzare i valori negativi per invertire il senso.")]
    public float speed = 100f;

    [Tooltip("Asse di rotazione locale (X, Y o Z). mettere 1 sull'asse che punta in avanti.")]
    public Vector3 rotationAxis = new Vector3(1, 0, 0);

    void Update()
    {
        // moltiplico per Time.deltaTime per rendere la rotazione 
        // fluida e indipendente dal frame rate del PC
        transform.Rotate(rotationAxis * speed * Time.deltaTime, Space.Self);
    }
}