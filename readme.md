# Scenario a0a93f

## Reference tree

- Main app directly references:
    - Direct dependency (by “1.0.0”)
        - Which references:
            - Transitive dependency (by "[1.0.0]”) 
    - Transitive dependency (by “2.0.0”) 

## Actual dependency resolution 

- Build fails altogether when warnings are treated as errors. 
    - The error is: NU1608 
- Otherwise “Transitive dependency” is resolved to version “2.0.0” for all the cases where its logic is used (Direct dependency in this case). 

## Potential issues 

- Either we (as library consumers) allow this to build and open ourselves to possible unexpected runtime behavior (the maintainer of “Direct dependency” required version 1.0.0 but did not specify it strictly thus allowing higher versions to be resolved). 
- Or we are locked from the ability to upgrade the “Direct dependency” package unless conflicting references are resolved. 