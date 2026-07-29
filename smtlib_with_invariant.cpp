#include <fstream>
#include <iostream>
#include <sstream>
#include <string>

std::string readFile(const std::string& filename)
{
    std::ifstream file(filename);

    if (!file)
    {
        throw std::runtime_error("Cannot open file: " + filename);
    }

    std::stringstream buffer;
    buffer << file.rdbuf();
    return buffer.str();
}

int main(int argc, char* argv[])
{
    if (argc != 2)
    {
        std::cerr << "Usage: " << argv[0] << " <name>\n";
        return 1;
    }

    std::string name = argv[1];

    std::string stepFile = name + "_step.smt";
    std::string invariantFile = name + "_invariant.smtlib";

    try
    {
        std::string stepContent = readFile(stepFile);
        std::string invariantContent = readFile(invariantFile);

        std::size_t pos = stepContent.find("(check-sat)");

        if (pos == std::string::npos)
        {
            std::cerr << "No (check-sat) found in " << stepFile << "\n";
            return 1;
        }

        stepContent.insert(pos, invariantContent + "\n");

        std::ofstream output(stepFile);

        if (!output)
        {
            std::cerr << "Cannot write to " << stepFile << "\n";
            return 1;
        }

        output << stepContent;

        std::cout << "Inserted contents of "
                  << invariantFile
                  << " into "
                  << stepFile
                  << "\n";
    }
    catch (const std::exception& e)
    {
        std::cerr << e.what() << "\n";
        return 1;
    }

    return 0;
}