# Scenario ea0545

## Reference tree

- Main app directly references:
    - Direct dependency (by “1.0.0”)
        - Which references:
            - Transitive dependency (by "1.0.0”)
    - Transitive dependency (by “2.0.0”) 

## Actual dependency resolution 

- “Transitive dependency” is resolved to version “2.0.0” for all the cases where its logic is used (Direct dependency in this case). 

## Potential issues 

- The maintainer of “Direct dependency” of version 1.0.0 only thinks about “Transitive dependency” of version 1.0.0 and may implement its logic based on the assumption that they really are always using “Transitive dependency” of version 1.0.0. However, here we can see that the “Main app” upgraded to “Transitive dependency” version 2.0.0 and it is now this version that is used by the “Direct dependency”. The maintainer’s assumption was wrong. By specifying version 1.0.0 using “1.0.0”, they allowed both the version 1.0.0 and any higher version of the “Transitive dependency” to be used in their logic. 