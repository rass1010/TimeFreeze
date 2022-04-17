using BepInEx;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;
using Utilla;
using UnityEngine.XR;

namespace TimeFreeze
{
    [Description("HauntedModMenu")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [ModdedGamemode]
    public class Plugin : BaseUnityPlugin
    {
        //bools
        public bool inAllowedRoom = false;
        public bool hauntedModMenuEnabled = true;
        public bool canFreeze;

        //float
        public float triggerpressed;

        //vectors
        public Vector3 lastVel;
        public Vector3 lastAngVel;

        void Awake()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        void Update()
        {
            if(inAllowedRoom && hauntedModMenuEnabled)
            {
                List<InputDevice> list = new List<InputDevice>();
                InputDevices.GetDevices(list);

                for (int i = 0; i < list.Count; i++) //Get input
                {
                    if (list[i].characteristics.HasFlag(InputDeviceCharacteristics.Right))
                    {
                        list[i].TryGetFeatureValue(CommonUsages.trigger, out triggerpressed);
                    }
                }

                if(triggerpressed > 0.1f)
                {
                    if (canFreeze)
                    {
                        lastVel = GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity;
                        lastAngVel = GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.angularVelocity;
                        canFreeze = false;
                    }

                    GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity = Vector3.zero;
                    GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.angularVelocity = Vector3.zero;
                    GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.useGravity = false;
                }
                else
                {
                    ResetFreeze();
                }

            }
        }

        [ModdedGamemodeJoin]
        private void RoomJoined(string gamemode)
        {
            // The room is modded. Enable mod stuff.


            inAllowedRoom = true;

        }

        [ModdedGamemodeLeave]
        private void RoomLeft(string gamemode)
        {
            // The room was left. Disable mod stuff.
            inAllowedRoom = false;
            ResetFreeze();
        }

        void OnEnable()
        {
            hauntedModMenuEnabled = true;
            
        }

        void OnDisable()
        {
            hauntedModMenuEnabled = false;
            ResetFreeze();
        }


        void ResetFreeze()
        {
            if (!canFreeze)
            {
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.useGravity = true;
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity = lastVel;
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.angularVelocity = lastAngVel;
                canFreeze = true;
            }
        }


    }

    
}
