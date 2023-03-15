# WList

这是一个能绑定IP的白名单插件，当玩家IP改变时它将阻止玩家进入除非解除绑定
这是我的第一个~~屎山~~插件

主指令: `/wlist`

可用子指令：

> /wlist add <玩家名> // 添加一个白名单

> /wlist del <玩家名> // 删除一个白名单

> /wlist list [页码] // 查看绑定的用户名及IP，每页10个内容

> /wlist unbind <玩家名> // 解除一个玩家对应的IP的绑定

配置文件:`wlist.json`

```json
{
  "玩家名": "IP", # 绑定了IP
  "玩家名": null # 未绑定IP
}
```

## 感谢

[豆沙的 BetterWhiteList](https://gitee.com/Crafty/BetterWhitelist)

[作为模板的 PluginTemplate](https://github.com/TShockResources/PluginTemplate)

[TShock](https://github.com/Pryaxis/TShock)

## 用途

~~这个插件的用途嘛...打算是结合Bot一块使用，增强玩家登录安全保障(不过话说用Chameleon会不会更好......)~~

后续会考虑和REST拓展配合Bot使用(前提是我写的出来)

