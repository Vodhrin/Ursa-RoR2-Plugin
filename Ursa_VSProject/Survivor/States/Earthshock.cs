using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using RoR2.Networking;
using RoR2.Audio;
using R2API.Networking;
using R2API.Networking.Interfaces;
using EntityStates;

namespace Ursa.States
{

    //Lots of inspiration/ideas used from EnforcerGang's King Dedede utility skill on Nemesis Enforcer and Rob's
    //Torpor special skill on Paladin

    class Earthshock : BaseSkillState
    {
        public static float damageCoefficient = Core.Config.utilityDamageCoefficient.Value;
        public static float procCoefficient = Core.Config.utilityProcCoefficient.Value;
        public static float force = Core.Config.utilityForce.Value;
        public static float radius = Core.Config.utilityRadius.Value;
        public static float buffDuration = Core.Config.utilityBuffDuration.Value;
        public static float downwardForce = 65f;
        public static float debuffDuration = Core.Config.utilityDebuffDuration.Value;
        public static float debuffSlow = Core.Config.utilityDebuffSlow.Value;

        private GameObject effect;
        private bool airSlam;
        private bool airSlamActive;

        public override void OnEnter()
        {
            base.OnEnter();
            this.effect = Core.Assets.earthshockEffect;

            this.airSlamActive = false;

            if (base.isGrounded)
            {
                this.airSlam = false;
            }
            else
            {
                this.airSlam = true;
            }

            if (base.characterBody)
            {
                if (NetworkServer.active)
                {
                    base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
                }
                else
                {
                    NetworkIdentity networkIdentity = base.gameObject.GetComponent<NetworkIdentity>();
                    if (networkIdentity)
                    {
                        new Core.NetMessages.BodyFlags(networkIdentity.netId, CharacterBody.BodyFlags.IgnoreFallDamage)
                            .Send(NetworkDestination.Server);
                    }
                }
            }

        }

        public override void OnExit()
        {
            if (base.characterBody)
            {
                if (NetworkServer.active)
                {
                    base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
                }
                else
                {
                    NetworkIdentity networkIdentity = base.gameObject.GetComponent<NetworkIdentity>();
                    if (networkIdentity)
                    {
                        new Core.NetMessages.RemoveBodyFlags(networkIdentity.netId, CharacterBody.BodyFlags.IgnoreFallDamage)
                            .Send(NetworkDestination.Server);
                    }
                }
            }

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!this.airSlam)
            {
                Slam();
                if (base.isAuthority) this.outer.SetNextStateToMain();
            }

            if (base.isAuthority && this.airSlam && !this.airSlamActive)
            {
                this.airSlamActive = true;
                base.characterMotor.disableAirControlUntilCollision = true;
                base.characterMotor.velocity -= new Vector3(0f, Earthshock.downwardForce, 0f);
            }

            if (this.airSlamActive && !base.characterMotor.disableAirControlUntilCollision && base.isGrounded)
            {
                Slam();
                if (base.isAuthority) this.outer.SetNextStateToMain();
            }

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        private void Slam()
        {
            EffectManager.SpawnEffect(effect, new EffectData
            {
                origin = base.characterBody.footPosition + new Vector3(0, 0.15f, 0),
                rotation = base.transform.rotation,
                scale = 100f
            }, true);
            Core.Utils.PlayNetworkedSound(Core.Assets.EarthshockSound, base.gameObject);
            PlayCrossfade("Gesture, Override", "Smash", 0.1f);
            if (ClientScene.readyConnection != null)
            {
                Core.Utils.PlayAnimationOnOtherClients(base.gameObject, "Gesture, Override", "Smash", 1f);
            }

            Explode();

            Core.Utils.ApplyNetworkedTimedBuff(base.gameObject, Survivor.Buffs.earthshockBuff, 1, Earthshock.buffDuration);

        }

        private void Explode()
        {
            if (base.isAuthority)
            {

                new BlastAttack
                {
                    attacker = base.gameObject,
                    inflictor = base.gameObject,
                    teamIndex = base.GetTeam(),
                    baseDamage = this.damageStat * Earthshock.damageCoefficient,
                    baseForce = Earthshock.force,
                    procCoefficient = Earthshock.procCoefficient,
                    damageType = DamageType.Generic,
                    position = base.transform.position,
                    radius = Earthshock.radius,
                    crit = Util.CheckRoll(this.critStat, base.characterBody.master),
                    falloffModel = BlastAttack.FalloffModel.Linear,
                    attackerFiltering = AttackerFiltering.NeverHit
                }.Fire();
            }

            IEnumerable<Collider> colliders = Physics.OverlapCapsule(base.transform.position, base.transform.position + new Vector3(0f, Earthshock.radius * 2f, 0f), Earthshock.radius);

            if (NetworkServer.active)
            {
                foreach (Collider i in colliders)
                {
                    if (!i || !i.gameObject.GetComponent<CharacterBody>() || i.gameObject.GetComponent<TeamComponent>().teamIndex == base.GetTeam()) continue;
                    i.gameObject.GetComponent<CharacterBody>().AddTimedBuff(Survivor.Buffs.earthshockDebuff, Earthshock.debuffDuration);
                }
            }
            else
            {
                foreach (Collider i in colliders)
                {
                    if (!i || !i.gameObject.GetComponent<CharacterBody>() || i.gameObject.GetComponent<TeamComponent>().teamIndex == base.GetTeam()) continue;
                    NetworkIdentity networkIdentity = i.gameObject.GetComponent<NetworkIdentity>();
                    if (networkIdentity)
                    {
                        new Core.NetMessages.TimedBuff(networkIdentity.netId, Survivor.Buffs.earthshockDebuff, 1, Earthshock.debuffDuration)
                            .Send(NetworkDestination.Server);
                    }
                }
            }
        }
    }   
}
