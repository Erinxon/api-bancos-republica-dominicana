# Api para consultar los bancos que están operando en República Dominicana

# Response GET api/Banks

``` json
{
  "data": [
    {
      "name": "string",
      "type": "string",
      "image": "string",
      "totalAssets": "string",
      "totalMarket": "string",
      "emloyeeAmount": "string",
      "infoDissolution": {
        "description": "string",
        "linkOfficialNotice": "string",
        "linkFrequentQuestions": "string"
      },
      "status": "string",
      "linkDetail": "string"
    }
  ],
  "succeeded": true,
  "message": "string"
}
```

# Response GET api/Banks/Detail?URL=

``` json
{
  "data": {
    "branchOffices": "string",
    "atMs": "string",
    "subagents": "string",
    "shareholders": "string",
    "registryNumber": "string",
    "businessName": "string",
    "rnc": "string",
    "authorizedOffer": "string",
    "mainOffice": "string",
    "phone": "string",
    "email": "string",
    "webPage": "string",
    "socialNetworks": [
      {
        "name": "string",
        "link": "string",
        "image": "string"
      }
    ],
    "mobilesAppStore": [
      {
        "name": "string",
        "link": "string",
        "image": "string"
      }
    ],
    "administrativeCouncil": [
      {
        "name": "string",
        "position": "string"
      }
    ],
    "mainOfficials": [
      {
        "name": "string",
        "position": "string"
      }
    ],
    "financialStatements": [
      {
        "link": "string",
        "date": "string",
        "format": "string",
        "size": "string"
      }
    ],
    "annualReports": [
      {
        "link": "string",
        "date": "string",
        "format": "string",
        "size": "string"
      }
    ]
  },
  "succeeded": true,
  "message": "string"
}
```
