import json
import sys

def get_json():
  if len(sys.argv) != 3:
    print("Error: Please provide a JSON file name as an argument.")
    print("Correct program usage:")
    print(" python validate.py <input file> <output file>")
    
    input_file = "test.json"
    output_file = "results.txt"
    print(f"Using default files '{input_file}' and '{output_file}'")
    sys.argv.append(input_file)
    sys.argv.append(output_file)

  try:
    filename = sys.argv[1]
    with open(filename, 'r') as r:
      data = json.load(r)
      r.close()
      return data["haikus"]
  
  except FileNotFoundError:
    print(f"File {filename} not found.")
    sys.exit(1)

  except Exception as e:
    print(f"An unexpected error has occured: {e}")
    sys.exit(1)
    print("Correct program usage:")
    print(" python validate.py <input file> <output file>")

def validate_line3(line, bad_lines):
  if not line.endswith(".") and not line.endswith("?") and line not in bad_lines:
    bad_lines.append(line)


if __name__ == "__main__":
  # Get haikus array
  haikus = get_json()
  total_haikus = 0

  try:
    with open(sys.argv[2], 'w') as f:

      # Bad lines without periods
      bad_lines = []

      for haiku in haikus:
        f.write("Haiku Set:\n")
        f.write("__________\n\n")

        for a in haiku["one"]:
          for b in haiku["two"]:
            for c in haiku["three"]:
              f.write(f"{a['en']}\n") # Line 1
              f.write(f"{b['en']}\n") # Line 2
              f.write(f"{c['en']}\n") # Line 3
              f.write("\n")
              validate_line3(c["en"], bad_lines)
              total_haikus += 1
        
      f.write(f"\nTotal haikus: {total_haikus}\n")
      if len(bad_lines) == 0:
        f.write("\nAll lines are good.")
      else:
        f.write("\nBad Lines:\n")
        f.write("______________\n\n")
        for line in bad_lines:
          f.write(f"{line}\n\n")

  except FileNotFoundError:
    print("Output file not found")
    sys.exit(1)


  
