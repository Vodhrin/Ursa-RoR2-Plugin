using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using RoR2.Audio;
using R2API.Networking;
using R2API.Networking.Interfaces;
using Ursa.Core;
using EntityStates;

namespace Ursa.States
{
    class SharpClaws : BaseSkillState
    {
        public int attackSwitch;
        public static float baseDuration = 1f;
        public static float damageCoefficient = 1.5f;
        public static float procCoefficient = 1f;

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
                this.attackSwitch = 1;
                this.animator.SetBool("attack1", false);
                this.animator.SetBool("attack2", true);
                this.hitBoxGroup = base.FindHitBoxGroup("RightClaw");
            }
            else 
            {
                this.attackSwitch = 2;
                this.animator.SetBool("attack2", false);
                this.animator.SetBool("attack1", true);
                this.hitBoxGroup = base.FindHitBoxGroup("LeftClaw");
            }

            Attack();
            Util.PlayScaledSound(Core.Assets.ursaSwingSound, base.gameObject,  this.attackSpeedStat);
            base.PlayAnimation("Gesture, Override", "Attack", "Attack.playbackRate" ,this.duration);
            if (ClientScene.readyConnection != null) 
            { 
            PlayAnimationOnOtherClients(base.gameObject, "Gesture, Override", "Attack", this.duration);
            }

            if (base.characterBody)
            {
                if (NetworkServer.active)
                {
                    bool hasRemoved = false;
                    foreach(CharacterBody.TimedBuff i in base.characterBody.timedBuffs)
                    {
                        if (hasRemoved) break;
                        if (i.buffIndex == UrsaPlugin.overpowerBuff)
                        {
                            i.timer = 0;
                            hasRemoved = true;
                        }
                    }
                }
                else
                {
                    NetworkIdentity networkIdentity = base.gameObject.GetComponent<NetworkIdentity>();

                    if (networkIdentity)
                    {
                        new NetMessages.RemoveTimedBuffMessage(networkIdentity.netId, UrsaPlugin.overpowerBuff, 1)
                            .Send(NetworkDestination.Server);
                    }
                }
            }
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

        private void PlayAnimationOnOtherClients(GameObject gameObject, string layerName, string animationStateName, float duration)
        {
            NetworkIdentity networkIdentity = gameObject.GetComponent<NetworkIdentity>();
            if(networkIdentity)
            {
                new NetMessages.AnimationMessage(networkIdentity.netId, layerName, animationStateName, duration)
                    .Send(NetworkDestination.Clients);
            }
        }
    }
}
