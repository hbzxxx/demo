# ExcelDataReader 安装指南

本文档说明如何安装ExcelDataReader包以解决Excel读取依赖问题。

## 安装方法

### 方法一：Unity Package Manager (推荐)

1. 打开Unity编辑器
2. 点击菜单 `Window` > `Package Manager`
3. 点击左上角的 `+` 号
4. 选择 `Add package from git URL...`
5. 输入以下URL：
   ```
   https://github.com/ExcelDataReader/ExcelDataReader.git
   ```
6. 点击 `Add` 等待安装完成

### 方法二：手动导入DLL

1. 下载ExcelDataReader：
   - 访问：https://github.com/ExcelDataReader/ExcelDataReader/releases
   - 下载最新的 `ExcelDataReader-x.x.x.dll` 和 `ExcelDataReader.Core-x.x.x.dll`

2. 将下载的DLL文件拖入Unity的 `Assets` 文件夹中

3. 确保DLL放置在 `Assets/Plugins` 或 `Assets` 目录下

### 方法三：通过NuGetForUnity

1. 如果你使用了NuGetForUnity：
   - 右键点击 `Packages/manifest.json`
   - 选择 `Manage NuGet Packages`
   - 搜索 `ExcelDataReader`
   - 点击安装

## 验证安装

安装完成后，ExcelTool脚本中的以下代码将不再报错：

```csharp
using Excel;
using ExcelDataReader;
```

## 常见问题

### Q: 安装后仍然报错？

A: 尝试以下步骤：
1. 在Unity中点击 `File` > `Build Settings` > `Switch Platform`
2. 如果使用DLL方式，确保DLL的 `.NET Standard 2.0` 版本
3. 重启Unity

### Q: 找不到ExcelDataReader？

A: 包名可能为 `ExcelDataReader`，如果搜索不到，尝试：
- `excel datareader`
- `excelreader`

### Q: 编译错误？

A: 检查是否同时导入了其他Excel相关DLL，可能存在冲突。只保留ExcelDataReader即可。

## 备选方案

如果ExcelDataReader安装困难，可以考虑使用CSV文件方案：
- 将Excel文件另存为CSV格式
- 使用CSVReader读取（项目中已有CSVReader.cs）

---

*如有问题请检查Unity控制台的详细错误信息*
