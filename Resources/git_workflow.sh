#!/bin/bash

# Get the absolute path of the script
script_dir=$(dirname "$(readlink -f "$0")")
json_file="session.txt"
name=$(grep -o '"Name":"[^"]*' "$json_file" | cut -d '"' -f 4)
# Construct the SSH key path
ssh_key_path="$script_dir/my_repo_deploy_key"
echo "-----------------------PATH: $ssh_key_path"
# Start SSH agent and add SSH key
eval "$(ssh-agent -s)"
ssh-add $ssh_key_path

# Clone the repository
git clone git@github.com:cpdsWorkshop/workshop2023.git

# Change to the cloned repository directory
cd repoWorkshop

# Checkout the main branch
git checkout main

# Pull the latest changes from the remote repository
git pull

# Create and switch to a new branch
git branch $name
git checkout $name

# Add files to commit
git add .

# Create a commit
git commit -m "Commit $name"

# Push changes to the remote repository
git push origin $name