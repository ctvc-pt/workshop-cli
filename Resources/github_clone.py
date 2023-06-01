import subprocess
from github import Github
import os


token = "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"
g = Github(token)

repo_owner = "cpdsWorkshop"  # Replace with the repository owner's username or organization name
repo_name = "workshops23"  # Replace with the repository name
destination_folder = os.path.join(os.path.expanduser("~"), "Desktop", "repoWorkshop")

repo = g.get_repo(f"{repo_owner}/{repo_name}")
clone_url = repo.clone_url

# Clone the repository using Git command
command = f"git clone {clone_url} {destination_folder}"
subprocess.call(command, shell=True)