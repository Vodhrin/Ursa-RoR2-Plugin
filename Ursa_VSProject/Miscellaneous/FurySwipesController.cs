using UnityEngine;
using UnityEngine.Networking;
using RoR2;

namespace Ursa.Miscellaneous
{
    class FurySwipesController : NetworkBehaviour
    {
        public float furySwipesMult;

        private CharacterBody characterBody;
        private float baseMass;

        private void Start()
        {
            this.furySwipesMult = Core.Config.passiveBaseDamageMult.Value;
            this.characterBody = gameObject.GetComponent<CharacterBody>();
            this.baseMass = characterBody.rigidbody.mass;
        }

        private void FixedUpdate()
        {
            if (characterBody.HasBuff(Survivor.Buffs.enrageBuff))
            {
                this.furySwipesMult = Core.Config.passiveBaseDamageMult.Value * 2f;
                this.characterBody.rigidbody.mass = Core.Config.specialMassMult.Value * this.baseMass;
            }
            else
            {
                this.furySwipesMult = Core.Config.passiveBaseDamageMult.Value;
                this.characterBody.rigidbody.mass = this.baseMass;
            }
        }
    }
}
