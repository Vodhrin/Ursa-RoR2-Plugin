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

namespace Ursa.Survivor
{
    internal static class Character
    {
        public static GameObject body;
        public static GameObject display;
        public static GameObject doppelgangerMaster;

        public static void Create()
        {
            body = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody"), "UrsaBody", true);
            NetworkIdentity networkIdentity = body.GetComponent<NetworkIdentity>();
            networkIdentity.localPlayerAuthority = true;

            GameObject model = CreateModel(body);

            GameObject gameObject = new GameObject("ModelBase");
            gameObject.transform.parent = body.transform;
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

            CharacterDirection characterDirection = body.GetComponent<CharacterDirection>();
            characterDirection.moveVector = Vector3.zero;
            characterDirection.targetTransform = gameObject.transform;
            characterDirection.overrideAnimatorForwardTransform = null;
            characterDirection.rootMotionAccumulator = null;
            characterDirection.modelAnimator = model.GetComponentInChildren<Animator>();
            characterDirection.driveFromRootRotation = false;
            characterDirection.turnSpeed = 720f;

            CharacterBody bodyComponent = body.GetComponent<CharacterBody>();
            bodyComponent.bodyIndex = BodyIndex.None;
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
            var stateMachine = body.GetComponent<EntityStateMachine>();
            stateMachine.mainStateType = new SerializableEntityStateType(typeof(States.UrsaMain));

            CharacterMotor characterMotor = body.GetComponent<CharacterMotor>();
            characterMotor.walkSpeedPenaltyCoefficient = 1f;
            characterMotor.characterDirection = characterDirection;
            characterMotor.muteWalkMotion = false;
            characterMotor.mass = 200f;
            characterMotor.airControl = 0.25f;
            characterMotor.disableAirControlUntilCollision = false;
            characterMotor.generateParametersOnAwake = true;
            //characterMotor.useGravity = true;
            //characterMotor.isFlying = false;

            InputBankTest inputBankTest = body.GetComponent<InputBankTest>();
            inputBankTest.moveVector = Vector3.zero;

            CameraTargetParams cameraTargetParams = body.GetComponent<CameraTargetParams>();
            cameraTargetParams.cameraParams = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<CameraTargetParams>().cameraParams;
            cameraTargetParams.cameraPivotTransform = null;
            cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
            cameraTargetParams.recoil = Vector2.zero;
            cameraTargetParams.idealLocalCameraPos = Vector3.zero;
            cameraTargetParams.dontRaycastToPivot = false;

            ModelLocator modelLocator = body.GetComponent<ModelLocator>();
            modelLocator.modelTransform = transform;
            modelLocator.modelBaseTransform = gameObject.transform;
            modelLocator.dontReleaseModelOnDeath = false;
            modelLocator.autoUpdateModelTransform = true;
            modelLocator.dontDetatchFromParent = false;
            modelLocator.noCorpse = false;
            modelLocator.normalizeToFloor = true;
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
            foreach (SkinnedMeshRenderer i in model.GetComponentsInChildren<SkinnedMeshRenderer>())
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
            };
            SkinDef skin = LoadoutAPI.CreateNewSkinDef(skinDefInfo);
            skinController.skins = new SkinDef[] { skin };

            TeamComponent teamComponent = null;
            if (body.GetComponent<TeamComponent>() != null) teamComponent = body.GetComponent<TeamComponent>();
            else teamComponent = body.GetComponent<TeamComponent>();
            teamComponent.hideAllyCardDisplay = false;
            teamComponent.teamIndex = TeamIndex.None;

            HealthComponent healthComponent = body.GetComponent<HealthComponent>();
            healthComponent.health = 90f;
            healthComponent.shield = 0f;
            healthComponent.barrier = 0f;
            healthComponent.magnetiCharge = 0f;
            healthComponent.body = null;
            healthComponent.dontShowHealthbar = false;
            healthComponent.globalDeathEventChanceCoefficient = 1f;

            body.GetComponent<Interactor>().maxInteractionDistance = 3f;
            body.GetComponent<InteractionDriver>().highlightInteractor = true;

            CharacterDeathBehavior characterDeathBehavior = body.GetComponent<CharacterDeathBehavior>();
            characterDeathBehavior.deathStateMachine = body.GetComponent<EntityStateMachine>();
            characterDeathBehavior.deathState = new SerializableEntityStateType(typeof(GenericCharacterDeath));

            SfxLocator sfxLocator = body.GetComponent<SfxLocator>();
            sfxLocator.deathSound = "Play_ui_player_death";
            sfxLocator.barkSound = "";
            sfxLocator.openSound = "";
            sfxLocator.landingSound = "Play_char_land";
            sfxLocator.fallDamageSound = "Play_char_land_fall_damage";
            sfxLocator.aliveLoopStart = "";
            sfxLocator.aliveLoopStop = "";

            Rigidbody rigidbody = body.GetComponent<Rigidbody>();
            rigidbody.mass = 100f;
            rigidbody.drag = 0f;
            rigidbody.angularDrag = 0f;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            rigidbody.interpolation = RigidbodyInterpolation.None;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.constraints = RigidbodyConstraints.None;

            CapsuleCollider capsuleCollider = body.GetComponent<CapsuleCollider>();
            capsuleCollider.isTrigger = false;
            capsuleCollider.material = null;
            capsuleCollider.center = new Vector3(0f, 0f, 0f);
            capsuleCollider.radius = 0.5f;
            capsuleCollider.height = 1.82f;
            capsuleCollider.direction = 1;

            KinematicCharacterMotor kinematicCharacterMotor = body.GetComponent<KinematicCharacterMotor>();
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

            // Adds component used for keeping track of individual Ursa's Fury Swipes damage bonus from Enrage.
            body.AddComponent<Miscellaneous.FurySwipesController>();

            // Particle System that only emits when Overpower buff is active.
            ParticleSystem.EmissionModule overpowerParticleSystemEmission = childLocator.FindChild("OverpowerParticles").GetComponent<ParticleSystem>().emission;
            overpowerParticleSystemEmission.enabled = false;

            //Doppelganger
            doppelgangerMaster = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterMasters/CommandoMonsterMaster"), "UrsaDoppelGanger", true);

            CharacterMaster component = doppelgangerMaster.GetComponent<CharacterMaster>();
            component.bodyPrefab = body;
        }

        public static void Initialize()
        {
            display = PrefabAPI.InstantiateClone(body.GetComponent<ModelLocator>().modelBaseTransform.gameObject, "UrsaDisplay", true);
            display.AddComponent<NetworkIdentity>();

            SurvivorDef survivorDef = ScriptableObject.CreateInstance<SurvivorDef>();
            survivorDef.cachedName = "URSA_NAME";
            survivorDef.descriptionToken = "URSA_DESCRIPTION";
            survivorDef.outroFlavorToken = "URSA_OUTRO_FLAVOR";
            survivorDef.primaryColor = Color.magenta;
            survivorDef.bodyPrefab = body;
            survivorDef.displayPrefab = display;

            SurvivorAPI.AddSurvivor(survivorDef);
            UrsaPlugin.characterMasters.Add(doppelgangerMaster);
        }

        private static GameObject CreateModel(GameObject main)
        {
            UnityEngine.Object.Destroy(main.transform.Find("ModelBase").gameObject);
            UnityEngine.Object.Destroy(main.transform.Find("CameraPivot").gameObject);
            UnityEngine.Object.Destroy(main.transform.Find("AimOrigin").gameObject);

            GameObject model = Core.Assets.mainAssetBundle.LoadAsset<GameObject>("mdlUrsa");

            return model;
        }
    }
}
