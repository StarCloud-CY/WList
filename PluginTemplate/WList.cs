using TerrariaApi.Server;
using Terraria;
using TShockAPI;
using TShockAPI.Hooks;

namespace WList;


[ApiVersion(2, 1)]
public class WList : TerrariaPlugin
{
    public override string Name => "WList";

    public override Version Version => new Version(1, 0, 0);

    public override string Author => "StarryCloud";

    public override string Description => "一个测试白名单插件";

    public WList(Main game) : base(game) { }

    public static string config_path = TShock.SavePath + "/wlist.json";

    public static Config config = Config.Load(config_path);
    public override void Initialize()
    {
        Commands.ChatCommands.Add(new Command("wlist.admin", new CommandDelegate(OnCommand), new string[]
        {
            "wlist",
            "白名单"
        })
        { HelpText = "增删白名单，使用/wlist help了解更多" }
        );
        ServerApi.Hooks.ServerJoin.Register(this, OnPlayerJoin);
        ServerApi.Hooks.NetGreetPlayer.Register(this, OnGreetPlayer);
        GeneralHooks.ReloadEvent += args =>
        {
            config = Config.Load(config_path);
            args.Player.SendSuccessMessage("[WList]重载配置文件完成!");
        };
    }


    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ServerApi.Hooks.ServerJoin.Deregister(this, OnPlayerJoin);
            ServerApi.Hooks.NetGreetPlayer.Deregister(this, OnGreetPlayer);
        }
        base.Dispose(disposing);
    }

    private void OnCommand(CommandArgs args)
    {
        TSPlayer user = args.Player;
        if (args.Parameters.Count >= 1 && args.Parameters.Count <= 2)
        {
            switch (args.Parameters[0])
            {
                case "add":
                    if (args.Parameters.Count == 2)
                    {
                        string username = args.Parameters[1];
                        if (!config.WhiteList.ContainsKey(username))
                        {
                            config.WhiteList.Add(username, null);
                            Config.Write(config_path, config);
                            user.SendSuccessMessage($"添加{username}的白名单成功！");
                        }
                        else
                        {
                            user.SendErrorMessage($"玩家{username}已有白名单");
                        }
                    }
                    else
                    {
                        user.SendErrorMessage("格式错误！请使用/wlist help获取帮助");
                    }
                    break;
                case "del":
                    if (args.Parameters.Count == 2)
                    {
                        string username = args.Parameters[1];
                        if (config.WhiteList.ContainsKey(username))
                        {
                            config.WhiteList.Remove(username);
                            Config.Write(config_path, config);
                            user.SendSuccessMessage($"删除{username}的白名单成功！");
                        }
                        else
                        {
                            user.SendErrorMessage($"玩家{username}无白名单");
                        }
                    }
                    else
                    {
                        user.SendErrorMessage("格式错误！请使用/wlist help获取帮助");
                    }
                    break;
                case "unbind":
                    if (args.Parameters.Count == 2)
                    {
                        string username = args.Parameters[1];
                        if (config.WhiteList.ContainsKey(username))
                        {
                            config.WhiteList[username] = null;
                            Config.Write(config_path, config);
                            user.SendSuccessMessage($"解除{username}与其IP的绑定成功！");
                        }
                        else
                        {
                            user.SendErrorMessage($"玩家{username}不存在！");
                        }
                    }
                    else
                    {
                        user.SendErrorMessage("格式错误！请使用/wlist help获取帮助");
                    }
                    break;
                case "list":
                    int page;
                    if (args.Parameters.Count == 1)
                    {
                        page = 1;
                    }
                    else
                    {
                        try
                        {
                            page = Convert.ToInt32(args.Parameters[1]);
                        }
                        catch (InvalidCastException)
                        {
                            page = 1;
                        }
                    }
                    int max_page = (int)Math.Ceiling((double)config.WhiteList.Count / 10);
                    if (page > max_page)
                    {
                        page = max_page;
                    }
                    int start_index = (page - 1) * 10;
                    int end_index = page * 10 - 1;
                    int index = 0;
                    string back_info = $"白名单列表({page}/{max_page})\n";
                    foreach (var item in config.WhiteList)
                    {
                        if (index >= start_index && index < end_index)
                        {
                            back_info = back_info + $"玩家名:{item.Key}=>IP:{item.Value}\n";
                        }
                        else
                        {
                            if (index == end_index)
                            {
                                back_info = back_info + $"玩家名:{item.Key}=>IP:{item.Value}";
                            }
                        }
                        index++;
                    }
                    user.SendInfoMessage(back_info);
                    break;
                case "help":
                    user.SendInfoMessage("wlist 帮助\n/wlist add <玩家名> 添加白名单\n/wlist del <玩家名> 删除白名单\n/wlist list 显示白名单列表\n/wlist unbind <玩家名> 解绑玩家IP");
                    break;
                default:
                    user.SendErrorMessage("格式错误!请使用/wlist help获取帮助");
                    break;
            }
        }
        else
        {
            user.SendErrorMessage("格式错误!请使用/wlist help获取帮助");
        }
    }

    private void OnPlayerJoin(JoinEventArgs args)
    {
        TSPlayer player = new TSPlayer(args.Who);
        if (!config.WhiteList.ContainsKey(player.Name))
        {
            player.Disconnect("你不在白名单内！");
        }
        else
        {
            if (config.WhiteList[player.Name] != player.IP && config.WhiteList[player.Name] != null)
            {
                player.Disconnect("你的IP与之前的IP不同，请联系管理员解决");
                TShock.Log.ConsoleWarn($"玩家{player.Name}的IP改变");
            }
        }
    }
    private void OnGreetPlayer(GreetPlayerEventArgs args)
    {
        TSPlayer player = new TSPlayer(args.Who);
        if (config.WhiteList[player.Name] == null)
        {
            config.WhiteList[player.Name] = player.IP;
            Config.Write(config_path, config);
            player.SendSuccessMessage($"你的IP已绑定为=>{player.IP},如要解绑请联系管理员");
            TShock.Log.ConsoleInfo($"玩家{player.Name}的IP已绑定为{player.IP}");
        }
    }
}
