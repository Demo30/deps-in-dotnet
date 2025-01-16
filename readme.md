# Scenario d0c14d

## Reference tree

- Main app directly references:
    - Direct dependency A (by “1.0.0”)
        - Which references:
            - Transitive dependency (by "1.0.0”) // [no strict reference]
    - Direct dependency B (by “2.0.0”)
        - Which references:
            - Transitive dependency (by “2.0.0”) // [no strict reference]

## Situation before upgrade 

- The Main app uses the result of method call from “Direct dependency A” which can never be null according to types (and in reality) and performs some operations on it. 

## Situation after upgrade of “Direct dependency B” 

- Everything compiles ok, but we get NRE in runtime, because the method from “Direct dependency A” can suddenly return null even when this conflicts with the type declaration. 