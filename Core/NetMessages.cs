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
    internal class NetMessages
    {

        public class TimedBuffMessage : INetMessage
        {
            private NetworkInstanceId targetId;
            private BuffIndex buffIndex;
            private int stacks;
            private float duration;

            public TimedBuffMessage()
            {
            }

            public TimedBuffMessage(NetworkInstanceId targetId, BuffIndex buffIndex, int stacks, float duration)
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

        public class RemoveTimedBuffMessage : INetMessage
        {
            private NetworkInstanceId targetId;
            private BuffIndex buffIndex;
            private int stacks;

            public RemoveTimedBuffMessage()
            {
            }

            public RemoveTimedBuffMessage(NetworkInstanceId targetId, BuffIndex buffIndex, int stacks)
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

        public class BodyFlagsMessage : INetMessage
        {
            private NetworkInstanceId targetId;
            private CharacterBody.BodyFlags bodyFlag;


            public BodyFlagsMessage()
            {
            }

            public BodyFlagsMessage(NetworkInstanceId targetId, CharacterBody.BodyFlags bodyFlag)
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
                UrsaPlugin.logger.LogMessage("Applying bodyflag");
                characterBody.bodyFlags |= this.bodyFlag;
            }
        }

        public class RemoveBodyFlagsMessage : INetMessage
        {
            private NetworkInstanceId targetId;
            private CharacterBody.BodyFlags bodyFlag;

            public RemoveBodyFlagsMessage()
            {
            }

            public RemoveBodyFlagsMessage(NetworkInstanceId targetId, CharacterBody.BodyFlags bodyFlag)
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

                UrsaPlugin.logger.LogMessage("Removing bodyflag");
                characterBody.bodyFlags &= ~this.bodyFlag;

            }
        }

        public class SoundMessage : INetMessage
        {
            private NetworkInstanceId targetId;
            private string soundName;
            private float rate;

            public SoundMessage()
            {
            }

            public SoundMessage(NetworkInstanceId targetId, string soundName, float rate = 1f)
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

        public class AnimationMessage : INetMessage
        {
            private NetworkInstanceId targetId;
            private string layerName;
            private string animationStateName;
            private float duration;

            public AnimationMessage()
            {
            }

            public AnimationMessage(NetworkInstanceId targetId, string layerName, string animationStateName, float duration)
            {
                this.targetId = targetId;
                this.layerName = layerName;
                this.animationStateName = animationStateName;
                this.duration = duration;
            }

            public void Serialize(NetworkWriter writer)
            {
                writer.Write(this.targetId);
                writer.Write(this.layerName);
                writer.Write(this.animationStateName);
                writer.Write(this.duration);
            }

            public void Deserialize(NetworkReader reader)
            {
                this.targetId = reader.ReadNetworkId();
                this.layerName = reader.ReadString();
                this.animationStateName = reader.ReadString();
                this.duration = reader.ReadSingle();
            }

            public void OnReceived()
            {
                GameObject clientGameObject = LocalUserManager.GetFirstLocalUser().cachedBodyObject;

                GameObject gameObject = Util.FindNetworkObject(this.targetId);
                ModelLocator modelLocator = gameObject.GetComponent<ModelLocator>();
                Animator animator = modelLocator.modelTransform.GetComponent<Animator>();

                if (!animator)
                {
                    UrsaPlugin.logger.LogMessage("Error syncing animations: target animator not found!");
                    return;
                }

                if(clientGameObject == gameObject)
                {
                    return;
                }

                int layerIndex = animator.GetLayerIndex(this.layerName);
                animator.speed = 1f;
                animator.Update(0f);
                animator.PlayInFixedTime(this.animationStateName, layerIndex, 0);
            }
        }

        public class UrsaResizeMessage : INetMessage
        {
            private NetworkInstanceId targetId;
            private Vector3 targetScale;

            public UrsaResizeMessage()
            {
            }

            public UrsaResizeMessage(NetworkInstanceId targetId, Vector3 targetScale)
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

        public class UrsaHandsGlowMessage : INetMessage
        {
            private NetworkInstanceId targetId;
            private bool glow;

            public UrsaHandsGlowMessage()
            {
            }

            public UrsaHandsGlowMessage(NetworkInstanceId targetId, bool glow)
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
                }
                else
                {
                    childLocator.FindChild("R_Hand").GetComponent<Light>().enabled = false;
                    childLocator.FindChild("L_Hand").GetComponent<Light>().enabled = false;
                }
            }
        }
    }
}
