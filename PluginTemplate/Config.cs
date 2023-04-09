using Newtonsoft.Json;
using System;
using System.Drawing.Text;
using TShockAPI;

namespace WList
{
    public class PlayerConfig
    {
        public string? IP { get; set; }
    }
    public class Lang
    {
        [JsonProperty("描述")]
        public string Describe = "这些参数是向REST处理以及玩家显示的内容";
        [JsonProperty("成功添加白名单")]
        public string SuccessRestAdd = "添加白名单完成";
        [JsonProperty("成功删除白名单")]
        public string SuccessRestDel = "删除白名单完成";
        [JsonProperty("成功解绑白名单")]
        public string SuccessRestUnbind = "解绑白名单完成";
        [JsonProperty("没有添加白名单")]
        public string NotWhitelist = "你不在白名单内!";
        [JsonProperty("缺少参数")]
        public string ErrorArgs = "缺少参数";
        [JsonProperty("没有玩家")]
        public string HasNotPlayer = "找不到玩家";
        [JsonProperty("绑定不一致")]
        public string NotMatch = "你的IP与绑定的IP不同,联系管理解决";
    }
    public class Config
    {
        [JsonProperty("白名单列表")]
        public Dictionary<string, PlayerConfig> WhiteList = new Dictionary<string, PlayerConfig>();
        [JsonProperty("返回信息")]
        public Lang lang = new Lang();
        public static Config Load(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    var json = JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
                    if (json != null)
                    {
                        return json;
                    }
                    else
                    {
                        TShock.Log.ConsoleError("[WList]读配置文件时出现错误");
                        return new Config();
                    }
                }
                else
                {
                    Write(path, new Config());
                    return new Config();
                }
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleError("[WList]读配置文件时出现错误" + ex.ToString());
                return new Config();
            }
        }
        public static void Write(string path, Config cfg)
        {
            try
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(cfg, Formatting.Indented));
            }
            catch (Exception ex)
            {
                TShock.Log.Error("[WList]写配置文件时出现错误" + ex.ToString());
            }
        }
    }
}
