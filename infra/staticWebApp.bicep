param name string
param location string = 'eastasia'

var staticApp = {
    name: 'sttapp-${name}'
    location: location
}

resource sttapp 'Microsoft.Web/staticSites@2022-03-01' = {
  name: staticApp.name
  location: location
  sku: {
    name: 'Free'
  }
  properties: {
    allowConfigFileUpdates: true
    stagingEnvironmentPolicy: 'Enabled'
  }
}

output id string = sttapp.id
output name string = sttapp.name
