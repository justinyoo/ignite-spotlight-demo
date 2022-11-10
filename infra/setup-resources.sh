#!/bin/bash

set -e

# Invoke GitHub Actions workflow_dispatch API
dispatched=$(curl -H "Accept: application/vnd.github+json" \
    -H "User-Agent: Autopilot" \
    -H "Authorization: token $GH_ACCESS_TOKEN" \
    https://api.github.com/repos/justinyoo/ignite-spotlight-demo/actions/workflows/azure-dev.yaml/dispatches \
    -d "{ \"ref\": \"main\", \"inputs\": { \"from_portal\": \"true\" } }")
