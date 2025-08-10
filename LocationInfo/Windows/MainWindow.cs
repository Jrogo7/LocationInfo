using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;

namespace LocationInfo.Windows;

public class MainWindow : Window, IDisposable
{
  public MainWindow(Plugin plugin) : base(
      "Location", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoResize)
  {
    this.SizeConstraints = new WindowSizeConstraints
    {
      MinimumSize = new Vector2(120, 120),
      MaximumSize = new Vector2(120, 120)
    };
  }

  public void Dispose()
  {
  }

  public override void Draw()
  {
    var localPlayer = Plugin.ClientState.LocalPlayer;
    if (localPlayer != null) {
      ImGui.Text("Player Position");
      ImGui.Text("  x: " + localPlayer.Position.X);
      ImGui.Text("  y: " + localPlayer.Position.Y);
      ImGui.Text("  z: " + localPlayer.Position.Z);
    }
  }
}
