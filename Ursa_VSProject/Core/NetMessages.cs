using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using RoR2.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;


namespace Ursa.Core
{
    class NetMessages
    {

        public class TimedBuff : INetMessage
        {
            private NetworkInstanceId targetId;
            private BuffIndex buffIndex;
            private int stacks;
            private float duration;

            public TimedBuff()
            {
            }

            public TimedBuff(NetworkInstanceId targetId, BuffIndex buffIndex, int stacks, float duration)
            {
                this.targetId = targetId;
                this.buffIndex = buffIndex;
                this.stacks = stacks;
                this.duration = duration;
            }

            public void Serialize(NetworkWriter writer)
            {
                writer.Write(this.targetId);
                writer.WriteBuffIndex(this.buffIndex);
                writer.Write(this.stacks);
                writer.Write(this.duration);
            }

            public void Deserialize(NetworkReader reader) 
            {
                this.targetId = reader.ReadNetworkId();
                this.buffIndex = reader.ReadBuffIndex();
                this.stacks = reader.ReadInt32();
                this.duration = reader.ReadSingle();
            }

            public void OnReceived()
            {
                CharacterBody characterBody = Util.FindNetworkObject(this.targetId).GetComponent<CharacterBody>();

                if (!characterBody)
                {
                    UrsaPlugin.logger.LogMessage("Error applying timed buff: character body does not exist!");
                    return;
                }

                for (int i = 0; i < stacks; i++)
                {
                    characterBody.AddTimedBuff(buffIndex, duration);
                }
            }
        }

        public class RemoveTimedBuff : INetMessage
        {
            private NetworkInstanceId targetId;
            private BuffIndex buffIndex;
            private int stacks;

            public RemoveTimedBuff()
            {
            }

            public RemoveTimedBuff(NetworkInstanceId targetId, BuffIndex buffIndex, int stacks)
            {
                this.targetId = targetId;
                this.buffIndex = buffIndex;
                this.stacks = stacks;
            }

            public void Serialize(NetworkWriter writer)
            {
                writer.Write(this.targetId);
                writer.WriteBuffIndex(this.buffIndex);
                writer.Write(this.stacks);
            }

            public void Deserialize(NetworkReader reader)
            {
                this.targetId = reader.ReadNetworkId();
                this.buffIndex = reader.ReadBuffIndex();
                this.stacks = reader.ReadInt32();
            }

            public void OnReceived()
            {
                CharacterBody characterBody = Util.FindNetworkObject(this.targetId).GetComponent<CharacterBody>();

                if (!characterBody)
                {
                    UrsaPlugin.logger.LogMessage("Error removing buff: character body does not exist!");
                    return;
                }

                bool hasRemoved = false;
                int count = 0;
                foreach (CharacterBody.TimedBuff i in characterBody.timedBuffs)
                {
                    if (hasRemoved) break;
                    if (i.buffIndex == this.buffIndex)
                    {
                        i.timer = 0;
                        count++;
                        if (count == this.stacks) hasRemoved = true;
                    }
                }
            }
        }

        public class BodyFlags : INetMessage
        {
            private NetworkInstanceId targetId;
            private CharacterBody.BodyFlags bodyFlag;


            public BodyFlags()
            {
            }

            public BodyFlags(NetworkInstanceId targetId, CharacterBody.BodyFlags bodyFlag)
            {
                this.targetId = targetId;
                this.bodyFlag = bodyFlag;
            }

            public void Serialize(NetworkWriter writer)
            {
                writer.Write(this.targetId);
                writer.Write((byte)bodyFlag);
            }

            public void Deserialize(NetworkReader reader)
            {
                this.targetId = reader.ReadNetworkId();
                this.bodyFlag = (CharacterBody.BodyFlags)reader.ReadByte();
            }

            public void OnReceived()
            {
                CharacterBody characterBody = Util.FindNetworkObject(this.targetId).GetComponent<CharacterBody>();

                if (!characterBody)
                {
                    UrsaPlugin.logger.LogMessage("Error changing bodyflag: character body does not exist!");
                    return;
                }
                characterBody.bodyFlags |= this.bodyFlag;
            }
        }

        public class RemoveBodyFlags : INetMessage
        {
            private NetworkInstanceId targetId;
            private CharacterBody.BodyFlags bodyFlag;

            public RemoveBodyFlags()
            {
            }

            public RemoveBodyFlags(NetworkInstanceId targetId, CharacterBody.BodyFlags bodyFlag)
            {
                this.targetId = targetId;
                this.bodyFlag = bodyFlag;

            }

            public void Serialize(NetworkWriter writer)
            {
                writer.Write(this.targetId);
                writer.Write((byte)bodyFlag);

            }

            public void Deserialize(NetworkReader reader)
            {
                this.targetId = reader.ReadNetworkId();
                this.bodyFlag = (CharacterBody.BodyFlags)reader.ReadByte();

            }

            public void OnReceived()
            {
                CharacterBody characterBody = Util.FindNetworkObject(this.targetId).GetComponent<CharacterBody>();

                if (!characterBody)
                {
                    UrsaPlugin.logger.LogMessage("Error changing bodyflag: character body does not exist!");
                    return;
                }
                characterBody.bodyFlags &= ~this.bodyFlag;
            }
        }

        public class Sound : INetMessage
        {
            private NetworkInstanceId targetId;
            private string soundName;
            private float rate;

            public Sound()
            {
            }

            public Sound(NetworkInstanceId targetId, string soundName, float rate = 1f)
            {
                this.targetId = targetId;
                this.soundName = soundName;
                this.rate = rate;
            }

            public void Serialize(NetworkWriter writer)
            {
                writer.Write(this.targetId);
                writer.Write(this.soundName);
                writer.Write(this.rate);
            }

            public void Deserialize(NetworkReader reader)
            {
                this.targetId = reader.ReadNetworkId();
                this.soundName = reader.ReadString();
                this.rate = reader.ReadSingle();
            }

            public void OnReceived()
            {
                GameObject gameObject = Util.FindNetworkObject(this.targetId);
                if (!gameObject)
                {
                    UrsaPlugin.logger.LogMessage("Error syncing sounds: target gameobject does not exist!");
                    return;
                }

                Util.PlayScaledSound(soundName, gameObject, rate);
            }
        }

        public class Animation : INetMessage
        {
            private NetworkInstanceId targetId;
            private string layerName;
            private string animationStateName;
            private float duration;
            private bool isSharpClaws;

            public Animation()
            {
            }

            public Animation(NetworkInstanceId targetId, string layerName, string animationStateName, float duration, bool isSharpClaws = false)
            {
                this.targetId = targetId;
                this.layerName = layerName;
                this.animationStateName = animationStateName;
                this.duration = duration;
                this.isSharpClaws = isSharpClaws;
            }

            public void Serialize(NetworkWriter writer)
            {
                writer.Write(this.targetId);
                writer.Write(this.layerName);
                writer.Write(this.animationStateName);
                writer.Write(this.duration);
                writer.Write(this.isSharpClaws);
            }

            public void Deserialize(NetworkReader reader)
            {
                this.targetId = reader.ReadNetworkId();
                this.layerName = reader.ReadString();
                this.animationStateName = reader.ReadString();
                this.duration = reader.ReadSingle();
                this.isSharpClaws = reader.ReadBoolean();
            }

            public void OnReceived()
            {
                GameObject clientGameObject = LocalUserManager.GetFirstLocalUser().cachedBodyObject;

                GameObject gameObject = Util.FindNetworkObject(this.targetId);
                ModelLocator modelLocator = gameObject.GetComponent<ModelLocator>();
                Animator animator = modelLocator.modelTransform.GetComponent<Animator>();
                int layerIndex = animator.GetLayerIndex(this.layerName);

                if (!animator)
                {
                    UrsaPlugin.logger.LogMessage("Error syncing animations: target animator not found!");
                    return;
                }

                if(layerIndex == -1)
                {
                    return;
                }

                if(clientGameObject == gameObject)
                {
                    return;
                }

                animator.speed = 1f;
                animator.Update(0f);
                animator.PlayInFixedTime(this.animationStateName, layerIndex, 0f);

                if (!this.isSharpClaws)
                {
                    return;
                }

                bool currentAttack = animator.GetBool("attackSwitch");
                animator.SetBool("attackSwitch", !currentAttack);
            }
        }

        public class UrsaResize : INetMessage
        {
            private NetworkInstanceId targetId;
            private Vector3 targetScale;

            public UrsaResize()
            {
            }

            public UrsaResize(NetworkInstanceId targetId, Vector3 targetScale)
            {
                this.targetId = targetId;
                this.targetScale = targetScale;
            }

            public void Serialize(NetworkWriter writer)
            {
                writer.Write(this.targetId);
                writer.Write(this.targetScale);
            }

            public void Deserialize(NetworkReader reader)
            {
                this.targetId = reader.ReadNetworkId();
                this.targetScale = reader.ReadVector3();
            }

            public void OnReceived()
            {
                GameObject clientGameObject = LocalUserManager.GetFirstLocalUser().cachedBodyObject;
                GameObject gameObject = Util.FindNetworkObject(this.targetId).gameObject;
                ModelLocator modelLocator = gameObject.GetComponent<ModelLocator>();

                if (!gameObject)
                {
                    UrsaPlugin.logger.LogMessage("Error syncing Ursa size: target id does not exist!");
                    return;
                }
                if(gameObject == clientGameObject)
                {
                    return;
                }

                modelLocator.modelTransform.localScale = this.targetScale;
            }
        }

        public class UrsaHandsGlow : INetMessage
        {
            private NetworkInstanceId targetId;
            private bool glow;

            public UrsaHandsGlow()
            {
            }

            public UrsaHandsGlow(NetworkInstanceId targetId, bool glow)
            {
                this.targetId = targetId;
                this.glow = glow;
            }

            public void Serialize(NetworkWriter writer)
            {
                writer.Write(this.targetId);
                writer.Write(this.glow);
            }

            public void Deserialize(NetworkReader reader)
            {
                this.targetId = reader.ReadNetworkId();
                this.glow = reader.ReadBoolean();
            }

            public void OnReceived()
            {
                GameObject clientGameObject = LocalUserManager.GetFirstLocalUser().cachedBodyObject;
                GameObject gameObject = Util.FindNetworkObject(this.targetId);
                ModelLocator modelLocator = gameObject.GetComponent<ModelLocator>();
                ChildLocator childLocator = modelLocator.modelTransform.GetComponent<ChildLocator>();
                ParticleSystem.EmissionModule particleSystemEmission = childLocator.FindChild("OverpowerParticles").GetComponent<ParticleSystem>().emission;

                if (!gameObject)
                {
                    UrsaPlugin.logger.LogMessage("Error syncing Ursa's glowing hands effect: target id game object was not found!");
                    return;
                }
                if(clientGameObject == gameObject)
                {
                    return;
                }

                if (glow)
                {
                    childLocator.FindChild("R_Hand").GetComponent<Light>().enabled = true;
                    childLocator.FindChild("L_Hand").GetComponent<Light>().enabled = true;
                    particleSystemEmission.enabled = true;
                }
                else
                {
                    childLocator.FindChild("R_Hand").GetComponent<Light>().enabled = false;
                    childLocator.FindChild("L_Hand").GetComponent<Light>().enabled = false;
                    particleSystemEmission.enabled = false;
                }
            }
        }
    }
}
