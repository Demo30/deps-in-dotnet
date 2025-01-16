# Scenario 25a851 

## Reference tree 

- Main app directly references:
    - Direct dependency (by “1.0.0”)
        - Which references:
            - Transitive dependency (by "1.0.0”) // [no strict reference] 
    - Transitive dependency (by “2.0.0”) 

## Actual dependency resolution

- Transitive dependency of verison 2.0.0 is resolved

## Issues

- By directly referencing transitive dependency of higher version, we also force Direct Dependency A to use the same version.
    - However the type of ICalculationResult has changed and now the Direct Dependency A (version 1.0.0 implemented against Transitive 1.0.0) is confused about the type.
        - failing in runtime