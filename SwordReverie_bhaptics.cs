using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using MelonLoader;
using HarmonyLib;
using MyBhapticsTactsuit;
using Il2Cpp;

[assembly: MelonInfo(typeof(SwordReverie_bhaptics.SwordReverie_bhaptics), "SwordReverie_bhaptics", "1.0.0", "Florian Fahrenberger")]
[assembly: MelonGame("IsekaiEntertainmentLLC", "SwordReverie")]

namespace SwordReverie_bhaptics
{
    public class SwordReverie_bhaptics : MelonMod
    {
        public static TactsuitVR tactsuitVr = null!;

        public override void OnInitializeMelon()
        {
            tactsuitVr = new TactsuitVR();
            tactsuitVr.PlaybackHaptics("HeartBeat");
        }
        
        
        [HarmonyPatch(typeof(BasicWeapon), "HitEnemy", new Type[] { typeof(Collider), typeof(Vector3), typeof(Vector3) })]
        public class bhaptics_HitEnemy
        {
            [HarmonyPostfix]
            public static void Postfix(BasicWeapon __instance)
            {
                //tactsuitVr.LOG("Hand: " + __instance.hand.ToString());
                bool isRightHand = (__instance.hand == 2);
                tactsuitVr.Recoil("Blade", isRightHand);
            }
        }

        [HarmonyPatch(typeof(AbilityPoseTriggerControl), "TriggerAbility", new Type[] { typeof(Ability), typeof(int), typeof(string), typeof(Transform) })]
        public class bhaptics_UseAbility
        {
            [HarmonyPostfix]
            public static void Postfix(AbilityPoseTriggerControl __instance, Ability ability, int hand)
            {
                if (ability == Ability.Shockwave)
                {
                    tactsuitVr.CastSpell("Fire", true);
                    tactsuitVr.CastSpell("Fire", false);
                }
                if (ability == Ability.Cleave)
                {
                    tactsuitVr.CastSpell("Fire", true);
                    tactsuitVr.CastSpell("Fire", false);
                }
            }
        }

        [HarmonyPatch(typeof(PlayerHealthandManaControl), "Update", new Type[] {  })]
        public class bhaptics_UpdateHealth
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerHealthandManaControl __instance)
            {
                if (__instance.CurrentHealth <= 0.2f * __instance.maxHealth) tactsuitVr.StartHeartBeat();
                else tactsuitVr.StopHeartBeat();
            }
        }

        [HarmonyPatch(typeof(PlayerHealthandManaControl), "GetDamage", new Type[] { typeof(int), typeof(EnemyClass), typeof(int), typeof(string) })]
        public class bhaptics_PlayerGetDamage
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerHealthandManaControl __instance, string damageType)
            {
                tactsuitVr.PlaybackHaptics("Impact");
            }
        }

        [HarmonyPatch(typeof(PlayerHealthandManaControl), "Dead", new Type[] {  })]
        public class bhaptics_PlayerDead
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopThreads();
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
                tactsuitVr.Recoil("Blade", isRightHand, 0.3f);

            }
        }
        
    }
}
