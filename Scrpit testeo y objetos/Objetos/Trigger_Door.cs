using UnityEngine;

public class Trigger_Door : MonoBehaviour
{
    [SerializeField] Animator door_animator;

    public bool state;

    void Awake()
    {
        state = false;
    }

    void Start()
    {
        door_animator = GetComponent<Animator>();
    }

    public void onOpen()
    {
        state = true;
        door_animator.SetBool("Open", state);
    }


}
