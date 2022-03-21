using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using MelonLoader;
using HarmonyLib;
using MyBhapticsTactsuit;

namespace SwordReverie_bhaptics
{
    public class SwordReverie_bhaptics : MelonMod
    {
        public static TactsuitVR tactsuitVr;

        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            tactsuitVr = new TactsuitVR();
            tactsuitVr.PlaybackHaptics("HeartBeat");
        }
        
        /*
        [HarmonyPatch(typeof(BasicWeapon), "HitEnemy", new Type[] { typeof(Collider), typeof(Vector3), typeof(Vector3) })]
        public class bhaptics_HitEnemy
        {
            [HarmonyPostfix]
            public static void Postfix(BasicWeapon __instance)
            {
                tactsuitVr.LOG("Hand: " + __instance.hand.ToString());
                bool isRightHand = (__instance.hand == 1);
                tactsuitVr.Recoil("Blade", isRightHand);
            }
        }
        
        [HarmonyPatch(typeof(BasicWeapon), "DealyedShockwave", new Type[] { typeof(HitProperties), typeof(float), typeof(int) })]
        public class bhaptics_Shockwave
        {
            [HarmonyPostfix]
            public static void Postfix(BasicWeapon __instance)
            {
                //tactsuitVr.LOG("Hand: " + __instance.hand.ToString());
                //bool isRightHand = (__instance.hand == 1);
                tactsuitVr.CastSpell("Fire", true);
                tactsuitVr.CastSpell("Fire", false);
            }
        }
        */

        [HarmonyPatch(typeof(PlayerHealth), "Update", new Type[] {  })]
        public class bhaptics_HealthUpdate
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerHealth __instance)
            {
                if (__instance.currentHealth <= 0.2f * __instance.startingHealth) tactsuitVr.StartHeartBeat();
                else tactsuitVr.StopHeartBeat();
            }
        }

        [HarmonyPatch(typeof(ShifuAbilityController), "UseShockwave", new Type[] { })]
        public class bhaptics_UseShockwave
        {
            [HarmonyPostfix]
            public static void Postfix(ShifuAbilityController __instance)
            {
                tactsuitVr.CastSpell("Fire", true);
                tactsuitVr.CastSpell("Fire", false);
            }
        }

        [HarmonyPatch(typeof(PlayerHealth), "DamagePlayer", new Type[] { typeof(float) })]
        public class bhaptics_DamagePlayer
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerHealth __instance)
            {
                tactsuitVr.LOG("Damage");
                if (__instance.currentHealth <= 0.2f * __instance.startingHealth) tactsuitVr.StartHeartBeat();
                else tactsuitVr.StopHeartBeat();
                tactsuitVr.PlaybackHaptics("Impact");
            }
        }

        /*
        [HarmonyPatch(typeof(PlayerWeaponCollider), "OnTriggerEnter", new Type[] { typeof(Collider) })]
        public class bhaptics_WeaponCollider
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerWeaponCollider __instance)
            {
                tactsuitVr.LOG("Collider");
                bool isRightHand = (__instance.weapon.hand == 2);
                tactsuitVr.Recoil("Blade", isRightHand);

            }
        }
        */
        [HarmonyPatch(typeof(PlayerWeaponColliderNew), "OnTriggerEnter", new Type[] { typeof(Collider) })]
        public class bhaptics_WeaponColliderNew
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerWeaponColliderNew __instance)
            {
                //tactsuitVr.LOG("ColliderNew");
                // 1 for left, 2 for right
                bool isRightHand = (__instance.weapon.hand == 2);
                tactsuitVr.Recoil("Blade", isRightHand);

            }
        }
    }
}
