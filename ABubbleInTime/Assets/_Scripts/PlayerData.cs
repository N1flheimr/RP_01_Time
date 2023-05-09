using System.Collections.Generic;
using UnityEngine;

namespace NifuDev
{
    [CreateAssetMenu(fileName = "newPlayerData", menuName = "PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [Header("Run Parameters")]
        public float moveSpeed_;
        public float acceleration_;
        public float deccelaration_;

        public float velPower_;
        public float frictionAmount;

        [Range(0, 1f)]
        public float accelInAir;
        [Range(0, 1f)]
        public float deccelInAir;

        [Space(5)]
        public float jumpForce_;
        public float fallClamp_;

        [Header("Assist")]
        [Range(0, 0.5f)]
        public float coyoteTime;
        [Range(0, 1.5f)]
        public float jumpInputBufferTime;
        [Range(0, 0.5f)]
        public float jumpApexTimeThreshold;
        [Space(5)]

        //JUMP APEX
        public float jumpApexMaxSpeedMult;
        public float jumpApexAccelerationMult;

        [Header("Gravity Multiplier")]
        public float gravityScale;
        public float fallGravityMult;
        public float jumpCutGravityMult;
        [Range(0, 1f)]
        public float jumpApexGravityMult;
    }
}

