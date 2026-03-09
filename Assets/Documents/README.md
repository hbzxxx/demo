# 游戏数据配置与系统使用文档

本文档介绍如何使用本项目的Excel数据配置系统和各个游戏功能模块。

---

## 目录

1. [Excel数据配置工具](#1-excel数据配置工具)
2. [Excel表格模板说明](#2-excel表格模板说明)
3. [数据加载与使用](#3-数据加载与使用)
4. [游戏系统使用指南](#4-游戏系统使用指南)
5. [事件系统](#5-事件系统)
6. [Unity编辑器配置](#6-unity编辑器配置)

---

## 1. Excel数据配置工具

### 1.1 打开工具

在Unity编辑器中，点击菜单: `Tools > Excel转JSON工具`

### 1.2 使用流程

1. 点击"选择Excel文件"按钮，选择`.xlsx`格式的Excel文件
2. 根据需要修改工作表名称（留空则自动匹配包含关键字的表）
3. 点击"转换Excel为JSON"按钮
4. 生成的文件保存在 `Assets/Resources/Data/GameData.json`

### 1.3 依赖库

Excel读取需要`Excel.dll`和`System.Data.dll`。Unity中可通过NuGet安装`ExcelDataReader`包。

---

## 2. Excel表格模板说明

### 2.1 Player (玩家数据)

| 字段 | 类型 | 说明 |
|------|------|------|
| ID | string | 唯一标识符 |
| Name | string | 玩家名称 |
| MaxHealth | float | 最大生命值 |
| MoveSpeed | float | 移动速度 |
| AttackDamage | float | 攻击力 |
| AttackSpeed | float | 攻击速度 |
| Defense | float | 防御力 |

### 2.2 Enemy (敌人数据)

| 字段 | 类型 | 说明 |
|------|------|------|
| ID | string | 唯一标识符 |
| Name | string | 敌人名称 |
| MaxHealth | float | 最大生命值 |
| MoveSpeed | float | 普通移动速度 |
| ChaseSpeed | float | 追击速度 |
| DetectionRadius | float | 感知玩家范围 |
| AttackRange | float | 攻击范围 |
| Damage | int | 伤害值 |
| AttackCooldown | float | 攻击冷却时间 |
| PatrolWaitTime | float | 巡逻等待时间 |

### 2.3 Item (物品数据)

| 字段 | 类型 | 说明 |
|------|------|------|
| ID | string | 唯一标识符 |
| Name | string | 物品名称 |
| Description | string | 物品描述 |
| Type | int | 物品类型(0=Weapon, 1=Armor, 2=Consumable, 3=Material, 4=Quest) |
| Price | int | 售价 |
| MaxStack | int | 最大堆叠数量 |
| Value | int | 物品价值/效果值 |

### 2.4 Quest (任务数据)

| 字段 | 类型 | 说明 |
|------|------|------|
| ID | string | 唯一标识符 |
| Title | string | 任务标题 |
| Description | string | 任务描述 |
| Type | int | 任务类型(0=Kill, 1=Collect, 2=Talk, 3=Reach) |
| TargetID | string | 目标ID(敌人ID或物品ID) |
| TargetCount | int | 目标数量 |
| RewardGold | int | 金币奖励 |
| RewardItems | string | 奖励物品ID(多个用逗号分隔) |
| PreQuestIDs | string | 前置任务ID(多个用逗号分隔) |
| NPCID | string | 关联NPC的ID |

### 2.5 NPC (NPC数据)

| 字段 | 类型 | 说明 |
|------|------|------|
| ID | string | 唯一标识符 |
| Name | string | NPC名称 |
| Title | string | NPC称号 |
| Description | string | NPC描述 |
| QuestIDs | string | 关联任务ID(多个用逗号分隔) |
| ShopID | string | 关联商店ID(无商店则留空) |
| DialogLines | string | 对话内容 |

### 2.6 Shop (商店数据)

| 字段 | 类型 | 说明 |
|------|------|------|
| ID | string | 唯一标识符 |
| Name | string | 商店名称 |

### 2.7 ShopItem (商店物品)

| 字段 | 类型 | 说明 |
|------|------|------|
| ShopID | string | 所属商店ID |
| ItemID | string | 物品ID |
| Price | int | 售价 |
| Stock | int | 库存数量 |
| IsUnlimited | bool | 是否无限库存(True/False) |

### 2.8 Weapon (武器数据)

| 字段 | 类型 | 说明 |
|------|------|------|
| ID | string | 唯一标识符 |
| Name | string | 武器名称 |
| FireRate | float | 射击间隔(秒) |
| ReloadTime | float | 换弹时间(秒) |
| ClipSize | int | 弹夹容量 |
| MaxReserveAmmo | int | 备用弹药上限 |
| BulletName | string | 子弹名称 |
| BulletSpeed | float | 子弹速度 |
| BulletTime | float | 子弹存在时间 |
| Damage | float | 伤害值 |
| MuzzleEffectsDisappear | float | 枪口特效消失时间 |
| BulletPerFire | int | 每次射击子弹数 |
| SpreadAngle | float | 散射角度 |

---

## 3. 数据加载与使用

### 3.1 自动加载

在场景中创建一个空物体，添加`GameDataLoader`组件。游戏启动时会自动加载`Resources/Data/GameData.json`。

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

### 3.2 数据结构

```csharp
// 玩家数据
public class PlayerData {
    public string ID;
    public string Name;
    public float MaxHealth;
    public float MoveSpeed;
    public float AttackDamage;
    public float AttackSpeed;
    public float Defense;
}

// 敌人数据
public class EnemyData {
    public string ID;
    public string Name;
    public float MaxHealth;
    public float MoveSpeed;
    public float ChaseSpeed;
    public float DetectionRadius;
    public float AttackRange;
    public int Damage;
    public float AttackCooldown;
    public float PatrolWaitTime;
}
```

---

## 4. 游戏系统使用指南

### 4.1 玩家血量系统

#### 添加血量组件

在玩家GameObject上添加`PlayerHealth`组件:

```csharp
// 获取血量
float health = playerHealth.CurrentHealth;
float maxHealth = playerHealth.MaxHealth;
float healthRatio = playerHealth.CurrentHealthRatio; // 0-1

// 造成伤害
playerHealth.TakeDamage(10f);

// 治疗
playerHealth.Heal(50f);
```

#### 血量UI显示

1. 创建UI Canvas
2. 添加Slider组件作为血条
3. 添加`HealthUI`脚本，关联Slider

```csharp
public class HealthUI : MonoBehaviour
{
    public Slider HealthSlider;
    public Image FillImage;
    public Color FullHealthColor = Color.green;
    public Color LowHealthColor = Color.red;
}
```

### 4.2 敌人AI系统

#### 创建敌人

1. 创建敌人GameObject
2. 添加组件:
   - `Rigidbody2D` (设置Body Type为Kinematic)
   - `BoxCollider2D`
   - `EnemyController`
3. 设置Visual子对象和Animator

#### EnemyController配置

| 参数 | 说明 |
|------|------|
| MoveSpeed | 巡逻移动速度 |
| ChaseSpeed | 追击速度 |
| DetectionRadius | 感知玩家范围 |
| AttackRange | 攻击范围 |
| PatrolPoints | 巡逻点(Transform数组) |
| AttackCooldown | 攻击冷却时间 |
| Damage | 伤害值 |

#### 状态说明

- **Idle**: 待机状态，等待一段时间后进入巡逻
- **Patrol**: 巡逻状态，前往巡逻点
- **Chase**: 追击状态，发现玩家后追击
- **Attack**: 攻击状态，进入攻击范围后攻击

### 4.3 任务系统

#### 接受任务

```csharp
// 接受任务
QuestManager.Instance.AcceptQuest("Q001");

// 获取可接任务列表
List<QuestData> availableQuests = QuestManager.Instance.GetAvailableQuests();

// 获取已接任务列表
List<QuestData> acceptedQuests = QuestManager.Instance.GetAcceptedQuests();
```

#### 更新任务进度

```csharp
// 击杀敌人时调用
QuestManager.Instance.UpdateQuestProgress("E001", 1);

// 收集物品时调用
QuestManager.Instance.UpdateQuestProgress("I004", 1);
```

#### 完成任务

```csharp
// 交付任务
QuestManager.Instance.CompleteQuest("Q001");

// 任务完成后自动发放奖励(金币和物品)
```

### 4.4 NPC系统

#### 创建NPC

1. 创建NPC GameObject
2. 添加`NPCController`组件
3. 设置`NPCID`属性

#### 与NPC交互

NPC通过点击触发交互:

```csharp
// NPCController会自动处理:
// 1. 显示对话
// 2. 显示任务列表
// 3. 打开商店(如果有ShopID)
```

### 4.5 背包系统

#### 基本操作

```csharp
// 添加物品
InventoryManager.Instance.AddItem("I001", 5);

// 移除物品
InventoryManager.Instance.RemoveItem("I001", 1);

// 获取物品数量
int count = InventoryManager.Instance.GetItemCount("I001");

// 添加金币
InventoryManager.Instance.AddGold(100);

// 消费金币
bool success = InventoryManager.Instance.SpendGold(50);

// 获取背包槽位
List<InventorySlot> slots = InventoryManager.Instance.GetAllSlots();
```

#### 背包配置

| 参数 | 说明 |
|------|------|
| MaxSlots | 背包最大槽位 |
| CurrentGold | 当前金币数量 |

### 4.6 商店系统

#### 打开商店

```csharp
// 通过商店ID打开
ShopManager.Instance.OpenShop("S001");

// 购买物品
bool success = ShopManager.Instance.BuyItem("I001");
```

#### 商店数据结构

```csharp
public class ShopData {
    public string ID;
    public string Name;
    public ShopItem[] Items;
}

public class ShopItem {
    public string ItemID;
    public int Price;
    public int Stock;
    public bool IsUnlimited;
}
```

### 4.7 武器系统

#### 获取武器数据

```csharp
WeaponData weapon = WeaponManager.Instance.GetWeaponData("W001");

// 使用数据
float fireRate = weapon.FireRate;
int clipSize = weapon.ClipSize;
float damage = weapon.Damage;
```

---

## 5. 事件系统

项目使用事件中心进行模块间通信。

### 5.1 事件类型

```csharp
public enum EventType
{
    PLAY_RELOAD_ANIMATION,
    NO_PLAY_RELOAD_ANIMATION,
    PLAYER_HEALTH_CHANGED,      // 参数: float (血量比例 0-1)
    PLAYER_DIED,
    QUEST_ACCEPTED,             // 参数: QuestData
    QUEST_PROGRESS_UPDATED,     // 参数: QuestData
    QUEST_COMPLETED,            // 参数: QuestData
    QUEST_FINISHED,             // 参数: QuestData
    NPC_DIALOG,                // 参数: string (标题), string (内容)
    NPC_QUEST_DIALOG,
    SHOP_OPENED,                // 参数: ShopData
    ITEM_PURCHASED,            // 参数: string (物品ID)
    INVENTORY_UPDATED,
    GOLD_CHANGED,              // 参数: int (金币数量)
}
```

### 5.2 监听事件

```csharp
// 监听血量变化
EventCenter.AddListener<float>(EventType.PLAYER_HEALTH_CHANGED, OnHealthChanged);

void OnHealthChanged(float healthRatio)
{
    Debug.Log($"血量变化: {healthRatio * 100}%");
}

// 监听背包更新
EventCenter.AddListener(EventType.INVENTORY_UPDATED, OnInventoryUpdated);

// 监听金币变化
EventCenter.AddListener<int>(EventType.GOLD_CHANGED, OnGoldChanged);
```

### 5.3 发送事件

```csharp
// 无参数
EventCenter.Broadcast(EventType.INVENTORY_UPDATED);

// 有参数
EventCenter.Broadcast<float>(EventType.PLAYER_HEALTH_CHANGED, 0.5f);
EventCenter.Broadcast<string, string>(EventType.NPC_DIALOG, "村长", "欢迎来到村庄!");
```

### 5.4 移除监听

```csharp
EventCenter.RemoveListener<float>(EventType.PLAYER_HEALTH_CHANGED, OnHealthChanged);
EventCenter.RemoveListener(EventType.INVENTORY_UPDATED, OnInventoryUpdated);
```

---

## 6. Unity编辑器配置

### 6.1 场景设置

1. **创建数据加载器**
   - 创建空物体，添加`GameDataLoader`组件

2. **创建UI管理器**
   - 创建Canvas，添加`UIManager`组件
   - 配置任务、背包、商店面板

3. **创建玩家**
   - 添加`PlayerController`组件
   - 添加`PlayerHealth`组件

4. **创建敌人**
   - 添加`EnemyController`组件
   - 配置感知范围、攻击参数

5. **创建NPC**
   - 添加`NPCController`组件
   - 设置NPCID

### 6.2 预制体建议结构

```
Player
├── PlayerController (脚本)
├── PlayerHealth (脚本)
├── Rigidbody2D
├── BoxCollider2D
├── Visual (子对象)
│   └── Animator
└── Weapon
    └── WeaponController

Enemy
├── EnemyController (脚本)
├── Rigidbody2D
├── BoxCollider2D
└── Visual
    └── Animator

NPC
├── NPCController (脚本)
├── SpriteRenderer
└── BoxCollider2D (IsTrigger = true)

UI
├── Canvas
├── UIManager (脚本)
├── QuestPanel
├── InventoryPanel
├── ShopPanel
└── DialogPanel
```

### 6.3 常用快捷键(示例)

```csharp
void Update()
{
    // 打开背包
    if (Input.GetKeyDown(KeyCode.B))
    {
        UIManager.Instance.ToggleInventoryPanel();
    }
    
    // 打开任务
    if (Input.GetKeyDown(KeyCode.Q))
    {
        UIManager.Instance.ToggleQuestPanel();
    }
}
```

---

## 附录: CSV模板文件

项目中提供了以下CSV模板文件，位于 `Assets/Resources/Data/` 目录:

- `PlayerTemplate.csv` - 玩家数据模板
- `EnemyTemplate.csv` - 敌人数据模板
- `ItemTemplate.csv` - 物品数据模板
- `QuestTemplate.csv` - 任务数据模板
- `NPCTemplate.csv` - NPC数据模板
- `ShopTemplate.csv` - 商店数据模板
- `ShopItemTemplate.csv` - 商店物品数据模板
- `WeaponTemplate.csv` - 武器数据模板

可以将这些模板导入Excel，然后填充实际数据后使用ExcelTool转换为JSON。

---

*本文档由MonkeyCode AI生成*
