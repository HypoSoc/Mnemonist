using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Mnemonist.MnemonistCode.Cards.Special;
using Mnemonist.MnemonistCode.Character;

namespace Mnemonist.MnemonistCode.Relics;

[Pool(typeof(MnemonistRelicPool))]
public class CranialTrauma() : MnemonistRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Amnesia>(), HoverTipFactory.FromCard<Anamnesis>()];

    private readonly List<CardModel> _exhaustedCards = [];

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side || combatState.RoundNumber > 1)
            return;
        _exhaustedCards.Clear();
        Flash();
        await CardPileCmd.AddGeneratedCardToCombat(combatState.CreateCard<Amnesia>(Owner), PileType.Hand, true);
    }
    

    public async Task UseExhaust(PlayerChoiceContext choiceContext)
    {
        foreach (var card in PileType.Draw.GetPile(Owner).Cards.ToList())
        {
            await CardCmd.Exhaust(choiceContext, card);
            if (card.Pile?.Type == PileType.Exhaust)
                _exhaustedCards.Add(card);
        }
        foreach (var card in PileType.Discard.GetPile(Owner).Cards.ToList())
        {
            await CardCmd.Exhaust(choiceContext, card);
            if (card.Pile?.Type == PileType.Exhaust)
                _exhaustedCards.Add(card);
        }
    }

    public async Task UseUnexhaust()
    {
        await CardPileCmd.Add(_exhaustedCards.Where(c => c.Pile?.Type == PileType.Exhaust), PileType.Discard);
        _exhaustedCards.Clear();
    }
}