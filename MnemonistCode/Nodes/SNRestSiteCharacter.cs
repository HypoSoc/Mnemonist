using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.RestSite;

namespace Mnemonist.MnemonistCode.Nodes;

[GlobalClass]
public partial class SNRestSiteCharacter : NRestSiteCharacter
{
    public override void _Ready()
    {
        Log.Info("Initializing rest site visuals");
        base._Ready();
    }
}