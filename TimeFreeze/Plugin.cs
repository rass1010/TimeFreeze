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
                    GorillaLocomotion.Player.Instance.transform.GetComponent<Rigidbody>().velocity = (GorillaTagger.Instance.offlineVRRig.transform.up * 0.073f) * GorillaLocomotion.Player.Instance.scale;
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
            inAllowedRoom = true;
        }

        [ModdedGamemodeLeave]
        private void RoomLeft(string gamemode)
        {
            inAllowedRoom = false;
            ResetFreeze();
        }


        void ResetFreeze()
        {
            if (!canFreeze)
            {
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity = lastVel;
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.angularVelocity = lastAngVel;
                canFreeze = true;
            }
        }
    }
}
