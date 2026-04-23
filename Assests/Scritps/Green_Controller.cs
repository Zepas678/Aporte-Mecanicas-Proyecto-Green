using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Green_Controller : MonoBehaviour
{
    public float speedWalk;

    public float speedRun;

    public float speedPush;

    [SerializeField] float speedCurrent;

    [SerializeField] Transform weaponExit;

    [SerializeField] SwithWeapon swithWeapon;

    new Rigidbody rigidbody;

    Vector2 moveInput;

    Vector3 movement;

    Animator animator;

    PlayerInput playerInput;

    InputAction a_move;

    InputAction a_attack;

    InputAction a_push;

    InputAction a_pause;

    bool movementIsExecute;

    float timeToFire;

    [HideInInspector] public AudioClip clicAttack;

    [HideInInspector] public GameObject weapon;

    [HideInInspector] public bool isPower;

    [HideInInspector] public bool canPush;

    [HideInInspector] public bool pushIsExecute;

    [HideInInspector] public int bulletWeapon;

    [HideInInspector] public float timeFire;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = transform.Find("Green_Model").GetComponent<Animator>();

        a_move = playerInput.actions["Move"];
        a_attack = playerInput.actions["Attack"];
        a_push = playerInput.actions["Push"];

        timeToFire = 0;

    }

    void OnEnable()
    {
        a_move.started += onMove;
        a_move.performed += onMove;
        a_move.canceled += onMove;

        a_attack.performed += onAttack;

        a_push.started += onPush;
        a_push.performed += onPush;
        a_push.canceled += onPush;

    }

    void OnDisable()
    {
        a_move.started -= onMove;
        a_move.performed -= onMove;
        a_move.canceled -= onMove;

        a_attack.performed -= onAttack;

        a_push.started -= onPush;
        a_push.performed -= onPush;
        a_push.canceled -= onPush;

    }

    void FixedUpdate()
    {
        if (!(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.ToString() == "green_attack") ||
        !(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.ToString() == "green_hurt"))
        {
            rigidbody.AddForce(movement * speedCurrent, ForceMode.Impulse);
        }
    }
    // Update is called once per frame
    void Update()
    {
        handleRotationChar();
        if (moveInput.magnitude == 0)
        {
            speedCurrent = 0;
        }
        if (moveInput.magnitude >= 0.1 && moveInput.magnitude <= 0.49 && !pushIsExecute)
        {
            speedCurrent = speedWalk;
            // Debug.Log("Caminando " + moveInput.magnitude);
        }
        if (moveInput.magnitude >= 0.5 && moveInput.magnitude <= 1 && !pushIsExecute)
        {
            speedCurrent = speedRun;
            // Debug.Log("Corriendo " + moveInput.magnitude);
        }

        if (moveInput.magnitude >= 0.1 && pushIsExecute && canPush)
        {
            speedCurrent = speedPush;
        }

        movement = new Vector3(-moveInput.y, transform.position.y, moveInput.x).normalized;
        animator.SetFloat("speedCurrent", speedCurrent);
        animator.SetBool("isPushing", pushIsExecute);
        animator.SetBool("isMoving", movementIsExecute);

        // Debug.Log(GameObject.Find("Ice_Muzzle").activeSelf);
    }

    void onMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        movementIsExecute = moveInput.x != 0 || moveInput.y != 0;
        //Debug.Log(moveInput.magnitude);
    }

    void onAttack(InputAction.CallbackContext context)
    {
        if (!movementIsExecute && !isPower)
        {
            animator.SetTrigger("Attack");
            AudioManager.instance.playSound(clicAttack);
        }
        else
        {
            if (!movementIsExecute && isPower)
            {
                onShootingWeapon(weapon);
            }
            if (movementIsExecute && isPower)
            {
                onShootingWeapon(weapon);
            }
        }
    }

    void onShootingWeapon(GameObject weapon)
    {
        if (bulletWeapon >= 1)
        {
            if (Time.time >= timeToFire)
            {
                animator.SetTrigger("Attack_2");
                onMuzzle(weapon);
                Instantiate(weapon, weaponExit.position, weaponExit.rotation * Quaternion.Euler(270f, 0f, 0f));
                timeToFire = Time.time + 1 / timeFire;
                AudioManager.instance.playSound(clicAttack);
                bulletWeapon--;
                swithWeapon.shotBulletWeapon(bulletWeapon, weapon);
            }
        }
    }

    void onMuzzle(GameObject weapon)
    {
        switch (weapon.name)
        {
            case "Weapon_Ice":
                gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case "Weapon_Fire":
                gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case "Weapon_Electric":
                gameObject.transform.GetChild(2).gameObject.transform.GetChild(2).gameObject.SetActive(true);
                break;
        }

    }

    void onPush(InputAction.CallbackContext context)
    {
        if (canPush)
        {
            if (context.started)
            {
                pushIsExecute = true;
            }

            if (context.canceled)
            {
                pushIsExecute = false;
            }

        }

        if (!canPush)
        {
            pushIsExecute = false;
        }
    }

    void handleRotationChar()
    {
        Vector3 positionLookAt;
        positionLookAt.x = movement.x;
        positionLookAt.y = 0.0f;
        positionLookAt.z = movement.z;

        Quaternion currentRotation = transform.rotation;

        if (movementIsExecute && !pushIsExecute)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 15f * Time.deltaTime);
        }

    }


    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Box")
        {
            if (pushIsExecute && animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.ToString() == "green_push")
            {
                Vector3 forceDirection = collision.gameObject.transform.position - transform.position;
                forceDirection.y = 0f;


                rigidbody.AddForceAtPosition(forceDirection.normalized * 3f, transform.position, ForceMode.Impulse);
            }
        }
    }

}
