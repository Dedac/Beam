#!/usr/bin/env bash
set -euo pipefail

workspace_folder="${1:?Workspace folder argument is required}"
export ASPNETCORE_ENVIRONMENT=Development
cd "$workspace_folder"

dotnet restore
dotnet tool restore
cd Beam.Server

for attempt in {1..30}; do
    if dotnet ef database update; then
        break
    fi

    if (( attempt == 30 )); then
        echo "Database migration failed after 30 attempts." >&2
        exit 1
    fi

    echo "SQL Server is not ready; retrying migration in 2 seconds..."
    sleep 2
done

dotnet dev-certs https
