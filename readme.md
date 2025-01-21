# Scenario c2fc88

## Reference tree and usage 

- Main app directly references:
    - Direct dependency A (by “1.0.0”)
        - Which references:
            - Transitive dependency (by "1.0.0”) // [no strict reference]
    - Direct dependency B (by “2.0.0”) 
        - Which references:
            - Transitive dependency (by “2.0.0”) // [no strict reference]

- Usage: 
    - In case of “Direct dependency A”, the Main app uses a functionality that in turn uses the Transitive dependency.
    - The “Direct dependency B” also uses the Transitive dependency internally, but only for some other functionality that the Main app doesn’t even use. The Main app may not be aware that “Direct dependency B” would even somehow influence the behavior of Transitive dependency.

## Situation after upgrade of “Direct dependency B”

- By upgrading “Direct dependency B” because of some other functionality, we have changed the behavior of “Direct dependency A” (by using different “Transitive dependency”). 

## Potential issues 

- Dependencies may be hidden many levels down the dependency line and by upgrading something that does not seem relevant at all, we change behavior of something completely different without realizing these could be in any way related. 