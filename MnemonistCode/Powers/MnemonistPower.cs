using BaseLib.Abstracts;
using BaseLib.Extensions;
using Mnemonist.MnemonistCode.Extensions;

namespace Mnemonist.MnemonistCode.Powers;

public abstract class MnemonistPower : CustomPowerModel
{
    //Loads from Mnemonist/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}