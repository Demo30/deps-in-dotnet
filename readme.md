# Scenario 25a851 

## Reference tree 

- Main app directly references:
    - Direct dependency (by “2.0.0”)
        - Which references:
            - Transitive dependency (by "2.0.0”) // [no strict reference] 
    - Transitive dependency (by “1.0.0”) 

## Actual dependency resolution + Potential issues 

- The same as scenario c61417. Doesn’t matter whether transitive dependency is strictly referenced or not, it is simply higher than the directly referenced “Transitive dependency”.