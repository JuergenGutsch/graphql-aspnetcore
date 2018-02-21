# How to contribute

This is a short introductional documentation about how to contribute to this repository.

## Git-Flow

I follow the git-flow process to work with git. git-flow is based on two main branches, which are "master" (the production branch) and "develop" (the next version branch). I never commit directly to "master". 

I also almost never commit directly to "develop". git-flow enforces me to follow the principle of feature-branches, which means to create a branch for every change I want to do. git-flow knows five kinds of feature-branches:

- feature
  - used for new features, changes, enhancements
  - created out of "develop"
  - merged back to "develop"
- bugfix
  - used for bug-fixes 
  - created out of "develop"
  - merged back to "develop"
  - not supported in SourceTree
- release   
  - used for creating a new release 
  - created out of "develop"
  - merged than to "master"
- hotfix
  - used for hotfix your production code
  - created out of "master"
  - merged to "master" and "develop"
- support
  - I never used this
  - not supported in SourceTree

Each of the feature branch types follows it's own process to branch, merge and tag your code. "release" and "hotfix" also tag your code. The tag is created out of the feature branch name, so the name should at least contain the version number. It always does in my repos.

I use git-flow using the console. This could be installed as an add-in for the git CLI. the syntax is "git flow <command>"

Some tools like SourceTree also support git-flow

In general git-flow bundles common git commands to just one command, so it helps, it saves time and makes the work with git a lot faster. You can do the same stuff with the native git commands, but this is a lot more work

## Contributing 

To contribute to this repository, please follow the git-flow process. Do it either using a tool that supports git-flow or do it manual. 

Because you shouldn't touch the "master" branch and I will not accept a PR that is changing the "master" branch, please just create "feature" or "bugfix" branches. The names of those feature branches should look like this:

- feature/[short-title-without-spaces-or-github-ticket-name]

- bugfix/[short-title-without-spaces-or-github-ticket-name]

If you're done, please push your branch to your fork on GitHub and create a PR out of that. GitHub will support you doing this. Link it to the relevant GitHub issue and add a description. GitHub will check for conflicts and AppVeyor will check for build and test errors. 

I will not accept conflicting PRs, so please fix the conflicts in your feature branch. Be sure your branch is up-to-date.

I will not accepting failing PRs, so please be sure the build and all the tests are working on your machine. To ensure this, run the build.ps1 in a PowerShell console on your machine.