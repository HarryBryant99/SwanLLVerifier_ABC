import csv
import sys

# List of properties to extract
initTARGET_PROPERTIES = {
"2_1_16_SignalReminder_group23",
"2_1_16_SignalReminder_group27",
"2_1_23_TrackSectionsInLineOfRoute_group1",
"2_1_23_TrackSectionsInLineOfRoute_group10",
"2_1_23_TrackSectionsInLineOfRoute_group11",
"2_1_23_TrackSectionsInLineOfRoute_group18",
"2_1_23_TrackSectionsInLineOfRoute_group20",
"2_1_23_TrackSectionsInLineOfRoute_group23",
"2_1_23_TrackSectionsInLineOfRoute_group4",
"2_1_23_TrackSectionsInLineOfRoute_group5",
"2_1_24_OverlapSectionsInLineOfRoute_group8",
"2_1_24_OverlapSectionsInLineOfRoute_group9",
"2_1_4_LastOpposingRouteSection_group1",
"2_1_4_LastOpposingRouteSection_group15",
"2_1_4_LastOpposingRouteSection_group17",
"2_1_4_LastOpposingRouteSection_group3",
"2_1_4_LastOpposingRouteSection_group4",
"2_1_4_LastOpposingRouteSection_group8",
"2_1_4_LastOpposingRouteSection_group9",
"2_1_5_LastOpposingOverlapRouteSection_group0",
"2_1_5_LastOpposingOverlapRouteSection_group1",
"2_1_5_LastOpposingOverlapRouteSection_group2",
"2_2_2_ProceedWithRouteSectionsLocked_group1",
"2_2_2_ProceedWithRouteSectionsLocked_group18",
"2_2_2_ProceedWithRouteSectionsLocked_group20",
"2_2_2_ProceedWithRouteSectionsLocked_group4",
"2_2_2_ProceedWithRouteSectionsLocked_group5",
"2_2_24_ProceedDuringEPR_group11",
"2_2_24_ProceedDuringEPR_group21",
"2_2_24_ProceedDuringEPR_group23",
"2_2_24_ProceedDuringEPR_group9",
"2_2_25_ProceedDuringSTR_group18",
"2_2_25_ProceedDuringSTR_group20",
"2_2_25_ProceedDuringSTR_group7",
"2_2_25_ProceedDuringSTR_group9",
"2_2_27_ProceedWithAspRestriction_group1",
"2_2_27_ProceedWithAspRestriction_group11",
"2_2_27_ProceedWithAspRestriction_group12",
"2_2_27_ProceedWithAspRestriction_group13",
"2_2_27_ProceedWithAspRestriction_group16",
"2_2_27_ProceedWithAspRestriction_group17",
"2_2_27_ProceedWithAspRestriction_group18",
"2_2_27_ProceedWithAspRestriction_group19",
"2_2_27_ProceedWithAspRestriction_group20",
"2_2_27_ProceedWithAspRestriction_group21",
"2_2_27_ProceedWithAspRestriction_group23",
"2_2_27_ProceedWithAspRestriction_group4",
"2_2_27_ProceedWithAspRestriction_group5",
"2_2_27_ProceedWithAspRestriction_group9",
"2_2_32_ProceedWithTPWSSuppressed_group5",
"2_2_34_RYGAspectWithSOMOutput_group3",
"2_2_55_SignalDisengagement_group22",
"2_2_55_SignalDisengagement_group24",
"2_2_56_ProceedWithRearPermissiveMovement_group0",
"2_2_56_ProceedWithRearPermissiveMovement_group1",
"2_2_56_ProceedWithRearPermissiveMovement_group2",
"2_2_7_ProceedWithTracksClear_group20"
}



# prefix each property in TARGET_PROPERTIES with "SiemensData\Mostyn_946_Data\mostyn_iv_tptp_files\SiemensData_Additional_Mostyn_Properties_Mostyn10_GSP_"
# and suffix with "_safetystep.tptp"
TARGET_PROPERTIES = {
    f"SiemensData\\Mostyn_946_Data\\mostyn_iv_tptp_files\\SiemensData_Additional_Mostyn_Properties_Mostyn10_GSP_{prop}_safetystep.tptp"
    for prop in initTARGET_PROPERTIES
}

def extract_matching_rows(input_file, output_file):
    """Extract rows matching TARGET_PROPERTIES from input CSV to output CSV"""
    try:
        with open(input_file, 'r', encoding='utf-8') as infile:
            reader = csv.reader(infile)
            header = next(reader)  # Read header
            
            matching_rows = [header]
            found_count = 0
            
            for row in reader:
                if row and row[0] in TARGET_PROPERTIES:
                    matching_rows.append(row)
                    found_count += 1
        
        # Write matching rows to output file
        with open(output_file, 'w', encoding='utf-8', newline='') as outfile:
            writer = csv.writer(outfile)
            writer.writerows(matching_rows)
        
        print(f"✓ {input_file}: Found {found_count} matching rows")
        print(f"✓ Output saved to: {output_file}")
        return found_count
    
    except FileNotFoundError:
        print(f"✗ Error: File '{input_file}' not found")
        return 0

if __name__ == "__main__":
    # Extract from both files
    file1 = "mostyn_tptp_iv_output_31_jul.csv"
    # file2 = "csvs/V11_NewVerifier_versus_IC3_Runs.csv"
    
    out1 = "extracted_mostyn_tptp_iv_output_31_jul.csv"
    # out2 = "extracted_V11_NewVerifier_versus_IC3_Runs.csv"
    
    total1 = extract_matching_rows(file1, out1)
    # total2 = extract_matching_rows(file2, out2)
    
    print(f"\n✓ Total matching rows extracted: {total1 + total2}")
    print(f"✓ Target properties: {len(TARGET_PROPERTIES)}")