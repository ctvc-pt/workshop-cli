import os
import io
import json
import requests
import base64
from datetime import datetime

def commit_and_push_folder(repo_owner, repo_name, branch_name, folder_path, commit_message, token):
    # Get the current branch's commit SHA
    current_commit_sha = get_branch_commit_sha(repo_owner, repo_name, branch_name, token)

    # Create a new tree for the commit
    tree_sha = create_commit_tree(repo_owner, repo_name, folder_path, token)

    # Create the commit using the new tree and current commit SHA
    new_commit_sha = create_commit(repo_owner, repo_name, commit_message, tree_sha, [current_commit_sha], branch_name, token)

    # Update the branch reference to the new commit
    update_branch_reference(repo_owner, repo_name, branch_name, new_commit_sha, token)

    print("Files committed and pushed successfully.")

def get_branch_commit_sha(repo_owner, repo_name, branch_name, token):
    url = f"https://api.github.com/repos/{repo_owner}/{repo_name}/git/ref/heads/{branch_name}"
    headers = {'Authorization': f"token {token}"}
    response = requests.get(url, headers=headers)
    response_json = response.json()
    print("Response JSON:", response_json)  # Add this line for debugging
    return response_json['object']['sha']

def create_commit_tree(repo_owner, repo_name, folder_path, token):
    # Print the folder path for debugging
    print("Folder Path:", folder_path)

    # Create a tree object for the commit
    tree = []

    # Check if the folder exists
    if not os.path.exists(folder_path):
        print(f"Folder '{folder_path}' does not exist.")
        return None

    # Iterate over all files and folders within the folder path
    for root, dirs, files in os.walk(folder_path):
        # Iterate over each file in the current directory
        for file_name in files:
            file_path = os.path.join(root, file_name)

            if file_path.endswith(('.png', '.jpg', '.jpeg', '.gif')):
                # Read and encode the content of image files as base64 strings
                with open(file_path, 'rb') as file:
                    file_content = base64.b64encode(file.read()).decode('utf-8')
            else:
                # Read the content of other files
                with open(file_path, 'r', encoding='utf-8') as file:
                    file_content = file.read()

            # Print the file content for debugging
            print(f"File Content ({file_path}):", file_content)

            if file_content is None:
                print(f"Skipping file '{file_path}'. Unable to read file content.")
                continue

            # Create a new tree entry for the file
            relative_path = os.path.relpath(file_path, folder_path)
            tree.append({
                'path': relative_path.replace(os.sep, '/'),
                'mode': '100644',  # Regular file mode
                'type': 'blob',
                'content': file_content
            })
            print(f"Added file '{file_path}' to the tree.")  # Print for debugging

    # Print the tree for debugging
    print("Tree:", tree)

    # Check if any files were found
    if not tree:
        print(f"No files found in folder '{folder_path}'.")
        return None

    # Create the tree on GitHub using the REST API
    url = f"https://api.github.com/repos/{repo_owner}/{repo_name}/git/trees"
    headers = {'Authorization': f"token {token}"}
    data = {'tree': tree}
    print("Request Data:", data)  # Print the request data for debugging
    response = requests.post(url, headers=headers, json=data)
    response_json = response.json()

    # Print the response JSON for further debugging
    print("Response JSON:", response_json)

    # Access 'sha' key under 'object' if the response is as expected
    return response_json.get('sha', '')

def create_commit(repo_owner, repo_name, commit_message, tree_sha, parent_shas, branch_name, token):
    url = f"https://api.github.com/repos/{repo_owner}/{repo_name}/git/commits"
    headers = {'Authorization': f"token {token}"}
    data = {
        'message': commit_message,
        'tree': tree_sha,
        'parents': parent_shas
    }
    response = requests.post(url, headers=headers, json=data)
    response_json = response.json()

    return response_json['sha']

def update_branch_reference(repo_owner, repo_name, branch_name, commit_sha, token):
    url = f"https://api.github.com/repos/{repo_owner}/{repo_name}/git/refs/heads/{branch_name}"
    headers = {'Authorization': f"token {token}"}
    data = {'sha': commit_sha}
    requests.patch(url, headers=headers, json=data)

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

commit_message = step_id

commit_and_push_folder(repo_owner, repo_name, branch_name, folder_path, commit_message, token)