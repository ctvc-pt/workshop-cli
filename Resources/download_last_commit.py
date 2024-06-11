import os
import json
import requests
import base64
from datetime import datetime

def download_and_replace_folder(repo_owner, repo_name, branch_name, folder_path, token):
    # Get the current branch's tree SHA
    tree_sha = get_branch_tree_sha(repo_owner, repo_name, branch_name, token)

    # Download the files from the tree
    files = download_files_from_tree(repo_owner, repo_name, tree_sha, token)

    # Replace the contents of the folder with the downloaded files
    replace_folder_contents(folder_path, files)

    print("Files restored successfully.")

def get_branch_tree_sha(repo_owner, repo_name, branch_name, token):
    url = f"https://api.github.com/repos/{repo_owner}/{repo_name}/git/trees/{branch_name}?recursive=1"
    headers = {'Authorization': f"token {token}"}
    response = requests.get(url, headers=headers)
    response_json = response.json()
    return response_json['sha']

def download_files_from_tree(repo_owner, repo_name, tree_sha, token):
    url = f"https://api.github.com/repos/{repo_owner}/{repo_name}/git/trees/{tree_sha}?recursive=1"
    headers = {'Authorization': f"token {token}"}
    response = requests.get(url, headers=headers)
    response_json = response.json()
    files = {}

    for item in response_json['tree']:
        if item['type'] == 'blob':
            file_url = item['url']
            file_response = requests.get(file_url, headers=headers)
            file_content = base64.b64decode(file_response.json()['content'])
            files[item['path']] = file_content

    return files

def replace_folder_contents(folder_path, files):
    if os.path.exists(folder_path):
        for root, dirs, file_names in os.walk(folder_path):
            for file_name in file_names:
                file_path = os.path.join(root, file_name)
                os.remove(file_path)
    
    for file_path, file_content in files.items():
        full_path = os.path.join(folder_path, file_path)
        os.makedirs(os.path.dirname(full_path), exist_ok=True)
        with open(full_path, 'wb') as file:
            file.write(file_content)

# Usage example
script_directory = os.path.dirname(os.path.abspath(__file__))
json_file_path = os.path.join(script_directory, 'token.json')

with open(json_file_path) as f:
    data = json.load(f)

token = data['TokenGit']
repo_owner = "cpdscrl"  # Replace with the repository owner's username or organization name
repo_name = "workshop-progress"  # Replace with the repository name

repo_url = f"{repo_owner}/{repo_name}"

# Get the directory of the current file
current_directory = os.path.dirname(os.path.abspath(__file__))

session_file_path = os.path.join(current_directory, "session.txt")

# Read the name from the session file
with open(session_file_path, "r", encoding="utf-8") as session_file:
    session_data = json.load(session_file)
    name = session_data.get("Name", "")
    step_id = session_data.get("StepId", "")
    branch_name = f"{name.replace(' ', '-')}_{datetime.now().strftime('%d-%m-%Y')}"

folder_path = os.path.join(os.path.expanduser("~"), "Desktop", "repoWorkshop", branch_name)

download_and_replace_folder(repo_owner, repo_name, branch_name, folder_path, token)
