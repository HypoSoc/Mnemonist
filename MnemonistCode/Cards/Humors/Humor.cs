using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using Mnemonist.MnemonistCode.Extensions;

namespace Mnemonist.MnemonistCode.Cards.Humors;

[Pool(typeof(TokenCardPool))]
public abstract class Humor(CardType type, TargetType target) : CustomCardModel(0, type, CardRarity.Token, target)
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
    
    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust,
    ];

    private static readonly List<Humor> CanonicalHumors =
        [ModelDb.Card<Choleric>(), ModelDb.Card<Melancholic>(), ModelDb.Card<Phlegmatic>(), ModelDb.Card<Sanguine>()];
    
    public static IEnumerable<CardModel> CreateRandom(Player owner, int amount, CombatState combatState, bool isUpgraded = false)
    {
        List<CardModel> humorList = new List<CardModel>();
        for (var index = 0; index < amount; ++index)
        {
            var choice = owner.RunState.Rng.CombatCardSelection.NextInt(4);
            var card = combatState.CreateCard(CanonicalHumors[choice], owner);
            if (isUpgraded)
                CardCmd.Upgrade(card);
            humorList.Add(card);
        }
        return humorList;
    }
    
    public static IEnumerable<Humor> Create<T>(Player owner, int amount, CombatState combatState, bool isUpgraded = false) where T : Humor
    {
        List<Humor> humorList = new List<Humor>();
        for (var index = 0; index < amount; ++index)
        {
            var card = combatState.CreateCard<T>(owner);
            if (isUpgraded)
                CardCmd.Upgrade(card);
            humorList.Add(card);
        }
        return humorList;
    }
}