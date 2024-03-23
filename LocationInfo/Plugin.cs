using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using LocationInfo.Windows;

namespace LocationInfo
{
  public sealed class Plugin : IDalamudPlugin
  {
    public string Name => "Location Info";
    private const string CommandName = "/loc";
    [PluginService] public static DalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] public static IClientState ClientState { get; private set; } = null!;
    [PluginService] public static IFramework Framework { get; private set; } = null!;
    [PluginService] public static IDataManager DataManager { get; private set; } = null!;
    [PluginService] public static ITextureProvider TextureProvider { get; private set; } = null!;
    // Game Objects 
    [PluginService] public static IObjectTable Objects { get; private set; } = null!;
    [PluginService] public static IPluginLog Log { get; private set; } = null!;
    [PluginService] public static IChatGui Chat { get; private set; } = null!;

    private ICommandManager CommandManager { get; init; }
    public Configuration Configuration { get; init; }

    // Windows 
    public WindowSystem WindowSystem = new("LocationInfo");
    private MainWindow MainWindow { get; init; }

    public Plugin(
        [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
        [RequiredVersion("1.0")] ICommandManager commandManager)
    {
      PluginInterface = pluginInterface;
      this.CommandManager = commandManager;

      this.Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
      this.Configuration.Initialize(PluginInterface);

      MainWindow = new MainWindow(this);

      WindowSystem.AddWindow(MainWindow);

      this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand) { ShowInHelp = true, HelpMessage = "Open location info window to see player location" });

      PluginInterface.UiBuilder.Draw += DrawUI;
    }

    public void Dispose()
    {
      this.WindowSystem.RemoveAllWindows();

      MainWindow.Dispose();

      this.CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args)
    {
      // in response to the slash command, just display our main ui
      MainWindow.IsOpen = true;
    }

    private void DrawUI()
    {
      this.WindowSystem.Draw();
    }

  } // Plugin
}
