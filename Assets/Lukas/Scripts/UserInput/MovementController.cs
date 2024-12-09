using Lukas.Scripts.Core.Skills;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class MovementController : MonoBehaviour
{
    Vector2 moveInput;
    Vector2 playerLook;
    Vector3 dashDirection;
    Vector3 currentDashVelocity;
    Rigidbody playerRigidbody;
    SkillSelector skillSelector;

    float xRotation;

    float dashTime;
    float dashCooldownTimer;

    [SerializeField] [Min(1)] float moveSpeed = 1.0f;
    [SerializeField] float lookSensitivity = 2.0f;
    [SerializeField] Camera playerCamera;
    [SerializeField] [Range(0, 90)] float cameraClampAngle;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 1f;
    [SerializeField] float dashForce;
    [SerializeField] AnimationCurve dashCurve;
    
    [SerializeField] float spawnDistance;

    void Awake()
    {
        skillSelector = GetComponent<SkillSelector>();
        playerRigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        dashCurve ??= AnimationCurve.EaseInOut(0, 1, 1, 0);
    }

    void FixedUpdate()
    {
        DoMove();
        if (dashCooldownTimer > 0) dashCooldownTimer -= Time.fixedDeltaTime;
        if (dashTime > 0)
        {
            dashTime -= Time.fixedDeltaTime;
            float dashFactor = dashCurve.Evaluate(1 - (dashTime / dashDuration));
            currentDashVelocity = dashDirection * (dashForce * dashFactor);
        }
        else
        {
            currentDashVelocity = Vector3.zero;
        }
    }

    void DoMove()
    {
        var finalDirection = transform.TransformDirection(moveInput.x * moveSpeed, 0, moveInput.y * moveSpeed);
        finalDirection += currentDashVelocity;
        playerRigidbody.velocity = new Vector3(finalDirection.x, playerRigidbody.velocity.y, finalDirection.z);
    }

    public void Look(InputAction.CallbackContext _callbackContext)
    {
        playerLook = _callbackContext.ReadValue<Vector2>();
        float lookX = playerLook.x * lookSensitivity;
        float lookY = playerLook.y * lookSensitivity;

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -cameraClampAngle, cameraClampAngle);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * lookX);
    }

    public void Move(InputAction.CallbackContext _callbackContext)
    {
        moveInput = _callbackContext.ReadValue<Vector2>();
    }

    public void Dash(InputAction.CallbackContext _callbackContext)
    {
        if (!_callbackContext.started || !(dashCooldownTimer <= 0)) return;

        dashDirection = transform.TransformDirection(moveInput.x, 0, moveInput.y).normalized;
        if (dashDirection == Vector3.zero) dashDirection = transform.forward;
        dashTime = dashDuration;
        dashCooldownTimer = dashCooldown;
        playerRigidbody.AddForce(dashDirection * dashForce, ForceMode.Impulse);
    }

    public void ChangeSkill(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.phase != InputActionPhase.Started) return;
        float scrollDirection = _callbackContext.ReadValue<float>();
        Debug.Log(scrollDirection);
        if(scrollDirection > 0) skillSelector.UpdateSelectedSkill(1);
        else skillSelector.UpdateSelectedSkill(-1);
        Debug.Log(skillSelector.CurrentSelectedSkill.name);
    }

    public void Fire(InputAction.CallbackContext _callbackContext)
    {
        if (!_callbackContext.started) return;
        var instance = Instantiate(skillSelector.CurrentSelectedSkill, playerCamera.transform.position + playerCamera.transform.forward * spawnDistance, playerCamera.transform.rotation);
        instance.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * 50, ForceMode.Impulse);
    }
}