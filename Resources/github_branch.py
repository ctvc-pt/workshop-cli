import os
import json
from datetime import datetime
from github import Github

def create_branch(repo_owner, repo_name, name_file_path, token):
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
    repo.create_git_ref(ref=f"refs/heads/{branch_name}", sha=main_branch.commit.sha)

    print(f"Branch '{branch_name}' created successfully.")

# Usage example
token = "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"  # Replace with your GitHub access token
repo_owner = "cpdsWorkshop"  # Replace with the repository owner's username or organization name
repo_name = "workshops23"  # Replace with the repository name

# Get the directory of the current file
current_directory = os.path.dirname(os.path.abspath(__file__))

name_file_path = os.path.join(current_directory, "session.txt")  

create_branch(repo_owner, repo_name, name_file_path, token)
