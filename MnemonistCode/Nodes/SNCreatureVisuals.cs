using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace Mnemonist.MnemonistCode.Nodes;

[GlobalClass]
public partial class SNCreatureVisuals : NCreatureVisuals
{
    public override void _Ready()
    {
        Log.Info("Initializing creature visuals");
        base._Ready();
    }
}