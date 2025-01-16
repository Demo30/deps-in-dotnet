# Scenario 05d335 

## Reference tree 

- Main app directly references:
    - Direct dependency A (by “1.0.0”)
        - Which references:
            - Transitive dependency (by "1.0.0”) // [no strict reference] 
    - Direct dependency B (by "2.0.0")
        - Which references:
            - Transitive dependency (by “2.0.0”) 

## Actual dependency resolution

- Transitive dependency of verison 2.0.0 is resolved

## Issues

- By directly referencing transitive dependency of higher version, we also force Direct Dependency A to use the same version.
    - Transitive dependency has changed namespace of return type in 2.0.0
        - The Main app crashes during runtime because expecting to be able to call Transitive dependency's method with return type of previous namespace.