# Cirno.ChinaGS.Injection.Permanent

## 程序结构

- 插件程序集 `Cirno.ChinaGS.Injection.Permanent.dll`
- 扩展功能包接口程序集 `CirnoFramework.Interface.Commands.dll`
- 扩展功能包文件夹 `CommandLib/`
- 基础扩展功能包 `CommandLib/CirnoBuiltins.dll`
- 扩展功能包管理器 `CommandLib/CirnoPM.dll`
- 可视化修改器 `CommandLib/CirnoModifier.dll` 使用班牌内置后台命令 `192608` 呼出该窗口
- 相关配置文件

## 附件：部署工具链

- 解压缩辅助命令行程序，用于远程控制时快速展开修改包 `ZipExtractHelper.exe`
- 命令发送器 `sender.py`

## 使用说明

### 1.安装

1. 解压缩程序包
2. 将 `./Cirno.ChinaGS.Injection.Permanent` 目录复制到班牌安装目录下 `Addons/` 文件夹
3. 重启班牌程序

### 2. 指令发送

使用 dnSpy 查看扩展功能包中的形如

```csharp
[Export(typeof(ICommand))]
[Command("Builtin.ShadowLantern")]
```

的内容，将 `[Command("Builtin.ShadowLantern")]` 括号中内容，如 `Builtin.ShadowLantern`，添加到 `command_parser.py` 中字典对象 `command` 中，如：

```python
commands = {
    0: "Builtin.ShadowLantern",
}
```

确保 `command_parser.py` 同一路径下有一个包含了以 CRLF 换行分隔的，名为 `IpList.txt` 的 IP 列表（此版本未上传，目前为旧版本，默认发送到 `127.0.0.1`），执行 `python command_parser.py`。

### 3. 扩展包开发

略
