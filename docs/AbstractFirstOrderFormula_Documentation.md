# AbstractFirstOrderFormula.cs Documentation

## Overview
The `AbstractFirstOrderFormula` class serves as the abstract base class for all logical formulas in the SwanLLVerifier system. It establishes the foundation for an Abstract Syntax Tree (AST) representation of logical expressions used throughout the verification framework for both ladder logic rungs and safety properties.

## Purpose
This class provides the structural foundation for representing logical formulas in a type-safe, extensible manner. It enables:
- **Uniform Representation**: All logical constructs share a common base type
- **Polymorphic Processing**: Algorithms can operate on formulas without knowing specific operator types
- **Type Safety**: Compile-time guarantees about formula structure
- **Extensibility**: New logical operators can be added by extending this base class

## Class Structure

### Abstract Base Class
```csharp
public abstract class AbstractFirstOrderFormula
```
**Design Pattern**: Abstract base class implementing the Composite pattern
**Academic Significance**: Provides the foundation for compositional semantics where complex formulas are built from simpler sub-formulas

### Enumeration: FOLFormulaType
Defines all supported logical operator types in the system:

#### Boolean Operators
- **`And`**: Logical conjunction (∧)
- **`Or`**: Logical disjunction (∨)  
- **`Implies`**: Logical implication (→)
- **`Equivalent`**: Logical equivalence (↔)
- **`Negation`**: Logical negation (¬)

#### Structural Operators
- **`Brackets`**: Parentheses for grouping and precedence control
- **`Predicate`**: Atomic propositions (variables and constants)

**Academic Significance**: This enumeration captures the essential operators of propositional logic, providing a complete basis for expressing ladder logic semantics and safety properties.

### Property: FormulaType
```csharp
public FOLFormulaType FormulaType { get; set; }
```
**Purpose**: Provides runtime type identification for formula objects
**Usage**: Enables pattern matching and type-based dispatch in evaluation algorithms
**Implementation Note**: Must be set by concrete subclasses to identify their operator type

## Design Patterns and Architecture

### Composite Pattern
The class hierarchy implements the Composite pattern where:
- **Component**: `AbstractFirstOrderFormula` (this class)
- **Leaf**: `Predicate` (atomic formulas)
- **Composite**: Binary operators (`And`, `Or`, `Implies`, `Equivalent`) and unary operators (`Negation`, `Brackets`)

### Visitor Pattern Support
The `FormulaType` property enables visitor-like pattern matching in methods such as:
- `Model.ParseAndEvaluate()`: Evaluates formulas recursively
- `AigConstructor.DecorateFormulaTree()`: Converts formulas to AIGER representation
- `PrettyPrinter.Prettify()`: Generates human-readable formula representations

## Integration with Verification System

### Formula Evaluation
The abstract base enables polymorphic evaluation through methods like:
```csharp
public bool ParseAndEvaluate(AbstractFirstOrderFormula formula)
{
    return formula switch
    {
        Predicate predicate => /* evaluate variable */,
        And and => /* evaluate conjunction */,
        // ... other cases
    };
}
```

### AIGER Conversion
During circuit generation, the type system supports systematic conversion:
- Each formula type maps to specific AIGER constructs
- Recursive traversal processes complex formulas into gate networks
- Type safety ensures all operators are properly handled

### Safety Property Integration
The same AST structure represents both:
- **Ladder Logic Formulas**: The logical conditions in individual rungs
- **Safety Properties**: The specifications to be verified
- This uniformity enables consistent processing across the verification workflow

## Concrete Implementations

### Binary Operators
Classes extending `BinaryOperatorType`:
- `And`: Logical conjunction
- `Or`: Logical disjunction  
- `Implies`: Logical implication
- `Equivalent`: Logical equivalence

### Unary Operators
Classes extending `UnaryOperatorType`:
- `Negation`: Logical negation
- `Brackets`: Grouping operator

### Atomic Formulas
- `Predicate`: Variables and constants

## Academic Context

### Formal Semantics
This design supports standard compositional semantics where:
- Formula meaning is determined by operator semantics and sub-formula meanings
- Evaluation proceeds recursively through the AST structure
- Type system ensures semantic consistency

### Verification Theory
The abstraction aligns with established formal verification practices:
- **Propositional Logic**: Provides the logical foundation for verification
- **Boolean Satisfiability**: Formula structure supports SAT-based verification
- **Model Checking**: Enables systematic state space exploration
- **Theorem Proving**: Supports logical inference and proof construction

## Usage Examples

### Creating Formulas
```csharp
// Using PropositionalFormulaBuilder helpers
AbstractFirstOrderFormula formula = MakeAnd(
    MakeVar("sensor_active"),
    MakeNegation(MakeVar("system_fault"))
);

// Formula type is automatically set by concrete classes
Debug.Assert(formula.FormulaType == FOLFormulaType.And);
```

### Pattern Matching
```csharp
string Analyze(AbstractFirstOrderFormula formula)
{
    return formula.FormulaType switch
    {
        FOLFormulaType.And => "Conjunction",
        FOLFormulaType.Or => "Disjunction", 
        FOLFormulaType.Predicate => "Atomic",
        _ => "Other operator"
    };
}
```

## Design Benefits

### Type Safety
Compile-time verification ensures:
- All formula constructs are properly typed
- Pattern matching covers all operator cases
- Extension points are clearly defined

### Maintainability  
The abstract base provides:
- Centralized type enumeration
- Consistent interface across all operators
- Clear extension mechanism for new operators

### Performance
The design enables:
- Efficient pattern matching through enumeration
- Minimal runtime type checking overhead
- Optimized recursive traversal algorithms

## Extension Guidelines

To add a new logical operator:
1. Add the operator type to `FOLFormulaType` enumeration
2. Create a concrete class extending appropriate base type
3. Update pattern matching in evaluation and conversion methods
4. Set `FormulaType` property in the concrete class constructor

This systematic approach ensures consistency and maintainability as the logical foundation evolves.
