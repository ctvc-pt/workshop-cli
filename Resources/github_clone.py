import subprocess
from github import Github
import os
import json

script_directory = os.path.dirname(os.path.abspath(__file__))
json_file_path = os.path.join(script_directory, 'token.json')

with open(json_file_path) as f:
    data = json.load(f)

token = data['TokenGit']
g = Github(token)

repo_owner = "cpdsWorkshop"  # Replace with the repository owner's username or organization name
repo_name = "workshops23"  # Replace with the repository name
destination_folder = os.path.join(os.path.expanduser("~"), "Desktop", "repoWorkshop")

repo = g.get_repo(f"{repo_owner}/{repo_name}")
clone_url = repo.clone_url

# Clone the repository using Git command
command = f"git clone {clone_url} {destination_folder}"
subprocess.call(command, shell=True)