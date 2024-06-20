using UnityEngine;

public class TractorController : MonoBehaviour
{

    [SerializeField] public bool canMove = true;
    [SerializeField] public Rigidbody sphereRb;
    [SerializeField] public float speed = 200f;
    [SerializeField] public float reverseSpeed = 100f;
    [SerializeField] float turnSpeed = 100f;

    private PlayerInputActions inputActions;


    void Awake()
    {
        inputActions = new();

        sphereRb.transform.SetParent(null);
    }


    void Update()
    {
        canMove = Level1GameplayLogic.gameInProgress;
        HandleMovement();
    }


    private void HandleMovement()
    {
        if (!canMove) return;

        //pos
        transform.position = sphereRb.transform.position;
        //rot
        float yRot = inputActions.Level1.Movement.ReadValue<Vector2>().x * turnSpeed * Time.deltaTime;
        transform.Rotate(0, yRot, 0);

    }

    void FixedUpdate()
    {
        if (!canMove) return;
        sphereRb.AddForce(transform.forward * inputActions.Level1.Movement.ReadValue<Vector2>().y * (inputActions.Level1.Movement.ReadValue<Vector2>().y > 0 ? speed : reverseSpeed), ForceMode.Acceleration);
    }

    void OnEnable()
    {
        inputActions.Enable();
    }
    void OnDisable()
    {
        inputActions.Disable();

    }



}
