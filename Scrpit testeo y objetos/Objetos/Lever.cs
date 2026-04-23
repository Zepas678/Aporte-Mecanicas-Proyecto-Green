using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] Trigger_Door trigger_Door;
    void OnTriggerEnter(Collider other)
    {
        if (GameObject.FindGameObjectWithTag("Tongue_attack"))
        {
            trigger_Door.onOpen();
            trigger_Door.enabled = false;
        }
    }
}
