# .github/copilot-instructions.md

## Mod Overview and Purpose

The MiningCo Mobile Mineral Sonar mod for RimWorld enhances the gameplay experience by introducing a sophisticated mineral detection system. This mod aims to provide players with advanced tools for locating and uncovering valuable mineral deposits beneath the surface of their colonies. The Mobile Mineral Sonar is designed to balance the challenge of resource management by introducing a novel tech-driven approach to mining.

## Key Features and Systems

- **Mobile Mineral Sonar Building:** A new building type, `Building_MobileMineralSonar`, is central to this mod. It allows players to scan the environment for mineral deposits and uncover them efficiently.
- **Scan Mechanics:** The sonar performs periodic scans, using methods such as `performScanUpdate()` and `drawSatelliteDish()`, to determine and display mineral locations in a defined range.
- **Dynamic Visuals:** Visual effects like scan rays are achieved through methods like `drawDynamicScanRangeAndScanRay()` to enhance the player’s immersive experience.
- **User Interaction:** The mod integrates seamlessly into the game, allowing easy placement and management of sonar units through classes such as `PlaceWorker_MobileMineralSonar`.
- **Research System Integration:** By using `ResearchMod_MobileMineralSonar`, players must invest in research to unlock and improve sonar capabilities.

## Coding Patterns and Conventions

- **Internal Classes for Game Logic:** Game logic is encapsulated in internal classes like `Building_MobileMineralSonar` to prevent external modifications that may unbalance gameplay.
- **Public Controller and Mod Integration:** High-level mod management is executed through the `Controller` class, which inherits from RimWorld's `Mod` class.
- **Separation of Concerns:** Helper methods, such as those handling visual elements, are carefully separated from core operational logic.
- **Naming Conventions:** CamelCase is used for method names (e.g., `performScanUpdate()`), while classes use PascalCase (e.g., `Building_MobileMineralSonar`).

## XML Integration

Currently, the project does not include details on XML integration, which could involve defining new game objects or modifying existing ones. If XML files are used to define custom ThingDefs, additional instructions would be required for XML structure and element usage.

## Harmony Patching

While Harmony patching is not explicitly mentioned in the provided C# summary, it is a common method for modifying existing game behavior in RimWorld. It is suggested to use Harmony for any method postfixes or alterations of vanilla methodologies to ensure mod compatibility and stability.

## Suggestions for Copilot

1. **Enhance Scan Functionality:** Suggest improvements to the scanning algorithms in `performScanUpdate()` for more efficient mineral detection.
2. **Add More Visual Effects:** Enhance the `drawDynamicScanRangeAndScanRay()` method with additional visual feedback, perhaps by integrating Unity or third-party libraries for graphics processing.
3. **Expand Research Capabilities:** Introduce more research tiers in `ResearchMod_MobileMineralSonar` that could unlock additional features or efficiencies in sonar operations.
4. **Error Logging:** Implement comprehensive logging in the `Controller` class to assist with debugging and track possible user errors during gameplay.
5. **XML Utilization:** Propose XML structures for defining new items, materials, and recipes, ensuring a seamless addition of future content and balance adjustments.
6. **Harmony Patches:** Encourage strategic use of Harmony to softly integrate mod elements into the core game without overwriting important vanilla components.

This file provides a structure to help future developers understand and expand upon this mod, utilizing Copilot's capabilities creatively and efficiently.
