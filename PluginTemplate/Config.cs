using Newtonsoft.Json;
using System;
using System.Drawing.Text;
using TShockAPI;

namespace WList
{
    public class Config
    {
        public Dictionary<string, string?> WhiteList = new Dictionary<string, string?>();
        public static Config Load(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    return JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
                }
                else
                {
                    Write(path, new Config());
                    return new Config();
                }
            }
            catch (Exception ex)
            {
                TShock.Log.Error("[WList]读文件时出现错误" + ex.ToString());
                return new Config();
            }
        }
        public static void Write(string path, Config cfg)
        {
            try
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(cfg));
            }
            catch (Exception ex)
            {
                TShock.Log.Error("[WList]写配置时出现错误" + ex.ToString());
            }
        }
    }
}
