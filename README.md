<div align="center">

# 💎 Dodgeball
### *Исходный код гиперкэд игры на Unity*

[![Unity](https://img.shields.io/badge/Unity-2022.3.17f1-black?style=for-the-badge&logo=unity)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-10.0-blue?style=for-the-badge&logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Entitas](https://img.shields.io/badge/Entitas-ECS-green?style=for-the-badge)](https://github.com/sschmid/Entitas-CSharp)
[![Zenject](https://img.shields.io/badge/Zenject-DI-orange?style=for-the-badge)](https://github.com/modesttree/Zenject)

### 🎮 [Play here on Itch.io](https://stoneprophet.itch.io/gemtd) 

*Исходный код tower defense игры, построенной на Entity Component System (Entitas) с использованием MVP паттерна для UI и Dependency Injection*

</div>

---

## 📋 О проекте

Исходный код tower defense игры с 8 типами башен, системой слияния, волнами врагов и динамическими лабиринтами. Реализована модульная архитектура с чётким разделением слоёв и ответственности.

---

## 🎬 Демонстрация геймплея

<div align="center">

<h3 align="center">🎮 YouTube Video</h3>

<p align="center">
  <a href="https://youtu.be/V91p_w_yB-k" target="_blank">
    <img src="https://img.youtube.com/vi/V91p_w_yB-k/maxresdefault.jpg" width="640" alt="Gameplay Preview"/>
  </a>
</p>

### ⚔️ Боевая система и механика башен
![Уточки](https://github.com/user-attachments/assets/50e4a759-7e1d-4740-85ea-da2c1e194487)

### 🔮 Слияние башен и система выбора
![Слияние башен](https://github.com/user-attachments/assets/b6ea78ed-458f-42f6-97bc-31c69f145b95)

### 🎮 Интерфейс игры и дизайн уровней
<img width="100%" alt="Интерфейс игры" src="https://github.com/user-attachments/assets/b5e80cba-4794-4c99-b559-a8fb3406e196" />

</div>

---

## 🏗️ Архитектура проекта

### 📚 Структура проекта

```
GEMTD/
├── Scripts/
│   ├── ECS/                          # Entity Component System
│   │   ├── Game/
│   │   │   ├── Features/             # Модульные игровые системы
│   │   │   │   ├── Towers/           # Система башен
│   │   │   │   ├── Enemies/          # ИИ и спавн врагов
│   │   │   │   ├── Players/          # Игровые системы
│   │   │   │   ├── Battle/           # Боевая механика
│   │   │   │   └── Selection/        # Система выбора объектов
│   │   │   └── Generated/            # Автогенерируемый код Entitas
│   │   └── GameEntityFactories.cs   # Фабрики сущностей
│   ├── Infrastructure/               # Слой приложения
│   │   ├── States/                   # Управление состояниями
│   │   ├── Installers/              # Настройка DI контейнера
│   │   └── Scenes/                   # Менеджмент сцен
│   ├── Services/                     # Сервисы
│   │   ├── AssetProviders/          # Управление ресурсами
│   │   ├── AudioServices/           # Звуковая система
│   │   └── Cameras/                 # Управление камерами
│   ├── UserInterface/               # MVP архитектура UI
│   │   ├── GameplayHeadsUpDisplay/  # Игровой HUD
│   │   ├── MainMenu/                # Главное меню
│   │   └── MazeSelectorMenu/        # Выбор уровня
│   └── Tools/                       # Инструменты разработки
│       └── MazeDesigner/            # Кастомный редактор лабиринтов
```

---

## 🏗️ Архитектурные решения

### **Entity Component System (Entitas)**
Игровая логика построена на ECS архитектуре, где данные хранятся в компонентах, а логика выполняется в системах. Сущности башен, врагов и игровых объектов управляются через единый GameContext.

### **MVP для пользовательского интерфейса**  
Весь UI разделён на View (Unity MonoBehaviour), Presenter (бизнес-логика) и Model (ECS сущности). Каждая панель интерфейса имеет свой презентер.

### **Dependency Injection (Zenject)**
Все зависимости между сервисами и системами управляются через DI контейнер. Настройка происходит в MonoInstaller классах для каждой сцены.

### **State Machine**
Переходы между игровыми экранами (меню, загрузка, геймплей) управляются через конечный автомат состояний.

---

## 🔧 Примеры реализации архитектурных паттернов

### 🏛️ **Entity Component System с Entitas**

**Компонент башни** (`TowerComponent.cs`):
```csharp
using Entitas;

namespace Game.Towers
{
    public class TowerComponent : IComponent
    {
    }
}
```

**Компонент типа башни** (`TowerEnumComponent.cs`):
```csharp
using Entitas;

namespace Game.Towers
{
    [Game]
    public class TowerEnumComponent : IComponent
    {
        public TowerEnum Value;
    }
}
```

**Система поворота башен** (`RotateTowerSystem.cs`):
```csharp
using Entitas;
using UnityEngine;

namespace Game.Towers
{
    public class RotateTowerSystem : IExecuteSystem
    {
        private readonly GameContext _game;
        private readonly IGroup<GameEntity> _towers;

        public RotateTowerSystem(GameContext game)
        {
            _game = game;
            _towers = game.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.TowerEnum,
                    GameMatcher.WorldPosition,
                    GameMatcher.Rotation
                )
            );
        }

        public void Execute()
        {
            foreach (GameEntity tower in _towers)
            {
                if (!tower.hasTargetId)
                    continue;
                
                GameEntity target = _game.GetEntityWithId(tower.TargetId);
                
                Vector3 direction = target.WorldPosition - tower.WorldPosition;
                direction.y = 0;
                Quaternion rotation = Quaternion.LookRotation(direction);
                tower.ReplaceRotation(rotation);
            }
        }
    }
}
```

### 🎭 **MVP паттерн**

**Базовый класс Presenter** (`Presenter.cs`):
```csharp
namespace UserInterface
{
    public abstract class Presenter<T> where T : View
    {
        protected T View { get; }

        protected Presenter(T view)
        {
            View = view;
        }

        public void Show() => View.Show();

        public virtual void Hide() => View.Hide();
    }
}
```

**Базовый класс View** (`View.cs`):
```csharp
using UnityEngine;

namespace UserInterface
{
    public abstract class View : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
```

### 🔌 **Dependency Injection с Zenject**

**Installer для геймплея** (`GameplayInstaller.cs`):
```csharp
using Services.Cameras;
using Services.ViewContainerProviders;
using UnityEngine;
using Zenject;

namespace Infrastructure.Scenes.Hubs
{
    public class GameplayInstaller : MonoInstaller, IInitializable
    {
        [Inject]
        private ViewContainerProvider _viewContainerProvider;

        [Inject]
        private ICameraProvider _cameraProvider;

        public Transform Common;
        public Transform Blocks;
        public Transform Enemies;
        public Transform Map;
        public Camera Camera;

        public override void InstallBindings()
        {
             Container.BindInterfacesAndSelfTo<GameplayInstaller>().FromInstance(this).AsSingle().NonLazy();
        }

        public void Initialize()
        {
            _cameraProvider.Camera = Camera;
            _viewContainerProvider.CommonContainer = Common;
            _viewContainerProvider.BlockContainer = Blocks;
            _viewContainerProvider.EnemyContainer = Enemies;
            _viewContainerProvider.MapContainer = Map;
        }
    }
}
```

### 🎮 **State Machine**

**Состояние игрового цикла** (`BattleLoopState.cs`):
```csharp
using Game.GameMainFeature;
using Infrastructure.States.StateInfrastructure;
using Services.SystemsFactoryServices;
using UserInterface.GameplayHeadsUpDisplay;

namespace Infrastructure.States.GameStates
{
    public class BattleLoopState : IState, IUpdateable
    {
        private readonly ISystemFactory _systems;
        private readonly GameContext _gameContext;
        private readonly GameplayHeadsUpDisplayPresenter _gameplayHeadsUpDisplayPresenter;

        private GameplayFeature _gameplayFeature;

        public BattleLoopState(
            ISystemFactory systems,
            GameContext gameContext,
            GameplayHeadsUpDisplayPresenter gameplayHeadsUpDisplayPresenter
        )
        {
            _systems = systems;
            _gameContext = gameContext;
            _gameplayHeadsUpDisplayPresenter = gameplayHeadsUpDisplayPresenter;
        }

        public void Enter()
        {
            _gameplayFeature = _systems.Create<GameplayFeature>();
            _gameplayFeature.Initialize();

            _gameplayHeadsUpDisplayPresenter.Enable();
        }

        public void Update()
        {
            if (_gameplayFeature == null)
                return;

            _gameplayFeature.Execute();

            _gameplayFeature?.Cleanup();
        }

        public void Exit()
        {
            _gameplayFeature.DeactivateReactiveSystems();
            _gameplayFeature.ClearReactiveSystems();

            DestructEntities();

            _gameplayFeature.Cleanup();
            _gameplayFeature.TearDown();
            _gameplayFeature = null;

            _gameplayHeadsUpDisplayPresenter.Disable();
        }

        private void DestructEntities()
        {
            foreach (GameEntity entity in _gameContext.GetEntities())
                entity.isDestructed = true;
        }
    }
}
```

### 🎪 **Feature система башен**

**Организация систем башен** (`TowerFeature.cs`):
```csharp
using Game.Towers.MergeSpirits.Systems;
using Game.Towers.SelectSpirits.Systems;
using Services.SystemsFactoryServices;

namespace Game.Towers
{
    public sealed class TowerFeature : Feature
    {
        public TowerFeature(ISystemFactory systems)
        {
            Add(systems.Create<RotateTowerSystem>());
            
            Add(systems.Create<SelectSpiritSystem>());
            
            Add(systems.Create<SpiritPlacedReactiveSystem>());

            MergeSystems(systems);
        }

        private void MergeSystems(ISystemFactory systems)
        {
            Add(systems.Create<MarkMergeableSpiritsSystem>());
            Add(systems.Create<FilterMergeableSpiritsSystem>());
            Add(systems.Create<FinilizeMergeSpiritsSystem>());
        }
    }
}
```

**Реальный пример MVP Presenter** (`GameplayHeadsUpDisplayPresenter.cs`):
```csharp
using System;
using Entitas;
using Game.Battle;
using Infrastructure.States.StateMachine;
using Services.StaticData;
using UnityEngine;
using UserInterface.GameplayHeadsUpDisplay.CheatsPanel;
using UserInterface.GameplayHeadsUpDisplay.FinishPanel;
using UserInterface.GameplayHeadsUpDisplay.GoldHealthPanels;
using UserInterface.GameplayHeadsUpDisplay.InfoPanel;
using UserInterface.GameplayHeadsUpDisplay.ObjectInfoPanel;
using UserInterface.GameplayHeadsUpDisplay.PlayerAbilityPanel;
using UserInterface.GameplayHeadsUpDisplay.PlayerAbilityPanel.SwapAbility;
using UserInterface.GameplayHeadsUpDisplay.PlayerPanel;
using UserInterface.GameplayHeadsUpDisplay.TimerPanel;
using Zenject;

namespace UserInterface.GameplayHeadsUpDisplay
{
    public class GameplayHeadsUpDisplayPresenter :
        Presenter<GameplayHeadsUpDisplayView>,
        IInitializable
    {
        private readonly ChooseTowerPanelPresenter _chooseTowerPanelPresenter;
        private readonly TowerMergePanelPresenter _towerMergePanelPresenter;
        private readonly TimerPanelPresenter _timerPanelPresenter;
        private readonly PlayerPanelPresenter _playerPanelPresenter;
        private readonly PlayerAbilityPanelPresenter _playerAbilityPanelPresenter;
        private readonly CheatsPanelPresenter _cheatsPanelPresenter;
        private readonly InfoPanelPresenter _infoPanelPresenter;
        private readonly TowerSwapAbilityPanelPresenter _towerSwapAbilityPanelPresenter;
        private readonly PausePanelPresenter _pausePanelPresenter;
        private readonly FinishPanelPresenter _finishPanelPresenter;
        private readonly ObjectInfoPanelPresenter _objectInfoPanelPresenter;
        private readonly GoldHealthPanelPresenter _goldHealthPanelPresenter;

        private readonly GameContext _gameContext;
        private readonly IGameStateMachine _stateMachine;
        private readonly IStaticDataService _staticDataService;

        private IGroup<GameEntity> _spirits;
        private IGroup<GameEntity> _humans;
        private IGroup<GameEntity> _aliveEnemies;

        private float _tickTime;

        public event Action SettingPanelActivated;
        
        // ... остальная реализация
    }
}
```

---

## 🎮 Игровые системы

### 🏰 **Система башен**
- **42 уникальные башни**: 7 типов × 6 уровней каждый
- **Механика слияния**: Объединение башен для создания более сильных вариантов
- **Стратегическое размещение**: Система позиционирования на сетке
- **Пути улучшения**: Множественные маршруты прогрессии для каждого типа башни

| Тип | Уровни | Описание | Особая способность |
|-----|--------|----------|-------------------|
| **B** (Синие) | 1-6 | ❄️ Ледяные башни с замедлением | Заморозка врагов |
| **D** (Алмазные) | 1-6 | 💎 Высокий урон кристальных башен | Цепная молния |
| **Y** (Жёлтые) | 1-6 | ⚡ Быстрая стрельба молниями | Область поражения |
| **G** (Зелёные) | 1-6 | 🌿 Природные башни с ядом | Ядовитое облако |
| **E** (Изумрудные) | 1-6 | 💚 Сбалансированный урон | Множественный выстрел |
| **Q** (Кварцевые) | 1-6 | 🔮 Магические башни | Телепорт врагов |
| **R** (Рубиновые) | 1-6 | 🔥 Огненные башни с поджигом | Взрывной урон |

### 👹 **ИИ система врагов**
- **Поиск пути**: Алгоритм A* для навигации по лабиринту
- **Динамический спавн**: Генерация врагов по волнам
- **Поведенческий ИИ**: Различные типы врагов с уникальными способностями
- **Оптимизация производительности**: ECS обеспечивает плавную работу с сотнями сущностей

### ⚔️ **Боевая система**
- **Бои в реальном времени**: Плавные сражения на 60fps
- **Типы урона**: Физический, магический, элементальный урон
- **Статусные эффекты**: Заморозка, поджиг, отравление, замедление
- **Система комбо**: Синергии и комбинации башен

---

## 🛠️ Технологический стек

<div align="center">

| **Категория** | **Технология** | **Назначение** |
|---------------|----------------|----------------|
| **🎮 Движок** | Unity 6000.0.38f1 | Платформа разработки игр |
| **💻 Язык** | C# 10.0 | Основной язык программирования |
| **🏗️ Архитектура** | Entity Component System | Производительная игровая архитектура |
| **📦 ECS Framework** | Entitas | Генерация кода и реализация ECS |
| **🔌 Dependency Injection** | Zenject (Extenject) | Модульная организация кода |
| **🎨 UI Pattern** | MVP (Model-View-Presenter) | Чистая архитектура UI |
| **🔊 Аудио** | Unity Audio System | Управление звуком |
| **📱 Платформы** | PC, Mac, WebGL | Мультиплатформенная поддержка |

</div>

---

## 🎯 Слои архитектуры

### **ECS Layer** (`Scripts/ECS/`)
- **Components** - Данные игровых объектов (здоровье, позиция, тип башни)
- **Systems** - Игровая логика (движение, стрельба, AI врагов)
- **Features** - Группировка систем по функциональности
- **Contexts** - Управление жизненным циклом сущностей

### **Infrastructure Layer** (`Scripts/Infrastructure/`)
- **States** - Управление переходами между экранами
- **Installers** - Настройка DI контейнера
- **Scene Management** - Загрузка и переключение сцен

### **Service Layer** (`Scripts/Services/`)
- **Cross-cutting concerns** - Аудио, камеры, ассеты, сохранения
- **Провайдеры** - Абстракция над Unity API
- **Фабрики** - Создание игровых объектов

### **Presentation Layer** (`Scripts/UserInterface/`)
- **MVP паттерн** - Разделение UI логики от данных
- **Презентеры** - Бизнес-логика интерфейса
- **Представления** - Unity UI компоненты

---

## 🔧 Особенности реализации

### **Модульность**
Каждая игровая механика (башни, враги, боевка) выделена в отдельную Feature с собственными компонентами и системами.

### **Кастомные инструменты**
- **MazeDesigner** - Редактор лабиринтов в Unity Editor
- **Entitas CodeGeneration** - Автогенерация ECS кода
- **Visual Debugging** - Отображение состояний систем

### **Разделение ответственности**
- Игровая логика отделена от представления
- Сервисы изолированы через интерфейсы
- Конфигурация вынесена в ScriptableObjects

---

## 📚 Техническая реализация

### **ECS в действии**
Использование Entity Component System для управления игровыми объектами, где каждая башня, враг или снаряд представлены как сущность с набором компонентов.

### **MVP архитектура UI**
Каждая панель интерфейса разделена на View (Unity UI), Presenter (логика) и Model (данные из ECS). Это обеспечивает тестируемость и переиспользование.

### **Dependency Injection**
Все сервисы и зависимости управляются через Zenject контейнер, что позволяет легко подменять реализации и тестировать компоненты изолированно.

### **State Management**
Игровые состояния (меню, загрузка, геймплей) управляются через паттерн State Machine с чёткими переходами между состояниями.

---

<div align="center">

### 🔍 **Исходный код tower defense игры с современной архитектурой** 

**🎮 [Играть](https://stoneprophet.itch.io/gemtd) • 📖 [Архитектура](#-архитектура-проекта) • 🔧 [Реализация](#-примеры-реализации-архитектурных-паттернов)**

</div>
