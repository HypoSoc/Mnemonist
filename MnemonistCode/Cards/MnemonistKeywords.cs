using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Mnemonist.MnemonistCode.Cards;

public static class MnemonistKeywords
{
    [CustomEnum, KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Persistent;
    
    [CustomEnum, KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Createshumors;
    
    [CustomEnum, KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Psych;
    
    [CustomEnum, KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Psychplus;
    
    public static bool IsPersistent(this CardModel card)
    {
        return card.Keywords.Contains(Persistent);
    }
}