using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using RoR2.Networking;
using RoR2.Audio;
using R2API.Networking;
using R2API.Networking.Interfaces;
using Ursa.Core;
using EntityStates;

namespace Ursa.States
{

    //Lots of inspiration/ideas used from EnforcerGang's King Dedede utility skill on Nemesis Enforcer and Rob's
    //Torpor special skill on Paladin

    class Earthshock : BaseSkillState
    {
        public static float damageCoefficient = 1.5f;
        public static float procCoefficient = 0f;
        public static float force = 500f;
        public static float radius = 20f;
        public static float bonusMoveSpeed = 0.3f;
        public static float bonusJumpHeight = 0.25f;
        public static float buffDuration = 3f;
        public static float downwardForce = 65f;
        public static float debuffDuration = 5f;
        public static float debuffSlow = 0.5f;

        private GameObject effect;
        private bool airSlam;
        private bool airSlamActive;

        public override void OnEnter()
        {
            base.OnEnter();
            this.effect = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");

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
                        new NetMessages.BodyFlagsMessage(networkIdentity.netId, CharacterBody.BodyFlags.IgnoreFallDamage)
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
                        new NetMessages.RemoveBodyFlagsMessage(networkIdentity.netId, CharacterBody.BodyFlags.IgnoreFallDamage)
                            .Send(NetworkDestination.Server);
                    }
                }
            }

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
    
            if(!this.airSlam)
            {
                Slam();
                if(base.isAuthority) this.outer.SetNextStateToMain();
            }

            if(base.isAuthority && this.airSlam && !this.airSlamActive)
            {
                this.airSlamActive = true;
                base.characterMotor.disableAirControlUntilCollision = true;
                base.characterMotor.velocity -= new Vector3(0f, Earthshock.downwardForce, 0f);
            }

            if(this.airSlamActive && !base.characterMotor.disableAirControlUntilCollision && base.isGrounded)
            {
                Slam();
               if(base.isAuthority) this.outer.SetNextStateToMain();
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
                scale = 6f
            }, true);
            PlaySound(Assets.ursaEarthshockSound, base.gameObject);
            PlayCrossfade("Gesture, Override", "Smash", 0.1f);
            if (ClientScene.readyConnection != null)
            {
                PlayAnimationOnOtherClients(base.gameObject, "Gesture, Override", "Smash", 1f);
            }

            Explode();

            ApplyNetworkedTimedBuff(base.gameObject, UrsaPlugin.earthshockBuff, 1, Earthshock.buffDuration);

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
                foreach(Collider i in colliders)
                {
                    if (!i || !i.gameObject.GetComponent<CharacterBody>() || i.gameObject.GetComponent<TeamComponent>().teamIndex == base.GetTeam()) continue;
                    i.gameObject.GetComponent<CharacterBody>().AddTimedBuff(UrsaPlugin.earthshockDebuff, Earthshock.debuffDuration);
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
                        new NetMessages.TimedBuffMessage(networkIdentity.netId, UrsaPlugin.earthshockDebuff, 1, Earthshock.debuffDuration)
                            .Send(NetworkDestination.Server);
                    }
                }
            }
        }

        private void PlaySound(string soundName, GameObject gameObject, float rate = 1)
        {
            NetworkIdentity networkIdentity = gameObject.GetComponent<NetworkIdentity>();
            if (networkIdentity)
            {
                new NetMessages.SoundMessage(networkIdentity.netId, soundName)
                    .Send(NetworkDestination.Clients);
            }
        }

        private void PlayAnimationOnOtherClients(GameObject gameObject, string layerName, string animationStateName, float duration)
        {
            NetworkIdentity networkIdentity = gameObject.GetComponent<NetworkIdentity>();
            if (networkIdentity)
            {
                new NetMessages.AnimationMessage(networkIdentity.netId, layerName, animationStateName, duration)
                    .Send(NetworkDestination.Clients);
            }
        }

        private void ApplyNetworkedTimedBuff(GameObject gameObject, BuffIndex buffIndex, int stacks, float duration)
        {
            CharacterBody characterBody = gameObject.GetComponent<CharacterBody>();

            if (characterBody)
            {
                if (NetworkServer.active)
                {
                    for (int i = 0; i < stacks; i++)
                    {
                        characterBody.AddTimedBuff(buffIndex, duration);
                    }
                }
                else
                {
                    NetworkIdentity networkIdentity = gameObject.GetComponent<NetworkIdentity>();
                    if (networkIdentity)
                    {
                        new NetMessages.TimedBuffMessage(networkIdentity.netId, buffIndex, stacks, duration)
                            .Send(NetworkDestination.Server);
                    }
                }
            }
        }
    }
}
