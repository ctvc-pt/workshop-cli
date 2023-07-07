#!/bin/bash

# Get the absolute path of the script
script_dir=$(dirname "$(readlink -f "$0")")
json_file="session.txt"
name=$(grep -o '"NameId":"[^"]*' "$json_file" | cut -d '"' -f 4)
step_id=$(grep -o '"StepId":"[^"]*' "$json_file" | cut -d '"' -f 4)
echo "++++++++++++++++++ $name"
# Construct the SSH key path
ssh_key_path="$script_dir/my_repo_deploy_key"
echo "-----------------------PATH: $ssh_key_path"
# Change the file permissions to 600
chmod 600 "$ssh_key_path"
# Start SSH agent and add SSH key
eval "$(ssh-agent -s)"
ssh-add $ssh_key_path
pwd
git checkout $name

# Change to the cloned repository directory
cd workshop2023


# Add files to commit
git add .

# Create a commit
git commit -m "Commit $step_id"

# Push changes to the remote repository
git push origin $name