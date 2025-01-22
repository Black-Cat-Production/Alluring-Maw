using System.Collections;
using Lukas.Scripts.Core;
using Lukas.Scripts.Core.Skills;
using Lukas.Scripts.Core.Visual;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lukas.Scripts.UserInput
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class InputController : MonoBehaviour
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

        bool isAllowedCasting;
    
        [SerializeField] [Min(1)] float moveSpeed = 1.0f;
        [SerializeField] float lookSensitivity = 2.0f;
        [SerializeField] Camera playerCamera;
        [SerializeField] [Range(0, 90)] float cameraClampAngle;
        [SerializeField] float dashDuration = 0.2f;
        [SerializeField] float dashCooldown = 1f;
        [SerializeField] float dashForce;
        [SerializeField] AnimationCurve dashCurve;
        [SerializeField] Animator animator;
        [SerializeField] float cancelCastCooldown;
    
    
        [SerializeField] MaterialHandler materialHandler;


        bool castingBlockRunning;
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
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("SkillHoldState"))
            {
                animator.SetBool("CanRelease", false);
                animator.SetBool("isHoldingSkill", false);
                animator.SetTrigger("CancelCasting");
                if (castingBlockRunning) return;
                StartCoroutine(BlockCasting());
            }
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
            if(scrollDirection > 0) skillSelector.UpdateSelectedSkill(1);
            else skillSelector.UpdateSelectedSkill(-1);
        }

        public void Fire(InputAction.CallbackContext _callbackContext)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("IdleState")) return;
            if (!_callbackContext.started) return;
            animator.SetTrigger("BaseAttack");
        }

        public void CastSkill(InputAction.CallbackContext _callbackContext)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("IdleState") && !animator.GetCurrentAnimatorStateInfo(0).IsName("SkillHoldState") || animator.GetBool("IsCastingBlocked")) return;
            if (!skillSelector.CanCastSpell()) return;
            if (_callbackContext.phase == InputActionPhase.Started)
            {
                animator.SetBool("isHoldingSkill", true);
                PushMaterialChange(false);
            }
            if (_callbackContext.phase != InputActionPhase.Canceled) return;
            animator.SetTrigger("ReleaseSkill");
            animator.ResetTrigger("CancelCasting");
            animator.SetBool("isHoldingSkill", false);
            PushMaterialChange(true);
        }

        public void ChangeReleaseBool(int _value)
        {
            bool value = _value != 0;
            animator.SetBool("CanRelease", value) ;
        }

        public void ReturnToMainMenu(InputAction.CallbackContext _callbackContext)
        {
            GameManager.Instance.RetreatToMainMenu();
        }

        IEnumerator BlockCasting()
        {
            castingBlockRunning = true;
            animator.SetBool("IsCastingBlocked", true);
            yield return new WaitForSeconds(cancelCastCooldown);
            animator.SetBool("IsCastingBlocked", false);
            castingBlockRunning = false;
        }

        void PushMaterialChange(bool _reverse)
        {
            var tagList = skillSelector.GetSelectedSkillTags();
            if (_reverse)
            {
                materialHandler.UpdateMaterialOnMeshes(EMaterialType.DefaultProtag);
                return;
            }
            if(tagList.Contains(ESkillTag.Light)) materialHandler.UpdateMaterialOnMeshes(EMaterialType.LightProtag);
            if(tagList.Contains(ESkillTag.Dark)) materialHandler.UpdateMaterialOnMeshes(EMaterialType.DarkProtag);
        }
    }
}