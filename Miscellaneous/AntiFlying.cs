using UnityEngine;
using UnityEngine.Networking;
using RoR2;

namespace Ursa.Miscellaneous
{
    //Most of the code here is from Rob's Paladin Torpor special skill

    class AntiFlying : NetworkBehaviour
    {
        private float pushForce;
        public CharacterBody body;
        private float mass;
        private Rigidbody rb;
        private CharacterMotor motor;
        private DamageInfo info;

        private void Start()
        {
            this.body = gameObject.GetComponent<CharacterBody>();

            if (this.body)
            {
                if (this.body.characterMotor)
                {
                    this.motor = this.body.characterMotor;
                    this.mass = this.motor.mass;
                }
                else if (this.body.rigidbody)
                {
                    this.rb = this.body.rigidbody;
                    this.mass = this.rb.mass;
                }

                if (this.mass < 50f) this.mass = 50f;
                this.pushForce = 20f * this.mass;

                this.info = new DamageInfo
                {
                    attacker = null,
                    inflictor = null,
                    damage = 1,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    crit = false,
                    dotIndex = DotController.DotIndex.None,
                    force = Vector3.down * this.pushForce * Time.fixedDeltaTime,
                    position = base.transform.position,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 0
                };
            }
        }

        private void Knockdown()
        {
            if (NetworkServer.active)
            {
                this.info = new DamageInfo
                {
                    attacker = null,
                    inflictor = null,
                    damage = 0,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    crit = false,
                    dotIndex = DotController.DotIndex.None,
                    force = Vector3.down * this.pushForce * Time.fixedDeltaTime,
                    position = base.transform.position,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 0
                };

                this.body.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;

                if (this.motor)
                {
                    this.body.healthComponent.TakeDamageForce(this.info);
                }
                else if (this.rb)
                {
                    this.body.healthComponent.TakeDamageForce(this.info);
                }
            }
        }

        private void FixedUpdate()
        {
            if (!this.body.HasBuff(UrsaPlugin.earthshockDebuff) || !this.body)
            {
                this.body.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
                Destroy(this);
                return;
            }

            Knockdown();
        }
    }
}
