param name string
param location string = resourceGroup().location

param gitHubBranchName string = 'main'
@secure()
param gitHubAccessToken string

var userAssignedIdentity = {
    name: 'spn-${name}'
    location: location
}

resource uai 'Microsoft.ManagedIdentity/userAssignedIdentities@2022-01-31-preview' = {
    name: userAssignedIdentity.name
    location: userAssignedIdentity.location
}

var roleAssignment = {
    name: guid(resourceGroup().id, 'contributor')
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions', 'b24988ac-6180-42a0-ab88-20f7382dd24c')
    principalType: 'ServicePrincipal'
}

resource role 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
    name: roleAssignment.name
    scope: resourceGroup()
    properties: {
        roleDefinitionId: roleAssignment.roleDefinitionId
        principalId: uai.properties.principalId
        principalType: roleAssignment.principalType
    }
}

var deploymentScript = {
    name: 'depscrpt-${name}'
    location: location
    resourceName: name
    gitHubBranchName: gitHubBranchName
    gitHubAccessToken: gitHubAccessToken
    containerGroupName: 'contgrp-${name}'
    azureCliVersion: '2.40.0'
    scriptUri: 'https://raw.githubusercontent.com/justinyoo/ignite-spotlight-demo/${gitHubBranchName}/infra/setup-resources.sh'
}

resource ds 'Microsoft.Resources/deploymentScripts@2020-10-01' = {
    name: deploymentScript.name
    location: deploymentScript.location
    dependsOn: [
        role
    ]
    kind: 'AzureCLI'
    identity: {
        type: 'UserAssigned'
        userAssignedIdentities: {
            '${uai.id}': {}
        }
    }
    properties: {
        azCliVersion: deploymentScript.azureCliVersion
        containerSettings: {
            containerGroupName: deploymentScript.containerGroupName
        }
        environmentVariables: [
            {
                name: 'AZURE_ENV_NAME'
                value: deploymentScript.resourceName
            }
            {
                name: 'AZURE_LOCATION'
                value: deploymentScript.location
            }
            {
                name: 'AZURE_SUBSCRIPTION_ID'
                value: subscription().subscriptionId
            }
            {
                name: 'GH_ACCESS_TOKEN'
                value: deploymentScript.gitHubAccessToken
            }
        ]
        primaryScriptUri: deploymentScript.scriptUri
        cleanupPreference: 'OnExpiration'
        retentionInterval: 'P1D'
    }
}
