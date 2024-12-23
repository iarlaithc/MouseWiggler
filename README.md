# IC's Mouse Wiggler

A simple but effective mouse wiggler application that prevents your system from going idle by making subtle mouse movements at specified intervals.

## Features

- Customizable interval timing (1000ms - 999999ms)
- Minimal diagonal mouse movement (1 pixel)
- Simple start/stop functionality
- Lightweight and easy to use

## Requirements

- Windows OS
- .NET 9 Runtime (will be prompted to install if missing)

## Installation

1. Download the installer from the [Releases](https://github.com/iarlaithc/MouseWiggler/releases/tag/release) section
2. Run the installer
3. If .NET 9 Runtime is not installed, you will be prompted to install it
4. Follow the installation wizard

## Usage

![Application Main Window](https://github.com/iarlaithc/MouseWiggler/blob/master/Wiggler/Resources/Images/Examples/Wiggler.PNG)


1. Launch IC's Mouse Wiggler
2. Enter your desired interval (in milliseconds)
   - Minimum: 1000ms (1 second)
   - Maximum: 999999ms (approximately 16.6 minutes)
3. Click "Start" to begin the wiggle operation
4. Click "Stop" to end the wiggle operation

## How It Works

The application moves your mouse cursor diagonally by 1 pixel at your specified interval. This minimal movement is enough to prevent your system from entering sleep mode or showing as idle, while being virtually unnoticeable during normal computer use.

## Screenshots

![Running](https://github.com/iarlaithc/MouseWiggler/blob/master/Wiggler/Resources/Images/Examples/wiggler2.PNG)
## Contributing

Feel free to submit issues and enhancement requests!

## License

MIT

## Support

If you encounter any problems or have suggestions, please [open an issue](issues) on GitHub.

## Upcoming Features
- Signing / Certifying
- Configuration saving
