# RemotelyControlledRobot

The RemotelyControlledRobot project consists of multiple components that allow remote control of a *Alphabot PI acce pack* using console commands and SignalR communication. The core component, "RemotelyControlledRobot.Core" serves as the heart of the robot's software and can be extended with new features.

## Features

- Control the robot using console commands:
  - Move the robot forward, backward, left, and right.
  - Adjust the camera's orientation (left, right, ahead).
  
- Control the robot's speed and direction using SignalR:
  - Achieve smoother and more precise robot movements.
  - Set the robot's speed and direction for smoother control.
  
- Control the camera using SignalR:
  - Adjust the camera's orientation with precision along both the horizontal (X) and vertical (Y) axes.

- Camera Module (RemotelyControlledRobot.IoT.Camera):
  - Provides a simple HTTP server for streaming live video from the robot.
  - The camera server runs on port 8080.

- Web API Module (RemotelyControlledRobot.WebApi):
  - Contains a RobotHub for SignalR communication.
  - Listens on port 5047 for SignalR connections.

## Usage

1. **Console Control:**
   - `ConsoleController` responsible for control the robot via the console.
   - Use keys 'W', 'A', 'S', 'D' to move the robot and arrow keys to adjust the camera orientation.
   - Press 'Q' to exit.

2. **SignalR Control:**
   - The `SignalRCommandController` allows controlling the robot's speed and direction via SignalR commands.

3. **Camera Streaming:**
   - The `RemotelyControlledRobot.IoT.Camera` project provides an HTTP server for streaming live video from the robot's camera.
   - Access the camera stream via: http://localhost:8080

4. **Web API for SignalR:**
   - The `RemotelyControlledRobot.WebApi` project contains a SignalR hub for controlling the robot remotely.
   - Connect to the RobotHub using SignalR clients.
   
### SignalR Communication

The `RemotelyControlledRobot.WebApi` project includes a SignalR hub called `RobotHub` for controlling the robot remotely. You can use this hub to send commands and receive data from the robot.

#### SignalR Commands

- **SendSpeedAndDirection:**
  Use to control the robot's speed and direction.
  - Parameters:
    - `speed` (double): The desired speed of the robot.
    - `direction` (double): The desired direction of the robot's movement.

- **SendCameraAngle:**
  Use to control the camera's orientation.
  - Parameters:
    - `angleX` (int): The desired camera angle along the horizontal axis.
    - `angleY` (int): The desired camera angle along the vertical axis.

### Connecting to the RobotHub

To establish a connection to the RobotHub in your client application, you can use the SignalR client library for your preferred platform. Refer to the SignalR documentation for your specific platform for more information on how to connect to and interact with the SignalR hub.

## Installation

1. Clone this repository.
2. Build and run the required projects:
   - `RemotelyControlledRobot.Core`
   - `RemotelyControlledRobot.IoT.Camera`
   - `RemotelyControlledRobot.WebApi`

## License

This project is open-source and available under the [MIT License](LICENSE).

## Contributors

- Petro Kopyl

