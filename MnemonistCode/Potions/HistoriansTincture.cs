using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Mnemonist.MnemonistCode.Potions;

public class HistoriansTincture: MnemonistPotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    protected override async Task OnUse(PlayerChoiceContext ctx, Creature? target)
    {
        CardModel? attack = PileType.Exhaust.GetPile(Owner).Cards.Where((c => c.Type == CardType.Attack)).ToList().UnstableShuffle(Owner.RunState.Rng.CombatCardSelection).FirstOrDefault();
        if (attack != null)
        {
            attack.SetToFreeThisTurn();
            await CardPileCmd.Add(attack, PileType.Hand);
        }
        CardModel? skill = PileType.Exhaust.GetPile(Owner).Cards.Where((c => c.Type == CardType.Skill)).ToList().UnstableShuffle(Owner.RunState.Rng.CombatCardSelection).FirstOrDefault();
        if (skill != null)
        {
            skill.SetToFreeThisTurn();
            await CardPileCmd.Add(skill, PileType.Hand);
        }
        CardModel? power = PileType.Exhaust.GetPile(Owner).Cards.Where((c => c.Type == CardType.Power)).ToList().UnstableShuffle(Owner.RunState.Rng.CombatCardSelection).FirstOrDefault();
        if (power != null)
        {
            power.SetToFreeThisTurn();
            await CardPileCmd.Add(power, PileType.Hand);
        }
    }
}