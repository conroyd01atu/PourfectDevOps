
# Pourfect - Pour-Over Coffee Companion App

A mobile application designed for pour-over coffee enthusiasts to track brews, manage recipes, and perfect their brewing technique.

## Features

- **User Authentication**: Register and login to keep your data secure
- **Brew Tracking**: Record detailed parameters for each brew including coffee, water, temperature, and tasting notes
- **Recipe Management**: Create, edit, and organize your favorite pour-over recipes with step-by-step instructions
- **Timer**: Built-in timer with common presets for pour-over brewing
- **Search & Filter**: Find past brews and recipes quickly
- **User Profiles**: Track your brewing statistics and manage your account

## Prerequisites

- Windows 10/11 (for development)
- Visual Studio 2022 or later
- .NET 8.0 SDK
- Android SDK (installed with Visual Studio)
- Android Emulator or physical Android device

## Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/pourfect.git
   cd pourfect
   ```

2. **Open in Visual Studio**
   - Launch Visual Studio 2022
   - Select "Open a project or solution"
   - Navigate to the cloned folder and open `PourfectApp.sln`

3. **Restore NuGet packages**
   - Visual Studio should automatically restore packages
   - If not, right-click the solution and select "Restore NuGet Packages"

4. **Set up Android Emulator**
   - Go to Tools → Android → Android Device Manager
   - Create a new virtual device (recommended: Pixel 5 with API 33)
   - Start the emulator

5. **Run the application**
   - Select the Android emulator from the device dropdown
   - Press F5 or click the green "Start" button

## First Time Setup

1. **Launch the app**
   - The login screen will appear

2. **Create an account**
   - Tap "Register" to create a new account
   - Enter username, email, and password (min 6 characters)
   - Or tap "Continue as Guest" to explore without an account

3. **Start brewing!**
   - Record your first brew in the "Record" tab
   - Browse recipes in the "Recipes" tab
   - Use the Timer for your brew process

## Project Structure

```
PourfectApp/
├── Models/          # Data models (Brew, Recipe, User)
├── Services/        # Database and helper services
├── Views/           # XAML pages and code-behind
├── Platforms/       # Platform-specific code
├── Resources/       # Images, fonts, and styles
└── MauiProgram.cs   # App configuration
```

## Technologies Used

- **.NET MAUI 8.0**: Cross-platform framework
- **SQLite**: Local database storage
- **C#**: Primary programming language
- **XAML**: UI markup language

## Dependencies

- `sqlite-net-pcl`: SQLite ORM for .NET
- `SQLitePCLRaw.bundle_green`: SQLite provider

## Troubleshooting

### Build Errors
1. Clean solution: Build → Clean Solution
2. Delete `bin` and `obj` folders
3. Rebuild: Build → Rebuild Solution

### Emulator Issues
- Ensure Hyper-V is enabled in Windows Features
- Try a different emulator image
- Use a physical device via USB debugging

### Database Issues
- App data is stored in the app's local folder
- Uninstall and reinstall the app to reset database

## Contributing

This is a university project. Feel free to fork and modify for your own use.

## License

Academic use only. Created for ATU Donegal BSc in Applied Computing.

## Author

Darragh Conroy - L00188299
Contemporary Software Development 2024/2025

## Acknowledgments

- Inspired by Aeromatic app for AeroPress
- Google C# Style Guide
- .NET MAUI documentation
- SQLite documentation
