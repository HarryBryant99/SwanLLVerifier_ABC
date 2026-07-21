# Ladder.cs Documentation

## Overview
The `Ladder` class represents a complete ladder logic program consisting of multiple rungs. It serves as the primary data structure for modeling PLC (Programmable Logic Controller) ladder logic in the SwanLLVerifier verification system.

## Purpose
This class provides a structured representation of ladder logic programs and essential methods for variable analysis, which are fundamental to the formal verification process. It enables the system to:
- Model complete PLC programs as collections of logical operations
- Analyze variable dependencies and relationships
- Separate input variables from output/state variables
- Support conversion to formal verification formats (AIGER, SMT-LIB)

## Class Structure

### Properties
- **`Rungs`**: `List<Rung>` - Collection of all rungs (individual logic elements) that comprise the ladder program

### Constructor
```csharp
public Ladder()
```
Initializes an empty ladder logic program with a new list to store rungs.

### Methods

#### `AddRung(Rung rung)`
**Purpose**: Adds a new rung to the ladder program
**Parameters**: 
- `rung`: A `Rung` object representing a single logical operation with its formula and output variable
**Usage**: Used during ladder construction to build up the complete program

#### `AllVariables()`
**Purpose**: Collects all variables used throughout the entire ladder program
**Returns**: `ISet<string>` containing all unique variable names
**Process**:
1. Iterates through each rung in the ladder
2. Unions the variables from each rung's formula with the collection
3. Adds the rung's output variable to ensure completeness
4. Uses `HashSet<string>` to maintain uniqueness
5. Includes debug output for development and troubleshooting

**Academic Significance**: This method provides the universe of discourse for the verification problem, establishing all variables that must be considered in formal analysis.

#### `AllInputs()`
**Purpose**: Determines which variables are external inputs (not produced by any rung)
**Returns**: `ISet<string>` containing variables that appear in formulas but are never outputs
**Algorithm**:
1. Collects all variables from rung formulas (excluding each rung's own output)
2. Removes any variables that appear as outputs in any rung
3. Ensures clean separation between inputs and internally-generated variables

**Academic Significance**: Essential for AIGER conversion and formal verification, as it establishes the boundary between the system and its environment.

#### `AllOutputVariables()`
**Purpose**: Collects all output variables (coils) from the ladder
**Returns**: `ISet<string>` containing all variables produced by the ladder
**Usage**: These represent the state variables or "latches" in the verification context

## Integration with Verification System

### Variable Classification
The methods in this class establish a fundamental partition of variables:
- **Inputs**: Variables that appear in formulas but are not produced by any rung (external signals)
- **Outputs/Latches**: Variables that are produced by rungs (internal state)
- **All Variables**: The complete universe of variables used in the ladder

### Formal Verification Context
This classification is crucial for:
- **AIGER Generation**: Inputs become AIGER inputs, outputs become latches with driving logic
- **SMT-LIB Export**: Proper variable declaration and constraint generation
- **Model Checking**: Establishing the state space and input space for verification

## Design Patterns
- **Collection Management**: Uses `List<Rung>` for ordered storage and `HashSet<string>` for unique variable sets
- **Separation of Concerns**: Each method has a single, well-defined responsibility
- **Debug Support**: Includes console output for development and troubleshooting

## Usage Example
```csharp
// Create a new ladder
Ladder ladder = new Ladder();

// Add rungs representing logical operations
Rung rung1 = new Rung { output = "x", formula = someFormula };
ladder.AddRung(rung1);

// Analyze the ladder
ISet<string> allVars = ladder.AllVariables();      // All variables used
ISet<string> inputs = ladder.AllInputs();          // External inputs only  
ISet<string> outputs = ladder.AllOutputVariables(); // Internal state variables
```

## Academic Context
This implementation follows established practices in formal verification literature for representing reactive systems. The clear separation between inputs and outputs enables the application of standard verification algorithms while maintaining compatibility with industrial PLC programming paradigms.

The class serves as a bridge between the practical world of ladder logic programming and the theoretical foundations of formal verification, enabling rigorous analysis of industrial control systems.
