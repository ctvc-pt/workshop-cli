#!/bin/bash
ssh_key_path=$1
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
git branch teste
git checkout teste

# Add files to commit
git add .

# Create a commit
git commit -m "Commit teste"

# Push changes to the remote repository
git push origin teste