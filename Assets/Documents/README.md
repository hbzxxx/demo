# 游戏数据配置系统 - 完整使用教程

本文档将一步步教你如何使用本项目的数据配置系统。

---

## 目录

1. [前置准备](#1-前置准备)
2. [生成数据（推荐方式）](#2-生成数据推荐方式)
3. [自定义数据（可选）](#3-自定义数据可选)
4. [在游戏中使用数据](#4-在游戏中使用数据)
5. [常见问题](#5-常见问题)

---

## 1. 前置准备

### 1.1 拉取项目代码

```bash
git clone <项目地址>
cd <项目目录>
```

### 1.2 用Unity打开项目

1. 安装Unity Hub和Unity编辑器（建议2020.3或更高版本）
2. 点击Open按钮，选择项目文件夹
3. 等待Unity编译完成

### 1.3 检查文件结构

确认项目包含以下文件：
```
Assets/
├── Scripts/
│   ├── Editor/
│   │   └── ExcelTool.cs          # 数据配置工具
│   ├── Data/
│   │   ├── DataManager.cs         # 数据管理器
│   │   ├── GameDataLoader.cs      # 数据加载器
│   │   └── ...
│   └── ...
└── Resources/
    └── Data/
        └── GameData.json          # 数据文件（生成后）
```

---

## 2. 生成数据（推荐方式）

**这是最简单的方式，适合首次使用**

### 步骤一：打开工具

在Unity编辑器顶部菜单，点击 `Tools` > `Excel转JSON工具`

![菜单位置](图片路径/placeholder.png)

### 步骤二：生成示例数据

在打开的窗口中，你会看到以下界面：

```
┌─────────────────────────────────────────┐
│  Excel数据配置工具                        │
├─────────────────────────────────────────┤
│  === 生成模板文件 ===                      │
│  模板输出路径: Assets/Resources/Data/    │
│  [生成CSV模板文件]                        │
│  [生成示例数据]  ← 点击这个！              │
│                                         │
│  === 转换Excel为JSON ===                  │
│  ...                                    │
└─────────────────────────────────────────┘
```

**点击「生成示例数据」按钮**

### 步骤三：确认生成成功

会弹出一个提示框：
```
成功
示例数据已生成到:
Assets/Resources/Data/GameData.json
```

点击确定。

### 步骤四：检查生成的文件

在Unity的Project窗口中，展开 `Assets/Resources/Data/`，可以看到生成了 `GameData.json` 文件。

双击打开，内容大致如下：

```json
{
  "Players": [
    {
      "ID": "P001",
      "Name": "骑士",
      "MaxHealth": 120,
      "MoveSpeed": 5,
      ...
    }
  ],
  "Enemies": [...],
  "Items": [...],
  ...
}
```

### 步骤五：运行游戏测试

按Unity的Play按钮运行游戏，数据会自动加载。

---

## 3. 自定义数据（可选）

如果你想创建自己的数据，而不是使用示例数据，按以下步骤操作：

### 3.1 生成模板文件

1. 点击 `Tools` > `Excel转JSON工具`
2. 点击「生成CSV模板文件」按钮
3. 会在 `Assets/Resources/Data/Templates/` 目录下生成以下文件：

```
Templates/
├── Player模板.csv      # 玩家数据模板
├── Enemy模板.csv       # 敌人数据模板
├── Item模板.csv       # 物品数据模板
├── Quest模板.csv      # 任务数据模板
├── NPC模板.csv        # NPC数据模板
├── Shop模板.csv       # 商店数据模板
├── ShopItem模板.csv   # 商店物品模板
└── Weapon模板.csv     # 武器数据模板
```

### 3.2 用Excel打开模板

**注意：用Excel或WPS打开，不要用记事本**

双击 `Player模板.csv`，Excel会提示导入，选择「分隔符号」，分隔符选择「逗号」。

### 3.3 填写数据

打开后你会看到类似这样的表格：

| ID | Name | MaxHealth | MoveSpeed | AttackDamage | AttackSpeed | Defense |
|----|------|-----------|-----------|--------------|-------------|---------|
| P001 | 骑士 | 120 | 5 | 25 | 1.2 | 10 |
| P002 | 弓箭手 | 80 | 6 | 20 | 2.0 | 5 |

**你可以修改值，或者添加新行**

例如，要添加一个新玩家：

| ID | Name | MaxHealth | MoveSpeed | AttackDamage | AttackSpeed | Defense |
|----|------|-----------|-----------|--------------|-------------|---------|
| P001 | 骑士 | 120 | 5 | 25 | 1.2 | 10 |
| P002 | 弓箭手 | 80 | 6 | 20 | 2.0 | 5 |
| **P003** | **刺客** | **60** | **8** | **30** | **2.5** | **2** |

### 3.4 保存文件

1. 保存文件
2. **重要：选择「CSV UTF-8」格式保存**

### 3.5 转换为JSON

1. 回到Unity
2. 点击 `Tools` > `Excel转JSON工具`
3. 在「Excel文件路径」输入框中输入CSV文件完整路径，或者点击右侧按钮选择文件

```
Excel文件路径: C:\项目路径\Assets\Resources\Data\Templates\Player模板.csv
```

4. 点击「转换Excel/CSV为JSON」按钮
5. 会在 `Assets/Resources/Data/` 目录下生成/更新 `GameData.json`

### 3.6 重复步骤

对每个需要自定义的数据类型（Enemy、Item、Quest等），重复3.2-3.5步骤。

**注意：所有CSV文件转换后会合并到一个GameData.json中**

---

## 4. 在游戏中使用数据

### 4.1 数据如何加载

项目启动时，`GameDataLoader` 组件会自动：
1. 读取 `Resources/Data/GameData.json`
2. 将数据分发到各个管理器

你不需要手动调用加载代码。

### 4.2 使用玩家数据

在玩家对象上添加 `PlayerHealth` 脚本，设置 `PlayerDataID` 为表格中的ID（如"P001"）：

```
┌────────────────────────────────┐
│  PlayerHealth (Script)         │
├────────────────────────────────┤
│  Player Data ID:  P001    ←    │
│  Max Health:      120           │
│  Current Health: 120           │
└────────────────────────────────┘
```

### 4.3 使用敌人数据

在敌人对象上添加 `EnemyController` 脚本，设置 `EnemyDataID` 为表格中的ID（如"E001"）：

```
┌────────────────────────────────┐
│  Enemy Controller (Script)     │
├────────────────────────────────┤
│  Enemy Data ID:  E001    ←     │
│  Move Speed:      2            │
│  Chase Speed:    3            │
│  Detection:      4            │
│  Damage:         5            │
└────────────────────────────────┘
```

### 4.4 使用物品/商店数据

物品数据和商店数据会被自动加载到 `InventoryManager` 和 `ShopManager` 中。

你可以通过代码获取：

```csharp
// 获取物品数据
ItemData item = InventoryManager.Instance.GetItemData("I001");

// 获取商店数据
ShopManager.Instance.OpenShop("S001");
```

### 4.5 使用武器数据

```csharp
// 获取武器数据
WeaponData weapon = WeaponManager.Instance.GetWeaponData("W001");

// 使用数据
Debug.Log(weapon.Name);           // 手枪
Debug.Log(weapon.Damage);         // 10
Debug.Log(weapon.FireRate);       // 0.2
```

---

## 5. 常见问题

### Q1: 点击Tools菜单找不到Excel转JSON工具

**答：** 确保 `ExcelTool.cs` 文件在 `Assets/Scripts/Editor/` 目录下。如果不在，尝试重新编译（点击Unity菜单 File > Save Project）。

### Q2: 生成示例数据按钮灰色不可点击

**答：** 这是界面显示问题，实际上应该可以点击。尝试直接点击该按钮。

### Q3: 生成的GameData.json是空的

**答：** 检查是否有编译错误。如果有错误，Unity会阻止编辑器工具运行。先修复其他编译错误。

### Q4: 运行游戏后数据没有生效

**答：** 
1. 检查GameData.json是否存在且有内容
2. 检查控制台是否有错误信息
3. 确认 `GameDataLoader` 脚本已添加到场景中

### Q5: 如何添加新的敌人类型？

**答：**
1. 在Enemy模板CSV中添加新行，填写数据
2. 转换生成JSON
3. 在场景中创建敌人对象，设置对应的EnemyDataID

### Q6: CSV文件用Excel打开乱码

**答：** 
1. 在Excel中点击「数据」>「从文本」
2. 选择CSV文件
3. 在导入向导中选择「分隔符号」
4. 选择「逗号」
5. 在「文件原始格式」中选择「Unicode (UTF-8)」

### Q7: 如何修改已有的数值？

**答：**
1. 直接修改GameData.json文件中的数值
2. 或者修改CSV后重新转换

---

## 快速参考表

### 数据ID前缀约定

| 类型 | 前缀 | 示例 |
|------|------|------|
| 玩家 | P | P001, P002 |
| 敌人 | E | E001, E002 |
| 物品 | I | I001, I002 |
| 任务 | Q | Q001, Q002 |
| NPC | N | N001, N002 |
| 商店 | S | S001, S002 |
| 武器 | W | W001, W002 |

### 物品类型对应表

| Type值 | 类型 | 说明 |
|--------|------|------|
| 0 | Weapon | 武器 |
| 1 | Armor | 防具 |
| 2 | Consumable | 消耗品 |
| 3 | Material | 材料 |
| 4 | Quest | 任务物品 |

### 任务类型对应表

| Type值 | 类型 | 说明 |
|--------|------|------|
| 0 | Kill | 击杀敌人 |
| 1 | Collect | 收集物品 |
| 2 | Talk | 对话任务 |
| 3 | Reach | 到达地点 |

---

## 视频教程

如果文字描述不够清楚，可以观看视频教程：[视频链接]

---

*如果还有其他问题，请提交Issue*
