using System.Collections;
using Scripts.Core;
using Scripts.Core.AudioScripts;
using Scripts.Core.Skills;
using Scripts.Core.UI;
using Scripts.Core.Visual;
using Scripts.Program;
using UnityEngine;
using UnityEngine.InputSystem;
using WWISE_Integration_Scripts;

namespace Scripts.UserInput
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
        PlayerSounds playerSounds;

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
        [SerializeField] Animator animator;
        [SerializeField] float cancelCastCooldown;
        [SerializeField] PauseUI pauseUI;
        [SerializeField] OptionsSaveSO optionsSaveSO;
        [SerializeField] float idleWalkBlendTime;
        [SerializeField] PlayerSkillAudio skillAudio;


        [SerializeField] MaterialHandler materialHandler;


        bool castingBlockRunning;
        bool isCastingSkill;

        public bool HasMoveInput => moveInput != Vector2.zero;

        Coroutine routine;

        static readonly int canRelease = Animator.StringToHash("CanRelease");
        static readonly int cancelCasting = Animator.StringToHash("CancelCasting");
        static readonly int dash = Animator.StringToHash("Dash");
        static readonly int xDirection = Animator.StringToHash("XDirection");
        static readonly int yDirection = Animator.StringToHash("YDirection");
        static readonly int baseAttack = Animator.StringToHash("BaseAttack");
        static readonly int isCastingBlocked = Animator.StringToHash("IsCastingBlocked");
        static readonly int isHoldingSkillLight = Animator.StringToHash("isHoldingSkillLIGHT");
        static readonly int isHoldingSkillDark = Animator.StringToHash("isHoldingSkillDARK");

        void Awake()
        {
            skillSelector = GetComponent<SkillSelector>();
            playerRigidbody = GetComponent<Rigidbody>();
            playerSounds = GetComponent<PlayerSounds>();
            Cursor.lockState = CursorLockMode.Locked;
            dashCurve ??= AnimationCurve.EaseInOut(0, 1, 1, 0);
            lookSensitivity = Remap(optionsSaveSO.MouseSense, 0f, 100f, 0.01f, 1f);
            StartCoroutine(FootstepRoutine());
        }

        void OnEnable()
        {
            skillSelector.OnSkillGotCast += ResetIsCasting;
        }

        void OnDisable()
        {
            skillSelector.OnSkillGotCast -= ResetIsCasting;
        }

        void ResetIsCasting()
        {
            StartCoroutine(ResetCastingWithDelay());
        }

        IEnumerator ResetCastingWithDelay()
        {
            yield return new WaitForSeconds(0.1f);
            isCastingSkill = false;
        }

        void FixedUpdate()
        {
            if (GameManager.Instance.IsPaused) return;
            DoMove();
            if (dashCooldownTimer > 0) dashCooldownTimer -= Time.fixedDeltaTime;
            if (dashTime > 0)
            {
                dashTime -= Time.fixedDeltaTime;
                float dashFactor = dashCurve.Evaluate(1 - dashTime / dashDuration);
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
            float isMovingFloat = moveInput.normalized.magnitude > 0 ? 1 : 0;
            animator.SetFloat("MoveInput", isMovingFloat, idleWalkBlendTime, Time.fixedDeltaTime);
        }

        public void Look(InputAction.CallbackContext _callbackContext)
        {
            if (GameManager.Instance.IsPaused) return;
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
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("SkillHoldStateLIGHT") || animator.GetCurrentAnimatorStateInfo(0).IsName("SkillHoldStateDARK"))
            {
                animator.SetBool(canRelease, false);
                animator.SetBool(isHoldingSkillLight, false);
                animator.SetBool(isHoldingSkillDark, false);
                animator.SetTrigger(cancelCasting);
                if (castingBlockRunning) return;
                StartCoroutine(BlockCasting());
                ResetIsCasting();
                StartCoroutine(ReturnToDefaultMaterial());
            }

            animator.SetTrigger(dash);
            dashDirection = transform.TransformDirection(moveInput.x, 0, moveInput.y).normalized;
            animator.SetFloat(xDirection, moveInput.normalized.x);
            animator.SetFloat(yDirection, moveInput.normalized.y);
            if (dashDirection == Vector3.zero) dashDirection = transform.forward;
            dashTime = dashDuration;
            dashCooldownTimer = dashCooldown;
            playerRigidbody.AddForce(dashDirection * dashForce, ForceMode.Impulse);
            playerSounds.PlayDashEvent();
        }

        public void ChangeSkill(InputAction.CallbackContext _callbackContext)
        {
            if (_callbackContext.phase != InputActionPhase.Started) return;
            if (isCastingSkill) return;
            float scrollDirection = _callbackContext.ReadValue<float>();
            if (scrollDirection > 0) skillSelector.UpdateSelectedSkill(1);
            else skillSelector.UpdateSelectedSkill(-1);
            playerSounds.PlaySkillSelectionSound();
        }

        public void Fire(InputAction.CallbackContext _callbackContext)
        {
            if (GameManager.Instance.IsPaused) return;
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("IdleState")) return;
            if (!_callbackContext.started) return;
            animator.SetTrigger(baseAttack);
            playerSounds.PlayLMCEvent();
        }

        public void CastSkill(InputAction.CallbackContext _callbackContext)
        {
            if (GameManager.Instance.IsPaused) return;
            if ((!animator.GetCurrentAnimatorStateInfo(0).IsName("IdleState") && !animator.GetCurrentAnimatorStateInfo(0).IsName("SkillHoldStateLIGHT") &&
                 !animator.GetCurrentAnimatorStateInfo(0).IsName("SkillHoldStateDARK")) || animator.GetBool(isCastingBlocked)) return;
            if (!skillSelector.CanCastSpell()) return;
            if (_callbackContext.phase == InputActionPhase.Started)
            {
                bool isLightSkill = skillSelector.GetSelectedSkillTags().Contains(ESkillTag.Light);
                switch (isLightSkill)
                {
                    case true:
                        playerSounds.PlayChargeEventLight();
                        break;
                    case false:
                        playerSounds.PlayChargeDarkEvent();
                        break;
                }
                isCastingSkill = true;
                animator.SetBool(skillSelector.GetSelectedSkillTags().Contains(ESkillTag.Light) ? isHoldingSkillLight : isHoldingSkillDark, true);
                if (routine != null) StopCoroutine(routine);
                PushMaterialChange(false);
            }

            if (_callbackContext.phase != InputActionPhase.Canceled) return;
            animator.ResetTrigger(cancelCasting);
            animator.SetBool(isHoldingSkillLight, false);
            animator.SetBool(isHoldingSkillDark, false);
            routine = StartCoroutine(ReturnToDefaultMaterial());
        }

        IEnumerator ReturnToDefaultMaterial()
        {
            while (!materialHandler.IsFinishedChangingUp()) yield return null;
            PushMaterialChange(true);
        }

        public void ChangeReleaseBool(int _value)
        {
            bool value = _value != 0;
            animator.SetBool(canRelease, value);
        }

        public void Pause(InputAction.CallbackContext _callbackContext)
        {
            if (_callbackContext.phase != InputActionPhase.Started) return;
            pauseUI.TogglePauseUI();
        }

        public void ReturnToMainMenu()
        {
            GameManager.Instance.RetreatToMainMenu();
        }

        IEnumerator BlockCasting()
        {
            castingBlockRunning = true;
            animator.SetBool(isCastingBlocked, true);
            yield return new WaitForSeconds(cancelCastCooldown);
            animator.SetBool(isCastingBlocked, false);
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

            if (tagList.Contains(ESkillTag.Light)) materialHandler.UpdateMaterialOnMeshes(EMaterialType.LightProtag);
            if (tagList.Contains(ESkillTag.Dark)) materialHandler.UpdateMaterialOnMeshes(EMaterialType.DarkProtag);
        }

        float Remap(float _value, float _fromMin, float _fromMax, float _toMin, float _toMax)
        {
            return _toMin + (_value - _fromMin) * (_toMax - _toMin) / (_fromMax - _fromMin);
        }

        IEnumerator FootstepRoutine()
        {
            while (gameObject.activeInHierarchy)
            {
                Debug.Log($"MoveInput: {HasMoveInput} -- CurrentDashVelocity: {currentDashVelocity}");
                if (!HasMoveInput || currentDashVelocity != Vector3.zero)
                {
                    yield return null;
                }
                else
                {
                    Debug.Log("PLAYING FOOTSTEPS");
                    playerSounds.PlayFootstepEvent();
                    yield return null;
                    yield return new WaitForSeconds(playerSounds.FootstepInterval);
                }
            }
        }
    }
}