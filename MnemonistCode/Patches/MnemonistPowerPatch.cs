using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Logging;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Patches;

[HarmonyPatch(typeof(Creature), nameof(Creature.AfterAddedToRoom))]
public class MnemonistPowerPatch
{
    [HarmonyPrefix]
    static void ApplyMnemonistPower(Creature __instance)
    {
        if (__instance.Player == null)
            return;
        if (__instance.Player.Character is not Character.Mnemonist)
            return;
        Task.Run(async () =>
        {
            await PowerCmd.Apply<MnemonicWall>(__instance, 1, __instance, null, true);
        });
    }
}