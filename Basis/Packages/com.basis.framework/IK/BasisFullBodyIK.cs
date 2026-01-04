using System.Runtime.CompilerServices;
namespace UnityEngine.Animations.Rigging
{
    /// <summary>
    /// Full-body pass: Head + Legs + Hips + Dual Driven TR + Dual TwoBoneIK Hands (with chest/hand capsule & elbow protection).
    /// All driven via a single job.
    /// </summary>
    [System.Serializable]
    public struct BasisFullBodyData : IAnimationJobData, IBasisFullBodyData
    {
        public const int Count = 22;

        // Live target positions (Vector3) pushed every frame from the manager.
        [SyncSceneToStream, SerializeField]
        public Vector3
            TargetPosition0, TargetPosition1, TargetPosition2, TargetPosition3, TargetPosition4,
            TargetPosition5, TargetPosition6, TargetPosition7, TargetPosition8, TargetPosition9,
            TargetPosition10, TargetPosition11, TargetPosition12, TargetPosition13, TargetPosition14,
            TargetPosition15, TargetPosition16, TargetPosition17, TargetPosition18, TargetPosition19,
            TargetPosition20, TargetPosition54;

        // Live target rotations (Quaternion) — stored as Quaternion on the component; bound as Vector4 by the job.
        [SyncSceneToStream, SerializeField]
        public Quaternion
            TargetRotation0, TargetRotation1, TargetRotation2, TargetRotation3, TargetRotation4,
            TargetRotation5, TargetRotation6, TargetRotation7, TargetRotation8, TargetRotation9,
            TargetRotation10, TargetRotation11, TargetRotation12, TargetRotation13, TargetRotation14,
            TargetRotation15, TargetRotation16, TargetRotation17, TargetRotation18, TargetRotation19,
            TargetRotation20, TargetRotation54;

        // Calibration offsets (applied on top of target each frame) — final = target * offset
        [SyncSceneToStream, SerializeField]
        public Quaternion
            OffsetRotation0, OffsetRotation1, OffsetRotation2, OffsetRotation3, OffsetRotation4,
            OffsetRotation5, OffsetRotation6, OffsetRotation7, OffsetRotation8, OffsetRotation9,
            OffsetRotation10, OffsetRotation11, OffsetRotation12, OffsetRotation13, OffsetRotation14,
            OffsetRotation15, OffsetRotation16, OffsetRotation17, OffsetRotation18, OffsetRotation19,
            OffsetRotation20, OffsetRotation54;

        // Per-slot enable/weights (0..1). Allows toggling bones independently within a single job.
        [SyncSceneToStream, SerializeField]
        public bool
            Weight0, Weight1, Weight2, Weight3, Weight4,
            Weight5, Weight6, Weight7, Weight8, Weight9,
            Weight10, Weight11, Weight12, Weight13, Weight14,
            Weight15, Weight16, Weight17, Weight18, Weight19,
            Weight20, Weight54;

        // Property name helpers for binding
        public string GetTargetPositionVector3Property(int index) => index switch
        {
            0 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition0)),
            1 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition1)),
            2 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition2)),
            3 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition3)),
            4 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition4)),
            5 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition5)),
            6 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition6)),
            7 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition7)),
            8 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition8)),
            9 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition9)),
            10 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition10)),
            11 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition11)),
            12 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition12)),
            13 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition13)),
            14 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition14)),
            15 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition15)),
            16 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition16)),
            17 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition17)),
            18 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition18)),
            19 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition19)),
            20 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition20)),
            54 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetPosition54)),
            _ => string.Empty
        };

        public string GetTargetRotationVector4Property(int index) => index switch
        {
            0 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation0)),
            1 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation1)),
            2 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation2)),
            3 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation3)),
            4 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation4)),
            5 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation5)),
            6 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation6)),
            7 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation7)),
            8 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation8)),
            9 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation9)),
            10 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation10)),
            11 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation11)),
            12 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation12)),
            13 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation13)),
            14 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation14)),
            15 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation15)),
            16 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation16)),
            17 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation17)),
            18 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation18)),
            19 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation19)),
            20 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation20)),
            54 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(TargetRotation54)),
            _ => string.Empty
        };

        public string GetOffsetRotationVector4Property(int index) => index switch
        {
            0 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation0)),
            1 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation1)),
            2 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation2)),
            3 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation3)),
            4 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation4)),
            5 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation5)),
            6 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation6)),
            7 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation7)),
            8 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation8)),
            9 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation9)),
            10 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation10)),
            11 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation11)),
            12 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation12)),
            13 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation13)),
            14 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation14)),
            15 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation15)),
            16 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation16)),
            17 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation17)),
            18 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation18)),
            19 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation19)),
            20 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation20)),
            54 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotation54)),
            _ => string.Empty
        };

        public string GetWeightFloatProperty(int index) => index switch
        {
            0 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight0)),
            1 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight1)),
            2 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight2)),
            3 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight3)),
            4 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight4)),
            5 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight5)),
            6 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight6)),
            7 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight7)),
            8 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight8)),
            9 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight9)),
            10 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight10)),
            11 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight11)),
            12 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight12)),
            13 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight13)),
            14 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight14)),
            15 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight15)),
            16 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight16)),
            17 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight17)),
            18 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight18)),
            19 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight19)),
            20 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight20)),
            54 => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(Weight54)),
            _ => string.Empty
        };
        [SerializeField] Transform m_Hips;
        [SyncSceneToStream, SerializeField] Transform m_chest;
        [SyncSceneToStream, SerializeField] Transform m_neck;
        [SerializeField] Transform m_head;

        [SerializeField] Transform m_LeftUpperLeg;
        [SerializeField] Transform m_LeftLowerLeg;
        [SerializeField] Transform m_leftFoot;
        [SerializeField] Transform m_RightUpperLeg;
        [SerializeField] Transform m_RightLowerLeg;
        [SerializeField] Transform m_RightFoot;

        [SerializeField] Transform m_LeftToe;
        [SerializeField] Transform m_RightToe;

        [SerializeField] Transform m_leftUpperArm;
        [SerializeField] Transform m_leftLowerArm;
        [SerializeField] Transform m_leftHand;

        [SerializeField] Transform m_RightUpperArm;
        [SerializeField] Transform m_RightLowerArm;
        [SerializeField] Transform m_rightHand;

        [SerializeField] Transform m_Spine;
        [SerializeField] Transform m_UpperChest;
        [SerializeField] Transform m_LeftShoulder;
        [SerializeField] Transform m_RightShoulder;

        // Head
        [SyncSceneToStream, SerializeField] public Vector3 PositionHead;
        [SyncSceneToStream, SerializeField] public Quaternion RotationHead;
        [SyncSceneToStream, SerializeField] public Vector3 HintPositionHead;
        [SyncSceneToStream, SerializeField] public Quaternion HintRotationHead;
        [SyncSceneToStream, SerializeField] public Quaternion m_CalibratedRotationHead;

        [SyncSceneToStream, SerializeField] public Quaternion m_CalibratedRotationRightToe;
        [SyncSceneToStream, SerializeField] public Quaternion m_CalibratedRotationLeftToe;
        [SyncSceneToStream, SerializeField] public Quaternion m_CalibratedRotationChest;

        [SyncSceneToStream, SerializeField] public Quaternion m_TargetRotationLeftShoulder;
        [SyncSceneToStream, SerializeField] public Quaternion m_TargetRotationRightShoulder;
        [SyncSceneToStream, SerializeField] public Quaternion m_CalibratedRotationNeck;

        // Hips
        [SyncSceneToStream, SerializeField] public Vector3 PositionHips;
        [SyncSceneToStream, SerializeField] public Quaternion RotationEulerHips;
        [SyncSceneToStream, SerializeField] public Quaternion OffsetRotationHips;

        // Left Leg
        [SyncSceneToStream, SerializeField] public Vector3 LeftFootPosition;
        [SyncSceneToStream, SerializeField] public Quaternion LeftFootRotation;
        [SyncSceneToStream, SerializeField] public Vector3 HintPositionLeftLowerLeg;
        [SyncSceneToStream, SerializeField] public Quaternion HintRotationLeftLowerLeg;
        [SyncSceneToStream, SerializeField] public Quaternion m_CalibratedRotationLeftFoot;

        // Right Leg
        [SyncSceneToStream, SerializeField] public Vector3 RightFootPosition;
        [SyncSceneToStream, SerializeField] public Quaternion RightFootRotation;
        [SyncSceneToStream, SerializeField] public Vector3 HintPositionRightFoot;
        [SyncSceneToStream, SerializeField] public Quaternion HintRotationRightFoot;
        [SyncSceneToStream, SerializeField] public Quaternion m_CalibratedRotationRightFoot;

        // Toes
        [SyncSceneToStream, SerializeField] public Vector3 OutGoingLeftToePosition;
        [SyncSceneToStream, SerializeField] public Quaternion OutGoingLeftToeRotation;
        [SyncSceneToStream, SerializeField] public Vector3 OutGoingRightToePosition;
        [SyncSceneToStream, SerializeField] public Quaternion OutGoingRightToeRotation;

        // Left Hand
        [SyncSceneToStream, SerializeField] public Vector3 PositionLeftHand;
        [SyncSceneToStream, SerializeField] public Quaternion RotationLeftHand;
        [SyncSceneToStream, SerializeField] public Vector3 HintPositionLeftHand;
        [SyncSceneToStream, SerializeField] public Quaternion HintRotationLeftHand;
        [SyncSceneToStream, SerializeField] public Quaternion m_CalibratedRotationLeftHand;
        [SyncSceneToStream, SerializeField] public Quaternion m_CalibratedRotationLeftHandHint;

        // Right Hand
        [SyncSceneToStream, SerializeField] public Vector3 PositionRightHand;
        [SyncSceneToStream, SerializeField] public Quaternion RotationRightHand;
        [SyncSceneToStream, SerializeField] public Vector3 HintPositionRightHand;
        [SyncSceneToStream, SerializeField] public Quaternion HintRotationRightHand;
        [SyncSceneToStream, SerializeField] public Quaternion m_CalibratedRotationRightHand;

        // Misc
        [SyncSceneToStream, SerializeField] public Vector3 m_HintDirection;
        [SyncSceneToStream, SerializeField] public float m_HandSkin;
        [SyncSceneToStream, SerializeField] public bool m_UseHandCapsule;
        [SyncSceneToStream, SerializeField, Min(0f)] public float m_HandRadius;
        [SyncSceneToStream, SerializeField, Min(0f)] public float m_ChestRadius;
        [SyncSceneToStream, SerializeField, Min(0f)] public float m_CollisionSkin;
        [SyncSceneToStream, SerializeField] bool m_CollisionsEnabled;
        [SyncSceneToStream, SerializeField] bool m_ProtectElbow;

        [SyncSceneToStream, SerializeField] bool m_HintHeadEnabled;
        [SyncSceneToStream, SerializeField] bool m_SpineIKEnabled;

        [SyncSceneToStream, SerializeField] public bool m_LeftToeEnabled;
        [SyncSceneToStream, SerializeField] public bool m_RightToeEnabled;

        [SyncSceneToStream, SerializeField] bool m_LeftLowerLegEnabled;
        [SyncSceneToStream, SerializeField] bool m_RightLowerLegEnabled;

        [SyncSceneToStream, SerializeField] bool m_HintLeftLowerLegEnabled;
        [SyncSceneToStream, SerializeField] bool m_HintRightLowerLegEnabled;

        [SyncSceneToStream, SerializeField] bool m_EnabledLeftHand;
        [SyncSceneToStream, SerializeField] bool m_EnabledRightHand;

        [SyncSceneToStream, SerializeField] bool m_HintRightHandEnabled;
        [SyncSceneToStream, SerializeField] bool m_HintLeftHandEnabled;

        [SyncSceneToStream, SerializeField] float m_MinHeadSpineHeight;
        [SyncSceneToStream, SerializeField] public bool m_enabledLeftShoulder;
        [SyncSceneToStream, SerializeField] public bool m_enabledRightShoulder;
        [SyncSceneToStream, SerializeField] public Quaternion m_CalibratedRotationRightShoulder;
        [SyncSceneToStream, SerializeField] public Quaternion m_CalibratedRotationLeftShoulder;

        [SyncSceneToStream, SerializeField] public float m_MaxBendDeg;
        [SyncSceneToStream, SerializeField] public float m_StruggleStart;
        [SyncSceneToStream, SerializeField] public float m_StruggleEnd;
        [SyncSceneToStream, SerializeField] public float m_MaxChestDeltaDeg;
        [SyncSceneToStream, SerializeField] public Vector3 PrevBendNormalLeftLeg;
        [SyncSceneToStream, SerializeField] public Vector3 PrevBendNormalRightLeg;
        [SyncSceneToStream, SerializeField] public Vector3 PrevBendNormalLeftArm;
        [SyncSceneToStream, SerializeField] public Vector3 PrevBendNormalRightArm;
        public Transform chest { get => m_chest; set => m_chest = value; }
        public Transform neck { get => m_neck; set => m_neck = value; }
        public Transform head { get => m_head; set => m_head = value; }
        public Transform LeftUpperLeg { get => m_LeftUpperLeg; set => m_LeftUpperLeg = value; }
        public Transform LeftLowerLeg { get => m_LeftLowerLeg; set => m_LeftLowerLeg = value; }
        public Transform leftFoot { get => m_leftFoot; set => m_leftFoot = value; }
        public Transform RightUpperLeg { get => m_RightUpperLeg; set => m_RightUpperLeg = value; }
        public Transform RightLowerLeg { get => m_RightLowerLeg; set => m_RightLowerLeg = value; }
        public Transform RightFoot { get => m_RightFoot; set => m_RightFoot = value; }
        public Transform hips { get => m_Hips; set => m_Hips = value; }
        public Transform LeftToe { get => m_LeftToe; set => m_LeftToe = value; }
        public Transform RightToe { get => m_RightToe; set => m_RightToe = value; }
        public Transform leftUpperArm { get => m_leftUpperArm; set => m_leftUpperArm = value; }
        public Transform leftLowerArm { get => m_leftLowerArm; set => m_leftLowerArm = value; }
        public Transform LeftHand { get => m_leftHand; set => m_leftHand = value; }
        public Transform RightUpperArm { get => m_RightUpperArm; set => m_RightUpperArm = value; }
        public Transform RightLowerArm { get => m_RightLowerArm; set => m_RightLowerArm = value; }
        public Transform RightHand { get => m_rightHand; set => m_rightHand = value; }

        public Transform spine { get => m_Spine; set => m_Spine = value; }
        public Transform upperChest { get => m_UpperChest; set => m_UpperChest = value; }
        public Transform LeftShoulder { get => m_LeftShoulder; set => m_LeftShoulder = value; }
        public Transform RightShoulder { get => m_RightShoulder; set => m_RightShoulder = value; }
        public string EnabledPropertySpineIK => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_SpineIKEnabled));
        public string HintWeightBoolPropertyHead => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_HintHeadEnabled));
        public string TargetPositionPropertyHead => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(PositionHead));
        public string TargetRotationPropertyHead => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(RotationHead));
        public string HintPositionPropertyHead => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(HintPositionHead));
        public string HintRotationPropertyHead => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(HintRotationHead));
        public string bendNormalHeadProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_HintDirection));
        public string EnabledPropertyLeftLowerLeg => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_LeftLowerLegEnabled));
        public string HintWeightBoolPropertyLeftLowerLeg => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_HintLeftLowerLegEnabled));
        public string TargetPositionPropertyLeftLowerLeg => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(LeftFootPosition));
        public string TargetRotationPropertyLeftLowerLeg => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(LeftFootRotation));
        public string HintPositionPropertyLeftLowerLeg => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(HintPositionLeftLowerLeg));
        public string HintRotationPropertyLeftLowerLeg => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(HintRotationLeftLowerLeg));
        public string EnabledPropertyRightLowerLeg => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_RightLowerLegEnabled));
        public string HintWeightBoolPropertyRightLowerLeg => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_HintRightLowerLegEnabled));
        public string TargetPositionPropertyRightLowerLeg => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(RightFootPosition));
        public string TargetRotationPropertyRightLowerLeg => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(RightFootRotation));
        public string HintPositionPropertyRightLowerLeg => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(HintPositionRightFoot));
        public string HintRotationPropertyRightLowerLeg => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(HintRotationRightFoot));
        public string TargetPositionPropertyHips => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(PositionHips));
        public string TargetRotationPropertyHips => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(RotationEulerHips));
        public string OffsetRotationPropertyHips => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OffsetRotationHips));
        public string LeftToeEnabledProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_LeftToeEnabled));
        public string RightToeEnabledProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_RightToeEnabled));
        public string LeftDrivenTargetPosProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OutGoingLeftToePosition));
        public string LeftDrivenTargetRotProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OutGoingLeftToeRotation));
        public string RightDrivenTargetPosProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OutGoingRightToePosition));
        public string RightDrivenTargetRotProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(OutGoingRightToeRotation));
        public string HintWeightBoolPropertyLeftHand => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_HintLeftHandEnabled));
        public string TargetPositionPropertyLeftHand => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(PositionLeftHand));
        public string TargetRotationPropertyLeftHand => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(RotationLeftHand));
        public string HintPositionPropertyLeftHand => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(HintPositionLeftHand));
        public string HintRotationPropertyLeftHand => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(HintRotationLeftHand));
        public string EnabledPropertyRightHand => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_EnabledRightHand));
        public string EnabledPropertyLeftHand => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_EnabledLeftHand));
        public string HintWeightBoolPropertyRightHand => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_HintRightHandEnabled));
        public string TargetPositionPropertyRightHand => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(PositionRightHand));
        public string TargetRotationPropertyRightHand => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(RotationRightHand));
        public string HintPositionPropertyRightHand => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(HintPositionRightHand));
        public string HintRotationPropertyRightHand => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(HintRotationRightHand));
        public string ChestRadiusFloatProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_ChestRadius));
        public string CollisionSkinFloatProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_CollisionSkin));
        public string CollisionsEnabledBoolProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_CollisionsEnabled));
        public string HandRadiusFloatProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_HandRadius));
        public string HandSkinFloatProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_HandSkin));
        public string UseHandCapsuleBoolProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_UseHandCapsule));
        public string ProtectElbowBoolProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_ProtectElbow));

        public string enabledLeftShoulderProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_enabledLeftShoulder));
        public string enabledRightShoulderProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_enabledRightShoulder));
        public string MinHeadSpineHeightFloatProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_MinHeadSpineHeight));

        public string TargetRotationLeftShoulderProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_TargetRotationLeftShoulder));
        public string TargetRotationRightShoulderProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_TargetRotationRightShoulder));

        public string MaxBendDegFloatProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_MaxBendDeg));
        public string MaxChestDeltaDegFloatProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_MaxChestDeltaDeg));

        public string PrevBendNormalLeftLegProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(PrevBendNormalLeftLeg));
        public string PrevBendNormalRightLegProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(PrevBendNormalRightLeg));
        public string PrevBendNormalLeftArmProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(PrevBendNormalLeftArm));
        public string PrevBendNormalRightArmProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(PrevBendNormalRightArm));
        public bool hintWeightHead { get => m_HintHeadEnabled; set => m_HintHeadEnabled = value; }
        public bool EnabledSpineIK { get => m_SpineIKEnabled; set => m_SpineIKEnabled = value; }
        public bool HintWeightLeftLowerLeg { get => m_HintLeftLowerLegEnabled; set => m_HintLeftLowerLegEnabled = value; }
        public bool EnableLeftLeg { get => m_LeftLowerLegEnabled; set => m_LeftLowerLegEnabled = value; }
        public bool HintWeightRightLowerLeg { get => m_HintRightLowerLegEnabled; set => m_HintRightLowerLegEnabled = value; }
        public bool EnableRightLeg { get => m_RightLowerLegEnabled; set => m_RightLowerLegEnabled = value; }
        public bool LeftToeEnabled { get => m_LeftToeEnabled; set => m_LeftToeEnabled = value; }
        public bool RightToeEnabled { get => m_RightToeEnabled; set => m_RightToeEnabled = value; }
        public bool hintWeightLeftHand { get => m_HintLeftHandEnabled; set => m_HintLeftHandEnabled = value; }
        public bool enabledLeftHand { get => m_EnabledLeftHand; set => m_EnabledLeftHand = value; }

        public bool enabledRightHand { get => m_EnabledRightHand; set => m_EnabledRightHand = value; }
        public bool protectElbow { get => m_ProtectElbow; set => m_ProtectElbow = value; }
        public bool hintWeightRightHand { get => m_HintRightHandEnabled; set => m_HintRightHandEnabled = value; }
        public float handRadius { get => m_HandRadius; set => m_HandRadius = value; }
        public float handSkin { get => m_HandSkin; set => m_HandSkin = value; }
        public bool useHandCapsule { get => m_UseHandCapsule; set => m_UseHandCapsule = value; }
        public float chestRadius { get => m_ChestRadius; set => m_ChestRadius = value; }
        public float collisionSkin { get => m_CollisionSkin; set => m_CollisionSkin = value; }
        public bool collisionsEnabled { get => m_CollisionsEnabled; set => m_CollisionsEnabled = value; }
        public bool EnabledRightShoulder { get => m_enabledRightShoulder; set => m_enabledRightShoulder = value; }
        public bool EnabledLeftShoulder { get => m_enabledLeftShoulder; set => m_enabledLeftShoulder = value; }

        public float SpineBendmaxBendDeg { get => m_MaxBendDeg; set => m_MaxBendDeg = value; }
        public float ClampedmaxChestDelta { get => m_MaxChestDeltaDeg; set => m_MaxChestDeltaDeg = value; }
        public float minHeadSpineHeight
        {
            get => m_MinHeadSpineHeight;
            set => m_MinHeadSpineHeight = value;
        }

        // ---------- Validation ----------
        bool IAnimationJobData.IsValid()
        {
            bool hipsValid = m_Hips != null;

            bool head = (m_head && m_neck && m_chest &&
                         m_head.IsChildOf(m_neck) && m_neck.IsChildOf(m_chest));

            bool lLeg = (m_leftFoot && m_LeftLowerLeg && m_LeftUpperLeg &&
                         m_leftFoot.IsChildOf(m_LeftLowerLeg) && m_LeftLowerLeg.IsChildOf(m_LeftUpperLeg));

            bool rLeg = (m_RightFoot && m_RightLowerLeg && m_RightUpperLeg &&
                         m_RightFoot.IsChildOf(m_RightLowerLeg) && m_RightLowerLeg.IsChildOf(m_RightUpperLeg));

            bool lHand = (m_leftHand && m_leftLowerArm && m_leftUpperArm &&
                          m_leftHand.IsChildOf(m_leftLowerArm) && m_leftLowerArm.IsChildOf(m_leftUpperArm));

            bool rHand = (m_rightHand && m_RightLowerArm && m_RightUpperArm &&
                          m_rightHand.IsChildOf(m_RightLowerArm) && m_RightLowerArm.IsChildOf(m_RightUpperArm));

            // Any of these being valid is enough to run.
            return head || lLeg || rLeg || lHand || rHand || hipsValid || (m_LeftToe != null) || (m_RightToe != null);
        }

        void IAnimationJobData.SetDefaultValues()
        {
            m_chest = m_neck = m_head = null;
            m_LeftUpperLeg = m_LeftLowerLeg = m_leftFoot = null;
            m_RightUpperLeg = m_RightLowerLeg = m_RightFoot = null;

            m_leftUpperArm = m_leftLowerArm = m_leftHand = null;
            m_RightUpperArm = m_RightLowerArm = m_rightHand = null;

            m_Hips = null;

            m_HintHeadEnabled = m_HintLeftLowerLegEnabled = m_HintRightLowerLegEnabled = true;
            m_SpineIKEnabled = m_LeftLowerLegEnabled = m_RightLowerLegEnabled = true;

            m_HintLeftHandEnabled = m_HintRightHandEnabled = true;
            m_EnabledLeftHand = m_EnabledRightHand = true;
            m_CalibratedRotationHead = m_CalibratedRotationLeftFoot = m_CalibratedRotationRightFoot = Quaternion.identity;
            m_CalibratedRotationLeftHand = m_CalibratedRotationRightHand = Quaternion.identity;

            m_HintDirection = Vector3.up;

            PositionHips = Vector3.zero;
            RotationEulerHips = Quaternion.identity;
            OffsetRotationHips = Quaternion.identity;

            // Integrated driven TR defaults
            m_LeftToe = null;
            m_RightToe = null;

            OutGoingLeftToePosition = OutGoingRightToePosition = Vector3.zero;
            OutGoingLeftToeRotation = OutGoingRightToeRotation = Quaternion.identity;
            m_LeftToeEnabled = false;
            m_RightToeEnabled = false;

            // Chest/hand capsule defaults (left)
            m_chest = m_neck = null;
            m_ChestRadius = 0.18f; m_CollisionSkin = 0.02f; m_CollisionsEnabled = true;
            m_HandRadius = 0.05f; m_HandSkin = 0.01f; m_UseHandCapsule = true;
            m_ProtectElbow = true;

            // Positions
            TargetPosition0 = TargetPosition1 = TargetPosition2 = TargetPosition3 = TargetPosition4 =
            TargetPosition5 = TargetPosition6 = TargetPosition7 = TargetPosition8 = TargetPosition9 =
            TargetPosition10 = TargetPosition11 = TargetPosition12 = TargetPosition13 = TargetPosition14 =
            TargetPosition15 = TargetPosition16 = TargetPosition17 = TargetPosition18 = TargetPosition19 =
            TargetPosition20 = TargetPosition54 = Vector3.zero;

            // Rotations
            TargetRotation0 = TargetRotation1 = TargetRotation2 = TargetRotation3 = TargetRotation4 =
            TargetRotation5 = TargetRotation6 = TargetRotation7 = TargetRotation8 = TargetRotation9 =
            TargetRotation10 = TargetRotation11 = TargetRotation12 = TargetRotation13 = TargetRotation14 =
            TargetRotation15 = TargetRotation16 = TargetRotation17 = TargetRotation18 = TargetRotation19 =
            TargetRotation20 = TargetRotation54 = Quaternion.identity;

            // Offsets
            OffsetRotation0 = OffsetRotation1 = OffsetRotation2 = OffsetRotation3 = OffsetRotation4 =
            OffsetRotation5 = OffsetRotation6 = OffsetRotation7 = OffsetRotation8 = OffsetRotation9 =
            OffsetRotation10 = OffsetRotation11 = OffsetRotation12 = OffsetRotation13 = OffsetRotation14 =
            OffsetRotation15 = OffsetRotation16 = OffsetRotation17 = OffsetRotation18 = OffsetRotation19 =
            OffsetRotation20 = OffsetRotation54 = Quaternion.identity;

            // Weights default to disabled
            Weight0 = Weight1 = Weight2 = Weight3 = Weight4 =
            Weight5 = Weight6 = Weight7 = Weight8 = Weight9 =
            Weight10 = Weight11 = Weight12 = Weight13 = Weight14 =
            Weight15 = Weight16 = Weight17 = Weight18 = Weight19 =
            Weight20 = Weight54 = false;

            PrevBendNormalLeftLeg = -Vector3.right;
            PrevBendNormalRightLeg = Vector3.right;
            PrevBendNormalLeftArm = Vector3.forward;
            PrevBendNormalRightArm = Vector3.forward;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTargetPosition(int idx, in Vector3 v)
        {
            switch (idx)
            {
                case 0: TargetPosition0 = v; break;
                case 1: TargetPosition1 = v; break;
                case 2: TargetPosition2 = v; break;
                case 3: TargetPosition3 = v; break;
                case 4: TargetPosition4 = v; break;
                case 5: TargetPosition5 = v; break;
                case 6: TargetPosition6 = v; break;
                case 7: TargetPosition7 = v; break;
                case 8: TargetPosition8 = v; break;
                case 9: TargetPosition9 = v; break;
                case 10: TargetPosition10 = v; break;
                case 11: TargetPosition11 = v; break;
                case 12: TargetPosition12 = v; break;
                case 13: TargetPosition13 = v; break;
                case 14: TargetPosition14 = v; break;
                case 15: TargetPosition15 = v; break;
                case 16: TargetPosition16 = v; break;
                case 17: TargetPosition17 = v; break;
                case 18: TargetPosition18 = v; break;
                case 19: TargetPosition19 = v; break;
                case 20: TargetPosition20 = v; break;
                case 54: TargetPosition54 = v; break;
                default:
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTargetRotation(int idx, in Quaternion q)
        {
            switch (idx)
            {
                case 0: TargetRotation0 = q; break;
                case 1: TargetRotation1 = q; break;
                case 2: TargetRotation2 = q; break;
                case 3: TargetRotation3 = q; break;
                case 4: TargetRotation4 = q; break;
                case 5: TargetRotation5 = q; break;
                case 6: TargetRotation6 = q; break;
                case 7: TargetRotation7 = q; break;
                case 8: TargetRotation8 = q; break;
                case 9: TargetRotation9 = q; break;
                case 10: TargetRotation10 = q; break;
                case 11: TargetRotation11 = q; break;
                case 12: TargetRotation12 = q; break;
                case 13: TargetRotation13 = q; break;
                case 14: TargetRotation14 = q; break;
                case 15: TargetRotation15 = q; break;
                case 16: TargetRotation16 = q; break;
                case 17: TargetRotation17 = q; break;
                case 18: TargetRotation18 = q; break;
                case 19: TargetRotation19 = q; break;
                case 20: TargetRotation20 = q; break;
                case 54: TargetRotation54 = q; break;
                default:
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetOffsetRotation(int idx, in Quaternion q)
        {
            switch (idx)
            {
                case 0: OffsetRotation0 = q; break;
                case 1: OffsetRotation1 = q; break;
                case 2: OffsetRotation2 = q; break;
                case 3: OffsetRotation3 = q; break;
                case 4: OffsetRotation4 = q; break;
                case 5: OffsetRotation5 = q; break;
                case 6: OffsetRotation6 = q; break;
                case 7: OffsetRotation7 = q; break;
                case 8: OffsetRotation8 = q; break;
                case 9: OffsetRotation9 = q; break;
                case 10: OffsetRotation10 = q; break;
                case 11: OffsetRotation11 = q; break;
                case 12: OffsetRotation12 = q; break;
                case 13: OffsetRotation13 = q; break;
                case 14: OffsetRotation14 = q; break;
                case 15: OffsetRotation15 = q; break;
                case 16: OffsetRotation16 = q; break;
                case 17: OffsetRotation17 = q; break;
                case 18: OffsetRotation18 = q; break;
                case 19: OffsetRotation19 = q; break;
                case 20: OffsetRotation20 = q; break;
                case 54: OffsetRotation54 = q; break;
                default:
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetWeight(int idx, bool State)
        {
            switch (idx)
            {
                case 0: Weight0 = State; break;
                case 1: Weight1 = State; break;
                case 2: Weight2 = State; break;
                case 3: Weight3 = State; break;
                case 4: Weight4 = State; break;
                case 5: Weight5 = State; break;
                case 6: Weight6 = State; break;
                case 7: Weight7 = State; break;
                case 8: Weight8 = State; break;
                case 9: Weight9 = State; break;
                case 10: Weight10 = State; break;
                case 11: Weight11 = State; break;
                case 12: Weight12 = State; break;
                case 13: Weight13 = State; break;
                case 14: Weight14 = State; break;
                case 15: Weight15 = State; break;
                case 16: Weight16 = State; break;
                case 17: Weight17 = State; break;
                case 18: Weight18 = State; break;
                case 19: Weight19 = State; break;
                case 20: Weight20 = State; break;
                case 54: Weight54 = State; break;
                default:
                    break;
            }
        }
    }
    public interface IBasisFullBodyData
    {
        string GetTargetPositionVector3Property(int index);
        string GetTargetRotationVector4Property(int index);
        string GetOffsetRotationVector4Property(int index);
        string GetWeightFloatProperty(int index);
    }
    public class BasisFullBodyIK : RigConstraint<BasisFullIKConstraintJob, BasisFullBodyData, BasisFullBodyJobBinder>
    {
        protected override void OnValidate()
        {
            base.OnValidate();
            // force serialize dirty for animated bools
            m_Data.hintWeightHead = m_Data.hintWeightHead;
            m_Data.HintWeightLeftLowerLeg = m_Data.HintWeightLeftLowerLeg;
            m_Data.HintWeightRightLowerLeg = m_Data.HintWeightRightLowerLeg;
            m_Data.EnabledSpineIK = m_Data.EnabledSpineIK;

            // new toggles
            m_Data.LeftToeEnabled = m_Data.LeftToeEnabled;
            m_Data.RightToeEnabled = m_Data.RightToeEnabled;

            // hands toggles
            m_Data.hintWeightLeftHand = m_Data.hintWeightLeftHand;
            m_Data.hintWeightRightHand = m_Data.hintWeightRightHand;
            m_Data.enabledLeftHand = m_Data.enabledLeftHand;
            m_Data.enabledRightHand = m_Data.enabledRightHand;
            m_Data.protectElbow = m_Data.protectElbow;
        }
    }

#if !UNITY_EDITOR
[Unity.Burst.BurstCompile]
#endif
    public struct BasisFullIKConstraintJob : IWeightedAnimationJob
    {
        const float k_SqrEpsilon = 1e-8f;
        const float k_MaxForwardDeg = 120f;
        const float k_MaxBackwardDeg = 25;

        const float k_SpineMaxForwardDeg = 140f;
        const float k_SpineMaxBackwardDeg = 35f;
        const float k_ArmMaxForwardDeg = 150f;
        const float k_ArmMaxBackwardDeg = 60f;

        const float k_SpineLinkMaxForwardDeg = 120f;
        const float k_SpineLinkMaxBackwardDeg = 25f;

        const float k_ChestLinkMaxForwardDeg = 110f;
        const float k_ChestLinkMaxBackwardDeg = 20f;

        const float k_NeckLinkMaxForwardDeg = 100f;
        const float k_NeckLinkMaxBackwardDeg = 15f;

        const float k_HeadLinkMaxForwardDeg = 120f;
        const float k_HeadLinkMaxBackwardDeg = 20f;

        public ReadWriteTransformHandle HandleChest, HandleNeck, HandleHead,
  HandleLeftUpperLeg, HandleLeftLowerLeg, HandleLeftFoot,
  HandleRightUpperLeg, HandleRightLowerLeg, HandleRightFoot,
  HandleHips, HandleSpine, HandleUpperChest,
            HandleLeftShoulder, HandleRightShoulder,

  HandleLeftToe, HandleRightToe,
  HandleLeftUpperArm, HandleLeftLowerArm, HandleLeftHand,
  HandleRightUpperArm, HandleRightLowerArm, HandleRightHand;

        public Vector3Property targetPositionHead, hintPositionHead, bendNormalHead,
targetPositionLeftLowerLeg, hintPositionLeftLowerLeg,
targetPositionRightLowerLeg, hintPositionRightLowerLeg,
targetPositionHips,
leftDrivenTargetPos, rightDrivenTargetPos,
targetPositionLeftHand, hintPositionLeftHand,
targetPositionRightHand, hintPositionRightHand,
p0, p1, p2, p3, p4, p5, p6, p7, p8, p9,
p10, p11, p12, p13, p14, p15, p16, p17, p18, p19,
p20, p54;

        public Vector4Property targetRotationHead, hintRotationHead,
targetRotationLeftLowerLeg, hintRotationLeftLowerLeg,
targetRotationRightLowerLeg, hintRotationRightLowerLeg,
targetRotationHips, offsetRotationHips,
leftDrivenTargetRot, rightDrivenTargetRot,
targetRotationLeftHand, hintRotationLeftHand,
targetRotationRightHand, hintRotationRightHand,
TargetRotationLeftShoulder, TargetRotationRightShoulder,
r0, r1, r2, r3, r4, r5, r6, r7, r8, r9,
r10, r11, r12, r13, r14, r15, r16, r17, r18, r19,
r20, r54,
o0, o1, o2, o3, o4, o5, o6, o7, o8, o9,
o10, o11, o12, o13, o14, o15, o16, o17, o18, o19,
o20, o54;

        public Quaternion targetOffsetNeck, targetOffsetHead, targetOffsetChest, targetOffsetLeftToe,
            targetOffsetRightToe, targetOffsetLeftShoulder, targetOffsetRightShoulder, targetOffsetLeftFoot,
            targetOffsetRightFoot, targetOffsetLeftHand, targetOffsetRightHand;

        public BoolProperty
hintWeightHead, enabledSpineIK,
hintWeightLeftLowerLeg, enabledLeftLowerLeg,
hintWeightRightLowerLeg, enabledRightLowerLeg,
            enabledLeftShoulder, enabledRightShoulder,

leftToeEnabled, RightToeEnabled,
hintWeightLeftHand, enabledLeftHand,
hintWeightRightHand, enabledRightHand,
useHandCapsule, protectElbow,
collisionsEnabled,
w0, w1, w2, w3, w4, w5, w6, w7, w8, w9,
w10, w11, w12, w13, w14, w15, w16, w17, w18, w19,
w20, w54;

        public FloatProperty handRadius, handSkin, chestRadius, collisionSkin, MinHeadSpineHeight, maxBendDeg, MaxChestDeltaDeg;

        const float k_Epsilon = 1e-5f; // or 0.00001f
        const float k_MinMag = 1e-6f;
        public FloatProperty jobWeight { get; set; }
        public Vector3Property prevBendNormalRightLeg;
        public Vector3Property prevBendNormalLeftArm;
        public Vector3Property prevBendNormalRightArm;

        public Vector3Property prevBendNormalLeftLeg;
        public float horizontal01;
        public void ProcessRootMotion(AnimationStream stream) { }
        public void ProcessAnimation(AnimationStream stream)
        {
            float w = jobWeight.Get(stream);
            if (w <= 0f)
            {
                Pass(stream, HandleHips, HandleLeftToe, HandleRightToe);
                Pass(stream, HandleChest, HandleNeck, HandleHead);
                Pass(stream, HandleLeftUpperLeg, HandleLeftLowerLeg, HandleLeftFoot);
                Pass(stream, HandleRightUpperLeg, HandleRightLowerLeg, HandleRightFoot);
                Pass(stream, HandleLeftUpperArm, HandleLeftLowerArm, HandleLeftHand);
                Pass(stream, HandleRightUpperArm, HandleRightLowerArm, HandleRightHand);
                return;
            }

            Vector3 WorldUP = Vector3.up;

            Vector3 headTargetPos = targetPositionHead.Get(stream);
            Vector3 hipsTargetPos = targetPositionHips.Get(stream);
            float MaxBlend = maxBendDeg.Get(stream);

            if (HandleHips.IsValid(stream))
            {
                Quaternion hipRot = HandleHips.GetRotation(stream);

                Vector3 hipForward = hipRot * Vector3.forward;
                Vector3 hipUp = hipRot * Vector3.up;

                Vector3 toHead = headTargetPos - hipsTargetPos;
                float dist = toHead.magnitude;
                if (dist > 1e-6f)
                {
                    Vector3 dir = toHead / dist;
                    dir = ClampDirectionAsymmetricCone(dir, hipForward, hipUp, k_SpineMaxForwardDeg, k_SpineMaxBackwardDeg);
                    headTargetPos = hipsTargetPos + dir * dist;

                    // write back so downstream spine solve uses clamped target
                    targetPositionHead.Set(stream, headTargetPos);
                }
            }
            // 1) Limit spine bend by pushing hips down if needed
            hipsTargetPos = EnforceSpineBendLimit(headTargetPos, hipsTargetPos, MaxBlend, WorldUP);

            float MinRange = MinHeadSpineHeight.Get(stream);
            float maxDist = MinRange * 1.25f;

            Quaternion hipsRot = HandleHips.IsValid(stream) ? HandleHips.GetRotation(stream) : Quaternion.identity;
            Quaternion chestRot = HandleChest.IsValid(stream) ? HandleChest.GetRotation(stream) : hipsRot;
            Vector3 hipsUp = hipsRot * WorldUP;

            // "lying down" detector
            float targetlyingdown = (Mathf.Abs(Vector3.Dot(hipsUp, WorldUP)) < 0.45f) ? 1f : 0f;
            horizontal01 = Mathf.Lerp(horizontal01, targetlyingdown, 1f - Mathf.Exp(-stream.deltaTime * 10f));
            bool isHorizontal = horizontal01 > 0.5f;
            // If horizontal, don't pull hips toward head (this causes the bad "enforced" feel)
            if (!isHorizontal)
            {
             //   hipsTargetPos = EnforceMaxHeadHipsDistance(headTargetPos, hipsTargetPos, maxDist);
            }
           // hipsTargetPos = EnforceMinHeadHipsDistance(headTargetPos, hipsTargetPos, MinRange, WorldUP);
            // 3) Solve hips + spine as before
            SolveHipsAndSpine(stream, hipsTargetPos, targetRotationHips, offsetRotationHips, enabledSpineIK, HandleHips, HandleChest, HandleNeck, HandleHead, targetPositionHead, targetRotationHead, targetOffsetHead, bendNormalHead);

            if (hintWeightHead.Get(stream))
            {
                if (HandleChest.IsValid(stream))
                {
                    // Neck rotation produced by your spine IK pass – we keep this
                    Quaternion neckRot = HandleNeck.IsValid(stream) ? HandleNeck.GetRotation(stream) : Quaternion.identity;

                    // Spine as an extra reference if available (nice stabiliser)
                    Quaternion spineRot = HandleSpine.IsValid(stream) ? HandleSpine.GetRotation(stream) : neckRot;

                    // Raw chest from tracker
                    Quaternion trackerChestRot = V4ToQuat(hintRotationHead.Get(stream)) * targetOffsetChest;

                    float Value = MaxChestDeltaDeg.Get(stream);
                    // Clamp relative to neck and spine
                    Quaternion clampedChestRot = ClampRotation(trackerChestRot, neckRot, Value);
                    clampedChestRot = ClampRotation(clampedChestRot, spineRot, Value);

                    HandleChest.SetRotation(stream, clampedChestRot);

                    // Build target + hint transforms
                    var tRot = V4ToQuat(targetRotationHead.Get(stream));
                    var target = new AffineTransform(targetPositionHead.Get(stream), tRot);
                    var bendNormal = bendNormalHead.Get(stream);

                    SolveTwoBoneSpine(stream, HandleChest, HandleNeck, HandleHead, target, targetOffsetHead, bendNormal);
                }
            }
            if (enabledSpineIK.Get(stream))
            {
                // Spine relative to hips (if you actually use HandleSpine; if not valid, it just skips)
                ClampSwingAsymmetric(stream, HandleHips, HandleSpine, k_SpineLinkMaxForwardDeg, k_SpineLinkMaxBackwardDeg);

                // Chest relative to spine (fallback: hips if spine not valid)
                if (HandleSpine.IsValid(stream))
                {
                    ClampSwingAsymmetric(stream, HandleSpine, HandleChest, k_ChestLinkMaxForwardDeg, k_ChestLinkMaxBackwardDeg);
                }
                else
                {
                    ClampSwingAsymmetric(stream, HandleHips, HandleChest, k_ChestLinkMaxForwardDeg, k_ChestLinkMaxBackwardDeg);
                }

                // Neck relative to chest
                ClampSwingAsymmetric(stream, HandleChest, HandleNeck, k_NeckLinkMaxForwardDeg, k_NeckLinkMaxBackwardDeg);

                // Head relative to neck
                ClampSwingAsymmetric(stream, HandleNeck, HandleHead, k_HeadLinkMaxForwardDeg, k_HeadLinkMaxBackwardDeg);
            }
            if (enabledLeftShoulder.Get(stream))
            {
                ApplyRotation(stream, HandleLeftShoulder, TargetRotationLeftShoulder, targetOffsetLeftShoulder);
            }
            if (enabledRightShoulder.Get(stream))
            {
                ApplyRotation(stream, HandleRightShoulder, TargetRotationRightShoulder, targetOffsetRightShoulder);
            }

            Vector3 hipsRight = hipsRot * Vector3.right;
            Vector3 chestForward = chestRot * Vector3.forward;

#if UNITY_EDITOR
           // BasisDebug.Log($"isHorizontal {isHorizontal}");
#endif
            // body-based knee normals (left bends ~-right, right bends ~+right)
            Vector3 kneeNormalLeft = -hipsRight;
            Vector3 kneeNormalRight = hipsRight;

            kneeNormalLeft = BlendNormals(kneeNormalLeft, prevBendNormalLeftLeg.Get(stream), isHorizontal ? 0.75f : 0.25f);
            kneeNormalRight = BlendNormals(kneeNormalRight, prevBendNormalRightLeg.Get(stream), isHorizontal ? 0.75f : 0.25f);

            // Disable hint influence when horizontal (prevents tracker-roll flips)
            bool useHintLeft = hintWeightLeftLowerLeg.Get(stream) && !isHorizontal;
            bool useHintRight = hintWeightRightLowerLeg.Get(stream) && !isHorizontal;

            prevBendNormalLeftLeg.Set(stream, kneeNormalLeft);
            prevBendNormalRightLeg.Set(stream, kneeNormalRight);

            SolveLegs(stream, enabledLeftLowerLeg,
                HandleLeftUpperLeg, HandleLeftLowerLeg, HandleLeftFoot,
                targetPositionLeftLowerLeg, targetRotationLeftLowerLeg,
                hintPositionLeftLowerLeg, hintRotationLeftLowerLeg,
                useHintLeft,
                targetOffsetLeftFoot,
                kneeNormalLeft);

            SolveLegs(stream, enabledRightLowerLeg,
                HandleRightUpperLeg, HandleRightLowerLeg, HandleRightFoot,
                targetPositionRightLowerLeg, targetRotationRightLowerLeg,
                hintPositionRightLowerLeg, hintRotationRightLowerLeg,
                useHintRight,
                targetOffsetRightFoot,
                kneeNormalRight);

            Vector3 elbowNormalLeft = SafeNormalize(chestForward - hipsRight * 0.35f, chestForward);
            Vector3 elbowNormalRight = SafeNormalize(chestForward + hipsRight * 0.35f, chestForward);

            Vector3 prevLA = prevBendNormalLeftArm.Get(stream);
            Vector3 prevRA = prevBendNormalRightArm.Get(stream);

            elbowNormalLeft = BlendNormals(elbowNormalLeft, prevLA, isHorizontal ? 0.65f : 0.20f);
            elbowNormalRight = BlendNormals(elbowNormalRight, prevRA, isHorizontal ? 0.65f : 0.20f);

            prevBendNormalLeftArm.Set(stream, elbowNormalLeft);
            prevBendNormalRightArm.Set(stream, elbowNormalRight);

            SolveHand(stream,
                    enabledLeftHand, HandleLeftUpperArm, HandleLeftLowerArm, HandleLeftHand,
                    targetPositionLeftHand, targetRotationLeftHand, hintPositionLeftHand, hintRotationLeftHand,
                    hintWeightLeftHand.Get(stream) && !isHorizontal,
                    targetOffsetLeftHand,
                    HandleChest, HandleNeck, chestRadius, collisionSkin, collisionsEnabled,
                    handRadius, handSkin, useHandCapsule, protectElbow,
                    elbowNormalLeft, WorldUP);

            SolveHand(stream,
                enabledRightHand, HandleRightUpperArm, HandleRightLowerArm, HandleRightHand,
                targetPositionRightHand, targetRotationRightHand, hintPositionRightHand, hintRotationRightHand,
                hintWeightRightHand.Get(stream) && !isHorizontal,
                targetOffsetRightHand,
                HandleChest, HandleNeck, chestRadius, collisionSkin, collisionsEnabled,
                handRadius, handSkin, useHandCapsule, protectElbow,
                elbowNormalRight, WorldUP);


            ApplyRotation(stream, leftToeEnabled, HandleLeftToe, leftDrivenTargetRot, targetOffsetLeftToe);
            ApplyRotation(stream, RightToeEnabled, HandleRightToe, rightDrivenTargetRot, targetOffsetRightToe);

            Apply(stream, HandleHips, p0, r0, o0, w0);
            Apply(stream, HandleLeftUpperLeg, p1, r1, o1, w1);
            Apply(stream, HandleRightUpperLeg, p2, r2, o2, w2);
            Apply(stream, HandleLeftLowerLeg, p3, r3, o3, w3);
            Apply(stream, HandleRightLowerLeg, p4, r4, o4, w4);
            Apply(stream, HandleLeftFoot, p5, r5, o5, w5);
            Apply(stream, HandleRightFoot, p6, r6, o6, w6);
            Apply(stream, HandleSpine, p7, r7, o7, w7);
            Apply(stream, HandleChest, p8, r8, o8, w8);
            Apply(stream, HandleNeck, p9, r9, o9, w9);
            Apply(stream, HandleHead, p10, r10, o10, w10);
            Apply(stream, HandleLeftShoulder, p11, r11, o11, w11);
            Apply(stream, HandleRightShoulder, p12, r12, o12, w12);
            Apply(stream, HandleLeftUpperArm, p13, r13, o13, w13);
            Apply(stream, HandleRightUpperArm, p14, r14, o14, w14);
            Apply(stream, HandleLeftLowerArm, p15, r15, o15, w15);
            Apply(stream, HandleRightLowerArm, p16, r16, o16, w16);
            Apply(stream, HandleLeftHand, p17, r17, o17, w17);
            Apply(stream, HandleRightHand, p18, r18, o18, w18);
            Apply(stream, HandleLeftToe, p19, r19, o19, w19);
            Apply(stream, HandleRightToe, p20, r20, o20, w20);
            Apply(stream, HandleUpperChest, p54, r54, o54, w54);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ClampSwingAsymmetric(AnimationStream stream,
    ReadWriteTransformHandle parent,
    ReadWriteTransformHandle child,
    float maxForwardDeg,
    float maxBackwardDeg)
        {
            if (!parent.IsValid(stream) || !child.IsValid(stream))
                return;

            Quaternion parentRot = parent.GetRotation(stream);
            Quaternion childRot = child.GetRotation(stream);

            // NOTE: assumes bone "forward" axis is +Z. If your rig uses a different axis, change Vector3.forward.
            Vector3 parentFwd = parentRot * Vector3.forward;
            Vector3 parentUp = parentRot * Vector3.up;

            Vector3 childFwd = childRot * Vector3.forward;

            Vector3 clampedFwd = ClampDirectionAsymmetricCone(childFwd, parentFwd, parentUp, maxForwardDeg, maxBackwardDeg);

            // Rotate child so its forward matches clamped forward (minimal correction)
            Quaternion delta = QuaternionExt.FromToRotation(childFwd, clampedFwd);
            child.SetRotation(stream, delta * childRot);
        }
        static Vector3 EnforceSpineBendLimit(Vector3 headPos, Vector3 hipsPos, float maxBendDeg, Vector3 up)
        {
            if (maxBendDeg <= 0f)
            {
                return hipsPos;
            }

            Vector3 diff = hipsPos - headPos;
            float sqrMag = diff.sqrMagnitude;
            if (sqrMag < k_MinMag)
            {
                return hipsPos;
            }
            // Decompose into vertical (along -up, hips below head) and lateral
            float verticalDot = Vector3.Dot(diff, -up); // positive if hips are "below" head
            Vector3 vertical = -up * verticalDot;
            Vector3 lateral = diff - vertical;

            float lateralLen = lateral.magnitude;
            float absVertical = Mathf.Abs(verticalDot);

            if (lateralLen < k_MinMag || absVertical < k_MinMag)
                return hipsPos;

            // Current bend angle from head to hips
            float currentAngle = Mathf.Atan2(lateralLen, absVertical) * Mathf.Rad2Deg;
            if (currentAngle <= maxBendDeg)
            {
                return hipsPos;
            }

            // We want lateral / newVertical = tan(maxBend)
            float maxRatio = Mathf.Tan(maxBendDeg * Mathf.Deg2Rad);
            float newVertical = lateralLen / Mathf.Max(maxRatio, k_MinMag);

            // Push hips further down in the same direction along -up
            float finalVertical = Mathf.Sign(verticalDot) * Mathf.Max(newVertical, absVertical);
            Vector3 newVerticalVec = -up * finalVertical;

            Vector3 newDiff = newVerticalVec + (lateralLen > k_MinMag ? lateral.normalized * lateralLen : Vector3.zero);
            return headPos + newDiff;
        }
        static Quaternion ClampRotation(Quaternion current, Quaternion reference, float maxAngleDeg)
        {
            // Angle between the two orientations
            float angle = Quaternion.Angle(reference, current);
            if (angle <= maxAngleDeg)
                return current;

            // Scale back toward the reference so the final difference is exactly maxAngleDeg
            float t = maxAngleDeg / Mathf.Max(angle, 1e-5f);
            return Quaternion.Slerp(reference, current, t);
        }
        public void ApplyRotation(AnimationStream stream, BoolProperty enabledProp, ReadWriteTransformHandle handle, Vector4Property targetRotProp, Quaternion RotationOffset)
        {
            if (!handle.IsValid(stream))
                return;

            if (enabledProp.Get(stream))
            {
                handle.SetRotation(stream, V4ToQuat(targetRotProp.Get(stream)) * RotationOffset);
            }
            else
            {
                PassThrough(stream, handle);
            }
        }
        public void ApplyRotation(AnimationStream stream, ReadWriteTransformHandle handle, Vector4Property targetRotProp, Quaternion RotationOffset)
        {
            if (!handle.IsValid(stream))
                return;

            handle.SetRotation(stream, V4ToQuat(targetRotProp.Get(stream)) * RotationOffset);
        }
        static Vector3 SafeNormalize(Vector3 v, Vector3 fallback)
        {
            float m2 = v.sqrMagnitude;
            if (m2 < 1e-6f) return fallback;
            return v / Mathf.Sqrt(m2);
        }
        static Vector3 ClampTargetToReach(Vector3 aPos, Vector3 tPos, float abLen, float bcLen)
        {
            Vector3 at = tPos - aPos;
            float atLen = at.magnitude;
            if (atLen < 1e-6f) return tPos;

            float maxReach = (abLen + bcLen) - 1e-4f;
            float minReach = Mathf.Abs(abLen - bcLen) + 1e-4f;

            if (atLen > maxReach) return aPos + at * (maxReach / atLen);
            if (atLen < minReach) return aPos + at * (minReach / atLen);

            return tPos;
        }
        static Vector3 BlendNormals(Vector3 bodyNormal, Vector3 prevNormal, float prevWeight)
        {
            Vector3 a = SafeNormalize(bodyNormal, Vector3.forward);
            Vector3 b = SafeNormalize(prevNormal, a);

            // Avoid cancelling to near-zero when opposite
            if (Vector3.Dot(a, b) < -0.5f) b = -b;

            Vector3 v = a * (1f - prevWeight) + b * prevWeight;
            return SafeNormalize(v, a);
        }
        public static Vector3 ClosestPointOnSegment(Vector3 p, Vector3 a, Vector3 b)
        {
            Vector3 ab = b - a;
            float abSqr = Vector3.Dot(ab, ab);
            if (abSqr <= k_SqrEpsilon) return a;
            float t = Mathf.Clamp01(Vector3.Dot(p - a, ab) / abSqr);
            return a + ab * t;
        }
        public static void SegmentSegmentClosestPoints(Vector3 p1, Vector3 q1, Vector3 p2, Vector3 q2, out float s, out float t, out Vector3 c1, out Vector3 c2)
        {
            Vector3 d1 = q1 - p1;
            Vector3 d2 = q2 - p2;
            Vector3 r = p1 - p2;
            float a = Vector3.Dot(d1, d1);
            float e = Vector3.Dot(d2, d2);
            float f = Vector3.Dot(d2, r);

            if (a <= k_SqrEpsilon && e <= k_SqrEpsilon)
            {
                s = t = 0.0f; c1 = p1; c2 = p2; return;
            }
            if (a <= k_SqrEpsilon)
            {
                s = 0.0f; t = Mathf.Clamp01(f / e);
            }
            else
            {
                float c = Vector3.Dot(d1, r);
                if (e <= k_SqrEpsilon)
                {
                    t = 0.0f; s = Mathf.Clamp01(-c / a);
                }
                else
                {
                    float b = Vector3.Dot(d1, d2);
                    float denom = a * e - b * b;

                    if (denom != 0.0f) s = Mathf.Clamp01((b * f - c * e) / denom);
                    else s = 0.0f;

                    t = (b * s + f) / e;
                    if (t < 0.0f) { t = 0.0f; s = Mathf.Clamp01(-c / a); }
                    else if (t > 1.0f) { t = 1.0f; s = Mathf.Clamp01((b - c) / a); }
                }
            }

            c1 = p1 + d1 * s;
            c2 = p2 + d2 * t;
        }
        public static Vector3 CapsuleCapsuleResolve(Vector3 p1, Vector3 q1, float r1, Vector3 p2, Vector3 q2, float r2, Vector3 Up)
        {
            SegmentSegmentClosestPoints(p1, q1, p2, q2, out _, out _, out var c1, out var c2);
            Vector3 n = c1 - c2;
            float dSqr = Vector3.Dot(n, n);
            float rSum = r1 + r2;

            if (dSqr >= rSum * rSum) return Vector3.zero;

            Vector3 normal;
            if (dSqr > k_SqrEpsilon) normal = n / Mathf.Sqrt(dSqr);
            else
            {
                Vector3 axis = (q2 - p2);
                normal = Vector3.Normalize(Vector3.Cross(axis, Up));
                if (normal.sqrMagnitude < 1e-6f)
                {
                    normal = Vector3.Normalize(Vector3.Cross(axis, Vector3.right));
                }

                if (normal.sqrMagnitude < 1e-6f)
                {
                    normal = Up;
                }
            }

            float d = Mathf.Sqrt(Mathf.Max(dSqr, 0f));
            float penetration = (rSum - d);
            return normal * penetration;
        }
        public static void SwingElbowAroundAC(AnimationStream stream, ReadWriteTransformHandle root, ReadWriteTransformHandle mid, ReadWriteTransformHandle tip, Vector3 desiredB)
        {
            Vector3 A = root.GetPosition(stream);
            Vector3 C = tip.GetPosition(stream);
            Vector3 B = mid.GetPosition(stream);

            Vector3 AC = C - A;
            float acSqr = Vector3.Dot(AC, AC);
            if (acSqr <= k_SqrEpsilon) return;

            Vector3 n = AC / Mathf.Sqrt(acSqr);
            Vector3 v1 = B - A; v1 -= n * Vector3.Dot(v1, n);
            Vector3 v2 = desiredB - A; v2 -= n * Vector3.Dot(v2, n);

            float v1Sqr = Vector3.Dot(v1, v1);
            float v2Sqr = Vector3.Dot(v2, v2);
            if (v1Sqr <= k_SqrEpsilon || v2Sqr <= k_SqrEpsilon) return;

            v1 /= Mathf.Sqrt(v1Sqr);
            v2 /= Mathf.Sqrt(v2Sqr);

            float dot = Mathf.Clamp(Vector3.Dot(v1, v2), -1f, 1f);
            float ang = Mathf.Acos(dot);
            Vector3 cross = Vector3.Cross(v1, v2);
            float dir = Mathf.Sign(Vector3.Dot(cross, n));
            Quaternion swing = Quaternion.AngleAxis(ang * dir * Mathf.Rad2Deg, n);

            root.SetRotation(stream, swing * root.GetRotation(stream));
        }
        public static void PassThrough(AnimationStream stream, ReadWriteTransformHandle handle)
        {
            handle.GetLocalTRS(stream, out Vector3 position, out Quaternion rotation, out Vector3 scale);
            handle.SetLocalTRS(stream, position, rotation, scale);
        }
        public static Vector3 PushOutFromCapsule(Vector3 p, Vector3 a, Vector3 b, float radiusWithSkin, Vector3 FallBackUp)
        {
            Vector3 q = ClosestPointOnSegment(p, a, b);
            Vector3 qp = p - q;
            float dSqr = Vector3.Dot(qp, qp);
            if (dSqr >= radiusWithSkin * radiusWithSkin) return p;
            float d = Mathf.Sqrt(Mathf.Max(dSqr, k_SqrEpsilon));
            Vector3 n = (d > 0f) ? (qp / d) : FallBackUp;
            return q + n * radiusWithSkin;
        }

        public Quaternion V4ToQuat(Vector4 v) => new Quaternion(v.x, v.y, v.z, v.w);

        public void SolveLegs(
            AnimationStream stream,
            BoolProperty enabledProp,
            ReadWriteTransformHandle root, ReadWriteTransformHandle mid, ReadWriteTransformHandle tip,
            Vector3Property targetPosProp, Vector4Property targetRotProp,
            Vector3Property hintPosProp, Vector4Property hintRotProp,
            bool useHint,
            Quaternion targetOffset,
            Vector3 bendNormal
        )
        {
            if (!enabledProp.Get(stream) || !(root.IsValid(stream) && mid.IsValid(stream) && tip.IsValid(stream)))
            {
                Pass(stream, root, mid, tip);
                return;
            }

            Vector3 aPos = root.GetPosition(stream);
            Vector3 bPos = mid.GetPosition(stream);
            Vector3 cPos = tip.GetPosition(stream);

            float abLen = (bPos - aPos).magnitude;
            float bcLen = (cPos - bPos).magnitude;

            Vector3 tPos = targetPosProp.Get(stream);

            // --- NEW: clamp leg target relative to hips so the foot cannot go "behind" the body beyond limits ---
            if (HandleHips.IsValid(stream))
            {
                Vector3 hipPos = HandleHips.GetPosition(stream);
                Quaternion hipRot = HandleHips.GetRotation(stream);

                Vector3 hipForward = hipRot * Vector3.forward;
                Vector3 hipUp = hipRot * Vector3.up;

                Vector3 toT = tPos - hipPos;
                float dist = toT.magnitude;

                if (dist > 1e-6f)
                {
                    Vector3 dir = toT / dist;

                    dir = ClampDirectionAsymmetricCone(dir, hipForward, hipUp, k_MaxForwardDeg, k_MaxBackwardDeg);
                    tPos = hipPos + dir * dist;
                }
            }

            // Reach clamp AFTER human-limit clamp
            tPos = ClampTargetToReach(aPos, tPos, abLen, bcLen);

            Quaternion tRot = V4ToQuat(targetRotProp.Get(stream));
            Quaternion hRot = V4ToQuat(hintRotProp.Get(stream));
            Vector3 hPos = hintPosProp.Get(stream);

            var target = new AffineTransform(tPos, tRot);
            var hint = new AffineTransform(hPos, hRot);

            SolveTwoBoneStable(stream, root, mid, tip, target, hint, useHint, targetOffset, bendNormal);
        }

        public void SolveTwoBoneStable(
           AnimationStream stream,
           ReadWriteTransformHandle root,
           ReadWriteTransformHandle mid,
           ReadWriteTransformHandle tip,
           AffineTransform target,
           AffineTransform hint,
           bool useHint,
           Quaternion targetOffset,
           Vector3 bendNormal
       )
        {
            Vector3 aPosition = root.GetPosition(stream);
            Vector3 bPosition = mid.GetPosition(stream);
            Vector3 cPosition = tip.GetPosition(stream);

            Vector3 tPosition = target.translation;
            Quaternion tRotation = target.rotation * targetOffset;

            Vector3 ab = bPosition - aPosition;
            Vector3 bc = cPosition - bPosition;
            Vector3 ac = cPosition - aPosition;

            float abLen = ab.magnitude;
            float bcLen = bc.magnitude;
            float acLen = ac.magnitude;

            // Clamp again defensively (target may have been modified upstream)
            tPosition = ClampTargetToReach(aPosition, tPosition, abLen, bcLen);

            Vector3 atCorrected = tPosition - aPosition;
            float atCorrectedLen = atCorrected.magnitude;

            float oldAbcAngle = TriangleAngle(acLen, abLen, bcLen);
            float newAbcAngle = TriangleAngle(atCorrectedLen, abLen, bcLen);

            // Stable axis selection:
            // 1) current plane cross(ab,bc)
            // 2) hint plane (if allowed)
            // 3) provided bendNormal (body-based / memory)
            Vector3 axisFromPose = Vector3.Cross(ab, bc);
            Vector3 axisFromHint = useHint ? Vector3.Cross(hint.translation - aPosition, bc) : Vector3.zero;

            Vector3 axis = axisFromPose;
            if (axis.sqrMagnitude < k_SqrEpsilon && axisFromHint.sqrMagnitude > k_SqrEpsilon)
                axis = axisFromHint;

            if (axis.sqrMagnitude < k_SqrEpsilon)
                axis = bendNormal;

            axis = SafeNormalize(axis, Vector3.forward);

            float halfAngle = 0.5f * (oldAbcAngle - newAbcAngle);
            float sin = Mathf.Sin(halfAngle);
            float cos = Mathf.Cos(halfAngle);
            Quaternion deltaR = new Quaternion(axis.x * sin, axis.y * sin, axis.z * sin, cos);
            mid.SetRotation(stream, deltaR * mid.GetRotation(stream));

            cPosition = tip.GetPosition(stream);
            ac = cPosition - aPosition;

            if (atCorrectedLen > k_Epsilon)
                root.SetRotation(stream, QuaternionExt.FromToRotation(ac, atCorrected) * root.GetRotation(stream));

            if (useHint)
            {
                float acSqrMag = ac.sqrMagnitude;
                if (acSqrMag > 0f)
                {
                    bPosition = mid.GetPosition(stream);
                    cPosition = tip.GetPosition(stream);
                    ab = bPosition - aPosition;
                    ac = cPosition - aPosition;

                    Vector3 acNorm = ac / Mathf.Sqrt(acSqrMag);
                    Vector3 ah = hint.translation - aPosition;
                    Vector3 abProj = ab - acNorm * Vector3.Dot(ab, acNorm);
                    Vector3 ahProj = ah - acNorm * Vector3.Dot(ah, acNorm);

                    float maxReach = abLen + bcLen;
                    if (abProj.sqrMagnitude > (maxReach * maxReach * 0.001f) && ahProj.sqrMagnitude > 0f)
                    {
                        Quaternion hintR = QuaternionExt.FromToRotation(abProj, ahProj);
                        hintR = QuaternionExt.NormalizeSafe(hintR);
                        root.SetRotation(stream, hintR * root.GetRotation(stream));
                    }
                }
            }

            tip.SetRotation(stream, tRotation);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Apply(AnimationStream stream, ReadWriteTransformHandle h, Vector3Property p, Vector4Property r, Vector4Property o, BoolProperty sw)
        {
            if (h.IsValid(stream))
            {
                if (sw.Get(stream))
                {

                    Vector3 targetPos = p.Get(stream);
                    Quaternion targetRot = V4ToQuat(r.Get(stream));
                    Quaternion offsetRot = V4ToQuat(o.Get(stream));
                    Quaternion finalRot = targetRot * offsetRot;

                    h.SetPosition(stream, targetPos);
                    h.SetRotation(stream, finalRot);
                }
                else
                {
                    PassThrough(stream, h);
                }
            }
        }
        public void SolveHand(
    AnimationStream stream,
    BoolProperty enabledProp, ReadWriteTransformHandle root, ReadWriteTransformHandle mid, ReadWriteTransformHandle tip,
    Vector3Property targetPosProp, Vector4Property targetRotProp,
    Vector3Property hintPosProp, Vector4Property hintRotProp,
    bool useHint,
    Quaternion targetOffset,
    ReadWriteTransformHandle chestStart, ReadWriteTransformHandle chestEnd, FloatProperty chestRadius, FloatProperty collisionSkin, BoolProperty collisionsEnabled,
    FloatProperty handRadius, FloatProperty handSkin, BoolProperty useHandCapsule, BoolProperty protectElbow,
    Vector3 bendNormal, Vector3 up
)
        {
            if (!enabledProp.Get(stream) || !(root.IsValid(stream) && mid.IsValid(stream) && tip.IsValid(stream)))
            {
                Pass(stream, root, mid, tip);
                return;
            }

            Vector3 aPos = root.GetPosition(stream);
            Vector3 bPos = mid.GetPosition(stream);
            Vector3 cPos = tip.GetPosition(stream);

            float abLen = (bPos - aPos).magnitude;
            float bcLen = (cPos - bPos).magnitude;

            Vector3 tgtPos = targetPosProp.Get(stream);
            Quaternion tgtRot = V4ToQuat(targetRotProp.Get(stream));
            Vector3 hintPos = hintPosProp.Get(stream);
            Quaternion hintRot = V4ToQuat(hintRotProp.Get(stream));

            if (HandleChest.IsValid(stream))
            {
                Vector3 chestPos = HandleChest.GetPosition(stream);
                Quaternion chestRot = HandleChest.GetRotation(stream);

                Vector3 chestForward = chestRot * Vector3.forward;
                Vector3 chestUp = chestRot * Vector3.up;

                // target
                {
                    Vector3 toT = tgtPos - chestPos;
                    float d = toT.magnitude;
                    if (d > 1e-6f)
                    {
                        Vector3 dir = toT / d;
                        dir = ClampDirectionAsymmetricCone(dir, chestForward, chestUp, k_ArmMaxForwardDeg, k_ArmMaxBackwardDeg);
                        tgtPos = chestPos + dir * d;
                    }
                }

                // hint (clamp a bit looser, or same — here same)
                {
                    Vector3 toH = hintPos - chestPos;
                    float d = toH.magnitude;
                    if (d > 1e-6f)
                    {
                        Vector3 dir = toH / d;
                        dir = ClampDirectionAsymmetricCone(dir, chestForward, chestUp, k_ArmMaxForwardDeg, k_ArmMaxBackwardDeg);
                        hintPos = chestPos + dir * d;
                    }
                }

                // Write back into locals (we already use locals later)
            }

            bool doCollisions = collisionsEnabled.Get(stream) && chestStart.IsValid(stream) && chestEnd.IsValid(stream);
            if (doCollisions)
            {
                Vector3 a = chestStart.GetPosition(stream);
                Vector3 b = chestEnd.GetPosition(stream);
                float chestR = Mathf.Max(0f, chestRadius.Get(stream) + collisionSkin.Get(stream));

                if (useHandCapsule.Get(stream))
                {
                    float hRad = Mathf.Max(0f, handRadius.Get(stream) + handSkin.Get(stream));
                    Vector3 handA = mid.GetPosition(stream);
                    Vector3 handB = tip.GetPosition(stream);

                    Vector3 correction = CapsuleCapsuleResolve(handA, handB, hRad, a, b, chestR, up);
                    if (correction.sqrMagnitude > 0f)
                    {
                        tgtPos += correction;
                        hintPos += correction * 0.25f;
                    }
                }
                else
                {
                    tgtPos = PushOutFromCapsule(tgtPos, a, b, chestR, up);
                    Vector3 nudgedHint = PushOutFromCapsule(hintPos, a, b, chestR * 0.9f, up);
                    hintPos = Vector3.Lerp(hintPos, nudgedHint, 0.6f);
                }
            }

            // Reach clamp BEFORE solve
            tgtPos = ClampTargetToReach(aPos, tgtPos, abLen, bcLen);

            var target = new AffineTransform(tgtPos, tgtRot);
            var hint = new AffineTransform(hintPos, hintRot);

            SolveTwoBoneIKArmsStable(stream, root, mid, tip, target, hint, useHint, targetOffset, bendNormal, up);

            if (protectElbow.Get(stream) && doCollisions)
            {
                Vector3 a = chestStart.GetPosition(stream);
                Vector3 b = chestEnd.GetPosition(stream);
                float chestR = Mathf.Max(0f, chestRadius.Get(stream) + collisionSkin.Get(stream));

                Vector3 B = mid.GetPosition(stream);
                Vector3 pushedB = PushOutFromCapsule(B, a, b, chestR, up);
                if ((pushedB - B).sqrMagnitude > 1e-10f)
                {
                    SwingElbowAroundAC(stream, root, mid, tip, pushedB);
                    SolveTwoBoneIKArmsStable(stream, root, mid, tip, target, hint, useHint, targetOffset, bendNormal, up);
                }
            }
        }
        public void SolveTwoBoneIKArmsStable(
                   AnimationStream stream,
                   ReadWriteTransformHandle root,
                   ReadWriteTransformHandle mid,
                   ReadWriteTransformHandle tip,
                   AffineTransform target,
                   AffineTransform hint,
                   bool useHint,
                   Quaternion targetOffset,
                   Vector3 bendNormal, Vector3 Up
               )
        {
            Vector3 aPosition = root.GetPosition(stream);
            Vector3 bPosition = mid.GetPosition(stream);
            Vector3 cPosition = tip.GetPosition(stream);

            Vector3 tPosition = target.translation;
            Quaternion tRotation = target.rotation * targetOffset;

            Vector3 ab = bPosition - aPosition;
            Vector3 bc = cPosition - bPosition;
            Vector3 ac = cPosition - aPosition;

            float abLen = ab.magnitude;
            float bcLen = bc.magnitude;
            float acLen = ac.magnitude;

            // Reach clamp
            tPosition = ClampTargetToReach(aPosition, tPosition, abLen, bcLen);

            Vector3 atCorrected = tPosition - aPosition;
            float atCorrectedLen = atCorrected.magnitude;

            float oldAbcAngle = TriangleAngle(acLen, abLen, bcLen);
            float newAbcAngle = TriangleAngle(atCorrectedLen, abLen, bcLen);

            Vector3 axisFromPose = Vector3.Cross(ab, bc);
            Vector3 axisFromHint = useHint ? Vector3.Cross(hint.translation - aPosition, bc) : Vector3.zero;

            Vector3 axis = axisFromPose;
            if (axis.sqrMagnitude < k_SqrEpsilon && axisFromHint.sqrMagnitude > k_SqrEpsilon)
                axis = axisFromHint;

            if (axis.sqrMagnitude < k_SqrEpsilon)
                axis = bendNormal;

            axis = SafeNormalize(axis, Up);

            float halfAngle = 0.5f * (oldAbcAngle - newAbcAngle);
            float sin = Mathf.Sin(halfAngle);
            float cos = Mathf.Cos(halfAngle);
            Quaternion deltaR = new Quaternion(axis.x * sin, axis.y * sin, axis.z * sin, cos);
            mid.SetRotation(stream, deltaR * mid.GetRotation(stream));

            cPosition = tip.GetPosition(stream);
            ac = cPosition - aPosition;

            if (atCorrectedLen > k_Epsilon)
            {
                Quaternion rootDelta = QuaternionExt.FromToRotation(ac, atCorrected);
                root.SetRotation(stream, rootDelta * root.GetRotation(stream));
            }

            if (useHint)
            {
                float acSqrMag = ac.sqrMagnitude;
                if (acSqrMag > 0f)
                {
                    bPosition = mid.GetPosition(stream);
                    cPosition = tip.GetPosition(stream);
                    ab = bPosition - aPosition;
                    ac = cPosition - aPosition;

                    Vector3 acNorm = ac / Mathf.Sqrt(acSqrMag);
                    Vector3 ah = hint.translation - aPosition;
                    Vector3 abProj = ab - acNorm * Vector3.Dot(ab, acNorm);
                    Vector3 ahProj = ah - acNorm * Vector3.Dot(ah, acNorm);

                    float maxReach = abLen + bcLen;
                    if (abProj.sqrMagnitude > (maxReach * maxReach * 0.001f) && ahProj.sqrMagnitude > 0f)
                    {
                        Quaternion hintR = QuaternionExt.FromToRotation(abProj, ahProj);
                        hintR = QuaternionExt.NormalizeSafe(hintR);
                        root.SetRotation(stream, hintR * root.GetRotation(stream));
                    }
                }
            }

            tip.SetRotation(stream, tRotation);
        }
        public void Pass(AnimationStream stream, ReadWriteTransformHandle root, ReadWriteTransformHandle mid, ReadWriteTransformHandle tip)
        {
            if (root.IsValid(stream)) PassThrough(stream, root);
            if (mid.IsValid(stream)) PassThrough(stream, mid);
            if (tip.IsValid(stream)) PassThrough(stream, tip);
        }
        public void SolveHipsAndSpine(AnimationStream stream, Vector3 targetPositionHips, Vector4Property targetRotationHips, Vector4Property offsetRotationHips, BoolProperty EnableSpineIK, ReadWriteTransformHandle HandleHips, ReadWriteTransformHandle HandleChest, ReadWriteTransformHandle HandleNeck, ReadWriteTransformHandle HandleHead, Vector3Property targetPositionHead, Vector4Property targetRotationHead, Quaternion targetOffsetHead, Vector3Property bendNormalHead)
        {
            // Early out: pass-through if spine IK disabled
            if (!EnableSpineIK.Get(stream))
            {
                Pass(stream, HandleChest, HandleNeck, HandleHead);
                PassThrough(stream, HandleHips);
                return;
            }

            // Apply hips driver if valid
            ApplyHipsDriver(stream, HandleHips, targetPositionHips, targetRotationHips, offsetRotationHips);

            // Validate required upper chain handles (Burst-safe: no params/arrays)

            bool IsValid = HandleChest.IsValid(stream) & HandleNeck.IsValid(stream) & HandleHead.IsValid(stream);

            if (!IsValid)
            {
                Pass(stream, HandleChest, HandleNeck, HandleHead);
                return;
            }

            // Build target + hint transforms
            var tRot = V4ToQuat(targetRotationHead.Get(stream));
            var target = new AffineTransform(targetPositionHead.Get(stream), tRot);
            var bendNormal = bendNormalHead.Get(stream);

            SolveTwoBoneSpine(stream, HandleChest, HandleNeck, HandleHead, target, targetOffsetHead, bendNormal);
        }
        public void ApplyHipsDriver(AnimationStream stream, ReadWriteTransformHandle hips, Vector3 targetPos, Vector4Property targetRot, Vector4Property offsetRot)
        {
            if (!hips.IsValid(stream))
            {
                return;
            }
            Quaternion hipRot = V4ToQuat(targetRot.Get(stream));
            Quaternion hipOff = V4ToQuat(offsetRot.Get(stream));

            hips.SetPosition(stream, targetPos);
            hips.SetRotation(stream, hipRot * hipOff); // apply offset in target space
        }
        public void SolveTwoBoneSpine(AnimationStream stream, ReadWriteTransformHandle root, ReadWriteTransformHandle mid, ReadWriteTransformHandle tip, AffineTransform target, Quaternion targetOffset, Vector3 bendNormal)
        {
            // Read current joint positions
            Vector3 aPos = root.GetPosition(stream);
            Vector3 bPos = mid.GetPosition(stream);
            Vector3 cPos = tip.GetPosition(stream);

            // Target with offset applied in target space
            Vector3 tPos = target.translation;
            Quaternion tRot = target.rotation * targetOffset;

            // Current bone vectors
            Vector3 ab = bPos - aPos;
            Vector3 bc = cPos - bPos;
            Vector3 ac = cPos - aPos;
            Vector3 at = tPos - aPos;

            float abLen = ab.magnitude;
            float bcLen = bc.magnitude;
            float acLen = ac.magnitude;
            float atLen = at.magnitude;
            float oldAbcAngle = TriangleAngle(acLen, abLen, bcLen);
            float newAbcAngle = TriangleAngle(atLen, abLen, bcLen);

            // Compute rotation axis for mid joint bend
            Vector3 axis = ComputeIkAxis(bendNormal);

            // Rotate mid joint by half the angle delta (distributes motion)
            float halfAngle = 0.5f * (oldAbcAngle - newAbcAngle);
            float s = Mathf.Sin(halfAngle);
            float c = Mathf.Cos(halfAngle);
            Quaternion deltaMid = new Quaternion(axis.x * s, axis.y * s, axis.z * s, c);
            mid.SetRotation(stream, deltaMid * mid.GetRotation(stream));

            // Re-evaluate and swing root so AC aligns with AT
            cPos = tip.GetPosition(stream);
            ac = cPos - aPos;
            root.SetRotation(stream, QuaternionExt.FromToRotation(ac, at) * root.GetRotation(stream));

            // Set tip rotation to match target orientation (+offset)
            tip.SetRotation(stream, tRot);
        }
        public float TriangleAngle(float aLen, float aLen1, float aLen2)
        {
            if (aLen1 <= k_SqrEpsilon || aLen2 <= k_SqrEpsilon)
            {
                return 0f;
            }

            float c = Mathf.Clamp((aLen1 * aLen1 + aLen2 * aLen2 - aLen * aLen) / (aLen1 * aLen2) / 2.0f, -1.0f, 1.0f);
            return Mathf.Acos(c);
        }
        private Vector3 ComputeIkAxis(Vector3 bendNormal)
        {
            Vector3 axis;
            axis = bendNormal;
            float mag2 = axis.sqrMagnitude;
            if (mag2 < k_SqrEpsilon)
            {
                // Deterministic fallback to avoid NaNs/garbage under Burst
                return Vector3.forward;
            }

            return axis / Mathf.Sqrt(mag2);
        }
        public static Vector3 ClampDirectionAsymmetricCone(
            Vector3 dir,
            Vector3 forward,
            Vector3 up,
            float maxForwardDeg,
            float maxBackwardDeg
        )
        {
            dir = SafeNormalize(dir, forward);
            forward = SafeNormalize(forward, Vector3.forward);
            up = SafeNormalize(up, Vector3.up);

            float dotF = Mathf.Clamp(Vector3.Dot(forward, dir), -1f, 1f);

            // Allowed angle depends on whether we're generally forward or behind.
            // If behind (dotF < 0), we allow only (90 + maxBackwardDeg) away from forward.
            float allowed = (dotF >= 0f) ? maxForwardDeg : (90f + maxBackwardDeg);

            float angle = Mathf.Acos(dotF) * Mathf.Rad2Deg;
            if (angle <= allowed) return dir;

            // Axis for rotating dir toward forward
            Vector3 axis = Vector3.Cross(dir, forward);
            if (axis.sqrMagnitude < 1e-8f)
            {
                // Degenerate: pick something perpendicular to forward using up
                axis = Vector3.Cross(up, forward);
                if (axis.sqrMagnitude < 1e-8f)
                    axis = Vector3.Cross(Vector3.right, forward);
            }
            axis = SafeNormalize(axis, Vector3.up);

            // Rotate *dir* toward *forward* by (angle - allowed)
            float delta = angle - allowed;

            // We want a rotation that reduces the angle to forward.
            // Rotate around axis in the direction that moves dir toward forward.
            Quaternion q = Quaternion.AngleAxis(delta, axis);

            Vector3 clamped = q * dir;

            // If we rotated the wrong way (rare but possible if axis sign is off), flip axis.
            if (Vector3.Angle(forward, clamped) > allowed + 0.01f)
            {
                q = Quaternion.AngleAxis(delta, -axis);
                clamped = q * dir;
            }

            return SafeNormalize(clamped, forward);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Vector3 EnforceMaxHeadHipsDistance(Vector3 headPos, Vector3 hipsPos, float maxDist)
        {
            if (maxDist <= 0f) return hipsPos;

            Vector3 d = hipsPos - headPos;
            float dist = d.magnitude;
            if (dist <= maxDist || dist < 1e-6f) return hipsPos;

            return headPos + d * (maxDist / dist);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Vector3 EnforceMinHeadHipsDistance(Vector3 headPos,Vector3 hipsPos,float minDist,Vector3 worldUp,bool preferDown = true)
        {
            if (minDist <= 0f) return hipsPos;

            Vector3 d = hipsPos - headPos;
            float dsq = d.sqrMagnitude;
            float minSq = minDist * minDist;

            if (dsq >= minSq) return hipsPos;

            // If basically coincident, pick a deterministic direction.
            Vector3 dir;
            if (dsq > 1e-10f)
            {
                dir = d / Mathf.Sqrt(dsq);
            }
            else
            {
                // Default: push hips downward away from head
                dir = preferDown ? -worldUp : worldUp;
            }

            // Bias toward pushing "down" (more natural) unless the direction is unusable.
            if (preferDown)
            {
                // Blend toward down to reduce sideways snapping.
                Vector3 down = -worldUp;
                // If dir is almost opposite down, keep dir (avoid flipping).
                float dot = Vector3.Dot(dir, down);
                float t = Mathf.Clamp01((dot + 1f) * 0.5f); // map [-1..1] to [0..1]
                dir = SafeNormalize(Vector3.Lerp(dir, down, t), down);
            }

            return headPos + dir * minDist;
        }
    }
    public class BasisFullBodyJobBinder : AnimationJobBinder<BasisFullIKConstraintJob, BasisFullBodyData>
    {
        public override BasisFullIKConstraintJob Create(Animator animator, ref BasisFullBodyData data, Component component)
        {
            var job = new BasisFullIKConstraintJob
            {
                // Transforms
                HandleHips = BindHandle(animator, data.hips),
                HandleChest = BindHandle(animator, data.chest),
                HandleNeck = BindHandle(animator, data.neck),
                HandleHead = BindHandle(animator, data.head),

                HandleLeftUpperLeg = BindHandle(animator, data.LeftUpperLeg),
                HandleLeftLowerLeg = BindHandle(animator, data.LeftLowerLeg),
                HandleLeftFoot = BindHandle(animator, data.leftFoot),

                HandleRightUpperLeg = BindHandle(animator, data.RightUpperLeg),
                HandleRightLowerLeg = BindHandle(animator, data.RightLowerLeg),
                HandleRightFoot = BindHandle(animator, data.RightFoot),

                HandleLeftToe = BindHandle(animator, data.LeftToe),
                HandleRightToe = BindHandle(animator, data.RightToe),

                HandleLeftUpperArm = BindHandle(animator, data.leftUpperArm),
                HandleLeftLowerArm = BindHandle(animator, data.leftLowerArm),
                HandleLeftHand = BindHandle(animator, data.LeftHand),

                HandleRightUpperArm = BindHandle(animator, data.RightUpperArm),
                HandleRightLowerArm = BindHandle(animator, data.RightLowerArm),
                HandleRightHand = BindHandle(animator, data.RightHand),

                HandleSpine = BindHandle(animator, data.spine),
                HandleUpperChest = BindHandle(animator, data.upperChest),
                HandleLeftShoulder = BindHandle(animator, data.LeftShoulder),
                HandleRightShoulder = BindHandle(animator, data.RightShoulder),

                targetPositionHips = Vector3Property.Bind(animator, component, data.TargetPositionPropertyHips),
                targetPositionHead = Vector3Property.Bind(animator, component, data.TargetPositionPropertyHead),
                hintPositionHead = Vector3Property.Bind(animator, component, data.HintPositionPropertyHead),
                bendNormalHead = Vector3Property.Bind(animator, component, data.bendNormalHeadProperty),

                targetPositionLeftLowerLeg = Vector3Property.Bind(animator, component, data.TargetPositionPropertyLeftLowerLeg),
                hintPositionLeftLowerLeg = Vector3Property.Bind(animator, component, data.HintPositionPropertyLeftLowerLeg),

                targetPositionRightLowerLeg = Vector3Property.Bind(animator, component, data.TargetPositionPropertyRightLowerLeg),
                hintPositionRightLowerLeg = Vector3Property.Bind(animator, component, data.HintPositionPropertyRightLowerLeg),

                leftDrivenTargetPos = Vector3Property.Bind(animator, component, data.LeftDrivenTargetPosProperty),
                rightDrivenTargetPos = Vector3Property.Bind(animator, component, data.RightDrivenTargetPosProperty),

                targetPositionLeftHand = Vector3Property.Bind(animator, component, data.TargetPositionPropertyLeftHand),
                hintPositionLeftHand = Vector3Property.Bind(animator, component, data.HintPositionPropertyLeftHand),

                targetPositionRightHand = Vector3Property.Bind(animator, component, data.TargetPositionPropertyRightHand),
                hintPositionRightHand = Vector3Property.Bind(animator, component, data.HintPositionPropertyRightHand),

                targetRotationHips = Vector4Property.Bind(animator, component, data.TargetRotationPropertyHips),
                offsetRotationHips = Vector4Property.Bind(animator, component, data.OffsetRotationPropertyHips),

                targetRotationHead = Vector4Property.Bind(animator, component, data.TargetRotationPropertyHead),
                hintRotationHead = Vector4Property.Bind(animator, component, data.HintRotationPropertyHead),

                TargetRotationLeftShoulder = Vector4Property.Bind(animator, component, data.TargetRotationLeftShoulderProperty),
                TargetRotationRightShoulder = Vector4Property.Bind(animator, component, data.TargetRotationRightShoulderProperty),

                targetRotationLeftLowerLeg = Vector4Property.Bind(animator, component, data.TargetRotationPropertyLeftLowerLeg),
                hintRotationLeftLowerLeg = Vector4Property.Bind(animator, component, data.HintRotationPropertyLeftLowerLeg),

                targetRotationRightLowerLeg = Vector4Property.Bind(animator, component, data.TargetRotationPropertyRightLowerLeg),
                hintRotationRightLowerLeg = Vector4Property.Bind(animator, component, data.HintRotationPropertyRightLowerLeg),

                leftDrivenTargetRot = Vector4Property.Bind(animator, component, data.LeftDrivenTargetRotProperty),
                rightDrivenTargetRot = Vector4Property.Bind(animator, component, data.RightDrivenTargetRotProperty),

                targetRotationLeftHand = Vector4Property.Bind(animator, component, data.TargetRotationPropertyLeftHand),
                hintRotationLeftHand = Vector4Property.Bind(animator, component, data.HintRotationPropertyLeftHand),

                targetRotationRightHand = Vector4Property.Bind(animator, component, data.TargetRotationPropertyRightHand),
                hintRotationRightHand = Vector4Property.Bind(animator, component, data.HintRotationPropertyRightHand),
                enabledSpineIK = BoolProperty.Bind(animator, component, data.EnabledPropertySpineIK),
                hintWeightHead = BoolProperty.Bind(animator, component, data.HintWeightBoolPropertyHead),

                enabledLeftLowerLeg = BoolProperty.Bind(animator, component, data.EnabledPropertyLeftLowerLeg),
                hintWeightLeftLowerLeg = BoolProperty.Bind(animator, component, data.HintWeightBoolPropertyLeftLowerLeg),

                enabledRightLowerLeg = BoolProperty.Bind(animator, component, data.EnabledPropertyRightLowerLeg),
                hintWeightRightLowerLeg = BoolProperty.Bind(animator, component, data.HintWeightBoolPropertyRightLowerLeg),

                leftToeEnabled = BoolProperty.Bind(animator, component, data.LeftToeEnabledProperty),
                RightToeEnabled = BoolProperty.Bind(animator, component, data.RightToeEnabledProperty),

                enabledLeftHand = BoolProperty.Bind(animator, component, data.EnabledPropertyLeftHand),
                hintWeightLeftHand = BoolProperty.Bind(animator, component, data.HintWeightBoolPropertyLeftHand),

                enabledRightHand = BoolProperty.Bind(animator, component, data.EnabledPropertyRightHand),
                hintWeightRightHand = BoolProperty.Bind(animator, component, data.HintWeightBoolPropertyRightHand),

                protectElbow = BoolProperty.Bind(animator, component, data.ProtectElbowBoolProperty),

                collisionsEnabled = BoolProperty.Bind(animator, component, data.CollisionsEnabledBoolProperty),
                useHandCapsule = BoolProperty.Bind(animator, component, data.UseHandCapsuleBoolProperty),
                chestRadius = FloatProperty.Bind(animator, component, data.ChestRadiusFloatProperty),
                collisionSkin = FloatProperty.Bind(animator, component, data.CollisionSkinFloatProperty),
                handRadius = FloatProperty.Bind(animator, component, data.HandRadiusFloatProperty),
                handSkin = FloatProperty.Bind(animator, component, data.HandSkinFloatProperty),

                maxBendDeg = FloatProperty.Bind(animator, component, data.MaxBendDegFloatProperty),
                MaxChestDeltaDeg = FloatProperty.Bind(animator, component, data.MaxChestDeltaDegFloatProperty),

                enabledLeftShoulder = BoolProperty.Bind(animator, component, data.enabledLeftShoulderProperty),
                enabledRightShoulder = BoolProperty.Bind(animator, component, data.enabledRightShoulderProperty),

                targetOffsetLeftShoulder = data.m_CalibratedRotationLeftShoulder,
                targetOffsetRightShoulder = data.m_CalibratedRotationRightShoulder,

                targetOffsetNeck = data.m_CalibratedRotationNeck,
                targetOffsetHead = data.m_CalibratedRotationHead,
                targetOffsetChest = data.m_CalibratedRotationChest,
                targetOffsetLeftToe = data.m_CalibratedRotationLeftToe,
                targetOffsetRightToe = data.m_CalibratedRotationRightToe,

                targetOffsetLeftFoot = data.m_CalibratedRotationLeftFoot,
                targetOffsetRightFoot = data.m_CalibratedRotationRightFoot,

                targetOffsetLeftHand = data.m_CalibratedRotationLeftHand,
                targetOffsetRightHand = data.m_CalibratedRotationRightHand,

                MinHeadSpineHeight = FloatProperty.Bind(animator, component, data.MinHeadSpineHeightFloatProperty),

                prevBendNormalLeftLeg = Vector3Property.Bind(animator, component, data.PrevBendNormalLeftLegProperty),
                prevBendNormalRightLeg = Vector3Property.Bind(animator, component, data.PrevBendNormalRightLegProperty),
                prevBendNormalLeftArm = Vector3Property.Bind(animator, component, data.PrevBendNormalLeftArmProperty),
                prevBendNormalRightArm = Vector3Property.Bind(animator, component, data.PrevBendNormalRightArmProperty),

            };

            // Bind positions
            job.p0 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(0));
            job.p1 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(1));
            job.p2 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(2));
            job.p3 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(3));
            job.p4 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(4));
            job.p5 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(5));
            job.p6 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(6));
            job.p7 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(7));
            job.p8 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(8));
            job.p9 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(9));
            job.p10 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(10));
            job.p11 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(11));
            job.p12 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(12));
            job.p13 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(13));
            job.p14 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(14));
            job.p15 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(15));
            job.p16 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(16));
            job.p17 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(17));
            job.p18 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(18));
            job.p19 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(19));
            job.p20 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(20));
            job.p54 = Vector3Property.Bind(animator, component, data.GetTargetPositionVector3Property(54));
            // Bind rotations (as Vector4)
            job.r0 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(0));
            job.r1 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(1));
            job.r2 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(2));
            job.r3 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(3));
            job.r4 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(4));
            job.r5 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(5));
            job.r6 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(6));
            job.r7 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(7));
            job.r8 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(8));
            job.r9 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(9));
            job.r10 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(10));
            job.r11 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(11));
            job.r12 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(12));
            job.r13 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(13));
            job.r14 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(14));
            job.r15 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(15));
            job.r16 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(16));
            job.r17 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(17));
            job.r18 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(18));
            job.r19 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(19));
            job.r20 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(20));
            job.r54 = Vector4Property.Bind(animator, component, data.GetTargetRotationVector4Property(54));
            // Bind offsets
            job.o0 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(0));
            job.o1 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(1));
            job.o2 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(2));
            job.o3 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(3));
            job.o4 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(4));
            job.o5 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(5));
            job.o6 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(6));
            job.o7 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(7));
            job.o8 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(8));
            job.o9 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(9));
            job.o10 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(10));
            job.o11 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(11));
            job.o12 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(12));
            job.o13 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(13));
            job.o14 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(14));
            job.o15 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(15));
            job.o16 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(16));
            job.o17 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(17));
            job.o18 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(18));
            job.o19 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(19));
            job.o20 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(20));
            job.o54 = Vector4Property.Bind(animator, component, data.GetOffsetRotationVector4Property(54));

            // Bind per-slot weights
            job.w0 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(0));
            job.w1 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(1));
            job.w2 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(2));
            job.w3 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(3));
            job.w4 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(4));
            job.w5 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(5));
            job.w6 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(6));
            job.w7 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(7));
            job.w8 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(8));
            job.w9 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(9));
            job.w10 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(10));
            job.w11 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(11));
            job.w12 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(12));
            job.w13 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(13));
            job.w14 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(14));
            job.w15 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(15));
            job.w16 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(16));
            job.w17 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(17));
            job.w18 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(18));
            job.w19 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(19));
            job.w20 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(20));
            job.w54 = BoolProperty.Bind(animator, component, data.GetWeightFloatProperty(54));
            return job;
        }
        static ReadWriteTransformHandle BindHandle(Animator animator, Transform t)
    => (t != null) ? ReadWriteTransformHandle.Bind(animator, t) : default;
        public override void Destroy(BasisFullIKConstraintJob job) { }
    }
}
