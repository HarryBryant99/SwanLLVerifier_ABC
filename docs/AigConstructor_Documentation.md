# AigConstructor.cs Documentation
FILE PATH: AIG\AigConstructor.cs
## Overview
The `AigConstructor` class is a critical component of the SwanLLVerifier system that transforms ladder logic programs into AIG (And-Inverter Graph) format. This conversion enables formal verification tools to analyze ladder logic programs by representing them in a standardized boolean circuit format.

## Purpose
This class serves as a bridge between ladder logic representations and formal verification engines by:
- Converting ladder logic formulas into AIGER format files
- Managing variable naming conventions across different formats
- Handling temporal logic constructs (current and next-cycle variables)
- Generating verification-ready circuit representations

## Class Structure

### Private Fields
- **`latchNamesAndValues`**: Dictionary storing computed boolean values for latch variables
- **`allLatchVariables`**: Collection of all latch variable names from the ladder
- **`decoratedTrees`**: Maps formula tree identifiers to their literal indices
- **`decoratedVarNames`**: Maps current-cycle variable names to literal indices
- **`decoratedVarNamesNextCycle`**: Maps next-cycle variables (suffixed with "_1") to indices
- **`decoratedLatchVariables`**: Maps latch variables to their decorated formula values
- **`ladder`**: The input ladder logic program to be converted
- **`safety`**: The safety property formula to be verified
- **`aigerFileLines`**: Accumulates output lines for the final AIGER file
- **`maxLiteralIndex`**: Tracks the maximum assigned literal index for unique numbering
- **`rungIndex`**: Current rung being processed (public field for external access)

### Constructor
```csharp
public AigConstructor(Ladder ldr, AbstractFirstOrderFormula sfty, IDictionary<string, bool> latchNameVal)
```
Initializes the constructor with:
- A ladder logic program
- A safety property formula
- Pre-computed latch variable values

### Core Methods

#### `Decorate()`
**Purpose**: Orchestrates the complete decoration process
**Process**:
1. Assigns literal numbers to all variables
2. Decorates latch variables and their formulas
3. Decorates the safety property
4. Includes commented debug output sections

#### `AssignLiteralNumberToAllVariables()`
**Purpose**: Assigns unique even-numbered literal indices to all variables
**Key Features**:
- Increments `maxLiteralIndex` by 2 for each variable (AIGER convention)
- Normalizes variable names by removing prefixes and suffixes
- Creates both current-cycle and next-cycle variable mappings
- Handles special character replacement for tool compatibility

**Variable Name Normalization**:
- Removes "v" prefix (from ladder.tptp format)
- Strips "_0" and "_1" temporal suffixes
- Replaces ".", "(", ")" with underscores

#### `DecorateLatches()`
**Purpose**: Processes each rung's output as a latch variable
**Process**:
1. Normalizes rung output names using the same conventions
2. Creates latch variable names with "_LATCH" suffix
3. Decorates the formula tree for each rung
4. Validates that decorated values are within valid range (≥ 2)
5. Stores the association between latch variables and their formula representations

#### `DecorateSafety()`
**Purpose**: Applies decoration to the safety property formula
**Note**: Uses the same `DecorateFormulaTree` method with "SAFETY" as the parent identifier

#### `DecorateFormulaTree(AbstractFirstOrderFormula formula, string parentLatchVarName)`
**Purpose**: Recursively processes logical formulas and assigns literal indices
**Return Value**: Integer representing the decorated literal index

**Supported Formula Types**:
- **Predicate**: Base variables - looks up in appropriate dictionary
- **Negation**: Increments operand's decorated value by 1 (AIGER negation convention)
- **And**: Creates new literal index and records the AND gate operation
- **Brackets**: Transparent - returns the decorated value of the inner operand

**Error Handling**: Throws exception if predicate keys are not found in any dictionary

#### `ConstructAigerFile()`
**Purpose**: Generates the final AIGER format file
**Output Structure**:

1. **Header Line**: `aag M I L O A B`
   - M: Maximum variable index
   - I: Number of inputs
   - L: Number of latches
   - O: Number of outputs (always 0 for this application)
   - A: Number of AND gates
   - B: Number of bad state literals (always 1)

2. **Input Section**: Lists literal indices for input variables

3. **Latch Section**: Format `latch_literal latch_input initial_value`
   - Maps each latch to its driving formula
   - Includes initial values (0 or 1) from the model

4. **Output Section**: Single line with the safety property literal (bad state)

5. **AND Gate Section**: Format `output_literal input1_literal input2_literal`
   - Defines all AND operations in the circuit

**Variable Classification**:
- **Inputs**: Variables that are not latch outputs
- **Latches**: Variables that have corresponding "_LATCH" entries
- **Internal Nodes**: AND gate outputs for compound formulas

## AIGER Format Compliance
The generated files conform to AIGER (And-Inverter Graph in Extended AIGER format) standards:
- Even literal indices represent positive literals
- Odd literal indices represent negated literals
- Literal 0 represents constant FALSE
- Literal 1 represents constant TRUE

## Integration with Verification Tools
The generated AIGER files can be processed by:
- Model checkers (ABC, SymQUANT, etc.)
- SAT solvers
- Formal verification engines
- Property checking tools

## Error Handling
- Validates that decorated values are within expected ranges
- Ensures all predicate variables are properly mapped
- Throws descriptive exceptions for missing dictionary entries

## Academic Context
This implementation follows established practices in formal verification literature for converting high-level specifications into circuit representations suitable for algorithmic analysis. The transformation preserves the semantic meaning of the original ladder logic while enabling the application of advanced verification algorithms.

## Usage Example
```csharp
// Create constructor with ladder, safety property, and latch values
AigConstructor constructor = new AigConstructor(ladder, safetyFormula, latchValues);

// Perform decoration process
constructor.Decorate();

// Generate AIGER file
constructor.ConstructAigerFile();
```

## Output File
The method generates a file named "test.aag" in the current directory containing the complete AIGER representation of the ladder logic program and safety property.
