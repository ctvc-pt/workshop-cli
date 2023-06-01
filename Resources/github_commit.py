import base64
import json
import os
import requests
from datetime import datetime


def commit_and_push_folder(repo_url, branch_name, folder_path, commit_message, token):
    # Get the current branch's commit SHA
    current_commit_sha = get_branch_commit_sha(repo_url, branch_name, token)

    # Create a new tree for the commit
    tree_sha = create_commit_tree(repo_url, folder_path, token)

    # Create the commit using the new tree and current commit SHA
    new_commit_sha = create_commit(repo_url, commit_message, tree_sha, [current_commit_sha], branch_name, token)

    # Update the branch reference to the new commit
    update_branch_reference(repo_url, branch_name, new_commit_sha, token)

    print("Files committed and pushed successfully.")


def get_branch_commit_sha(repo_url, branch_name, token):
    url = f"https://api.github.com/repos/{repo_url}/git/ref/heads/{branch_name}"
    headers = {'Authorization': f"token {token}"}
    response = requests.get(url, headers=headers)
    response_json = response.json()

    return response_json['object']['sha']


def create_commit_tree(repo_url, folder_path, token):
    # Create a tree object for the commit
    tree = []

    # Iterate over all files and folders within the folder path
    for root, dirs, files in os.walk(folder_path):
        # Iterate over each file in the current directory
        for file_name in files:
            file_path = os.path.join(root, file_name)

            # Read the content of the file in binary mode
            file_content = read_file(file_path, binary=True)

            if file_content is None:
                print(f"Skipping file '{file_path}'. Unable to read file content.")
                continue

            # Create a new tree entry for the file
            relative_path = os.path.relpath(file_path, folder_path)
            tree.append({
                'path': relative_path.replace(os.sep, '/'),
                'mode': '100644',  # Regular file mode
                'type': 'blob',
                'content': base64.b64encode(file_content).decode('utf-8')
            })


    # Create the tree on GitHub using the REST API
    url = f"https://api.github.com/repos/{repo_url}/git/trees"
    headers = {'Authorization': f"token {token}"}
    data = {'tree': tree}
    response = requests.post(url, headers=headers, json=data)
    response_json = response.json()

    return response_json['sha']


def create_commit(repo_url, commit_message, tree_sha, parent_shas, branch_name, token):
    url = f"https://api.github.com/repos/{repo_url}/git/commits"
    headers = {'Authorization': f"token {token}"}
    data = {
        'message': commit_message,
        'tree': tree_sha,
        'parents': parent_shas
    }
    response = requests.post(url, headers=headers, json=data)
    response_json = response.json()

    return response_json['sha']


def update_branch_reference(repo_url, branch_name, commit_sha, token):
    url = f"https://api.github.com/repos/{repo_url}/git/refs/heads/{branch_name}"
    headers = {'Authorization': f"token {token}"}
    data = {'sha': commit_sha}
    requests.patch(url, headers=headers, json=data)


def read_file(file_path, binary=False):
    try:
        mode = 'rb' if binary else 'r'
        with open(file_path, mode) as file:
            return file.read()
    except:
        return None


# Usage example
token = "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"  # Replace with your GitHub access token
repo_owner = "cpdsWorkshop"  # Replace with the repository owner's username or organization name
repo_name = "workshops23"  # Replace with the repository name

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
    foldername = f"{name.replace(' ', '-')}_{datetime.now().strftime('%Y')}"
    folder_path = os.path.join(os.path.expanduser("~"), "Desktop", "repoWorkshop", foldername)
 


commit_message = step_id

commit_and_push_folder(repo_url, branch_name, folder_path, commit_message, token)
