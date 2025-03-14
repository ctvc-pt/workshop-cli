import os
import shutil
import time

def get_desktop_path():
    # Gets the Desktop path
    return os.path.join(os.path.expanduser("~"), "Desktop")

def find_usb_drive():
    # Searches for a removable drive (USB) on Windows
    drives = [chr(x) + ":" for x in range(65, 91) if os.path.exists(chr(x) + ":")]
    for drive in drives:
        try:
            # Checks if it is a removable drive (USB)
            if os.path.exists(drive) and os.path.isdir(drive) and 'Removable' in os.popen(f"fsutil fsinfo drivetype {drive}").read():
                return drive
        except:
            continue
    return None

def copy_repo_to_usb(table_number):
    # Path to the repoWorkshop folder on the Desktop
    desktop_path = get_desktop_path()
    source_folder = os.path.join(desktop_path, "repoWorkshop")
    
    # Checks if the folder exists
    if not os.path.exists(source_folder):
        print("Error: The 'repoWorkshop' folder was not found on the Desktop!")
        return
    
    # Finds the USB drive
    usb_drive = find_usb_drive()
    if usb_drive is None:
        print("Error: No USB drive detected! Please insert a USB drive and try again.")
        return
    
    # Destination path on the USB drive with table number
    destination_folder = os.path.join(usb_drive, f"workshopBackup-{table_number}")
    
    try:
        # Checks if the folder already exists on the USB drive and removes it if necessary
        if os.path.exists(destination_folder):
            print(f"The 'workshopBackup-{table_number}' folder already exists on the USB drive ({usb_drive}). Replacing...")
            shutil.rmtree(destination_folder)
        
        # Copies the folder to the USB drive
        print(f"Copying 'repoWorkshop' from the Desktop to {usb_drive} as 'workshopBackup-{table_number}'...")
        shutil.copytree(source_folder, destination_folder)
        print("Copy completed successfully!")
    
    except Exception as e:
        print(f"Error copying the folder: {e}")

def main():
    print("Starting the copy script to the USB drive...")
    
    # Asks for the table number
    while True:
        table_number = input("Please enter the table number: ").strip()
        if table_number.isdigit() and int(table_number) > 0:
            break
        print("Invalid input! Please enter a valid table number (positive integer).")
    
    copy_repo_to_usb(table_number)
    print("\nPress Enter to exit...")
    input()

if __name__ == "__main__":
    main()