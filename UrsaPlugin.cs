using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using RoR2.Audio;
using RoR2.Skills;
using RoR2.Projectile;
using RoR2.CharacterAI;
using BepInEx;
using BepInEx.Configuration;
using R2API;
using R2API.Utils;
using R2API.Networking;
using R2API.Networking.Interfaces;
using EntityStates;
using KinematicCharacterController;

namespace Ursa
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin("com.Vodhr.UrsaSurvivor", "Ursa Survivor", "0.3.0")]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "SurvivorAPI",
        "LoadoutAPI",
        "BuffAPI",
        "LanguageAPI",
        "SoundAPI",
        "EffectAPI",
        "UnlockablesAPI",
        "ResourcesAPI",
        "EntityAPI",
        "NetworkingAPI"
    })]

    public class UrsaPlugin : BaseUnityPlugin
    {
        public static GameObject ursaBody;
        public static GameObject ursaDisplay;
        public static GameObject ursaDoppelgangerMaster;

        public static Vector3 ursaBaseSize = new Vector3(1.3f, 1.3f, 1.3f);

        public static BuffIndex overpowerBuff;
        public static BuffIndex earthshockBuff;
        public static BuffIndex earthshockDebuff;
        public static BuffIndex enrageBuff;
        public static BuffIndex furySwipesDebuff;

        public static BepInEx.Logging.ManualLogSource logger;
        public static UrsaPlugin instance;

        public void Awake()
        {
            instance = this;

            Core.Assets.Initialize();
            Core.Config.Read();
            CreateHooks();
            CreateCharacter();
            Core.ItemDisplays.Create();
            InitializeCharacter();
            InitializeBuffs();
            InitializeSkills();
            InitializeSounds();
            InitializeEffects();
            InitializeINetMessages();
            CreateDoppelganger();

            logger = this.Logger;
        }

        private void CreateHooks()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.CharacterBody.ClearTimedBuffs += CharacterBody_ClearTimedBuffs;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private static void CreateCharacter() 
        {
            LanguageAPI.Add("URSA_NAME", "Ursa");
            LanguageAPI.Add("URSA_DESCRIPTION", "Angry bear." + Environment.NewLine);
            LanguageAPI.Add("URSA_SUBTITLE", "Den Father" + Environment.NewLine);
            LanguageAPI.Add("URSA_OUTRO_FLAVOR", "..and so he left, licking his wounds.");

            LanguageAPI.Add("URSA_SKIN_DEFAULT_NAME", "Default");

            ursaBody = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody"), "UrsaBody", true);
            NetworkIdentity networkIdentity = ursaBody.GetComponent<NetworkIdentity>();
            networkIdentity.localPlayerAuthority = true;

            GameObject model = CreateModel(ursaBody);

            GameObject gameObject = new GameObject("ModelBase");
            gameObject.transform.parent = ursaBody.transform;
            gameObject.transform.localPosition = new Vector3(0f, -0.81f, 0f);
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

            GameObject gameObject2 = new GameObject("CameraPivot");
            gameObject2.transform.parent = gameObject.transform;
            gameObject2.transform.localPosition = new Vector3(0f, 1.6f, 0f);
            gameObject2.transform.localRotation = Quaternion.identity;
            gameObject2.transform.localScale = Vector3.one;

            GameObject gameObject3 = new GameObject("AimOrigin");
            gameObject3.transform.parent = gameObject.transform;
            gameObject3.transform.localPosition = new Vector3(0f, 1.4f, 0f);
            gameObject3.transform.localRotation = Quaternion.identity;
            gameObject3.transform.localScale = Vector3.one;

            Transform transform = model.transform;
            transform.parent = gameObject.transform;
            transform.localPosition = Vector3.zero;
            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.localRotation = Quaternion.identity;

            CharacterDirection characterDirection = ursaBody.GetComponent<CharacterDirection>();
            characterDirection.moveVector = Vector3.zero;
            characterDirection.targetTransform = gameObject.transform;
            characterDirection.overrideAnimatorForwardTransform = null;
            characterDirection.rootMotionAccumulator = null;
            characterDirection.modelAnimator = model.GetComponentInChildren<Animator>();
            characterDirection.driveFromRootRotation = false;
            characterDirection.turnSpeed = 720f;

            CharacterBody bodyComponent = ursaBody.GetComponent<CharacterBody>();
            bodyComponent.bodyIndex = -1;
            bodyComponent.baseNameToken = "URSA_NAME";
            bodyComponent.subtitleNameToken = "URSA_SUBTITLE";
            bodyComponent.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;
            bodyComponent.rootMotionInMainState = false;
            bodyComponent.mainRootSpeed = 0;
            bodyComponent.baseMaxHealth = Core.Config.baseMaxHealth.Value;
            bodyComponent.levelMaxHealth = Core.Config.levelMaxHealth.Value;
            bodyComponent.baseRegen = Core.Config.baseRegen.Value;
            bodyComponent.levelRegen = Core.Config.levelRegen.Value;
            bodyComponent.baseMaxShield = Core.Config.baseMaxShield.Value;
            bodyComponent.levelMaxShield = Core.Config.levelMaxShield.Value;
            bodyComponent.baseMoveSpeed = Core.Config.baseMoveSpeed.Value;
            bodyComponent.levelMoveSpeed = Core.Config.levelMoveSpeed.Value;
            bodyComponent.baseAcceleration = Core.Config.baseAcceleration.Value;
            bodyComponent.baseJumpPower = Core.Config.baseJumpPower.Value;
            bodyComponent.levelJumpPower = Core.Config.levelJumpPower.Value;
            bodyComponent.baseJumpCount = Core.Config.baseJumpCount.Value;
            bodyComponent.baseDamage = Core.Config.baseDamage.Value;
            bodyComponent.levelDamage = Core.Config.levelDamage.Value;
            bodyComponent.baseAttackSpeed = Core.Config.baseAttackSpeed.Value;
            bodyComponent.levelAttackSpeed = Core.Config.levelAttackSpeed.Value;
            bodyComponent.baseCrit = Core.Config.baseCrit.Value;
            bodyComponent.levelCrit = Core.Config.levelCrit.Value;
            bodyComponent.baseArmor = Core.Config.baseArmor.Value;
            bodyComponent.levelArmor = Core.Config.levelArmor.Value;
            bodyComponent.sprintingSpeedMultiplier = Core.Config.sprintingSpeedMultiplier.Value;
            bodyComponent.wasLucky = false;
            bodyComponent.hideCrosshair = false;
            bodyComponent.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/simpledotcrosshair");
            bodyComponent.aimOriginTransform = gameObject3.transform;
            bodyComponent.hullClassification = HullClassification.Human;
            bodyComponent.portraitIcon = Core.Assets.portrait.texture;
            bodyComponent.isChampion = false;
            bodyComponent.currentVehicle = null;
            bodyComponent.skinIndex = 0U;

            LoadoutAPI.AddSkill(typeof(States.UrsaMain));
            var stateMachine = ursaBody.GetComponent<EntityStateMachine>();
            stateMachine.mainStateType = new SerializableEntityStateType(typeof(States.UrsaMain));

            CharacterMotor characterMotor = ursaBody.GetComponent<CharacterMotor>();
            characterMotor.walkSpeedPenaltyCoefficient = 1f;
            characterMotor.characterDirection = characterDirection;
            characterMotor.muteWalkMotion = false;
            characterMotor.mass = 200f;
            characterMotor.airControl = 0.25f;
            characterMotor.disableAirControlUntilCollision = false;
            characterMotor.generateParametersOnAwake = true;
            //characterMotor.useGravity = true;
            //characterMotor.isFlying = false;

            InputBankTest inputBankTest = ursaBody.GetComponent<InputBankTest>();
            inputBankTest.moveVector = Vector3.zero;

            CameraTargetParams cameraTargetParams = ursaBody.GetComponent<CameraTargetParams>();
            cameraTargetParams.cameraParams = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<CameraTargetParams>().cameraParams;
            cameraTargetParams.cameraPivotTransform = null;
            cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
            cameraTargetParams.recoil = Vector2.zero;
            cameraTargetParams.idealLocalCameraPos = Vector3.zero;
            cameraTargetParams.dontRaycastToPivot = false;

            ModelLocator modelLocator = ursaBody.GetComponent<ModelLocator>();
            modelLocator.modelTransform = transform;
            modelLocator.modelBaseTransform = gameObject.transform;
            modelLocator.dontReleaseModelOnDeath = false;
            modelLocator.autoUpdateModelTransform = true;
            modelLocator.dontDetatchFromParent = false;
            modelLocator.noCorpse = false;
            modelLocator.normalizeToFloor = true; // set true if you want your character to rotate on terrain like acrid does
            modelLocator.preserveModel = false;

            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            HitBoxGroup rightClawHitBoxGroup = model.AddComponent<HitBoxGroup>();
            HitBoxGroup leftClawHitBoxGroup = model.AddComponent<HitBoxGroup>();

            GameObject rightClawGameObject = new GameObject("RightClawHitBox");
            Transform rightClaw = childLocator.FindChild("R_Hand");
            rightClawGameObject.transform.parent = rightClaw;
            rightClawGameObject.transform.localPosition = new Vector3(-0.2f, 0.2f, 0.3f);
            rightClawGameObject.transform.localScale = Vector3.one * 6.25f;
            rightClawGameObject.transform.localRotation = Quaternion.identity;
            HitBox rightClawHitBox = rightClawGameObject.AddComponent<HitBox>();

            GameObject leftClawGameObject = new GameObject("LeftClawHitBox");
            Transform leftClaw = childLocator.FindChild("L_Hand");
            leftClawGameObject.transform.parent = leftClaw;
            leftClawGameObject.transform.localPosition = new Vector3(-0.2f, 0.2f, 0.3f);
            leftClawGameObject.transform.localScale = Vector3.one * 6.25f;
            leftClawGameObject.transform.localRotation = Quaternion.identity;
            HitBox leftClawHitBox = leftClawGameObject.AddComponent<HitBox>();

            rightClawHitBoxGroup.hitBoxes = new HitBox[] { rightClawHitBox };
            rightClawHitBoxGroup.groupName = "RightClaw";
            leftClawHitBoxGroup.hitBoxes = new HitBox[] { leftClawHitBox };
            leftClawHitBoxGroup.groupName = "LeftClaw";

            Light rightLight = rightClaw.gameObject.AddComponent<Light>();
            rightLight.type = LightType.Point;
            rightLight.color = Color.yellow;
            rightLight.range = 0.85f;
            rightLight.intensity = 95f;
            rightLight.enabled = false;

            Light leftLight = leftClaw.gameObject.AddComponent<Light>();
            leftLight.type = LightType.Point;
            leftLight.color = Color.yellow;
            leftLight.range = 0.85f;
            leftLight.intensity = 95f;
            leftLight.enabled = false;

            CharacterModel characterModel = model.AddComponent<CharacterModel>();
            characterModel.body = bodyComponent;
            List<CharacterModel.RendererInfo> rendererInfos = new List<CharacterModel.RendererInfo>();
            foreach(SkinnedMeshRenderer i in model.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                CharacterModel.RendererInfo info = new CharacterModel.RendererInfo
                {
                    defaultMaterial = i.material,
                    renderer = i,
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                };
                rendererInfos.Add(info);
            }
            characterModel.baseRendererInfos = rendererInfos.ToArray();

            characterModel.autoPopulateLightInfos = true;
            characterModel.invisibilityCount = 0;
            characterModel.temporaryOverlays = new List<TemporaryOverlay>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            LoadoutAPI.SkinDefInfo skinDefInfo = new LoadoutAPI.SkinDefInfo
            {
                BaseSkins = Array.Empty<SkinDef>(),
                GameObjectActivations = new SkinDef.GameObjectActivation[0],
                Icon = Core.Assets.defaultSkinIcon,
                MeshReplacements = new SkinDef.MeshReplacement[0],
                MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0],
                Name = "URSA_SKIN_DEFAULT_NAME",
                NameToken = "URSA_SKIN_DEFAULT_NAME",
                ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0],
                RendererInfos = rendererInfos.ToArray(),
                RootObject = model,
                UnlockableName = "",
            };
            SkinDef skin = LoadoutAPI.CreateNewSkinDef(skinDefInfo);
            skinController.skins = new SkinDef[] { skin };

            TeamComponent teamComponent = null;
            if (ursaBody.GetComponent<TeamComponent>() != null) teamComponent = ursaBody.GetComponent<TeamComponent>();
            else teamComponent = ursaBody.GetComponent<TeamComponent>();
            teamComponent.hideAllyCardDisplay = false;
            teamComponent.teamIndex = TeamIndex.None;

            HealthComponent healthComponent = ursaBody.GetComponent<HealthComponent>();
            healthComponent.health = 90f;
            healthComponent.shield = 0f;
            healthComponent.barrier = 0f;
            healthComponent.magnetiCharge = 0f;
            healthComponent.body = null;
            healthComponent.dontShowHealthbar = false;
            healthComponent.globalDeathEventChanceCoefficient = 1f;

            ursaBody.GetComponent<Interactor>().maxInteractionDistance = 3f;
            ursaBody.GetComponent<InteractionDriver>().highlightInteractor = true;

            CharacterDeathBehavior characterDeathBehavior = ursaBody.GetComponent<CharacterDeathBehavior>();
            characterDeathBehavior.deathStateMachine = ursaBody.GetComponent<EntityStateMachine>();
            characterDeathBehavior.deathState = new SerializableEntityStateType(typeof(GenericCharacterDeath));

            SfxLocator sfxLocator = ursaBody.GetComponent<SfxLocator>();
            sfxLocator.deathSound = "Play_ui_player_death";
            sfxLocator.barkSound = "";
            sfxLocator.openSound = "";
            sfxLocator.landingSound = "Play_char_land";
            sfxLocator.fallDamageSound = "Play_char_land_fall_damage";
            sfxLocator.aliveLoopStart = "";
            sfxLocator.aliveLoopStop = "";

            Rigidbody rigidbody = ursaBody.GetComponent<Rigidbody>();
            rigidbody.mass = 100f;
            rigidbody.drag = 0f;
            rigidbody.angularDrag = 0f;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            rigidbody.interpolation = RigidbodyInterpolation.None;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.constraints = RigidbodyConstraints.None;

            CapsuleCollider capsuleCollider = ursaBody.GetComponent<CapsuleCollider>();
            capsuleCollider.isTrigger = false;
            capsuleCollider.material = null;
            capsuleCollider.center = new Vector3(0f, 0f, 0f);
            capsuleCollider.radius = 0.5f;
            capsuleCollider.height = 1.82f;
            capsuleCollider.direction = 1;

            KinematicCharacterMotor kinematicCharacterMotor = ursaBody.GetComponent<KinematicCharacterMotor>();
            kinematicCharacterMotor.CharacterController = characterMotor;
            kinematicCharacterMotor.Capsule = capsuleCollider;
            kinematicCharacterMotor.Rigidbody = rigidbody;

            capsuleCollider.radius = 0.5f;
            capsuleCollider.height = 1.82f;
            capsuleCollider.center = new Vector3(0, 0, 0);
            capsuleCollider.material = null;

            kinematicCharacterMotor.DetectDiscreteCollisions = false;
            kinematicCharacterMotor.GroundDetectionExtraDistance = 0f;
            kinematicCharacterMotor.MaxStepHeight = 0.2f;
            kinematicCharacterMotor.MinRequiredStepDepth = 0.1f;
            kinematicCharacterMotor.MaxStableSlopeAngle = 55f;
            kinematicCharacterMotor.MaxStableDistanceFromLedge = 0.5f;
            kinematicCharacterMotor.PreventSnappingOnLedges = false;
            kinematicCharacterMotor.MaxStableDenivelationAngle = 55f;
            kinematicCharacterMotor.RigidbodyInteractionType = RigidbodyInteractionType.None;
            kinematicCharacterMotor.PreserveAttachedRigidbodyMomentum = true;
            kinematicCharacterMotor.HasPlanarConstraint = false;
            kinematicCharacterMotor.PlanarConstraintAxis = Vector3.up;
            kinematicCharacterMotor.StepHandling = StepHandlingMethod.None;
            kinematicCharacterMotor.LedgeHandling = true;
            kinematicCharacterMotor.InteractiveRigidbodyHandling = true;
            kinematicCharacterMotor.SafeMovement = false;

            HurtBoxGroup hurtBoxGroup = model.AddComponent<HurtBoxGroup>();

            HurtBox componentInChildren = model.GetComponentInChildren<CapsuleCollider>().gameObject.AddComponent<HurtBox>();
            componentInChildren.gameObject.layer = LayerIndex.entityPrecise.intVal;
            componentInChildren.healthComponent = healthComponent;
            componentInChildren.isBullseye = true;
            componentInChildren.damageModifier = HurtBox.DamageModifier.Normal;
            componentInChildren.hurtBoxGroup = hurtBoxGroup;
            componentInChildren.indexInGroup = 0;

            hurtBoxGroup.hurtBoxes = new HurtBox[]
            {
                componentInChildren
            };

            hurtBoxGroup.mainHurtBox = componentInChildren;
            hurtBoxGroup.bullseyeCount = 1;

            FootstepHandler footstepHandler = model.AddComponent<FootstepHandler>();
            footstepHandler.baseFootstepString = "Play_player_footstep";
            footstepHandler.sprintFootstepOverrideString = "";
            footstepHandler.enableFootstepDust = true;
            footstepHandler.footstepDustPrefab = Resources.Load<GameObject>("Prefabs/GenericFootstepDust");

            RagdollController ragdollController = model.AddComponent<RagdollController>();
            ragdollController.bones = null;
            ragdollController.componentsToDisableOnRagdoll = null;

            AimAnimator aimAnimator = model.AddComponent<AimAnimator>();
            aimAnimator.inputBank = inputBankTest;
            aimAnimator.directionComponent = characterDirection;
            aimAnimator.pitchRangeMax = 55f;
            aimAnimator.pitchRangeMin = -50f;
            aimAnimator.yawRangeMin = -44f;
            aimAnimator.yawRangeMax = 44f;
            aimAnimator.pitchGiveupRange = 30f;
            aimAnimator.yawGiveupRange = 10f;
            aimAnimator.giveupDuration = 8f;

            ursaBody.AddComponent<Miscellaneous.FurySwipesController>();
        }

        private void InitializeCharacter() 
        {

            ursaDisplay = PrefabAPI.InstantiateClone(ursaBody.GetComponent<ModelLocator>().modelBaseTransform.gameObject, "UrsaDisplay", true);
            ursaDisplay.AddComponent<NetworkIdentity>();

            SurvivorDef survivorDef = new SurvivorDef
            {
                name = "URSA_NAME",
                unlockableName = "",
                descriptionToken = "URSA_DESCRIPTION",
                outroFlavorToken = "URSA_OUTRO_FLAVOR",
                primaryColor = Color.magenta,
                bodyPrefab = ursaBody,
                displayPrefab = ursaDisplay
            };

            SurvivorAPI.AddSurvivor(survivorDef);

            BodyCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(ursaBody);
            };
        }

        private void InitializeBuffs()
        {
            BuffDef overpowerBuffDef = new BuffDef
            {
                name = "Overpower",
                iconPath = "textures/bufficons/texWarcryBuffIcon",
                buffColor = Color.magenta,
                canStack = true,
                eliteIndex = EliteIndex.None

            };
            CustomBuff overpowerCustomBuff = new CustomBuff(overpowerBuffDef);
            overpowerBuff = BuffAPI.Add(overpowerCustomBuff);

            BuffDef enrageBuffDef = new BuffDef
            {
                name = "Enrage",
                iconPath = "textures/bufficons/texBuffGenericShield",
                buffColor = Color.magenta,
                canStack = false,
                eliteIndex = EliteIndex.None

            };
            CustomBuff enrageCustomBuff = new CustomBuff(enrageBuffDef);
            enrageBuff = BuffAPI.Add(enrageCustomBuff);

            BuffDef earthshockBuffDef = new BuffDef
            {
                name = "Earthshock",
                iconPath = "textures/bufficons/texMovespeedBuffIcon",
                buffColor = Color.magenta,
                canStack = false,
                eliteIndex = EliteIndex.None

            };
            CustomBuff earthshockCustomBuff = new CustomBuff(earthshockBuffDef);
            earthshockBuff = BuffAPI.Add(earthshockCustomBuff);

            BuffDef earthshockDebuffDef = new BuffDef
            {
                name = "Earthshock Debuff",
                iconPath = "textures/bufficons/texBuffSlow50Icon",
                buffColor = Color.magenta,
                canStack = false,
                isDebuff = true,
                eliteIndex = EliteIndex.None

            };
            CustomBuff earthshockCustomDebuff = new CustomBuff(earthshockDebuffDef);
            earthshockDebuff = BuffAPI.Add(earthshockCustomDebuff);

            BuffDef furySwipesDebuffDef = new BuffDef
            {
                name = "Fury Swipes",
                iconPath = "@Ursa:Assets/UrsaSurvivor/UrsaSurvivorAssets/FurySwipesBuffIcon.png",
                buffColor = Color.magenta,
                canStack = true,
                isDebuff = true,
                eliteIndex = EliteIndex.None

            };
            CustomBuff furySwipesCustomDebuff = new CustomBuff(furySwipesDebuffDef);
            furySwipesDebuff = BuffAPI.Add(furySwipesCustomDebuff);
        }

        private void InitializeSkills() 
        {
            LanguageAPI.Add("URSA_PASSIVE_NAME", "Fury Swipes");
            LanguageAPI.Add("URSA_PASSIVE_DESCRIPTION", "Consecutive attacks on the same target deal <style=cIsDamage>"+ Core.Config.passiveBaseDamageMult.Value * 100 +"% more base damage</style>. If the same target is not attacked after 20 seconds, the bonus damage is lost.");
            LanguageAPI.Add("URSA_PRIMARY_NAME", "Sharp Claws");
            LanguageAPI.Add("URSA_PRIMARY_DESCRIPTION", "Swipe at enemies in front of you, dealing <style=cIsDamage>"+ Core.Config.primaryDamageCoefficient.Value * 100 +"% damage</style>.");
            LanguageAPI.Add("URSA_SECONDARY_NAME", "Overpower");
            LanguageAPI.Add("URSA_SECONDARY_DESCRIPTION", "For each charge consumed, you gain <style=cIsUtility>"+ Core.Config.secondaryAttackSpeedMult.Value +"x attack speed</style> on your next use of Sharp Claws. Has <style=cIsUtility>"+ Core.Config.secondaryBaseCharges.Value +" charges</style> at base.");
            LanguageAPI.Add("URSA_UTILITY_NAME", "Earthshock");
            LanguageAPI.Add("URSA_UTILITY_DESCRIPTION", "<style=cIsUtility>Slowing</style>. <style=cIsUtility>Grounding</style>. Slams the ground, dealing <style=cIsDamage>"+ Core.Config.utilityDamageCoefficient.Value * 100 +"% damage</style> " +
                "and granting you <style=cIsUtility> bonus move speed and jump height </style>.");
            LanguageAPI.Add("URSA_SPECIAL_NAME", "Enrage");
            LanguageAPI.Add("URSA_SPECIAL_DESCRIPTION", "<style=cIsUtility>Grants "+ Core.Config.specialBonusArmor.Value +" armor</style> and <style=cIsDamage>"+ Core.Config.specialDamageMult.Value +"x Fury Swipes damage</style> for 8 seconds.");

            SkillDef sharpClawsSkillDef = ScriptableObject.CreateInstance<SkillDef>();
            sharpClawsSkillDef.activationState = new SerializableEntityStateType(typeof(States.SharpClaws));
            sharpClawsSkillDef.activationStateMachineName = "Weapon";
            sharpClawsSkillDef.baseMaxStock = 1;
            sharpClawsSkillDef.baseRechargeInterval = 0f;
            sharpClawsSkillDef.beginSkillCooldownOnSkillEnd = false;
            sharpClawsSkillDef.canceledFromSprinting = false;
            sharpClawsSkillDef.fullRestockOnAssign = true;
            sharpClawsSkillDef.interruptPriority = InterruptPriority.Any;
            sharpClawsSkillDef.isBullets = false;
            sharpClawsSkillDef.isCombatSkill = true;
            sharpClawsSkillDef.mustKeyPress = false;
            sharpClawsSkillDef.noSprint = true;
            sharpClawsSkillDef.rechargeStock = 1;
            sharpClawsSkillDef.requiredStock = 1;
            sharpClawsSkillDef.shootDelay = 0f;
            sharpClawsSkillDef.stockToConsume = 1;
            sharpClawsSkillDef.icon = Core.Assets.icon1;
            sharpClawsSkillDef.skillDescriptionToken = "URSA_PRIMARY_DESCRIPTION";
            sharpClawsSkillDef.skillName = "URSA_PRIMARY_NAME";
            sharpClawsSkillDef.skillNameToken = "URSA_PRIMARY_NAME";

            SkillDef overpowerSkillDef = ScriptableObject.CreateInstance<SkillDef>();
            overpowerSkillDef.activationState = new SerializableEntityStateType(typeof(States.Overpower));
            overpowerSkillDef.activationStateMachineName = "Body";
            overpowerSkillDef.baseMaxStock = Core.Config.secondaryBaseCharges.Value;
            overpowerSkillDef.baseRechargeInterval = Core.Config.secondaryBaseCooldown.Value;
            overpowerSkillDef.beginSkillCooldownOnSkillEnd = false;
            overpowerSkillDef.canceledFromSprinting = false;
            overpowerSkillDef.fullRestockOnAssign = true;
            overpowerSkillDef.interruptPriority = InterruptPriority.Skill;
            overpowerSkillDef.isBullets = false;
            overpowerSkillDef.isCombatSkill = true;
            overpowerSkillDef.mustKeyPress = true;
            overpowerSkillDef.noSprint = false;
            overpowerSkillDef.rechargeStock = 1;
            overpowerSkillDef.requiredStock = 1;
            overpowerSkillDef.shootDelay = 0f;
            overpowerSkillDef.stockToConsume = 1;
            overpowerSkillDef.icon = Core.Assets.icon2;
            overpowerSkillDef.skillDescriptionToken = "URSA_SECONDARY_DESCRIPTION";
            overpowerSkillDef.skillName = "URSA_SECONDARY_NAME";
            overpowerSkillDef.skillNameToken = "URSA_SECONDARY_NAME";

            SkillDef earthshockSkillDef = ScriptableObject.CreateInstance<SkillDef>();
            earthshockSkillDef.activationState = new SerializableEntityStateType(typeof(States.Earthshock));
            earthshockSkillDef.activationStateMachineName = "Weapon";
            earthshockSkillDef.baseMaxStock = 1;
            earthshockSkillDef.baseRechargeInterval = Core.Config.utilityBaseCooldown.Value;
            earthshockSkillDef.beginSkillCooldownOnSkillEnd = true;
            earthshockSkillDef.canceledFromSprinting = false;
            earthshockSkillDef.fullRestockOnAssign = true;
            earthshockSkillDef.interruptPriority = InterruptPriority.Skill;
            earthshockSkillDef.isBullets = false;
            earthshockSkillDef.isCombatSkill = false;
            earthshockSkillDef.mustKeyPress = true;
            earthshockSkillDef.noSprint = false;
            earthshockSkillDef.rechargeStock = 1;
            earthshockSkillDef.requiredStock = 1;
            earthshockSkillDef.shootDelay = 0f;
            earthshockSkillDef.stockToConsume = 1;
            earthshockSkillDef.icon = Core.Assets.icon3;
            earthshockSkillDef.skillDescriptionToken = "URSA_UTILITY_DESCRIPTION";
            earthshockSkillDef.skillName = "URSA_UTILITY_NAME";
            earthshockSkillDef.skillNameToken = "URSA_UTILITY_NAME";

            SkillDef enrageSkillDef = ScriptableObject.CreateInstance<SkillDef>();
            enrageSkillDef.activationState = new SerializableEntityStateType(typeof(States.Enrage));
            enrageSkillDef.activationStateMachineName = "Body";
            enrageSkillDef.baseMaxStock = 1;
            enrageSkillDef.baseRechargeInterval = Core.Config.specialBaseCooldown.Value;
            enrageSkillDef.beginSkillCooldownOnSkillEnd = true;
            enrageSkillDef.canceledFromSprinting = false;
            enrageSkillDef.fullRestockOnAssign = true;
            enrageSkillDef.interruptPriority = InterruptPriority.Frozen;
            enrageSkillDef.isBullets = false;
            enrageSkillDef.isCombatSkill = true;
            enrageSkillDef.mustKeyPress = true;
            enrageSkillDef.noSprint = true;
            enrageSkillDef.rechargeStock = 1;
            enrageSkillDef.requiredStock = 1;
            enrageSkillDef.shootDelay = 0f;
            enrageSkillDef.stockToConsume = 1;
            enrageSkillDef.icon = Core.Assets.icon4;
            enrageSkillDef.skillDescriptionToken = "URSA_SPECIAL_DESCRIPTION";
            enrageSkillDef.skillName = "URSA_SPECIAL_NAME";
            enrageSkillDef.skillNameToken = "URSA_SPECIAL_NAME";

            LoadoutAPI.AddSkillDef(sharpClawsSkillDef);
            LoadoutAPI.AddSkillDef(overpowerSkillDef);
            LoadoutAPI.AddSkillDef(earthshockSkillDef);
            LoadoutAPI.AddSkillDef(enrageSkillDef);

            foreach(GenericSkill i in ursaBody.GetComponentsInChildren<GenericSkill>())
            {
                DestroyImmediate(i);
            }

            SkillLocator skillLocator = ursaBody.GetComponent<SkillLocator>();

            //Passive
            skillLocator.passiveSkill.enabled = true;
            skillLocator.passiveSkill.skillNameToken = "URSA_PASSIVE_NAME";
            skillLocator.passiveSkill.skillDescriptionToken = "URSA_PASSIVE_DESCRIPTION";
            skillLocator.passiveSkill.icon = Core.Assets.icon5;

            //Primary
            skillLocator.primary = ursaBody.AddComponent<GenericSkill>();
            SkillFamily newFamilyPrimary = ScriptableObject.CreateInstance<SkillFamily>();
            newFamilyPrimary.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamilyPrimary);
            skillLocator.primary.SetFieldValue("_skillFamily", newFamilyPrimary);
            SkillFamily skillFamilyPrimary = skillLocator.primary.skillFamily;
            skillFamilyPrimary.variants[0] = new SkillFamily.Variant
            {
                skillDef = sharpClawsSkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(sharpClawsSkillDef.skillNameToken, false, null)
            };

            //Secondary
            skillLocator.secondary = ursaBody.AddComponent<GenericSkill>();
            SkillFamily newFamilySecondary = ScriptableObject.CreateInstance<SkillFamily>();
            newFamilySecondary.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamilySecondary);
            skillLocator.secondary.SetFieldValue("_skillFamily", newFamilySecondary);
            SkillFamily skillFamilySecondary = skillLocator.secondary.skillFamily;
            skillFamilySecondary.variants[0] = new SkillFamily.Variant
            {
                skillDef = overpowerSkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(overpowerSkillDef.skillNameToken, false, null)
            };

            //Utility
            skillLocator.utility = ursaBody.AddComponent<GenericSkill>();
            SkillFamily newFamilyUtility = ScriptableObject.CreateInstance<SkillFamily>();
            newFamilyUtility.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamilyUtility);
            skillLocator.utility.SetFieldValue("_skillFamily", newFamilyUtility);
            SkillFamily skillFamilyUtility = skillLocator.utility.skillFamily;
            skillFamilyUtility.variants[0] = new SkillFamily.Variant
            {
                skillDef = earthshockSkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(earthshockSkillDef.skillNameToken, false, null)
            };

            //Special
            skillLocator.special = ursaBody.AddComponent<GenericSkill>();
            SkillFamily newFamilySpecial = ScriptableObject.CreateInstance<SkillFamily>();
            newFamilySpecial.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamilySpecial);
            skillLocator.special.SetFieldValue("_skillFamily", newFamilySpecial);
            SkillFamily skillFamilySpecial = skillLocator.special.skillFamily;
            skillFamilySpecial.variants[0] = new SkillFamily.Variant
            {
                skillDef = enrageSkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(enrageSkillDef.skillNameToken, false, null)
            };

        }

        private void InitializeSounds() 
        {

            Core.Assets.ursaHitNetworkSoundEventDef = CreateNetworkSoundEventDef(Core.Assets.ursaHitSound);

            NetworkSoundEventCatalog.getSoundEventDefs += delegate (List<NetworkSoundEventDef> list)
            {
                list.Add(Core.Assets.ursaHitNetworkSoundEventDef);

            };

        }

        private void InitializeEffects()
        {
            if (!Core.Config.utilityRocksEffect.Value)
            {
                var rocks = Core.Assets.earthshockEffect.transform.Find("RockKickUp").gameObject;
                if (rocks)
                {
                    UnityEngine.Object.Destroy(Core.Assets.earthshockEffect.transform.Find("RockKickUp").gameObject);
                }
            }
        }

        private void InitializeINetMessages()
        {
            NetworkingAPI.RegisterMessageType<Core.NetMessages.TimedBuff>();
            NetworkingAPI.RegisterMessageType<Core.NetMessages.RemoveTimedBuff>();
            NetworkingAPI.RegisterMessageType<Core.NetMessages.BodyFlags>();
            NetworkingAPI.RegisterMessageType<Core.NetMessages.RemoveBodyFlags>();
            NetworkingAPI.RegisterMessageType<Core.NetMessages.Sound>();
            NetworkingAPI.RegisterMessageType<Core.NetMessages.Animation>();
            NetworkingAPI.RegisterMessageType<Core.NetMessages.UrsaResize>();
            NetworkingAPI.RegisterMessageType<Core.NetMessages.UrsaHandsGlow>();
        }
        
        private void CreateDoppelganger() 
        {
            ursaDoppelgangerMaster = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterMasters/CommandoMonsterMaster"), "UrsaDoppelGanger", true);

            MasterCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(ursaDoppelgangerMaster);
            };

            CharacterMaster component = ursaDoppelgangerMaster.GetComponent<CharacterMaster>();
            component.bodyPrefab = ursaBody;
        }

        private static GameObject CreateModel(GameObject main)
        {
            Destroy(main.transform.Find("ModelBase").gameObject);
            Destroy(main.transform.Find("CameraPivot").gameObject);
            Destroy(main.transform.Find("AimOrigin").gameObject);

            GameObject model = Core.Assets.MainAssetBundle.LoadAsset<GameObject>("mdlUrsa");

            return model;
        }

        private static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;
            networkSoundEventDef.index = new NetworkSoundEventIndex();

            return networkSoundEventDef;
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            if (self)
            {
                if (self.HasBuff(enrageBuff))
                {
                    self.armor += Core.Config.specialBonusArmor.Value;
                }
                if (self.HasBuff(earthshockBuff))
                {
                    float bonusSpeed = self.moveSpeed * Core.Config.utilityBonusMoveSpeed.Value;
                    float bonusJumpHeight = self.jumpPower * Core.Config.utilityBonusJumpPower.Value;
                    self.moveSpeed += bonusSpeed;
                    self.jumpPower += bonusJumpHeight;
                }
                if (self.HasBuff(earthshockDebuff))
                {
                    self.moveSpeed *= States.Earthshock.debuffSlow;
                    if (!self.gameObject.GetComponent<Miscellaneous.AntiFlyingComponent>())
                    {
                        self.gameObject.AddComponent<Miscellaneous.AntiFlyingComponent>();
                    }
                }
            }
        }

        private void CharacterBody_ClearTimedBuffs(On.RoR2.CharacterBody.orig_ClearTimedBuffs orig, CharacterBody self, BuffIndex buffType)
        {
            if (buffType == furySwipesDebuff) return;

            orig(self, buffType);
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo) 
        {

            if (damageInfo == null) { orig(self, damageInfo); return; }
            else if (!self || !self.gameObject.GetComponent<CharacterBody>()) { orig(self, damageInfo); return; }
            else if (!damageInfo.attacker || !damageInfo.attacker.GetComponent<CharacterBody>()) { orig(self, damageInfo); return; }

            CharacterBody characterBody = self.gameObject.GetComponent<CharacterBody>();
            CharacterBody attackerCharacterBody = damageInfo.attacker.GetComponent<CharacterBody>();
            Miscellaneous.FurySwipesController furySwipesController = attackerCharacterBody.GetComponent<Miscellaneous.FurySwipesController>();

            if(attackerCharacterBody.baseNameToken == "URSA_NAME" && characterBody.HasBuff(furySwipesDebuff) && damageInfo.procCoefficient >= 1 && furySwipesController)
            {
                int count = characterBody.GetBuffCount(furySwipesDebuff);
                float bonusDamage = count * (attackerCharacterBody.damage * furySwipesController.furySwipesMult);
                damageInfo.damage += bonusDamage;
            }

            orig(self, damageInfo);

            if (attackerCharacterBody.baseNameToken == "URSA_NAME" && damageInfo.procCoefficient >= 1)
            {
                characterBody.AddTimedBuff(furySwipesDebuff, Core.Config.passiveDebuffDuration.Value);
                foreach(CharacterBody.TimedBuff i in characterBody.timedBuffs)
                {
                    i.timer = Core.Config.passiveDebuffDuration.Value;
                }
            }
        }
    }
}
