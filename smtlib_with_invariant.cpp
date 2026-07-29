#include <fstream>
#include <iostream>
#include <sstream>
#include <string>

int main(int argc, char* argv[])
{
    if (argc != 2)
    {
        std::cerr << "Usage: " << argv[0] << " <name>\n";
        return 1;
    }

    std::string name = argv[1];
    std::string filename = name + "_step.smt";

    std::ifstream input(filename);
    if (!input)
    {
        std::cerr << "Cannot open " << filename << "\n";
        return 1;
    }

    std::stringstream buffer;
    buffer << input.rdbuf();
    std::string content = buffer.str();
    input.close();

    std::string invariant = "(assert INV)\n";

    std::size_t pos = content.find("(check-sat)");
    if (pos == std::string::npos)
    {
        std::cerr << "No (check-sat) found in file\n";
        return 1;
    }

    content.insert(pos, invariant);

    std::ofstream output(filename);
    if (!output)
    {
        std::cerr << "Cannot write " << filename << "\n";
        return 1;
    }

    output << content;

    std::cout << "Inserted invariant into " << filename << "\n";
    return 0;
}