# PropositionalFormulaBuilder.cs Documentation

## Overview
The `PropositionalFormulaBuilder` class provides a comprehensive factory API for constructing logical formulas in the SwanLLVerifier system. It serves as the primary interface for building Abstract Syntax Trees (ASTs) representing propositional logic expressions used in ladder logic rungs, safety properties, and verification queries.

## Purpose
This class centralizes formula construction through a clean, type-safe API that ensures proper AST structure and formula type assignment. It enables:
- **Consistent Construction**: Standardized methods for creating all logical operators
- **Type Safety**: Automatic assignment of correct formula types
- **Readability Enhancement**: Intelligent bracketing for complex expressions
- **N-ary Operations**: Support for operations with multiple operands
- **Compositional Building**: Easy construction of complex formulas from simpler parts

## Class Structure

### Static Factory Pattern
The class implements the Static Factory pattern, providing static methods for creating formula objects without requiring class instantiation. This design choice emphasizes the utility nature of the class and enables convenient usage through static imports.

## Core Factory Methods

### Binary Operators

#### `MakeAnd(AbstractFirstOrderFormula leftOperand, AbstractFirstOrderFormula rightOperand)`
**Purpose**: Creates logical conjunction (∧)
**Returns**: `BinaryOperatorType` representing AND operation
**Implementation**: 
- Creates `And` object with proper operand assignment
- Sets `FormulaType` to `FOLFormulaType.And`
- Initializes `Operands` array for compatibility

#### `MakeOr(AbstractFirstOrderFormula leftOperand, AbstractFirstOrderFormula rightOperand)`
**Purpose**: Creates logical disjunction (∨)
**Returns**: `BinaryOperatorType` representing OR operation
**Academic Significance**: Fundamental for expressing alternative conditions in ladder logic

#### `MakeImplication(AbstractFirstOrderFormula leftOperand, AbstractFirstOrderFormula rightOperand)`
**Purpose**: Creates logical implication (→)
**Returns**: `UnaryOperatorType` (wrapped in brackets)
**Implementation**: Automatically wraps result in brackets for clarity
**Academic Significance**: Essential for safety property specification and conditional logic

#### `MakeEquivalence(AbstractFirstOrderFormula leftOperand, AbstractFirstOrderFormula rightOperand)`
**Purpose**: Creates logical equivalence (↔)
**Returns**: `UnaryOperatorType` (wrapped in brackets)
**Usage**: Common in safety specifications and system invariants

### Unary Operators

#### `MakeNegation(AbstractFirstOrderFormula operand)`
**Purpose**: Creates logical negation (¬)
**Returns**: `Negation` object
**Smart Bracketing**: Automatically adds brackets around complex operands for readability
**Implementation Logic**:
```csharp
// Adds brackets if operand is neither Predicate nor already bracketed
if ((n.OperandType != FOLFormulaType.Predicate) && (n.OperandType != FOLFormulaType.Brackets))
{
    // Wrap in brackets for clarity
}
```

#### `MakeBrackets(AbstractFirstOrderFormula operand)`
**Purpose**: Creates explicit grouping operator
**Returns**: `Brackets` object
**Usage**: Controls precedence and enhances readability in complex expressions

### Atomic Formulas

#### `MakeVar(string name)`
**Purpose**: Creates atomic propositions (variables)
**Returns**: `Predicate` object representing a boolean variable
**Academic Significance**: These are the leaves of the AST, representing basic propositions

#### `MakeNegatedVar(string name)`
**Purpose**: Convenience method for creating negated variables
**Returns**: Negation of a Predicate
**Implementation**: Composes `MakeNegation(MakeVar(name))`

## Advanced Construction Methods

### N-ary Operations

#### `MakeAnd(List<AbstractFirstOrderFormula> operands)`
**Purpose**: Creates conjunction of multiple operands
**Returns**: `AbstractFirstOrderFormula` (single operand) or `BinaryOperatorType` (multiple operands)
**Graceful Handling**: Returns single operand directly if list contains only one element
**Implementation**: Uses `FoldR1BinaryOperator` for tree construction

#### `MakeOr(List<AbstractFirstOrderFormula> operands)`
**Purpose**: Creates disjunction of multiple operands
**Similar behavior to n-ary AND operation**

### Utility Functions

#### `FoldR1BinaryOperator(Func<AbstractFirstOrderFormula, AbstractFirstOrderFormula, BinaryOperatorType> makeFn, List<AbstractFirstOrderFormula> operands)`
**Purpose**: Applies binary operator in right-associative fold pattern
**Algorithm**: For operands [a, b, c, d], produces f(a, f(b, f(c, d)))
**Academic Significance**: Implements right-associative tree construction, important for maintaining logical precedence
**Error Handling**: Validates minimum operand count (≥ 2)

### Bracketed Variants

#### `MakeAndWithBrackets()` and `MakeOrWithBrackets()`
**Purpose**: Create operations with explicit bracketing
**Returns**: `UnaryOperatorType` (brackets wrapper)
**Usage**: When explicit grouping is required for clarity or precedence control

## Design Patterns and Principles

### Factory Method Pattern
Each `Make*` method implements the Factory Method pattern, encapsulating object creation complexity and ensuring consistent initialization.

### Composite Pattern Support
The methods create AST nodes that participate in the Composite pattern established by `AbstractFirstOrderFormula`, enabling recursive processing.

### Fluent Interface Potential
The static methods can be chained for complex formula construction:
```csharp
var formula = MakeAnd(
    MakeOr(MakeVar("a"), MakeVar("b")),
    MakeNegation(MakeVar("c"))
);
```

## Integration with Verification System

### Ladder Logic Construction
Used extensively in `Program.cs` test methods to construct rung formulas:
```csharp
AbstractFirstOrderFormula form1 = MakeAnd(MakeNegation(varX), varY);
```

### Safety Property Specification
Enables construction of complex safety properties:
```csharp
AbstractFirstOrderFormula safety = MakeImplication(
    MakeVar("button_pressed"),
    MakeOr(MakeVar("valve_open"), MakeVar("alarm_active"))
);
```

### Parser Integration
Provides the construction API used by `SafetyPropertyParser` and `TptpParser` for building ASTs from textual input.

## Academic Context

### Propositional Logic Foundation
The methods provide a complete basis for propositional logic construction, supporting all standard connectives needed for formal verification.

### Compositional Semantics
The factory methods enable compositional construction where complex formulas are built from simpler components, supporting the compositional semantics required for formal analysis.

### Normal Form Construction
The methods can be used to construct formulas in various normal forms (CNF, DNF) required by different verification algorithms.

## Usage Examples

### Basic Construction
```csharp
// Create variables
AbstractFirstOrderFormula a = MakeVar("sensor_active");
AbstractFirstOrderFormula b = MakeVar("valve_open");

// Create complex formula: (sensor_active AND valve_open) OR NOT sensor_active
AbstractFirstOrderFormula formula = MakeOr(
    MakeAnd(a, b),
    MakeNegation(a)
);
```

### N-ary Operations
```csharp
// Multiple conditions must all be true
List<AbstractFirstOrderFormula> conditions = new()
{
    MakeVar("temp_ok"),
    MakeVar("pressure_ok"), 
    MakeVar("flow_ok")
};
AbstractFirstOrderFormula allOk = MakeAnd(conditions);
```

### Safety Properties
```csharp
// Safety: If emergency button pressed, then system stops
AbstractFirstOrderFormula safety = MakeImplication(
    MakeVar("emergency_button"),
    MakeVar("system_stopped")
);
```

## Design Benefits

### Type Safety
All methods ensure proper type assignment and return strongly-typed results, preventing runtime type errors.

### Consistency
Centralized construction ensures all formulas follow the same initialization patterns and type conventions.

### Maintainability
Changes to formula construction logic can be made in one location, affecting the entire system consistently.

### Readability
The intelligent bracketing and clear method names enhance the readability of both construction code and resulting formulas.

## Extension Guidelines

To add support for new logical operators:
1. Add the operator type to `FOLFormulaType` enumeration
2. Create the concrete operator class
3. Add corresponding `Make*` method to this builder class
4. Follow existing patterns for type assignment and bracketing
5. Update evaluation and conversion methods accordingly

This systematic approach ensures consistency and maintainability as the logical foundation evolves.
