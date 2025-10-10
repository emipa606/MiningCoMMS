# .github/copilot-instructions.md

## Mod Overview and Purpose
**Mod Name:** MiningCo. MMS (Continued)

MiningCo. MMS is an updated version of the original mod by Rikiki, aimed at enhancing miners' efficiency in RimWorld by introducing the Mobile Mineral Sonar. This tool allows players to detect metallic objects deep within mountains—providing a strategic advantage in resource-scarce environments. The update includes added translation support to make the mod accessible to non-English speaking players.

## Key Features and Systems
1. **Mobile Mineral Sonar:**
   - **Scanning:** The sonar scans large mountain ranges for metallic objects.
   - **Mobility:** It is minifiable, allowing players to move it after a scan is complete.
   - **Power Source:** Can operate on battery but benefits from a power network connection to increase scan speed.

2. **Technical Enhancements:**
   - **Custom Place Worker Restricter:** Limits where the sonar can be placed to ensure effectiveness.
   - **Dynamic Textures and Fog Integration:** Visuals that adjust based on scanning activity and environmental factors.

## Coding Patterns and Conventions
- **Class Naming:** Classes use PascalCase, e.g., `Building_MobileMineralSonar`.
- **Method Naming:** Methods use camelCase, e.g., `performScanUpdate()`.
- **File Organization:** Each major component (e.g., building, controller, settings) is encapsulated in separate files for clarity and modularity.

## XML Integration
- XML is used for defining configurations and game data that can be easily modified without altering the core C# code.
- XML files should be well-commented to allow for easy modification and understanding of parameters, especially for translation purposes.

## Harmony Patching
- Harmony is utilized for patching the game's functions, enabling modifications and enhancements without altering the base game code.
- Ensure that Harmony patches are cleanly implemented and aim to avoid conflicts with other mods by being specific in targeting methods.

## Suggestions for Copilot
- Implement helper methods to improve readability and reuse of common operations like calculating scan ranges or managing power states.
- Be explicit with method arguments to ensure the clarity of what is being passed and expected.
- Utilize comments to outline the functionality of complex methods or algorithms, aiding in maintainability and future development.
- Consider creating unit tests for critical methods (e.g., scanning logic) to validate functionality after changes.
- Suggest try-catch blocks for any methods interacting with game state or external inputs to handle potential errors gracefully.

By adhering to these guidelines, contributors can maintain consistency and quality in the development and enhancement of the MiningCo. MMS mod. Feel free to explore existing code for inspiration on implementing new features or optimizing existing ones!
