# Scenario ac8156

## Reference tree and usage

- The main app used to reference “Direct dependency” by “1.0.0” which in turn referenced “Transitive dependency” by “[1.0.0]”.
- Both the main app and the direct library uses some parts of the “Transitive dependency” directly.

## Situation after upgrade of “Direct dependency”

- The main app changed the reference to “Direct dependency” by “2.0.0” and the version 2.0.0 in turn references “Transitive dependency” by “[2.0.0]”.
- This results in potentially new behavior in both the direct code in the Main app and in the code of “Direct dependency”.

## Potential issues

- By upgrading the “Direct dependency”, behavior may change even in the places of direct usage of “Transient dependency”. If the developer of the Main app wants to have the behavior under their control as much as possible, they should explicitly reference the “Transitive dependency” directly instead of relying on it being transitively available. 