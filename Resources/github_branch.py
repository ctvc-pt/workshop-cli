import os
import json
from datetime import datetime
from github import Github
import subprocess

def create_branch_and_clone(repo_owner, repo_name, name_file_path, clone_destination, token):
    try:
        # Connect to the GitHub repository using the access token
        g = Github(token)
        repo = g.get_repo(f"{repo_owner}/{repo_name}")

        # Read the name from the name file
        with open(name_file_path, "r") as name_file:
            name_data = json.load(name_file)
            name = name_data.get("Name", "")
        
        # Create a branch name with the format: {name}_{date}
        branch_name = f"{name.replace(' ', '-')}_{datetime.now().strftime('%d-%m-%Y')}"

        # Create a new branch
        main_branch = repo.get_branch("main")
        new_branch = repo.create_git_ref(ref=f"refs/heads/{branch_name}", sha=main_branch.commit.sha)

        print(f"Branch '{branch_name}' created successfully.")

        # Print the SHA of the created branch for debugging
        print(f"Branch SHA: {new_branch.object.sha}")

        # Clone the repository
        clone_url = repo.clone_url.replace("https://", f"https://{token}@")  # Append token to URL
        clone_destination_folder = os.path.join(clone_destination, repo_name)
        subprocess.call(f"git clone {clone_url} {clone_destination_folder}", shell=True)

        # Change directory to the cloned repository folder
        os.chdir(clone_destination_folder)

        # Checkout the desired branch
        subprocess.call(f"git checkout {branch_name}", shell=True)
        print(f"Repository cloned to branch '{branch_name}' in '{clone_destination_folder}'.")
    except Exception as e:
        print(f"An error occurred: {e}")

# Usage example
script_directory = os.path.dirname(os.path.abspath(__file__))
json_file_path = os.path.join(script_directory, 'token.json')

with open(json_file_path) as f:
    data = json.load(f)

token = data['TokenGit']
repo_owner = "cpdscrl"  # Replace with the repository owner's username or organization name
repo_name = "workshop-progress"  # Replace with the repository name
clone_destination = os.path.join(os.path.expanduser("~"), "Desktop", "repoWorkshop")
name_file_path = os.path.join(script_directory, "session.txt")  

create_branch_and_clone(repo_owner, repo_name, name_file_path, clone_destination, token)

#git fetch && git pull
