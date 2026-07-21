import os
import sys

def clean_filename(filename):
    """Remove prefix and suffix from filename"""
    prefix = r"SiemensData\Mostyn_946_Data\mostyn_iv_tptp_files\SiemensData_Additional_Mostyn_Properties_Mostyn10_GSP_"
    suffix = "_safetystep.tptp"
    
    # Remove prefix
    if filename.startswith(prefix):
        filename = filename[len(prefix):]
    
    # Remove suffix
    if filename.endswith(suffix):
        filename = filename[:-len(suffix)]
    
    return filename

def process_csv(input_file, output_file):
    """Process CSV file and clean filenames in first column"""
    try:
        with open(input_file, 'r', encoding='utf-8') as infile:
            lines = infile.readlines()
        
        cleaned_lines = []
        for line in lines:
            parts = line.split(',', 1)  # Split only on first comma
            if len(parts) == 2:
                cleaned_path = clean_filename(parts[0])
                cleaned_lines.append(cleaned_path + ',' + parts[1])
            else:
                cleaned_lines.append(line)
        
        with open(output_file, 'w', encoding='utf-8') as outfile:
            outfile.writelines(cleaned_lines)
        
        print(f"✓ Processed {len(cleaned_lines)} lines")
        print(f"✓ Output saved to: {output_file}")
    
    except FileNotFoundError:
        print(f"Error: File '{input_file}' not found")
        sys.exit(1)

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python clean_paths.py <input_csv> [output_csv]")
        print("Example: python clean_paths.py mostyn_tptp_iv_output_31_jul.csv cleaned_output.csv")
        sys.exit(1)
    
    input_file = sys.argv[1]
    output_file = sys.argv[2] if len(sys.argv) > 2 else "cleaned_" + input_file
    
    process_csv(input_file, output_file)