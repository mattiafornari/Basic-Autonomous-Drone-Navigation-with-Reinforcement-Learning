# Autonomous Drone Navigation using Reinforcement Learning

<!--![Demo GIF](Demo/demo.gif)-->

A Unity-based simulation of autonomous quadcopter navigation using Deep Reinforcement Learning (PPO algorithm) for Computer Graphics course project.

<!--## 🎥 Demo Video

[Watch Full Demo](link-to-video)-->

## 📋 Project Overview

- **Course**: Fundamentals of Computer Graphics A.A. 2024-2025
- **Student**: Mattia Fornari 
- **Objective**: Train a drone to navigate from spawn to goal while avoiding obstacles
- **Algorithm**: Proximal Policy Optimization (PPO)
- **Framework**: Unity ML-Agents

## 🛠️ Technologies Used

- Unity 6 (6000.0.40f1)
- ML-Agents Toolkit 1.1.0
- Python 3.10.12
- PyTorch 2.2.2
- TensorBoard

## 🎮 Features

- Autonomous navigation using deep RL
- Ray-based obstacle detection
- Real-time GUI with mission metrics
- Two experimental scenarios (Empty / Few Obstacles)
- PBR rendering with URP
- Soft shadows and realistic physics

## 📊 Results

| Metric | Empty Scene | Few Obstacles |
|--------|-------------|---------------|
| Success Rate | XX% | YY% |
| Avg. Time | XX.Xs | YY.Ys |
| Collisions | X.X | Y.Y |

## 🚀 How to Run

### Prerequisites
Follow https://docs.unity3d.com/Packages/com.unity.ml-agents@4.0/manual/Installation.html
```bash
# Python environment
conda create -n ml-agents python=3.10
conda activate ml-agents
pip install mlagents==1.1.0
```

### Training
```bash
mlagents-learn Assets/Configs/trainer_config.yaml \
  --run-id=my_training \
  --env=Builds/Empty/DroneEnv
```

### Inference (Play Mode)
1. Open Unity project
2. Load scene: `Assets/Scenes/TrainingScene_Empty.unity`
3. Drone → Behavior Parameters:
   - Model: `Results/empty_final/DroneAgent.onnx`
   - Behavior Type: Inference Only
4. Press Play

## 📁 Project Structure
```
Assets/
├── Scenes/          # Unity scenes (Empty, Few)
├── Scripts/         # C# agent scripts
├── Configs/         # ML-Agents YAML configs
└── Materials/       # PBR materials

Results/             # Trained models (.onnx)
Documentation/       # Report & figures
```

## 🎓 Key Implementation Details

### State Space (14D)
- Direction to goal (local frame)
- Linear/angular velocity
- Drone orientation
- Normalized distance

### Action Space (4D)
- Thrust, Pitch, Roll, Yaw (continuous [-1, 1])

### Reward Function
```
r_t = 0.5*(d_{t-1} - d_t) - 0.001 + r_upright + r_terminal
```

## 📖 Documentation

- [Full Report](Documentation/Report.pdf)
- [Progress Report](Documentation/ProgressReport.pdf)
- [Project Proposal](Documentation/ProjectProposal.pdf)

## 🙏 Acknowledgments

- **Assets**: 
  - [PBR Racing Drone](https://assetstore.unity.com/...)
  - [Low Poly Nature Pack](https://assetstore.unity.com/...)
- **Development Support**: Claude AI (Anthropic) for coding assistance
- **References**: Unity ML-Agents Documentation, PPO paper (Schulman et al., 2017)

## 📄 License

This project is licensed under the MIT License - see LICENSE file for details.

## 📧 Contact

Mattia Fornari - [email] - [LinkedIn]

Project Link: [https://github.com/mattiafornari/Basic-Autonomous-Drone-Navigation-with-Reinforcement-Learning](https://github.com/mattiafornari/Basic-Autonomous-Drone-Navigation-with-Reinforcement-Learning)
```

---

## 🗂️ .gitignore Template
```
# Unity generated
[Ll]ibrary/
[Tt]emp/
[Oo]bj/
[Bb]uild/
[Bb]uilds/
[Ll]ogs/
[Uu]ser[Ss]ettings/

# Visual Studio cache
.vs/
*.csproj
*.unityproj
*.sln
*.suo
*.user

# OS generated
.DS_Store
Thumbs.db

# ML-Agents results (large files)
results/*/events.out.tfevents.*

# Keep .onnx models
!results/**/*.onnx
!results/**/configuration.yaml
