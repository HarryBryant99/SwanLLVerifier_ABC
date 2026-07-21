# Rung.cs Documentation

## Overview
The `Rung` class represents a single rung in a ladder logic program. In PLC programming, a rung is an individual logical operation that reads input conditions and drives an output variable (called a "coil"). This class encapsulates the essential components of a rung: its logical formula and the output it controls.

## Purpose
This class serves as the fundamental building block of ladder logic programs in the SwanLLVerifier system. Each rung represents one logical relationship of the form `output := formula`, where the output variable is assigned the boolean result of evaluating the formula. The class enables:
- Representation of individual PLC logic operations
- Integration with the broader ladder logic verification framework
- Support for initialization semantics common in industrial control systems

## Class Structure

### Properties

#### `output` (string)
**Purpose**: Specifies the variable name that this rung drives
**Academic Significance**: In formal verification terms, this represents a state variable or "latch" that will be updated based on the rung's formula evaluation. In PLC terminology, this is the "coil" that the rung energizes or de-energizes.

#### `formula` (AbstractFirstOrderFormula)  
**Purpose**: Contains the logical condition that determines the output value
**Type**: Uses the abstract syntax tree representation from the ETCSDC_Properties namespace
**Academic Significance**: This represents the transition relation for the output variable, defining how its next-state value depends on current-state and input variables.

#### `Initialised` (bool)
**Purpose**: Indicates whether this rung's output should be initialized to true at system startup
**Default Behavior**: When false (default), the output initializes to false following Siemens PLC conventions
**Academic Significance**: This property handles the initial state assignment in formal verification models, which is crucial for model checking and inductive verification.

### Methods

#### `AllVariables()`
**Purpose**: Extracts all variables involved in this rung
**Returns**: `ISet<string>` containing unique variable names
**Implementation**:
1. Calls `PropositionalFormulaUtils.AllVariablesFromFormula(formula)` to extract variables from the logical formula
2. Adds the output variable to ensure completeness
3. Returns the unified set of variables

**Academic Significance**: This method supports dependency analysis and variable classification essential for formal verification workflows.

## Integration with Verification System

### Formula Evaluation
The rung's formula is evaluated using the `Model.ParseAndEvaluate()` method, which recursively processes the abstract syntax tree to compute boolean results. This evaluation follows the semantics defined in the ETCSDC_Properties operators.

### AIGER Conversion
During AIGER generation:
- The `output` becomes a latch variable in the circuit representation
- The `formula` is converted to AND-gate networks via `AigConstructor.DecorateFormulaTree()`
- The `Initialised` flag determines the latch's initial value in the AIGER file

### Variable Analysis
The `AllVariables()` method supports:
- **Input/Output Classification**: Helps `Ladder.AllInputs()` determine which variables are external inputs
- **Dependency Analysis**: Enables understanding of variable relationships across the ladder
- **Scope Analysis**: Ensures all variables are properly accounted for in verification

## Ladder Logic Semantics

### Execution Model
In PLC execution:
1. All rung formulas are evaluated simultaneously using current variable values
2. Output assignments occur atomically after all evaluations complete
3. This creates a synchronous, deterministic execution model suitable for formal verification

### Initialization Semantics
- **Default (Initialised = false)**: Output starts at false, following Siemens conventions
- **Explicit (Initialised = true)**: Output starts at true, used for fail-safe logic or specific control requirements

## Design Patterns

### Immutable Structure
Once created, a rung's logical structure (formula and output) typically remains fixed, supporting predictable verification workflows.

### Separation of Concerns
- **Structure**: The rung defines what logic exists
- **Evaluation**: The `Model` class handles how logic is computed  
- **Conversion**: The `AigConstructor` handles how logic is represented

### Compositional Design
Rungs compose naturally into `Ladder` objects, enabling modular construction and analysis of complex control systems.

## Usage Example
```csharp
// Create a rung representing: output_valve := sensor_high AND NOT sensor_low
AbstractFirstOrderFormula formula = MakeAnd(
    MakeVar("sensor_high"), 
    MakeNegation(MakeVar("sensor_low"))
);

Rung rung = new Rung
{
    output = "output_valve",
    formula = formula,
    Initialised = false  // Start with valve closed
};

// Analyze the rung
ISet<string> variables = rung.AllVariables(); 
// Returns: {"sensor_high", "sensor_low", "output_valve"}
```

## Academic Context
This implementation aligns with standard approaches in formal verification of reactive systems, where individual transitions are represented as logical formulas over state and input variables. The clear separation between formula structure and evaluation semantics enables the application of various verification techniques while maintaining compatibility with industrial PLC programming practices.

The rung abstraction provides a natural bridge between the operational semantics of ladder logic and the mathematical foundations required for formal analysis, enabling rigorous verification of industrial control systems.
