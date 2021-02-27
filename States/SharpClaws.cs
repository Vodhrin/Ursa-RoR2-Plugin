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
        public static float hopVelocity = Core.Config.primaryHopVelocity.Value;

        private bool hasFired;
        private float duration;
        private Animator animator;
        private HitBoxGroup hitBoxGroup;
        private bool hasHopped;
        private OverlapAttack attack;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();
            this.hasFired = false;

            if (base.characterBody.HasBuff(UrsaPlugin.overpowerBuff))
            {
                this.duration = SharpClaws.baseDuration / (this.attackSpeedStat * Overpower.attackSpeedMult);
            }
            else
            {
                this.duration = SharpClaws.baseDuration / this.attackSpeedStat;
            }

            StartAimMode(2);

            if (animator.GetBool("attackSwitch")) 
            {
                this.animator.SetBool("attackSwitch", false);
                this.hitBoxGroup = base.FindHitBoxGroup("LeftClaw");
            }
            else 
            {
                this.animator.SetBool("attackSwitch", true);
                this.hitBoxGroup = base.FindHitBoxGroup("RightClaw");
            }

            attack = new OverlapAttack 
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
            };

            Util.PlayScaledSound(Core.Assets.ursaSwingSound, base.gameObject,  this.attackSpeedStat);
            base.PlayAnimation("Gesture, Override", "Attack", "Attack.playbackRate" ,this.duration);
            if (ClientScene.readyConnection != null) 
            { 
            Core.Utils.PlayAnimationOnOtherClients(base.gameObject, "Gesture, Override", "Attack", this.duration, true);
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

            bool fireStarted = base.fixedAge >= this.duration * 0.2f;
            bool fireEnded = base.fixedAge >= this.duration * 0.5f;

            //to guarantee that if the attack speed is so fast that fixedage skipped past the firing duration a hitbox still comes out
            if ((fireStarted && !fireEnded) || (fireStarted && fireEnded && !this.hasFired))
            {
                this.hasFired = true;
                this.Attack();
            }                

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
                if (attack.Fire() && !base.isGrounded && !this.hasHopped)
                {
                    base.SmallHop(base.characterMotor, SharpClaws.hopVelocity);
                    this.hasHopped = true;
                }

                if (!this.hasFired) 
                {
                    //play swipe effect
                }
            }
        }
    }
}
