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

| Metric | Few Obstacles |
|--------|---------------|
| Success Rate | 100% |
| Avg. Time | 4.49s |
| Collisions | 0 |

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
The reward at timestep *t* is computed as:
```
r_t = r_progress + r_time + r_upright + r_altitude + r_collision + r_terminal
```

Where:

- **r_progress** = `0.5 × (d_{t-1} - d_t)` — progress toward goal
- **r_time** = `-0.001` — step penalty (encourages efficiency)
- **r_upright** = `0.02 × (uprightness - 0.7)` if uprightness < 0.7, else 0 — penalizes excessive tilting
- **r_altitude** = `-0.003` if height_error < -20m — penalizes flying too low
- **r_collision** = `-3` (ground/boundary) or `-5` (obstacle)
- **r_terminal** = `+15` (success) | `-5` (out of bounds)

Uprightness is defined as the dot product between the drone's up vector and world up: `up · (0,1,0)`


## 🙏 Acknowledgments

- **Assets**: 
  - [PBR Racing Drone](https://assetstore.unity.com/packages/3d/vehicles/air/pbr-racing-drone-59751)
  - [Low Poly Nature Pack](https://assetstore.unity.com/packages/3d/environments/landscapes/low-poly-simple-nature-pack-162153)
- **Development Support**: Claude AI (Anthropic) for coding assistance
- **References**: Unity ML-Agents Documentation, PPO paper (Schulman et al., 2017)

## 📄 License

This project is licensed under the MIT License - see LICENSE file for details.

Project Link: [https://github.com/mattiafornari/Basic-Autonomous-Drone-Navigation-with-Reinforcement-Learning](https://github.com/mattiafornari/Basic-Autonomous-Drone-Navigation-with-Reinforcement-Learning)

