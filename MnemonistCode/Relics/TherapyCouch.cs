using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mnemonist.MnemonistCode.Character;

namespace Mnemonist.MnemonistCode.Relics;

[Pool(typeof(MnemonistRelicPool))]
public class TherapyCouch() : MnemonistRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(2m, ValueProp.Unpowered)];

    public override async Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        if (card.Owner != Owner || Owner.Creature.CombatState == null)
            return;
        if (card.Type != CardType.Status)
            return;
        Creature? target = Owner.RunState.Rng.CombatTargets.NextItem(Owner.Creature.CombatState.HittableEnemies);
        if (target is null)
            return;
        Flash();
        await CreatureCmd.Damage(choiceContext, target, DynamicVars.Damage, Owner.Creature);
    }
}