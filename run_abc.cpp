#include <iostream>
#include <cstdio>
#include <string>
#include <fstream>
#include <sstream>
#include <vector>

int main(int argc, char* argv[]) {
    if (argc < 2) {
        std::cerr << "Usage: " << argv[0] << " <aiger_file>\n";
        return 1;
    }

    std::string aigFile = argv[1];

    // Create .inv filename
    std::string invFile = aigFile;
    size_t posExt = invFile.rfind(".aig");
    if (posExt != std::string::npos) {
        invFile.replace(posExt, 4, ".inv");
    } else {
        invFile += ".inv";
    }

    // Build ABC command
    std::string cmd =
        "echo \"read_aiger " + aigFile +
        "; print_stats; pdr -d -w -e -I " + invFile +
        "; quit\" | ./abc/abc";

    // Run ABC and capture output
    FILE* pipe = popen(cmd.c_str(), "r");
    if (!pipe) {
        std::cerr << "Failed to run abc\n";
        return 1;
    }

    char buffer[512];
    std::string output;

    std::cout << "=== ABC OUTPUT ===\n";

    while (fgets(buffer, sizeof(buffer), pipe)) {
        std::cout << buffer;
        output += buffer;
    }

    pclose(pipe);

    // === Parse stats ===
    int inputs = -1;
    int outputs = -1;
    int latches = -1;

    size_t pos = output.find("i/o =");
    if (pos != std::string::npos) {
        sscanf(output.c_str() + pos,
               "i/o = %d/%d  lat = %d",
               &inputs, &outputs, &latches);
    }

    std::cout << "\n=== PARSED STATS ===\n";
    std::cout << "Inputs:  " << inputs << "\n";
    std::cout << "Outputs: " << outputs << "\n";
    std::cout << "Latches: " << latches << "\n";

    // === READ + PRINT .inv FILE ===
    std::ifstream file(invFile);

    if (!file) {
        std::cerr << "\nFailed to open " << invFile << "\n";
        return 1;
    }

    std::vector<std::string> vars;
    std::vector<std::string> cubes;
    std::string line;

    std::cout << "\n=== RAW INVARIANT FILE ===\n";

    while (std::getline(file, line)) {
        // Print raw content
        std::cout << line << "\n";

        // Extract variable names
        if (line.rfind(".ilb", 0) == 0) {
            std::stringstream ss(line.substr(5));
            std::string var;
            while (ss >> var) {
                vars.push_back(var);
            }
        }
        // Skip metadata
        else if (line.empty() || line[0] == '.' || line[0] == '#') {
            continue;
        }
        // Cube lines
        else {
            cubes.push_back(line);
        }
    }

    file.close();

    // === BUILD BOOLEAN EXPRESSION ===
    std::vector<std::string> expressions;

    for (const std::string& cubeLine : cubes) {
        std::stringstream ss(cubeLine);
        std::string pattern, outVal;
        ss >> pattern >> outVal;

        if (outVal != "1") continue;

        std::vector<std::string> terms;

        for (size_t i = 0; i < pattern.size(); i++) {
            if (pattern[i] == '1') {
                terms.push_back("v" + vars[i] + "_1");
            }
            else if (pattern[i] == '0') {
                terms.push_back("¬" + "v" + vars[i] + "_1");
            }
        }

        std::string expr;

        if (!terms.empty()) {
            expr = terms[0];
            for (size_t i = 1; i < terms.size(); i++) {
                expr += " ∧ " + terms[i];
            }
        } else {
            expr = "TRUE";
        }

        expressions.push_back(expr);
    }

    // Combine cubes with OR
    std::string finalExpr;


    if (!expressions.empty()) {
        if (expressions.size() == 1) {
            // No brackets needed
            finalExpr = expressions[0];
        } else {
            // Multiple expressions → use brackets
            finalExpr = "(" + expressions[0] + ")";
            for (size_t i = 1; i < expressions.size(); i++) {
                finalExpr += " ∨ (" + expressions[i] + ")";
            }
        }
    }

    std::cout << "\n=== CUBE FORMULA ===\n";
    std::cout << finalExpr << "\n";

    std::cout << "\n=== INVARIANT ===\n";
    std::cout << "¬(" << finalExpr << ")\n";

    // === SMT-LIB CONVERSION ===
    std::vector<std::string> smtCubes;

    for (const std::string& cube : expressions) {
        std::stringstream ss(cube);
        std::string token;

        std::vector<std::string> smtTerms;

        while (ss >> token) {
            if (token == "∧") continue;

            if (token.rfind("¬", 0) == 0) {  // starts with ¬
                std::string neg = "¬";
                smtTerms.push_back("(not " + token.substr(neg.size()) + ")");
            } else {
                smtTerms.push_back(token);
            }
        }

        std::string smtCube;

        if (smtTerms.empty()) {
            smtCube = "true";
        }
        else if (smtTerms.size() == 1) {
            smtCube = smtTerms[0];
        }
        else {
            smtCube = "(and";
            for (const auto& t : smtTerms) {
                smtCube += " " + t;
            }
            smtCube += ")";
        }

        smtCubes.push_back(smtCube);
    }

    // === BUILD OR EXPRESSION ===
    std::string smtExpr;

    if (smtCubes.empty()) {
        smtExpr = "false";
    }
    else if (smtCubes.size() == 1) {
        smtExpr = smtCubes[0];
    }
    else {
        smtExpr = "(or";
        for (const auto& e : smtCubes) {
            smtExpr += " " + e;
        }
        smtExpr += ")";
    }

    // === PRINT OUTPUTS ===
    std::cout << "\n=== INVARIANT (SMT-LIB) ===\n";
    std::cout << "(assert (not " << smtExpr << "))\n";

    // === WRITE TO SMTLIB ===

    std::string outputFile = aigFile;

    if (posExt != std::string::npos) {
        outputFile.replace(posExt, 4, "_invariant.smtlib");
    } else {
        outputFile += "_invariant.smtlib";
    }

    std::ofstream smtFile(outputFile);

    if (smtFile.is_open()) {
        //smtFile << "; Auto-generated SMT-LIB invariant\n";

        //for (const auto& var : vars) {
        //    smtFile << "(declare-fun " << var << " () Bool)\n";
        //}

        //smtFile << "\n";
        smtFile << "(assert (not " << smtExpr << "))";
        //smtFile << "\n(check-sat)\n";

        smtFile.close();

        std::cout << "\nSMT-LIB written to " << outputFile << "\n";
    } else {
        std::cerr << "Error: Could not open file for writing.\n";
    }

    return 0;
}
