using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Mnemonist.MnemonistCode.Cards.Uncommon;
using Mnemonist.MnemonistCode.Extensions;

namespace Mnemonist.MnemonistCode.Powers;

public class ForgottenFrustrationStrPower : TemporaryStrengthPower, ICustomPower
{
    public string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
    public string? CustomBigBetaIconPath => null;
    
    public override AbstractModel OriginModel => ModelDb.Card<ForgottenFrustration>();
    protected override bool IsPositive => true;
}