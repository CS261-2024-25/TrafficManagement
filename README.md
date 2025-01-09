# TrafficManagement
CS261 coursework.

### Table of contents
- [Setup](#setup)
- [General housekeeping](#general-housekeeping)
- [Testing](#testing)

# Setup
Dependencies you'll need installed. This is OS-dependent so I won't be able to help you here. You'll need:
- Dotnet SDK 9.0
- Unity 6


# General housekeeping
All work should go inside of Assets. Do not push binaries to the repository. Build the project inside `TrafficManagement/build/`.

## Conventional git
If you have any questions or problems, please ask!!!
Following [this guide](https://www.conventionalcommits.org/en/v1.0.0/) do. Generally, commit messages should be of the form:
```
feat(login): implemented database linkage to application frontend
```
Where you have the type of commit, followed by what part of the app you changed in brackets, and a detailed message indicating what exactly was changed.

Pushing to main wont be possible, so you'll need to **CREATE A NEW BRANCH FOR YOUR WORK BEFORE YOU START WORKING**. Otherwise you will have a headache of a time solving. This is a warning!!!

Publish your branch to the GitHub and create a __pull request__ when ready to get work reviewed. We'll then merge to main and continue developing.

# Testing
We'll be using the `Unity Test Framework` for this. Find the docs [here](https://docs.unity3d.com/Packages/com.unity.test-framework@1.1/manual/). I've set up an example test in `Assets/Scripts/Backend`, which is a `Test Assembly` generated in Unity inside of `Backend/Tests`, and then a `Test Script` inside of that directory. ChatGPT will probably get it wrong so please **read the docs**.