using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NifuDev
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance;

        [SerializeField] private PlayerData data;

        #region COMPONENTS
        private Rigidbody2D rb_;
        [SerializeField] private SpriteRenderer playerSprite;
        [SerializeField] private Transform wallCheckTransform;
        [SerializeField] private Transform slowMotionBubbleTransform;
        private Animator animator;
        #endregion

        #region STATE PARAMETERS
        private bool isGrounded_;
        private bool isJumping_;
        private bool isJumpCut_;

        private bool isOnRightWall_;
        private bool isOnLeftWall_;

        private bool wasOnGround = true;

        private bool isHoldingBubble = true;
        #endregion

        #region TIMER
        private float lastOnGroundTime;
        private float lastOnWallTime;
        private float lastOnWallRightTime;
        private float lastOnWallLeftTime;
        private float lastPressedJumpTime;
        #endregion

        #region INPUT PARAMETERS
        private Vector2 moveInput_;
        #endregion

        #region CHECK PARAMETERS
        [Header("Ground Checks")]
        [SerializeField] private Transform groundCheckPoint_;
        [SerializeField] private Vector2 groundCheckSize_;
        [Space(5)]
        [Header("Wall Checks")]
        [SerializeField] private Transform frontWallCheckPoint_;
        [SerializeField] private Transform backWallCheckPoint_;
        [SerializeField] private Vector2 wallCheckSize_;
        #endregion

        #region Layers & Tags
        [Header("Layers & Tags")]
        [SerializeField] private LayerMask groundLayer_;
        [Space(5)]
        #endregion

        [SerializeField] private ParticleSystem footstepsParticleSystem;
        [SerializeField] private ParticleSystem impactParticleSystem;
        private ParticleSystem.EmissionModule footEmission;

        private bool isFacingRight;

        [SerializeField] private bool isUsingDeveloperMode;

        private void Awake() {
            if (rb_ == null) {
                rb_ = GetComponent<Rigidbody2D>();
            }
            Instance = this;

            animator = GetComponent<Animator>();
            if (footstepsParticleSystem != null) {
                footEmission = footstepsParticleSystem.emission;
            }
        }

        private void Start() {
            isFacingRight = true;
            SetGravityScale(data.gravityScale);
        }

        private void Update() {
            #region TIMERS
            lastOnGroundTime -= Time.deltaTime;
            lastPressedJumpTime -= Time.deltaTime;
            #endregion

            moveInput_.x = Input.GetAxisRaw("Horizontal");
            moveInput_.y = Input.GetAxisRaw("Vertical");

            #region GENERAL CHECKS
            if (moveInput_.x != 0) {
                CheckDirectionToFace(moveInput_.x > 0.01f);
                if (!isJumping_ && isGrounded_) {
                }
            }
            #endregion

            SetSlowMotionBubble();

            #region PHYSICS CHECKS
            if (!isJumping_) {
                //Ground Check
                if (Physics2D.OverlapBox(groundCheckPoint_.position, groundCheckSize_, 0, groundLayer_)) {
                    lastOnGroundTime = data.coyoteTime;
                    isGrounded_ = true;
                }
                else {
                    isGrounded_ = false;
                }
                //Right Wall Check
                if (((Physics2D.OverlapBox(frontWallCheckPoint_.position, wallCheckSize_, 0, groundLayer_) && isFacingRight)
                        || (Physics2D.OverlapBox(backWallCheckPoint_.position, wallCheckSize_, 0, groundLayer_) && !isFacingRight))) {
                    lastOnWallRightTime = data.coyoteTime;
                }
                //Left Wall Check
                if (((Physics2D.OverlapBox(frontWallCheckPoint_.position, wallCheckSize_, 0, groundLayer_) && !isFacingRight)
                    || (Physics2D.OverlapBox(backWallCheckPoint_.position, wallCheckSize_, 0, groundLayer_) && isFacingRight))) {
                    lastOnWallLeftTime = data.coyoteTime;
                }

                lastOnWallTime = Mathf.Max(lastOnWallLeftTime, lastOnWallRightTime);
            }
            #endregion


            #region JUMP
            OnJump();

            if (animator != null) {
                if (rb_.velocity.y < 0 && isJumping_) {
                    isJumping_ = false;
                    animator.SetBool("IsJumping", false);
                }
                if (rb_.velocity.y > 0 && isJumping_) {
                    animator.SetBool("IsJumping", true);
                    animator.SetBool("IsJumpFalling", false);
                }

                if (rb_.velocity.y < 0 && !isGrounded_) {
                    animator.SetBool("IsJumpFalling", true);
                }
            }
            else {
                if (rb_.velocity.y < 0 && isJumping_) {
                    isJumping_ = false;
                }
            }

            if (CanJump() && lastPressedJumpTime > 0) {
                isJumping_ = true;
                isJumpCut_ = false;
                Jump();
            }

            //Jump Cut
            if (Input.GetKeyUp(KeyCode.Space)) {
                OnJumpUp();
            }
            #endregion

            #region Gravity
            if (rb_.velocity.y < 0) {
                SetGravityScale(data.gravityScale * data.fallGravityMult);
                rb_.velocity = new Vector2(rb_.velocity.x, Mathf.Max(rb_.velocity.y, -data.fallClamp_));
            }
            else if (isJumpCut_) {
                SetGravityScale(data.gravityScale * data.jumpCutGravityMult);
                rb_.velocity = new Vector2(rb_.velocity.x, Mathf.Max(rb_.velocity.y, -data.fallClamp_));
            }
            else if (isJumping_ && Mathf.Abs(rb_.velocity.y) < data.jumpApexTimeThreshold) {
                SetGravityScale(data.gravityScale * data.jumpApexGravityMult);
                Debug.Log("Apex");
            }
            else {
                SetGravityScale(data.gravityScale);
            }

            #endregion

            if (lastOnGroundTime > 0 && Mathf.Abs(moveInput_.x) < 0.01f) {
                float amount = Mathf.Min(Mathf.Abs(rb_.velocity.x), Mathf.Abs(data.frictionAmount));
                amount *= Mathf.Sign(rb_.velocity.x);
                rb_.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
            }

            #region Animator & ParticleSystem
            if (footstepsParticleSystem != null) {
                if (!wasOnGround && isGrounded_) {
                    PlayImpactPSEffect();
                    animator.SetTrigger("LandingTrigger");
                    animator.SetBool("IsJumpFalling", false);
                    //SoundManager.PlaySound(SoundManager.SoundType.JumpLanding, 0.75f);
                }
            }
            wasOnGround = isGrounded_;

            if (footstepsParticleSystem != null) {
                if (Mathf.Abs(rb_.velocity.x) > 0.01f && isGrounded_ && !isJumping_) {
                    footEmission.rateOverTime = 25f;
                }
                else {
                    footEmission.rateOverTime = 0f;
                }
            }


            #endregion
        }

        private void FixedUpdate() {
            Run(1);
        }

        private void Run(float lerpAmounts) {
            float targetSpeed = moveInput_.x * data.moveSpeed_;
            targetSpeed = Mathf.Lerp(rb_.velocity.x, targetSpeed, lerpAmounts);

            float accelRate;

            if (lastOnGroundTime > 0) {
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.acceleration_ : data.deccelaration_;
            }
            else {
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.acceleration_ * data.accelInAir : data.deccelaration_ * data.deccelInAir;
            }

            if ((isJumping_) && Mathf.Abs(rb_.velocity.y) < data.jumpApexTimeThreshold) {
                accelRate *= data.jumpApexAccelerationMult;
                targetSpeed *= data.jumpApexMaxSpeedMult;
            }

            float speedDiff = targetSpeed - rb_.velocity.x;

            float movement = speedDiff * accelRate;

            if (animator != null) {
                animator.SetFloat("Speed", Mathf.Abs(moveInput_.x));
                //SoundManager.PlaySound(SoundManager.SoundType.PlayerFootsteps,1f);
            }
            rb_.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }

        private void Jump() {
            lastPressedJumpTime = 0;
            lastOnGroundTime = 0;

            #region Perform Jump
            float force = data.jumpForce_;
            if (rb_.velocity.y < 0)
                force -= rb_.velocity.y;

            rb_.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            #endregion
        }

        private bool CanJump() {
            return lastOnGroundTime > 0 && !isJumping_;
        }

        public void OnJump() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                lastPressedJumpTime = data.jumpInputBufferTime;
            }
        }

        public void SetSlowMotionBubble() {

            if (Input.GetKeyDown(KeyCode.E) && isHoldingBubble) {
                Transform newSlowMotionBubbleTransform = Instantiate(slowMotionBubbleTransform, slowMotionBubbleTransform.position, Quaternion.identity);
                Destroy(slowMotionBubbleTransform.gameObject);
                slowMotionBubbleTransform = newSlowMotionBubbleTransform;
                isHoldingBubble = false;
            }
            else if (Input.GetKeyDown(KeyCode.E) && !isHoldingBubble) {
                slowMotionBubbleTransform.transform.SetParent(this.transform);
                slowMotionBubbleTransform.transform.position = transform.position;
                isHoldingBubble = true;
            }
        }

        public void OnJumpUp() {
            if (CanJumpCut()) {
                isJumpCut_ = true;
            }
        }

        private bool CanJumpCut() {
            return isJumping_ && rb_.velocity.y > 0;
        }

        private void JumpCut() {
            rb_.AddForce(Vector2.down * rb_.velocity * (1f - data.jumpCutGravityMult), ForceMode2D.Impulse);
        }
        public void SetGravityScale(float scale) {
            rb_.gravityScale = scale;
        }

        private void Turn() {
            playerSprite.flipX = !playerSprite.flipX;

            Vector3 wallCheckScale = wallCheckTransform.localScale;
            wallCheckScale.x *= -1f;
            wallCheckTransform.localScale = wallCheckScale;

            isFacingRight = !isFacingRight;
        }

        #region CHECK METHODS
        public void CheckDirectionToFace(bool isMovingRight) {
            if (isMovingRight != isFacingRight)
                Turn();
        }
        #endregion

        private void PlayImpactPSEffect() {
            impactParticleSystem.gameObject.SetActive(true);
            impactParticleSystem.Stop();
            impactParticleSystem.transform.position = footstepsParticleSystem.transform.position;
            impactParticleSystem.Play();
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheckPoint_.position, groundCheckSize_);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(frontWallCheckPoint_.position, wallCheckSize_);
            Gizmos.DrawWireCube(backWallCheckPoint_.position, wallCheckSize_);
        }
    }
}