using BepInEx;
using UnityEngine;
using Utilla;

namespace TimeStop
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [ModdedGamemode]
    public class Plugin : BaseUnityPlugin
    {
        //bools
        public bool inAllowedRoom = false;
        public bool canFreeze;

        //float
        public bool activate;

        //vectors
        public Vector3 lastVel;
        public Vector3 lastAngVel;

        void Awake()
        {
        }

        void Update()
        {
            if (inAllowedRoom)
            {
                activate = ControllerInputPoller.instance.rightControllerPrimaryButton;

                if (activate)
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
