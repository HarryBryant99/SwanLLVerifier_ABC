Model.cs

## Overview
This file defines a `Model` class that represents a computational model for evaluating ladder logic programs. It evaluates the logical formulas within each rung.

## Class Structure

### Fields
- **`ladder`**: A `Ladder` object containing the ladder logic to be modeled
- **`latchNamesAndValues`**: A dictionary that stores variable names and their computed boolean values, representing the state of "latches" (memory elements in ladder logic)

### Constructor
- **`Model(Ladder ldr)`**: Takes a ladder logic program and initializes the model with it

### Properties
- **`LatchNamesAndValues`**: Read-only property providing access to the internal dictionary of latch states

## Key Methods

### `InitialiseModel()`
This method sets up the initial state of the model:

1. **Follows Siemens convention**: Initializes all variables to FALSE as specified by Siemens documentation
2. **Variable name processing**: 
   - Removes the "v" prefix from variable names (used in ladder.tptp format)
   - Removes "_0" or "_1" suffixes 
   - Replaces special characters (`.`, `(`, `)`) with underscores for normalization
3. **Latch creation**: For each rung output, creates a corresponding latch variable with "_LATCH" suffix
4. **Formula evaluation**: Evaluates each rung's formula using `ParseAndEvaluate()` and stores the result

### `ParseAndEvaluate(AbstractFirstOrderFormula formula)`
A recursive formula evaluator that handles different types of logical operators:

- **`Predicate`**: Base case - looks up variable values in the dictionary
- **`And`**: Logical AND operation (`&`)
- **`Implies`**: Logical implication (`!(A & !B)`)
- **`Equivalent`**: Logical equivalence (equality check)
- **`Or`**: Logical OR operation (`|`)
- **`Negation`**: Logical NOT operation (`!`)
- **`Brackets`**: Parentheses grouping (evaluates inner formula)

### `GetVariableValueFromDictionary(string predicateName)`
Helper method that:
- Looks up variables with "_LATCH" suffix in the dictionary
- Returns `false` for any variable not found (following the initialization convention)

## Purpose and Context


1. **Models PLC behavior**: Simulates how a PLC would execute ladder logic
2. **Handles variable naming conventions**: Processes different naming formats used in verification tools
3. **Evaluates complex formulas**: Can handle both AIG (And-Inverter Graph) and ladder logic formulas
4. **Maintains state**: Tracks the values of latches/memory elements

The InitialiseModel() makes calls to ParseAndEvaluate(AbstractFirstOrderFormula formula) -> GetVariableValueFromDictionary(string predicateName) 