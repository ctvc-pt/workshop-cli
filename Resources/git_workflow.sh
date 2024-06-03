#!/bin/bash

# Get the absolute path of the script
script_dir=$(dirname "$(readlink -f "$0")")
json_file="session.txt"
name=$(grep -o '"NameId":"[^"]*' "$json_file" | cut -d '"' -f 4)
echo "++++++++++++++++++ $name"
# Construct the SSH key path
ssh_key_path="$script_dir/id_ed25519"
echo "-----------------------PATH: $ssh_key_path"
# Change the file permissions to 600
chmod 600 "$ssh_key_path"
# Start SSH agent and add SSH key
eval "$(ssh-agent -s)"
ssh-add $ssh_key_path

# Clone the repository
git clone git@github.com:cpdscrl/workshop-progress.git

# Change to the cloned repository directory
cd workshop-progress

pwd

# Checkout the main branch
git checkout main

# Pull the latest changes from the remote repository
git pull

# Create and switch to a new branch
git branch $name
git checkout $name

folder="$script_dir/work-progress"
# Add files to commit
git add .


# Create a commit
git commit -m "Commit $name"

# Push changes to the remote repository
git push origin $name