# Scenario bc9686

## Reference tree and usage 

- Main app directly references:
    - Direct dependency A (by “1.0.0”)
        - Which references:
            - Transitive dependency A (by "1.0.0”) // [no strict reference]
                - Which references:
                    - Second level transitive dependency (by "1.0.0") // [no strict reference]
    - Direct dependency B (by “2.0.0”) 
        - Which references:
            - Transitive dependency B (by “2.0.0”) // [no strict reference]
                - Which references:
                    - Second level transitive dependency (by "2.0.0") // [no strict reference]

## Situation after upgrade of “Direct dependency B”

- By upgrading “Direct dependency B” the project now uses "Second level transitive dependency" of version 2.0.0

## Potential issues 

- Since "Direct dependency A" (1.0.0) now uses Second level transitive dependency of version 2.0.0 transitively through "Transitive dependency A" (1.0.0):
    - app **is not broken** when Calculate2() method is called even though "Second level transitive dependency" of version 2.0.0 which is binary incompatible is used
    - app **is broken** when Calculate() method is called even for flows **not** using breaking changed second level transitive method