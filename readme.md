# Scenario c61417

## Reference tree 

- Main app directly references:
    - Direct dependency (by “2.0.0”)
        - Which references:
            - Transitive dependency (by "[2.0.0]”) 
    - Transitive dependency (by “1.0.0”) 

## Actual dependency resolution 

- Regardless of treat warnings as errors setting, the restore/build fails in this case. 
- The error is: NU1605 
- This behavior is different from scenario a0a93f 

## Potential issues 

- Based on behavior of scenario a0a93f the developer may expect a non-issue when upgrading “Direct dependency” to version 2.0.0 (which in turn references “Transitive dependency” v 2.0.0) since their directly referenced “Transitive dependency” library was specified by “1.0.0” which translates to: “>= 1.0.0”. 
    - However, this is not the case, and the error-level conflict arises. 
- Besides unexpected behavior, the main app consumer may be locked from upgrading to a higher version of “Direct dependency” without resolving additional issues with references. 
    - The main app developer may use some other parts of “Transitive dependency” which are now problematic to resolve with never version. 