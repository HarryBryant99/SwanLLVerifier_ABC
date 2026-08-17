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

void insertFilesIntoSMT(
    const std::string& targetFile,
    const std::vector<std::string>& insertFiles)
{
    std::string targetContent = readFile(targetFile);

    std::size_t pos = targetContent.find("(check-sat)");

    if (pos == std::string::npos)
    {
        throw std::runtime_error(
            "No (check-sat) found in " + targetFile);
    }

    std::string additions;

    for (const auto& file : insertFiles)
    {
        additions += readFile(file);
        additions += "\n";
    }

    targetContent.insert(pos, additions);

    std::ofstream output(targetFile);

    if (!output)
    {
        throw std::runtime_error(
            "Cannot write to " + targetFile);
    }

    output << targetContent;

    std::cout
        << "Inserted "
        << insertFiles.size()
        << " file(s) into "
        << targetFile
        << "\n";
}

int main(int argc, char* argv[])
{
    if (argc != 2)
    {
        std::cerr << "Usage: " << argv[0] << " <name>\n";
        return 1;
    }

    std::string name = argv[1];

    insertFilesIntoSMT(
    name + "_step.smt",
    {
        name + "_invariant.smtlib",
        name + "_invariant_base.smtlib"
    });
}