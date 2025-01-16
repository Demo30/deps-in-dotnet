# Scenario a4328d

## Reference tree 

- Main app directly references:
    - Direct dependency A (by “1.0.0”)
        - Which references:
            - Transitive dependency (by "1.0.0”) // [no strict reference]
    - Direct dependency B (by “2.0.0”)
        - Which references:
            - Transitive dependency (by “2.0.0”) // [no strict reference]

## Usage 
- Direct dependency A v 1.0.0 is implemented in such a way that it expects types declared in Transitive dependency v 1.0.0. 
- Direct dependency B v 2.0.0 is implemented so that it expect new types/method declarations from the Transitive dependency v 2.0.0 

## Actual dependency resolution 

- The project uses transitive dependency of version 2.0.0 

## Resulting situation, issues 

- By referencing Direct dependency B v 2.0.0 along with Direct A v 1.0.0 we break functionality of Direct A during runtime. Direct A is unable to find suitable method declaration. 