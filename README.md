# Homebridge Speaker App

## Overview

**Homebridge Speaker App** is a Windows Forms application that provides an intuitive interface for controlling Homebridge-connected speakers directly from the system tray. The app allows users to toggle their speakers on or off and view the current status, all from the convenience of their desktop.

## Features

- **Tray Icon Control**: Quickly toggle the speaker status (On/Off) using the system tray icon.
- **Status Indicator**: The tray icon updates dynamically to reflect the current speaker status.
- **Context Menu Options**:
  - **Toggle On**: Turn on the speakers.
  - **Toggle Off**: Turn off the speakers.
  - **Logs**: View the application's logs.
  - **Exit**: Close the application.
- **Automatic Token Management**: Handles Homebridge authentication and token renewal seamlessly.

## Prerequisites

- **Windows OS**: The application is designed for Windows operating systems.
- **.NET Framework**: Requires .NET Framework 4.7.2 or later.
- **Homebridge Server**: A running Homebridge server with appropriate accessories configured.

## Installation

1. **Download** the latest release from the [Releases](https://github.com/edisonduong/HomebridgeSpeakerApp/releases) section.
2. **Extract** the contents of the downloaded ZIP file to a desired location.
3. **Run** the `Homebridge Speakers.exe` file to launch the application.

## Optional: Start the App Automatically on Windows Startup

To ensure the Homebridge Speaker App launches automatically when you start your computer, follow these steps:

1. Press `Win + R` to open the Run dialog.
2. Type `shell:startup` and press `Enter` to open the Startup folder.
3. Copy the `Homebridge Speakers.exe` file or a shortcut of it into the Startup folder.
4. The application will now start automatically every time you log in to Windows.

Alternatively, you can add the application to the startup apps list:

1. Press `Ctrl + Shift + Esc` to open Task Manager.
2. Go to the **Startup** tab.
3. Click **Open File Location** next to the `Homebridge Speakers.exe` file, and move the file to the Startup folder as described above.
4. Enable the app in the Task Manager's Startup tab if it's disabled.

By following these steps, you can have the Homebridge Speaker App run automatically when Windows starts.
