#!/bin/bash

# Get the absolute path of the script
script_dir=$(dirname "$(readlink -f "$0")")
json_file="session.txt"
name=$(grep -o '"Name":"[^"]*' "$json_file" | cut -d '"' -f 4)
# Construct the SSH key path
ssh_key_path="$script_dir"

# Start SSH agent and add SSH key
eval "$(ssh-agent -s)"
ssh-add $ssh_key_path

git checkout $name

# Add files to commit
git add workshop2023/teste.txt

# Create a commit
git commit -m "Commit $name"

# Push changes to the remote repository
git push origin $name