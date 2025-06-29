using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using SimpleDPSMeter.Windows;
using SimpleDPSMeter.Data;

namespace SimpleDPSMeter;

public sealed class Plugin : IDalamudPlugin
{
    public string Name => "SimpleDPSMeter";
    private const string CommandName = "/dpsmeter";

    private DalamudPluginInterface PluginInterface { get; init; }
    private ICommandManager CommandManager { get; init; }
    private IFramework Framework { get; init; }
    private IClientState ClientState { get; init; }
    private IObjectTable ObjectTable { get; init; }
    private IChatGui ChatGui { get; init; }
    private IGameGui GameGui { get; init; }

    public Configuration Configuration { get; init; }
    public WindowSystem WindowSystem = new("SimpleDPSMeter");
    
    private ConfigWindow ConfigWindow { get; init; }
    private MainWindow MainWindow { get; init; }
    
    public CombatDataManager CombatDataManager { get; init; }
    
    public Plugin(
        [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
        [RequiredVersion("1.0")] ICommandManager commandManager,
        [RequiredVersion("1.0")] IFramework framework,
        [RequiredVersion("1.0")] IClientState clientState,
        [RequiredVersion("1.0")] IObjectTable objectTable,
        [RequiredVersion("1.0")] IChatGui chatGui,
        [RequiredVersion("1.0")] IGameGui gameGui)
    {
        PluginInterface = pluginInterface;
        CommandManager = commandManager;
        Framework = framework;
        ClientState = clientState;
        ObjectTable = objectTable;
        ChatGui = chatGui;
        GameGui = gameGui;

        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Configuration.Initialize(PluginInterface);

        CombatDataManager = new CombatDataManager(this);
        
        ConfigWindow = new ConfigWindow(this);
        MainWindow = new MainWindow(this);
        
        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(MainWindow);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Toggle the DPS meter window"
        });

        PluginInterface.UiBuilder.Draw += DrawUI;
        PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        
        Framework.Update += CombatDataManager.OnFrameworkUpdate;
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();
        
        ConfigWindow.Dispose();
        MainWindow.Dispose();
        
        CommandManager.RemoveHandler(CommandName);
        
        Framework.Update -= CombatDataManager.OnFrameworkUpdate;
        CombatDataManager.Dispose();
    }

    private void OnCommand(string command, string args)
    {
        MainWindow.IsOpen = !MainWindow.IsOpen;
    }

    private void DrawUI()
    {
        WindowSystem.Draw();
    }

    public void DrawConfigUI()
    {
        ConfigWindow.IsOpen = true;
    }
}