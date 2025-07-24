# 🔬 BioLED Controller  
**Modern WPF-based desktop application for precise control of Mightex BioLED devices**

![Main Window - Light Mode](https://github.com/user-attachments/assets/846872fc-f9ca-4495-8687-47de6e9c514e)

---
<img width="5334" height="2994" alt="original to modern UX-01" src="https://github.com/user-attachments/assets/bfb02609-54ab-4604-ba11-3408556982c5" />

## ✨ Summary
BioLED Controller is a modern, space-efficient and user-friendly Windows application designed to control **Mightex BioLED light modules** (e.g., BLS-series) for use in laboratory and microscopy environments. Developed using **C# and WPF**, the application prioritizes UI simplicity, compact design, and hardware integration via both **DLL-based SDKs** and **custom Arduino interfaces**.

---

## 💡 Key Features
- ✅ **Compact UX**: Uses only 2–9% of screen real estate compared to 19% for Mightex’s original controller.
- 🎨 **Modern Material Design UI**: Implemented using [MaterialDesignInXAML Toolkit](https://github.com/MaterialDesignInXAML).
- 🌓 **Dark Mode Support**: Designed for light-sensitive lab environments.
- 📦 **Resizability**: Scales from full UI to ultra-compact mode without losing functionality.
- 🔌 **Hardware Integration**:
  - Direct control of BLS-series devices via `Mightex_BLSDriver_SDK.dll`.
  - Serial communication to custom Arduino hardware for additional controls.

---

## 🧪 Enhanced Functionality

### Clean Menus and Dialogs
| Edit LED Dialog (light) | Menu (dark) |
|--------------------------|-------------|
| ![Edit Dialog](https://github.com/user-attachments/assets/1c18cf31-23e7-458c-8cf3-4aad584c495f) | ![Menu Dark](https://github.com/user-attachments/assets/d285bf93-55dc-46e8-ac94-617221c17b06) |

### Customizable Presets
Users can create and switch between preset intensity levels for rapid configuration changes.

![LED Intensity Presets](https://github.com/user-attachments/assets/142deb44-40a3-4059-8051-202b905b3ef8)

---

## 🔧 Technical Stack
- **Frontend**: WPF (.NET 9.0), MaterialDesignXAML
- **Backend**: C#, MVVM pattern, DLL interop
- **Hardware**:
  - `Mightex_BLSDriver_SDK.dll` for LED control
  - `SerialPort` communication with Arduino microcontrollers
- **Setup**: Visual Studio solution with deployment packaging (`.vdproj`)

---

## 🛠 Additional Tools
Includes a standalone legacy WinForms utility (`CommunicateWithArduino`) to test and debug serial communication with Arduino-based devices.

---

## 💻 Example Use Cases
- Neuroscience or fluorescence microscopy labs needing compact, responsive LED control.
- Teaching labs integrating DIY Arduino-based controllers.
- Developers modernizing legacy device interfaces for clinical or research environments.
