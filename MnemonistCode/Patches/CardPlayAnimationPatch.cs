using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;

namespace Mnemonist.MnemonistCode.Patches;

[HarmonyPatch(typeof(Hook), nameof(Hook.AfterCardPlayed))]
public static class CardPlayAnimationPatch
{
    [HarmonyPostfix]
    public static void Postfix(CombatState combatState,
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner?.Character is Character.Mnemonist character)
        {
            if (cardPlay.Card.Type == CardType.Attack)
            {
                character.PlayAnimation(cardPlay.Card.Owner.Creature, "attack");
            }
        }
    }
}