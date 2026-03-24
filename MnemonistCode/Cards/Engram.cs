using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using Mnemonist.MnemonistCode.Extensions;
using Mnemonist.MnemonistCode.Powers;

namespace Mnemonist.MnemonistCode.Cards;

[Pool(typeof(TokenCardPool))]
public class Engram() : CustomCardModel(-1, CardType.Status, CardRarity.Status, TargetType.None)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.ToLowerInvariant()}.png".CardImagePath();

    public override int MaxUpgradeLevel => 0;
    
    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Unplayable,
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Memory>()];
    
    public override async Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        if (card != this)
            return;

        await CardCmd.Exhaust(choiceContext, this);
        await PowerCmd.Apply<Memory>(this.Owner.Creature, 1, this.Owner.Creature, (CardModel) this, true);
        await CardPileCmd.Draw(choiceContext, 1, this.Owner);
    }
    
    public static IEnumerable<Engram> Create(Player owner, int amount, CombatState combatState)
    {
        List<Engram> engramList = new List<Engram>();
        for (int index = 0; index < amount; ++index)
            engramList.Add(combatState.CreateCard<Engram>(owner));
        return engramList;
    }
}