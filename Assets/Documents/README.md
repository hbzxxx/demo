# 游戏数据配置系统使用文档

本文档详细介绍如何使用本项目的Excel数据配置系统和各个游戏功能模块。

---

## 目录

1. [快速开始](#1-快速开始)
2. [Excel数据配置工具](#2-excel数据配置工具)
3. [Excel表格模板说明](#3-excel表格模板说明)
4. [数据加载与使用](#4-数据加载与使用)
5. [游戏系统使用指南](#5-游戏系统使用指南)
6. [事件系统](#6-事件系统)
7. [Unity编辑器配置](#7-unity编辑器配置)

---

## 1. 快速开始

### 1.1 一键生成示例数据（推荐首次使用）

如果是首次使用项目，建议先点击生成示例数据，然后根据需要修改：

1. 打开Unity编辑器
2. 点击菜单 `Tools` > `Excel转JSON工具`
3. 点击「生成示例数据」按钮
4. 在 `Assets/Resources/Data/` 目录下会生成 `GameData.json`
5. 运行游戏即可使用示例数据

### 1.2 从零开始创建数据

如果需要创建自己的数据：

1. 点击「生成CSV模板文件」按钮
2. 在 `Assets/Resources/Data/Templates/` 目录下会生成8个CSV模板文件
3. 用Excel/WPS打开CSV文件，填写数据
4. 保存后使用「转换Excel/CSV为JSON」生成JSON文件

---

## 2. Excel数据配置工具

### 2.1 工具位置

在Unity编辑器中，点击菜单: `Tools` > `Excel转JSON工具`

### 2.2 功能说明

| 按钮 | 功能 |
|------|------|
| 生成CSV模板文件 | 生成8个空白CSV模板，方便填写数据 |
| 生成示例数据 | 生成包含示例数据的GameData.json |
| 转换Excel/CSV为JSON | 将Excel或CSV文件转换为JSON数据 |

### 2.3 支持格式

- `.xlsx` - Excel格式
- `.csv` - CSV格式（推荐）

### 2.4 使用流程

1. **准备数据文件**
   - 方式一：使用「生成CSV模板」获取空白模板，填写后保存
   - 方式二：直接编辑CSV文件

2. **转换数据**
   - 点击"选择Excel/CSV文件"按钮，选择准备好的文件
   - 点击"转换Excel/CSV为JSON"按钮
   - 生成的文件保存在 `Assets/Resources/Data/GameData.json`

### 2.5 注意事项

- CSV文件第一行必须为字段名称
- 从第二行开始为数据
- 多个值用逗号分隔（如RewardItems: I001,I002）

---

## 2. Excel表格模板说明

### 2.1 Player（玩家数据）

用于配置玩家的基本属性。

| 字段 | 类型 | 说明 | 示例 |
|------|------|------|------|
| ID | string | 唯一标识符，不可重复 | P001 |
| Name | string | 玩家名称 | Knight |
| MaxHealth | float | 最大生命值 | 120 |
| MoveSpeed | float | 移动速度 | 5 |
| AttackDamage | float | 攻击力 | 25 |
| AttackSpeed | float | 攻击速度（越大越快） | 1.2 |
| Defense | float | 防御力 | 10 |

**示例数据：**
```
ID      Name     MaxHealth  MoveSpeed  AttackDamage  AttackSpeed  Defense
P001    Knight   120        5          25            1.2          10
P002    Archer   80         6          20            2.0          5
P003    Mage     60         4          35            0.8          3
```

### 2.2 Enemy（敌人数据）

用于配置敌人的属性和行为参数。

| 字段 | 类型 | 说明 | 示例 |
|------|------|------|------|
| ID | string | 唯一标识符 | E001 |
| Name | 敌人名称 | Slime |
| MaxHealth | float | 最大生命值 | 30 |
| MoveSpeed | float | 巡逻移动速度 | 2 |
| ChaseSpeed | float | 追击玩家速度 | 3 |
| DetectionRadius | float | 感知玩家范围（像素） | 4 |
| AttackRange | float | 攻击范围 | 1 |
| Damage | int | 每次攻击伤害 | 5 |
| AttackCooldown | float | 攻击冷却时间（秒） | 2 |
| PatrolWaitTime | float | 巡逻等待时间（秒） | 2 |

**示例数据：**
```
ID      Name    MaxHealth  MoveSpeed  ChaseSpeed  DetectionRadius  AttackRange  Damage  AttackCooldown  PatrolWaitTime
E001    Slime   30         2          3           4                1            5       2               2
E002    Goblin  50         3          4           5                1.5          10      1.5             1.5
E003    Orc     100        2          3.5         6                2            20      3               2
```

### 2.3 Item（物品数据）

用于配置游戏中可使用的物品。

| 字段 | 类型 | 说明 | 示例 |
|------|------|------|------|
| ID | string | 唯一标识符 | I001 |
| Name | string | 物品名称 | 生命药水 |
| Description | string | 物品描述 | 恢复50点生命值 |
| Type | int | 物品类型 | 详见下方类型说明 |
| Price | int | 售价（金币） | 50 |
| MaxStack | int | 最大堆叠数量 | 99 |
| Value | int | 物品效果值 | 50 |

**Type类型说明：**
- 0 = Weapon（武器）
- 1 = Armor（防具）
- 2 = Consumable（消耗品）
- 3 = Material（材料）
- 4 = Quest（任务物品）

**示例数据：**
```
ID   Name       Description         Type  Price  MaxStack  Value
I001 生命药水   恢复50点生命值      2     50     99        50
I002 铁剑       基础武器伤害+10     0     100    1         10
I003 皮甲       基础防具防御+5      1     80     1         5
I004 史莱姆凝胶 任务材料            3     5      99        0
```

### 2.4 Quest（任务数据）

用于配置游戏中的任务。

| 字段 | 类型 | 说明 | 示例 |
|------|------|------|------|
| ID | string | 唯一标识符 | Q001 |
| Title | string | 任务标题 | 讨伐史莱姆 |
| Description | string | 任务描述 | 击败5只史莱姆 |
| Type | int | 任务类型 | 详见下方 |
| TargetID | string | 目标ID | E001 |
| TargetCount | int | 目标数量 | 5 |
| RewardGold | int | 金币奖励 | 100 |
| RewardItems | string | 物品奖励ID（多个用逗号分隔） | I002 |
| PreQuestIDs | string | 前置任务ID（多个用逗号分隔） | Q001 |
| NPCID | string | 关联NPC的ID | N001 |

**Type类型说明：**
- 0 = Kill（击杀敌人）
- 1 = Collect（收集物品）
- 2 = Talk（对话任务）
- 3 = Reach（到达指定地点）

**示例数据：**
```
ID    Title      Description       Type  TargetID  TargetCount  RewardGold  RewardItems  PreQuestIDs  NPCID
Q001  讨伐史莱姆  击败5只史莱姆     0     E001      5            100         I002                     N001
Q002  收集材料   收集10个史莱姆凝胶 1     I004      10           50                                   N001
```

### 2.5 NPC（NPC数据）

用于配置游戏中的非玩家角色。

| 字段 | 类型 | 说明 | 示例 |
|------|------|------|------|
| ID | string | 唯一标识符 | N001 |
| Name | string | NPC名称 | 村长 |
| Title | string | NPC称号 | 村长 |
| Description | string | NPC描述 | 村庄的村长 |
| QuestIDs | string | 关联任务ID（多个用逗号分隔） | Q001,Q002 |
| ShopID | string | 关联商店ID（无商店留空） | S001 |
| DialogLines | string | 对话内容 | 欢迎来到村庄！ |

**示例数据：**
```
ID   Name   Title  Description                   QuestIDs    ShopID  DialogLines
N001 村长   村长   村庄的村长有重要任务委托      Q001;Q002           欢迎来到小村庄！
N002 商人   杂货商 出售各种道具和装备                      S001    看看我的商品吧！
```

### 2.6 Shop（商店数据）

用于配置商店基本信息。

| 字段 | 类型 | 说明 | 示例 |
|------|------|------|------|
| ID | string | 唯一标识符 | S001 |
| Name | string | 商店名称 | 杂货店 |

### 2.7 ShopItem（商店物品）

用于配置商店中出售的物品。

| 字段 | 类型 | 说明 | 示例 |
|------|------|------|------|
| ShopID | string | 所属商店ID | S001 |
| ItemID | string | 物品ID | I001 |
| Price | int | 售价 | 50 |
| Stock | int | 库存数量（-1或留空表示无限） | 99 |
| IsUnlimited | bool | 是否无限库存 | True |

**示例数据：**
```
ShopID  ItemID  Price  Stock  IsUnlimited
S001    I001    50     99     True
S001    I002    100    5      False
```

### 2.8 Weapon（武器数据）

用于配置武器的属性。

| 字段 | 类型 | 说明 | 示例 |
|------|------|------|------|
| ID | string | 唯一标识符 | W001 |
| Name | string | 武器名称 | 手枪 |
| FireRate | float | 射击间隔（秒） | 0.2 |
| ReloadTime | float | 换弹时间（秒） | 1.5 |
| ClipSize | int | 弹夹容量 | 12 |
| MaxReserveAmmo | int | 备用弹药上限 | 60 |
| BulletName | string | 子弹名称 | Bullet |
| BulletSpeed | float | 子弹速度 | 20 |
| BulletTime | float | 子弹存在时间（秒） | 2 |
| Damage | float | 伤害值 | 10 |
| MuzzleEffectsDisappear | float | 枪口特效消失时间 | 0.1 |
| BulletPerFire | int | 每次射击子弹数 | 1 |
| SpreadAngle | float | 散射角度 | 5 |

**示例数据：**
```
ID   Name   FireRate  ReloadTime  ClipSize  MaxReserveAmmo  BulletName  BulletSpeed  BulletTime  Damage  MuzzleEffectsDisappear  BulletPerFire  SpreadAngle
W001 手枪   0.2       1.5         12        60              Bullet      20           2           10      0.1                     1              5
W002 步枪   0.1       2           30        120             Bullet      30           3           15      0.1                     1              3
W003 霰弹枪 1         3           6         24              ShotgunBullet 15         1         25      0.15                    5              15
```

---

## 3. 数据加载与使用

### 3.1 数据加载器

在场景中创建一个空物体，添加`GameDataLoader`组件。游戏启动时会自动加载`Resources/Data/GameData.json`。

```csharp
// GameDataLoader会自动完成以下加载:
// - 玩家数据 -> DataManager
// - 敌人数据 -> DataManager
// - 物品数据 -> InventoryManager
// - 任务数据 -> QuestManager
// - NPC数据 -> NPCManager
// - 商店数据 -> ShopManager
// - 武器数据 -> WeaponManager
```

### 3.2 数据获取方式

```csharp
// 获取玩家数据
PlayerData player = DataManager.Instance.GetPlayerData("P001");

// 获取敌人数据
EnemyData enemy = DataManager.Instance.GetEnemyData("E001");

// 获取物品数据
ItemData item = InventoryManager.Instance.GetItemData("I001");

// 获取武器数据
WeaponData weapon = WeaponManager.Instance.GetWeaponData("W001");
```

### 3.3 数据结构

```csharp
// 玩家数据
[Serializable]
public class PlayerData
{
    public string ID;           // 唯一标识符
    public string Name;         // 玩家名称
    public float MaxHealth;     // 最大生命值
    public float MoveSpeed;     // 移动速度
    public float AttackDamage;  // 攻击力
    public float AttackSpeed;  // 攻击速度
    public float Defense;       // 防御力
}

// 敌人数据
[Serializable]
public class EnemyData
{
    public string ID;               // 唯一标识符
    public string Name;             // 敌人名称
    public float MaxHealth;         // 最大生命值
    public float MoveSpeed;         // 巡逻速度
    public float ChaseSpeed;        // 追击速度
    public float DetectionRadius;   // 感知范围
    public float AttackRange;       // 攻击范围
    public int Damage;              // 伤害值
    public float AttackCooldown;    // 攻击冷却
    public float PatrolWaitTime;    // 巡逻等待时间
}
```

---

## 4. 游戏系统使用指南

### 4.1 玩家生命值系统

#### 4.1.1 组件添加

在玩家GameObject上添加`PlayerHealth`组件，并设置`PlayerDataID`属性。

#### 4.1.2 代码使用

```csharp
// 获取血量
float health = playerHealth.CurrentHealth;
float maxHealth = playerHealth.MaxHealth;
float healthRatio = playerHealth.CurrentHealthRatio;

// 造成伤害
playerHealth.TakeDamage(10f);

// 治疗
playerHealth.Heal(50f);
```

#### 4.1.3 UI显示

1. 创建UI Canvas
2. 添加Slider组件作为血条
3. 添加`HealthUI`脚本，关联Slider

```csharp
public class HealthUI : MonoBehaviour
{
    public Slider HealthSlider;           // 血条Slider组件
    public Image FillImage;              // 填充图片（用于颜色变化）
    public Color FullHealthColor = Color.green;   // 满血颜色
    public Color LowHealthColor = Color.red;      // 低血量颜色
}
```

### 4.2 敌人AI系统

#### 4.2.1 组件添加

1. 创建敌人GameObject
2. 添加组件：
   - `Rigidbody2D`（设置Body Type为Kinematic）
   - `BoxCollider2D`
   - `EnemyController`
3. 设置`Visual`子对象和`Animator`

#### 4.2.2 配置参数

在Inspector中设置`EnemyDataID`，系统会自动从表格加载所有属性。

| 参数 | 说明 |
|------|------|
| EnemyDataID | 敌人数据ID（对应Excel中的ID列） |
| PatrolPoints | 巡逻点数组（可选） |

#### 4.2.3 状态说明

敌人AI包含以下状态：

- **Idle（待机）**: 原地等待一段时间后进入巡逻
- **Patrol（巡逻）**: 按照设定路径移动巡逻
- **Chase（追击）**: 发现玩家后追击
- **Attack（攻击）**: 进入攻击范围后进行攻击

### 4.3 任务系统

#### 4.3.1 接受任务

```csharp
// 接受任务
QuestManager.Instance.AcceptQuest("Q001");

// 获取可接任务列表
List<QuestData> availableQuests = QuestManager.Instance.GetAvailableQuests();

// 获取已接任务列表
List<QuestData> acceptedQuests = QuestManager.Instance.GetAcceptedQuests();
```

#### 4.3.2 更新任务进度

```csharp
// 击杀敌人时调用
QuestManager.Instance.UpdateQuestProgress("E001", 1);

// 收集物品时调用
QuestManager.Instance.UpdateQuestProgress("I004", 1);
```

#### 4.3.3 完成任务

```csharp
// 交付任务
QuestManager.Instance.CompleteQuest("Q001");

// 任务完成后自动发放奖励（金币和物品）
```

### 4.4 NPC系统

#### 4.4.1 创建NPC

1. 创建NPC GameObject
2. 添加`NPCController`组件
3. 设置`NPCID`属性（对应Excel中的ID）

#### 4.4.2 交互方式

NPC通过点击触发交互，自动处理：
- 显示对话
- 显示任务列表
- 打开商店（如有设置ShopID）

### 4.5 背包系统

#### 4.5.1 基本操作

```csharp
// 添加物品
InventoryManager.Instance.AddItem("I001", 5);

// 移除物品
InventoryManager.Instance.RemoveItem("I001", 1);

// 获取物品数量
int count = InventoryManager.Instance.GetItemCount("I001");

// 添加金币
InventoryManager.Instance.AddGold(100);

// 消费金币（会检查金币是否足够）
bool success = InventoryManager.Instance.SpendGold(50);

// 获取背包槽位
List<InventorySlot> slots = InventoryManager.Instance.GetAllSlots();
```

#### 4.5.2 配置参数

| 参数 | 说明 |
|------|------|
| MaxSlots | 背包最大槽位数量 |
| CurrentGold | 当前金币数量 |

### 4.6 商店系统

#### 4.6.1 打开商店

```csharp
// 通过商店ID打开商店
ShopManager.Instance.OpenShop("S001");

// 购买物品
bool success = ShopManager.Instance.BuyItem("I001");
```

### 4.7 武器系统

```csharp
// 获取武器数据
WeaponData weapon = WeaponManager.Instance.GetWeaponData("W001");

// 使用数据
float fireRate = weapon.FireRate;        // 射击间隔
int clipSize = weapon.ClipSize;          // 弹夹容量
float damage = weapon.Damage;             // 伤害值
```

---

## 5. 事件系统

### 5.1 事件类型列表

| 事件名称 | 参数 | 说明 |
|----------|------|------|
| PLAYER_HEALTH_CHANGED | float (血量比例 0-1) | 玩家血量变化 |
| PLAYER_DIED | 无 | 玩家死亡 |
| QUEST_ACCEPTED | QuestData | 任务已接受 |
| QUEST_PROGRESS_UPDATED | QuestData | 任务进度更新 |
| QUEST_COMPLETED | QuestData | 任务已完成 |
| QUEST_FINISHED | QuestData | 任务已交付 |
| NPC_DIALOG | string (标题), string (内容) | NPC对话 |
| SHOP_OPENED | ShopData | 商店已打开 |
| ITEM_PURCHASED | string (物品ID) | 物品已购买 |
| INVENTORY_UPDATED | 无 | 背包已更新 |
| GOLD_CHANGED | int (金币数量) | 金币变化 |

### 5.2 监听事件

```csharp
// 在Start或Awake中添加监听
private void OnEnable()
{
    // 监听血量变化
    EventCenter.TriggerEvent<float>(EventType.PLAYER_HEALTH_CHANGED, OnHealthChanged);
}

// 错误写法：
// EventCenter.AddListener<float>(EventType.PLAYER_HEALTH_CHANGED, OnHealthChanged);

// 正确写法 - 使用TriggerEvent：
private void OnHealthChanged(float healthRatio)
{
    Debug.Log($"血量变化: {healthRatio * 100}%");
}
```

### 5.3 发送事件

```csharp
// 无参数
EventCenter.TriggerEvent(EventType.INVENTORY_UPDATED);

// 有参数
EventCenter.TriggerEvent<float>(EventType.PLAYER_HEALTH_CHANGED, 0.5f);
EventCenter.TriggerEvent<string, string>(EventType.NPC_DIALOG, "村长", "欢迎!");
```

### 5.4 EventCenter类说明

EventCenter是项目的事件中心，使用`TriggerEvent`方法发送事件。

```csharp
// 发送无参数事件
EventCenter.TriggerEvent(EventType.事件类型);

// 发送1个参数的事件
EventCenter.TriggerEvent<T>(EventType.事件类型, 参数);

// 发送2个参数的事件
EventCenter.TriggerEvent<T1, T2>(EventType.事件类型, 参数1, 参数2);
```

---

## 6. Unity编辑器配置

### 6.1 场景设置步骤

1. **创建数据加载器**
   - 创建空物体，命名为"GameManager"
   - 添加`GameDataLoader`组件

2. **创建UI管理器**
   - 创建Canvas
   - 添加`UIManager`组件
   - 配置任务、背包、商店面板

3. **创建玩家**
   - 添加`PlayerController`组件
   - 添加`PlayerHealth`组件
   - 设置`PlayerDataID`为"P001"
   - 添加`Rigidbody2D`和`BoxCollider2D`

4. **创建敌人**
   - 添加`EnemyController`组件
   - 设置`EnemyDataID`为"E001"
   - 添加`Rigidbody2D`和`BoxCollider2D`

5. **创建NPC**
   - 添加`NPCController`组件
   - 设置`NPCID`为对应的NPC ID
   - 添加`BoxCollider2D`（勾选Is Trigger）

### 6.2 预制体结构

```
Player (层级结构示例)
├── PlayerController (脚本)
├── PlayerHealth (脚本)
├── Rigidbody2D
├── BoxCollider2D
├── Visual (子对象)
│   └── Animator
└── Weapon
    └── WeaponController

Enemy (层级结构示例)
├── EnemyController (脚本)
├── Rigidbody2D
├── BoxCollider2D
└── Visual (子对象)
    └── Animator

NPC (层级结构示例)
├── NPCController (脚本)
├── SpriteRenderer
└── BoxCollider2D (Is Trigger = true)

UI (层级结构示例)
├── Canvas
├── UIManager (脚本)
├── QuestPanel
├── InventoryPanel
├── ShopPanel
└── DialogPanel
```

### 6.3 常用快捷键绑定示例

```csharp
void Update()
{
    // 打开/关闭背包
    if (Input.GetKeyDown(KeyCode.B))
    {
        UIManager.Instance.ToggleInventoryPanel();
    }
    
    // 打开/关闭任务面板
    if (Input.GetKeyDown(KeyCode.Q))
    {
        UIManager.Instance.ToggleQuestPanel();
    }
    
    // 打开/关闭商店
    if (Input.GetKeyDown(KeyCode.K))
    {
        UIManager.Instance.ToggleShopPanel();
    }
}
```

---

## 附录：Excel文件示例

### 创建一个完整的Excel文件

1. 新建Excel工作簿
2. 创建以下工作表：
   - Player（玩家数据）
   - Enemy（敌人数据）
   - Item（物品数据）
   - Quest（任务数据）
   - NPC（NPC数据）
   - Shop（商店数据）
   - ShopItem（商店物品）
   - Weapon（武器数据）

3. 每个工作表第一行为字段名
4. 从第二行开始填入数据

5. 保存为`.xlsx`格式

6. 使用`Tools > Excel转JSON工具`转换为JSON文件

---

*文档版本: 1.0*
*最后更新: 2026*
