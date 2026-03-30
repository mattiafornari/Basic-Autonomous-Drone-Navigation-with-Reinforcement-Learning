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
  - [PBR Racing Drone](https://assetstore.unity.com/packages/3d/vehicles/air/pbr-racing-drone-59751?srsltid=AfmBOop3NtzRlwhv-41Utkvh3fRiJrMKwiQodaFLxWgaGDdRJrSAtCaA)
  - [Low Poly Nature Pack](https://assetstore.unity.com/...](https://assetstore.unity.com/packages/3d/environments/landscapes/low-poly-simple-nature-pack-162153?srsltid=AfmBOorDtnp7c2YJke3KIYKSZQ0-gS2Am5uJKjSpoZ0C-AD5zH5Ktp5p))
- **Development Support**: Claude AI (Anthropic) for coding assistance
- **References**: Unity ML-Agents Documentation, PPO paper (Schulman et al., 2017)

## 📄 License

This project is licensed under the MIT License - see LICENSE file for details.

## 📧 Contact

Mattia Fornari - [email] - [LinkedIn]

Project Link: [https://github.com/mattiafornari/Basic-Autonomous-Drone-Navigation-with-Reinforcement-Learning](https://github.com/mattiafornari/Basic-Autonomous-Drone-Navigation-with-Reinforcement-Learning)
```
