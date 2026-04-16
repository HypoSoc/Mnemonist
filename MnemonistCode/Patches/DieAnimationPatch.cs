using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;

namespace Mnemonist.MnemonistCode.Patches;

[HarmonyPatch(typeof(Creature), nameof(Creature.InvokeDiedEvent))]
public static class DieAnimationPatch
{
    [HarmonyPrefix]
    public static void Prefix(Creature __instance)
    {
        if (__instance.Player?.Character is Character.Mnemonist character)
        {
            character.PlayAnimation(__instance, "dead");
        }
    }
}