
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static float RUN_CONDITION = 0.5f;

    private CharacterController controller;
    public Animator animator;

    [Header("Default Config")]
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float runSpeed = 3f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float rotateSpeed = 0.1f;

    [Space(10)]
    [SerializeField] private Joystick joyStick;

    [Space(10)]
    [Header("Gravity")]
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.4f;

    [Space(10)]
    [Header("Interact")]
    [SerializeField] private LayerMask interactMask;
    [SerializeField] private float interactRadious;
    [SerializeField] private Transform interactTarget;

    float currentVelocity;
    bool isGround = false;
    Vector3 velocity;
    Interactible interactibleTarget;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        PlayerInteract();
        Movement(new(joyStick.Horizontal, joyStick.Vertical));
        Gravity();
    }
    private void Movement(Vector2 input)
    {
        float currentSpeed = 0f;
        Vector3 moveDir = new Vector3(input.x, 0f, input.y).normalized;
        if (moveDir.magnitude >= 0.1f)
        {
            currentSpeed = walkSpeed;
            float magnitude = input.magnitude;
            float mappedValue = Mathf.Clamp01(magnitude);
            if (mappedValue >= RUN_CONDITION)
            {
                currentSpeed = runSpeed;
            }
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, rotateSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            controller.Move(currentSpeed * Time.deltaTime * moveDir);
        }
        animator.SetFloat("Speed", currentSpeed);
    }
    public void Gravity()
    {
        isGround = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
        velocity.y += gravity * Time.deltaTime;
        if (isGround && velocity.y < 0f)
        {
            velocity.y = -2f;
            animator.SetBool("Jump", false);
        }
        controller.Move(velocity * Time.deltaTime);
    }
    public void Jump()
    {
        if (isGround)
        {
            velocity.y = Mathf.Sqrt(gravity * -2f * jumpHeight);
            animator.SetBool("Jump", true);
        }
    }
    private void PlayerInteract()
    {
        InteractController.instance.ClearInteractText();
        Collider[] hit = Physics.OverlapSphere(interactTarget.position, interactRadious, interactMask);
        if (hit.Length > 0)
        {
            if (hit[0].gameObject.TryGetComponent<Interactible>(out var interactible))
            {
                interactibleTarget = interactible;
                Vector3 screenPos = Camera.main.WorldToScreenPoint(hit[0].transform.position);
                Vector3 offset = Vector3.zero;
                if (interactibleTarget.TryGetComponent<Offset>(out var targetOffset))
                {
                    offset.y = targetOffset.interactOffsetY;
                    offset.x = targetOffset.interactOffsetX;
                }
                InteractController.instance.ChangeInteractText(interactibleTarget.promptMessage, screenPos + offset);
            }
            else
            {
                interactibleTarget = null;
            }
        }
        else
        {
            interactibleTarget = null;
        }
    }
    public void OnInteract()
    {
        interactibleTarget?.Interact();
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactTarget.position, interactRadious);
    }

}
