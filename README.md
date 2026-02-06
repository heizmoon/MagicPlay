# [Project Name] - Real-time Roguelike Card Battler

![Unity Version](https://img.shields.io/badge/Unity-2021.3.4f1-blue.svg?logo=unity)
![Platform](https://img.shields.io/badge/Platform-Android%20|%20iOS-green.svg)
![License](https://img.shields.io/badge/License-MIT-yellow.svg) ![Status](https://img.shields.io/badge/Status-Open%20for%20Dev-orange.svg)

> A Unity 2D mobile game project blending Roguelike progression with Real-time Card Battle mechanics.

## 📖 About The Project | 项目简介

这是一个基于 **Unity 2D** 开发的手机游戏项目，核心玩法结合了 **Roguelike** 的随机探索与成长要素，以及 **即时制卡牌对战 (Real-time Card Battle)** 的策略性。

本项目旨在提供一个稳健的开发底座（Base），代码结构清晰，易于扩展。无论是想要学习 Unity 2D 开发，还是希望在此基础上制作完整商业项目的开发者，都可以自由使用、修改和交流。

**注意 (Note):** 本项目基于较早期的开发版本整理开源，与我其他的游戏项目（如 *YoyoGirl* 等）无关，请作为独立项目看待。

### Key Features | 核心特性

* **即时卡牌战斗系统 (Real-time Combat):** 区别于回合制，采用费用自动回复与即时出牌机制，强调反应与决策。
* **Roguelike 循环:** 随机生成的地图路径、随机事件以及战斗后的卡牌/遗物获取 (Drafting)。
* **Unity 2D 架构:** 使用 Unity 2D 工具集（Sprite Renderer, Tilemap, Unity UI）构建。
* **模块化设计:** 卡牌效果、敌人行为、关卡生成均已抽象为易于扩展的 ScriptableObject 或基类。

## 🛠️ Built With | 技术栈

* **Engine:** Unity **2021.3.4f1** (LTS)
* **Language:** C#
* **IDE:** Visual Studio / JetBrains Rider / VS Code

## 🚀 Getting Started | 快速开始

所有人都可以在此版本基础上进行开发。请遵循以下步骤配置环境。

### Prerequisites | 前置要求

* 确保安装 **Unity Hub**。
* 安装 Unity 版本 **2021.3.4f1**。如果尚未安装，请在 Unity Hub 的 "Installs" 选项卡中添加此版本（建议包含 Android/iOS Build Support 模块）。

### Installation | 安装步骤

1.  克隆本仓库到本地：
    ```sh
    git clone [https://github.com/yourusername/your-repo-name.git](https://github.com/yourusername/your-repo-name.git)
    ```
2.  打开 Unity Hub，点击 "Open"，选择项目根目录。
3.  等待 Unity 导入资源（首次打开可能需要几分钟）。
4.  打开 `Assets/Scenes/MainMenu` (或 `GameStart`) 场景开始运行。

## 🗺️ Roadmap & TODO | 路线图与待办

目前项目处于基础版本，欢迎社区成员针对以下方向进行贡献：

- [ ] **卡牌系统扩展:** 添加更多类型的卡牌效果（Buff/Debuff/AoE）。
- [ ] **视觉表现优化:** 增加卡牌打出的特效与打击感反馈。
- [ ] **数值平衡:** 调整敌人血量与卡牌消耗的平衡性。
- [ ] **保存系统:** 完善 Roguelike 的局内存档 (Run Save) 功能。

## 🤝 Contributing | 如何贡献

非常欢迎任何形式的贡献！无论是修复 Bug、提交新功能还是优化文档。

1.  **Fork** 本仓库。
2.  基于 `main` 分支创建您的特性分支 (`git checkout -b feature/AmazingFeature`)。
3.  提交您的更改 (`git commit -m 'Add some AmazingFeature'`)。
4.  推送到分支 (`git push origin feature/AmazingFeature`)。
5.  提交 **Pull Request**。

*如果您有关于游戏设计的想法，也欢迎在 [Issues] 区发起讨论。*

## 📄 License | 许可协议

本项目基于 MIT License 分发。详情请参阅 `LICENSE` 文件。
(This project is open source and available under the MIT License.)

## 📮 Contact | 联系方式

* **Project Maintainer:** [ ]
* **Email:** [heizmoonlight@gmail.com]
* **Issue Tracker:** [ ]

---
*Created with ❤️ for the Unity Developer Community.*
