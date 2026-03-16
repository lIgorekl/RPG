# Unity RPG Architecture Project

Учебный проект 3D RPG игры, разработанный на **Unity (C#)** в рамках дисциплины **Архитектура программного обеспечения**.

Основная цель проекта — реализовать игровые механики с правильной архитектурой кода, разделением ответственности и использованием архитектурных паттернов.

---

# Основные механики

В текущей версии реализованы базовые игровые системы.

## Игрок

* передвижение (ходьба / бег)
* управление камерой
* ближняя атака
* магическая атака (снаряды)
* система HP
* получение урона
* стан при получении урона
* кулдауны способностей
* смерть игрока
* UI здоровья
* UI кулдауна способности

---

## Враги

Реализовано два типа врагов.

### Ближний враг (Melee Enemy)

* патрулирование (wander)
* обнаружение игрока
* преследование
* атака вблизи
* кулдаун атаки
* стан при получении урона
* смерть

### Дальний враг (Ranged Enemy)

* обнаружение игрока
* поддержание дистанции
* стрельба снарядами
* кулдаун атаки
* стан при получении урона
* смерть

---

# AI система врагов

Поведение врагов реализовано через **State Machine**.

### Состояния ближнего врага

```
Idle
↓
Chase
↓
Attack
```

### Состояния дальнего врага

```
Idle
↓
MaintainDistance
↓
Attack
```

---

# Боевая система

Боевая система построена через абстракции урона.

### Основные компоненты

```
Damage
IDamageable
IDamageDealer
Health
DamageableEntity
```

### Цепочка нанесения урона

```
Weapon / Projectile
        ↓
      Damage
        ↓
    IDamageable
        ↓
DamageableEntity
        ↓
      Health
```

---

# Архитектура проекта

Проект разделён на несколько слоёв.

```
Core
Gameplay
Presentation
```

---

## Core

Содержит базовые абстракции системы.

```
Core
 ├ Combat
 │   ├ Damage
 │   ├ IDamageable
 │   └ IDamageDealer
 │
 ├ Stats
 │   └ IHealth
 │
 └ Gameplay
     └ Cooldown
```

---

## Gameplay

Содержит игровую логику и доменные сущности.

```
Gameplay
 ├ Characters
 │   ├ BaseCharacter
 │   ├ PlayerEntity
 │   ├ EnemyEntity
 │   └ DamageableEntity
 │
 └ Stats
     ├ CharacterStats
     └ Health
```

---

## Presentation

Слой взаимодействия с Unity.

```
Presentation
 ├ Player
 │   ├ PlayerController
 │   ├ PlayerMovement
 │   ├ PlayerCombat
 │   └ PlayerCameraController
 │
 ├ AI
 │   ├ EnemyBehaviour
 │   ├ RangedEnemyBehaviour
 │   ├ EnemyStateMachine
 │   └ Enemy States
 │
 ├ Combat
 │   ├ ProjectileView
 │   └ SwordHitbox
 │
 ├ Scene
 │   ├ BaseEnemyView
 │   ├ MeleeEnemyView
 │   └ RangedEnemyView
 │
 └ UI
     ├ HPBarView
     ├ EnemyHealthBarView
     ├ MagicCooldownView
     └ GameOverView
```

---

# Используемые архитектурные принципы

В проекте применяются следующие принципы.

### SOLID

* **SRP** — разделение ответственности
* **OCP** — возможность расширения без изменения базового кода
* **DIP** — использование интерфейсов (`IDamageable`, `IHealth`)

---

### Архитектурные паттерны

* **State Machine** — поведение врагов
* **Observer** — события HP и смерти
* **Entity / Domain Model** — игровые сущности
* **Separation of Concerns** — разделение слоёв

---

# UI система

UI построен на основе событий.

Это позволяет обновлять интерфейс только при изменении состояния, а не каждый кадр.

---

# Цель проекта

Главная цель проекта — продемонстрировать:

* архитектуру игрового кода
* модульность системы
* слабую связанность компонентов
* правильное разделение ответственности
