using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using Mnemonist.MnemonistCode.Cards.Ancient;
using Mnemonist.MnemonistCode.Cards.Basic;

namespace Mnemonist.MnemonistCode.Patches;

[HarmonyPatch(typeof(ArchaicTooth), "GetTranscendenceStarterCard")]
class StarterFindPatch
{
    [HarmonyPrefix]
    static bool StarterCardPatch(Player player, ref CardModel? __result)
    {
        if (player.Character is Character.Mnemonist)
        {
            __result = player.Deck.Cards.FirstOrDefault(c => c.Id == ModelDb.Card<NaggingThought>().Id);
            return __result == null;
        }
        return true;
    }
}

[HarmonyPatch(typeof(ArchaicTooth), "GetTranscendenceTransformedCard")]
class TranscendenceTrasformPatch
{
    [HarmonyPrefix]
    static bool AncientCardPatch(CardModel starterCard, ref CardModel? __result)
    {
        if (starterCard is NaggingThought)
        {
            CardModel canonicalCard = ModelDb.Card<ObsessiveThought>();
            CardModel card = starterCard.Owner.RunState.CreateCard(canonicalCard, starterCard.Owner);
            if (starterCard.IsUpgraded)
                CardCmd.Upgrade(card);
            if (starterCard.Enchantment != null)
            {
                EnchantmentModel enchantment = (EnchantmentModel) starterCard.Enchantment.MutableClone();
                CardCmd.Enchant(enchantment, card, (Decimal) enchantment.Amount);
            }
            __result = card;
            return false;
        }
        return true;
    }
}

[HarmonyPatch(typeof(DustyTome), nameof(DustyTome.SetupForPlayer))]
class DustyTomePatch
{
    [HarmonyPrefix]
    static bool AncientPowerPatch(DustyTome __instance, Player player)
    {
        if (player.Character is Character.Mnemonist)
        {
            __instance.AncientCard = ModelDb.Card<RecursiveMemory>().Id;
            return false;
        }
        return true;
    }
}

[HarmonyPatch(typeof(DustyTome), nameof(DustyTome.AfterObtained))]
class DustyTomeForceEvaluationPatch
{
    [HarmonyPrefix]
    static void ForceEvaluation(DustyTome __instance)
    {
        if (__instance.Owner.Character is Character.Mnemonist)
        {
            __instance.SetupForPlayer((__instance.Owner));
        }
    }
}