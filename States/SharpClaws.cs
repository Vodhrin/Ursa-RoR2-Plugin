using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using RoR2.Audio;
using R2API.Networking;
using R2API.Networking.Interfaces;
using EntityStates;

namespace Ursa.States
{
    class SharpClaws : BaseSkillState
    {
        public static float baseDuration = Core.Config.primaryBaseAttackDuration.Value;
        public static float damageCoefficient = Core.Config.primaryDamageCoefficient.Value;
        public static float procCoefficient = Core.Config.primaryProcCoefficient.Value;

        private float duration;
        private Animator animator;
        private HitBoxGroup hitBoxGroup;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();

            if (base.characterBody.HasBuff(UrsaPlugin.overpowerBuff))
            {
                this.duration = SharpClaws.baseDuration / (this.attackSpeedStat * Overpower.attackSpeedMult);
            }
            else
            {
                this.duration = SharpClaws.baseDuration / this.attackSpeedStat;
            }

            base.characterDirection.forward = base.GetAimRay().direction;

            if (animator.GetBool("attack1")) {
                this.animator.SetBool("attack1", false);
                this.animator.SetBool("attack2", true);
                this.hitBoxGroup = base.FindHitBoxGroup("RightClaw");
            }
            else 
            {
                this.animator.SetBool("attack2", false);
                this.animator.SetBool("attack1", true);
                this.hitBoxGroup = base.FindHitBoxGroup("LeftClaw");
            }

            Attack();

            Util.PlayScaledSound(Core.Assets.ursaSwingSound, base.gameObject,  this.attackSpeedStat);
            base.PlayAnimation("Gesture, Override", "Attack", "Attack.playbackRate" ,this.duration);
            if (ClientScene.readyConnection != null) 
            { 
            Core.Utils.PlayAnimationOnOtherClients(base.gameObject, "Gesture, Override", "Attack", this.duration);
            }

            Core.Utils.RemoveNetworkedTimedBuff(base.gameObject, UrsaPlugin.overpowerBuff, 1);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if(base.fixedAge >= this.duration)
            {
                base.outer.SetNextStateToMain();
                return;
            }

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        private void Attack()
        {

            if (base.isAuthority)
            {
                new OverlapAttack
                {
                    attacker = base.gameObject,
                    inflictor = base.gameObject,
                    damage = base.damageStat * SharpClaws.damageCoefficient,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    isCrit = Util.CheckRoll(this.critStat, this.characterBody.master),
                    hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/ImpactImpSwipe"),
                    hitBoxGroup = hitBoxGroup,
                    impactSound = Core.Assets.ursaHitNetworkSoundEventDef.index,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = SharpClaws.procCoefficient,
                    forceVector = Vector3.zero,
                    pushAwayForce = 400f,
                    teamIndex = base.GetTeam(),
                }.Fire();
            }
        }
    }
}
