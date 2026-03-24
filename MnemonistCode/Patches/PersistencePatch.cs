using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using Mnemonist.MnemonistCode.Cards;

namespace Mnemonist.MnemonistCode.Patches;

[HarmonyPatch(typeof(CardCmd), nameof(CardCmd.Exhaust))]
public class PersistencePatch
{
    [HarmonyPostfix]
    static Task UnexhaustPersistentCards(Task __result, CardModel card)
    {
        if (!card.IsPersistent())
            return __result;
        return Task.Run(async () =>
        {
            await __result;
            await CardPileCmd.Add(card, PileType.Draw, position: CardPilePosition.Random);
        });
    }
}