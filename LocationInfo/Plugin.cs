using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using LocationInfo.Windows;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;

namespace LocationInfo
{
  public sealed class Plugin : IDalamudPlugin
  {
    public string Name => "Location Info";
    private const string CommandName = "/loc";
    [PluginService] public static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] public static IClientState ClientState { get; private set; } = null!;
    [PluginService] public static IFramework Framework { get; private set; } = null!;
    [PluginService] public static IDataManager DataManager { get; private set; } = null!;
    [PluginService] public static ITextureProvider TextureProvider { get; private set; } = null!;
    // Game Objects 
    [PluginService] public static IObjectTable Objects { get; private set; } = null!;
    [PluginService] public static IPlayerState PlayerState { get; private set; } = null!;
    [PluginService] public static IPluginLog Log { get; private set; } = null!;
    [PluginService] public static IChatGui Chat { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;

    public Configuration Configuration { get; init; }

    // Windows 
    public WindowSystem WindowSystem = new("LocationInfo");
    private MainWindow MainWindow { get; init; }

    public Plugin()
    {
      this.Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
      this.Configuration.Initialize(PluginInterface);

      MainWindow = new MainWindow(this);

      WindowSystem.AddWindow(MainWindow);

      CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand) { ShowInHelp = true, HelpMessage = "Open location info window to see player location" });

      PluginInterface.UiBuilder.Draw += DrawUI;

      // This adds a button to the plugin installer entry of this plugin which allows
      // to toggle the display status of the configuration ui
      PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;

      // Adds another button that is doing the same but for the main ui of the plugin
      PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;
    }

    public void Dispose()
    {
      this.WindowSystem.RemoveAllWindows();

      MainWindow.Dispose();

      CommandManager.RemoveHandler(CommandName);
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

    public void ToggleConfigUI() => MainWindow.Toggle();
    public void ToggleMainUI() => MainWindow.Toggle();

  } // Plugin
}
