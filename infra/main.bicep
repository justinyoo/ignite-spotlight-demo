targetScope = 'subscription'

param name string
param location string = 'Korea Central'

param apiManagementPublisherName string = 'Ignite Spotlight Demo'
param apiManagementPublisherEmail string = 'apim@contoso.com'
param gitHubBranchName string = 'main'
@secure()
param gitHubAccessToken string

var apps = [
    {
        suffix: 'maps'
        apiName: 'MAPS'
        apiPath: 'maps'
        apiFormat: 'openapi+json-link'
        apiExtension: 'json'
    }
    {
        suffix: 'sms'
        apiName: 'SMS'
        apiPath: 'sms'
        apiFormat: 'openapi+json-link'
        apiExtension: 'json'
    }
    {
        suffix: 'sms-verify'
        apiName: 'SMS-VERIFY'
        apiPath: 'sms/verify'
        apiFormat: 'openapi+json-link'
        apiExtension: 'json'
    }
    // {
    //     suffix: 'bff'
    //     apiName: 'BFF'
    //     apiPath: 'bff'
    //     apiFormat: 'openapi-link'
    //     apiExtension: 'yaml'
    // }
    // {
    //     suffix: 'kakao'
    //     apiName: 'KAKAO'
    //     apiPath: 'kakao'
    //     apiFormat: 'openapi-link'
    //     apiExtension: 'yaml'
    // }
]
var storageContainerName = 'openapis'

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
    name: 'rg-${name}'
    location: location
}

module sttapp './staticWebApp.bicep' = {
    name: 'StaticWebApp'
    scope: rg
    params: {
        name: name
        location: 'eastasia'
    }
}

module apim './provision-apiManagement.bicep' = {
    name: 'ApiManagement'
    scope: rg
    params: {
        name: name
        location: location
        storageContainerName: storageContainerName
        apiMgmtPublisherName: apiManagementPublisherName
        apiMgmtPublisherEmail: apiManagementPublisherEmail
        apiMgmtPolicyFormat: 'xml-link'
        apiMgmtPolicyValue: 'https://raw.githubusercontent.com/justinyoo/ignite-spotlight-demo/${gitHubBranchName}/infra/apim-global-policy.xml'
        staticWebAppHostname: sttapp.outputs.hostname
    }
}

module fncapps './provision-functionApp.bicep' = [for (app, index) in apps: {
    name: 'FunctionApp_${app.suffix}'
    scope: rg
    dependsOn: [
        apim
    ]
    params: {
        name: name
        suffix: app.suffix
        location: location
        storageContainerName: storageContainerName
        apimApiPath: app.apiPath
    }
}]

module apis './provision-apiManagementApi.bicep' = [for (app, index) in apps: {
    name: 'ApiManagementApi_${app.suffix}'
    scope: rg
    dependsOn: [
        apim
        fncapps
    ]
    params: {
        name: name
        location: location
        apiMgmtNameValueName: 'X_FUNCTIONS_KEY_${replace(toUpper(app.apiName), '-', '_')}'
        apiMgmtNameValueDisplayName: 'X_FUNCTIONS_KEY_${replace(toUpper(app.apiName), '-', '_')}'
        apiMgmtNameValueValue: 'to_be_replaced'
        apiMgmtApiName: app.apiName
        apiMgmtApiDisplayName: app.apiName
        apiMgmtApiDescription: app.apiName
        apiMgmtApiServiceUrl: 'https://fncapp-${name}-${app.suffix}.azurewebsites.net/api'
        apiMgmtApiPath: app.apiPath
        apiMgmtApiFormat: app.apiFormat
        apiMgmtApiValue: 'https://raw.githubusercontent.com/justinyoo/ignite-spotlight-demo/${gitHubBranchName}/infra/openapi-${replace(toLower(app.apiName), '-', '')}.${app.apiExtension}'
        apiMgmtApiPolicyFormat: 'xml-link'
        apiMgmtApiPolicyValue: 'https://raw.githubusercontent.com/justinyoo/ignite-spotlight-demo/${gitHubBranchName}/infra/apim-api-policy-${replace(toLower(app.apiName), '-', '')}.xml'
    }
}]

// module depscrpt './deploymentScript.bicep' = {
//     name: 'DeploymentScript'
//     scope: rg
//     dependsOn: [
//         apim
//         fncapps
//     ]
//     params: {
//         name: name
//         location: location
//         gitHubBranchName: gitHubBranchName
//         gitHubAccessToken: gitHubAccessToken
//     }
// }
