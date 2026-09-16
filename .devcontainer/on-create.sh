#!/usr/bin/env bash
set -euo pipefail

workspace_folder="${1:?Workspace folder argument is required}"
export ASPNETCORE_ENVIRONMENT=Development
cd "$workspace_folder"

dotnet restore
dotnet tool restore
cd Beam.Server
dotnet ef database update
dotnet dev-certs https
