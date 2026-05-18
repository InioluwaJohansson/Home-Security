# 🏠 Home Security System

A real-time smart home security and automation system built with ASP.NET Core, SignalR, MQTT, ESP32 microcontrollers, and React/TypeScript frontend integration.

This application provides centralized control and monitoring for:

* Doors
* Windows
* Lights
* Appliances
* Cameras
* Sensors
* Automated Actions
* Fingerprint Authentication
* Real-time Device Synchronization
* Logs and Activity Monitoring

---

# 🚀 Features

## 🔐 Authentication & Authorization

* JWT Authentication
* Role-Based Access Control
* Secure password hashing using BCrypt
* Authorization codes
* SignalR authenticated real-time connections

---

## 👤 Person Management

* Create and manage users
* Assign roles
* Upload profile images
* Add multiple addresses
* Add contact details
* Fingerprint enrollment
* Disable/Delete users

---

## 🏡 Smart Home Management

### Rooms & Sections

* Create sections
* Create rooms
* Group facilities by room or section

### Facility Control

Supports:

* Lights
* Appliances
* Doors
* Windows
* Cameras

Each facility supports:

* Real-time status updates
* Active/Inactive states
* Lock/Open states
* Brightness control
* Power monitoring

---

## ⚡ Smart Automation Actions

Create reusable automation actions like:

### Example:

"Night Mode"

Can:

* Lock all doors
* Close windows
* Dim lights
* Turn off appliances
* Enable cameras

Actions can be triggered:

* Manually
* By sensors
* By external hardware
* Automatically

---

## 📡 ESP32 / Microcontroller Integration

Microcontrollers communicate with backend APIs using:

* HardwareId
* AuthKey
* Timestamps

Supports:

* Real-time synchronization
* Facility state polling
* State feedback verification
* Hardware-device mapping

---

## 📶 Real-Time Communication

Built using SignalR.

### Real-time updates include:

* Device state changes
* Action execution
* Logs
* User changes
* Camera updates
* Sensor activity

Authenticated users automatically receive updates instantly.

---

## 🧠 Fingerprint Authentication

Supports:

* Fingerprint enrollment
* Fingerprint verification
* Biometric access control

---

## 📹 Camera System

Supports:

* External cameras
* Camera state control
* RTSP streaming integration
* Real-time camera updates

---

## 📜 Activity Logs

Tracks:

* Device activations
* User actions
* Door/window activity
* Automation triggers
* Camera activity
* Security events

---

# 🛠️ Tech Stack

## Backend

* ASP.NET Core Web API
* Entity Framework Core
* SignalR
* MQTT
* JWT Authentication

## Frontend

* React
* TypeScript
* React Router

## Hardware

* ESP32
* ACS712 Current Sensor
* ZMPT101B Voltage Sensor
* Relays
* OLED SSD1306 Display
* RTC Modules

## Database

* MySQL / SQL Server

---

# 📂 Project Structure

```bash
Home_Security/
│
├── Controllers/
├── Entities/
├── Interfaces/
├── Implementations/
├── DTOs/
├── Hubs/
├── RealTimeServices/
├── Streaming/
├── Repositories/
├── Context/
├── Middleware/
└── Helpers/
```

---

# 🔧 Installation

## 1. Clone Repository

```bash
git clone <repository-url>
```

---

## 2. Configure Database

Update:

```json
appsettings.json
```

```json
"ConnectionStrings": {
  "DefaultConnection": "your_connection_string"
}
```

---

## 3. Configure JWT

```json
"JwtSettings": {
  "Key": "your_secret_key"
}
```

---

## 4. Install Dependencies

```bash
dotnet restore
```

---

## 5. Run Migrations

```bash
dotnet ef database update
```

---

## 6. Start Application

```bash
dotnet run
```

---

# 📡 SignalR Setup

## Register SignalR

```csharp
builder.Services.AddSignalR();
```

---

## Map Hub

```csharp
app.MapHub<HomeSecurityHub>("/homeSecurityHub");
```

---

## Frontend SignalR Example

```typescript
import * as signalR from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:5001/homeSecurityHub", {
    accessTokenFactory: () => token
  })
  .withAutomaticReconnect()
  .build();

await connection.start();
```

---

# 🔔 SignalR Events

Examples:

```csharp
await _realtimeNotificationService.NotifyAuthenticatedUsers(
    "ActionUpdated",
    payload
);
```

---

## Example Frontend Listener

```typescript
connection.on("ActionUpdated", (data) => {
    console.log(data);
});
```

---

# 📡 MQTT Integration

Supports:

* ESP32 communication
* Real-time hardware updates
* Device synchronization

Example topics:

```bash
home/+/state
home/+/sync
```

---

# 🔒 Security

* JWT secured APIs
* BCrypt password hashing
* SignalR authenticated connections
* Authorization-based device control
* Secure hardware authentication

---

# ⚙️ Automation Workflow

```text
Sensor Trigger
      ↓
External Device
      ↓
Action Service
      ↓
Facility Services
      ↓
SignalR Notifications
      ↓
Frontend Real-Time Update
```

---

# 📱 Supported Smart Facilities

| Facility   | Features           |
| ---------- | ------------------ |
| Lights     | Brightness, On/Off |
| Doors      | Lock/Open          |
| Windows    | Lock/Open          |
| Cameras    | Enable/Disable     |
| Appliances | Power Control      |

---

# 🧪 Testing

## Swagger

```bash
https://localhost:5001/swagger
```

---

# 🧠 Future Improvements

* AI intrusion detection
* Facial recognition
* Voice assistant integration
* Mobile app
* Push notifications
* Cloud synchronization
* Energy analytics dashboard
* Multi-home support

---

# 👨‍💻 Author

Developed by Inioluwa Johansson.

---

# 📄 License

This project is for educational and research purposes.
